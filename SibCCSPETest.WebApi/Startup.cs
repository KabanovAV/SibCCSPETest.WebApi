using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;
using SibCCSPETest.WebApi.Middlewares;

namespace SibCCSPETest.WebApi
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSerilog();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Version 1.0", new OpenApiInfo
                {
                    Version = "Version 1.0",
                    Title = "Web API",
                    Description = "Try repeat what I learn from DotNetTutorials"
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:7113;http://localhost:5168")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var connection = _configuration.GetConnectionString("PostgreConnection");
            services.AddDbContext<DbDataContext>(options => options.UseNpgsql(connection));

            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IRepoServiceManager, RepoServiceManager>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/Version 1.0/swagger.json", "Web API Version 1.0"));
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseNotFoundCustomMiddleware();
            app.UseLoggingCustomMiddleware();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
