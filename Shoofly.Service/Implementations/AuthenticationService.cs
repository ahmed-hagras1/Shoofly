using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shoofly.Data.Entities.Identity;
using Shoofly.Data.Helpers;
using Shoofly.Infrastructure.Abstracts;
using Shoofly.Service.Abstracts;
using Shoofly.Shared.Resources;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly JWTSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public AuthenticationService(IOptions<JWTSettings> jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
            UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }
        #endregion

        #region Public Methods

        public async Task<JWTAuthResult> GetJWTToken(ApplicationUser user)
        {
            var (jwtToken, accessToken) = await GenerateJWTToken(user);
            var refreshTokenString = GenerateRefreshToken();

            var jwtAuthResult = new JWTAuthResult()
            {
                AccessToken = accessToken,
                RefreshToken = new RefreshToken()
                {
                    UserName = user.UserName!,
                    TokenString = refreshTokenString,
                    ExpireAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays)
                }
            };

            var userRefreshToken = new UserRefreshToken()
            {
                UserId = user.Id,
                Token = refreshTokenString,
                JWTId = jwtToken.Id,
                IsUsed = false,
                IsRevoked = false,
                AddedTime = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays)
            };

            await _refreshTokenRepository.AddAsync(userRefreshToken);
            return jwtAuthResult;
        }

        public async Task<JWTAuthResult> GetRefreshToken(string accessToken, string refreshToken)
        {
            var jwtToken = ReadJWTToken(accessToken);

            // Use claims defined in UserClaimModel for verification
            var jti = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName))?.Value;

            if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(userName))
                throw new SecurityTokenException("TokenClaimsMissing");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) throw new SecurityTokenException("UserNotFound");

            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(x => x.Token == refreshToken && x.UserId == user.Id && x.JWTId == jti);

            if (userRefreshToken == null) throw new SecurityTokenException("RefreshTokenNotFound");

            if (userRefreshToken.ExpiryDate <= DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                await _refreshTokenRepository.UpdateAsync(userRefreshToken);
                throw new SecurityTokenException("RefreshTokenExpired");
            }

            if (!userRefreshToken.IsActive) throw new SecurityTokenException("RefreshTokenRevoked");

            userRefreshToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(userRefreshToken);

            return await GetJWTToken(user);
        }

        public async Task<string> ValidateToken(string accessToken)
        {
            var jwtToken = ReadJWTToken(accessToken);
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(jti)) throw new SecurityTokenException("JtiClaimMissing");
            return jti;
        }

        public async Task<string> RevokeRefreshToken(string accessToken)
        {
            var jti = await ValidateToken(accessToken);
            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(x => x.JWTId == jti);

            if (userRefreshToken == null)
                throw new KeyNotFoundException(SharedResourcesKeys.TokenNotFound);

            userRefreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(userRefreshToken);

            return SharedResourcesKeys.LoggedOutSuccessfully;
        }
        public async Task<string> RevokeToken(string refreshToken)
        {
            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                    .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (userRefreshToken == null)
                throw new KeyNotFoundException(SharedResourcesKeys.TokenNotFound);

            if (userRefreshToken.IsRevoked)
                return SharedResourcesKeys.TokenAlreadyRevoked;

            userRefreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(userRefreshToken);

            return SharedResourcesKeys.TokenRevokedSuccessfully;
        }
        public async Task<string> RevokeAllSessions(string userId)
        {
            // Get all active (not yet revoked) tokens for this user
            var userTokens = await _refreshTokenRepository.GetTableNoTracking()
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync();

            if (!userTokens.Any())
                return SharedResourcesKeys.NoActiveSessionsFound;

            // Mark all as revoked
            foreach (var token in userTokens)
            {
                token.IsRevoked = true;
                // Your repository likely handles the Attach/Update logic
                await _refreshTokenRepository.UpdateAsync(token);
            }

            return SharedResourcesKeys.AllSessionsRevokedSuccessfully;
        }

        #endregion

        #region Private Helpers

        private async Task<(JwtSecurityToken, string)> GenerateJWTToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            // Dynamically determine UserType based on the class instance
            var userType = user.GetType().Name;

            var claims = new List<Claim>
            {
                // Core Identity
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                
                // Mapping properties using UserClaimModel names for consistency
                new Claim(nameof(UserClaimModel.UserName), user.UserName ?? ""),
                new Claim(nameof(UserClaimModel.Email), user.Email ?? ""),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber ?? ""),
                
                // Shoofly Specific Details
                new Claim(nameof(UserClaimModel.FullName), user.FullName),
                new Claim(nameof(UserClaimModel.PreferredLanguage), user.PreferredLanguage),
                new Claim(nameof(UserClaimModel.CountryId), user.CountryId.ToString()),
                new Claim(nameof(UserClaimModel.UserType), userType)
            };

            claims.AddRange(userClaims);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return (jwtToken, new JwtSecurityTokenHandler().WriteToken(jwtToken));
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private JwtSecurityToken ReadJWTToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) throw new ArgumentNullException(nameof(accessToken));

            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = false
            };

            try
            {
                handler.ValidateToken(accessToken, parameters, out var validatedToken);
                return (JwtSecurityToken)validatedToken;
            }
            catch
            {
                throw new SecurityTokenException("TokenIsInvalid");
            }
        }

        #endregion
    }
}
