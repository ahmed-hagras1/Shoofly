using Shoofly.Infrastructure.Dependencies;
using Shoofly.Service.Dependencies;
using Shoofly.Core.Dependencies;

namespace Shoofly.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            try
            {
                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                #region Dependency injection.
                // 🟢 THIS IS THE BEST POSITION
                // Just call your custom extension method.
                // It will register all infrastructure dependencies
                // service dependencies at once.
                // and core dependencies.
                builder.Services.AddInfrastructureDependencies(builder.Configuration)
                    .AddServiceDependencies()
                    .AddCoreDependencies()
                    .AddIdentityDependencies(builder.Configuration);
                #endregion

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }


        }
    }
}
