using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Youtube.Data;
using Youtube.Services;

namespace Youtube
{
    public class Startup
    {
        IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddDbContext<YoutubeDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("Youtube")));
            //services.AddTransient<InitialCreate>();
            services.AddScoped<IUserData, SqlUserData>();
            services.AddScoped<IFollowData, SqlFollowData>();
            services.AddScoped<IVideoData, SqlVideoData>();
            services.AddScoped<ICommentData, SqlCommentData>();
            services.AddScoped<ILikeVideoData, SqlLikeVideoData>();
            services.AddScoped<ILikeCommentData, SqlLikeCommentData>();
            services.AddMvc()
            .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseNodeModules(env.ContentRootPath);

            app.UseSession();

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseMvc(configureRoutes);

            //initialCreate.Seed().Wait();

        }
        private void configureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller}/{action}/{id?}");
        }
    }
}
