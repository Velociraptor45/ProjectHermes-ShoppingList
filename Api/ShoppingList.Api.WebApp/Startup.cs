using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Domain;
using ProjectHermes.ShoppingList.Api.Endpoint;
using ProjectHermes.ShoppingList.Api.Infrastructure;

namespace ProjectHermes.ShoppingList.Api.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDomain();
            services.AddEndpointControllers();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("temporary",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); //todo
                });
            });
            services.AddInfrastructure(Configuration.GetConnectionString("Shopping-Database"));
            services.AddApplicationServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingList API v1");
            });

            app.UseRouting();
            app.UseCors("temporary");
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}