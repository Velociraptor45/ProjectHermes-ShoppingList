using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Database;
using AutoMapper;
using ShoppingList.Mapping.Profiles;

namespace ShoppingList
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

#if RELEASE
            services.AddDbContext<ShoppingContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("Shopping-Productive"), opt =>
                    opt.EnableRetryOnFailure(3)));
#elif DEBUG
            services.AddDbContext<ShoppingContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("Shopping-Development"), opt =>
                    opt.EnableRetryOnFailure(3)));
#endif

            services.AddSingleton(GetMapper());
            services.AddTransient<IShoppingRepositoryFactory, ShoppingRepositoryFactory>();
        }

        private IMapper GetMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ShoppingEntitiesProfile));
            });
            return mapperConfiguration.CreateMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
