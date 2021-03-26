using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using Npgsql;

namespace WebApplication1
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

            services.AddMvc();

            //mssqllocaldb
            //services.AddDbContext<WebApplication1Context>(options => options.UseSqlServer(Configuration.GetConnectionString("WebApplication1Context")));

            //local postgresql database
            //services.AddDbContext<WebApplication1Context>(options => options.UseNpgsql(Configuration.GetConnectionString("WebApplication1Context")));

            //heroku postgresql
            services.AddDbContext<WebApplication1Context>(options => options.UseNpgsql(GetHerokuConnectionString()));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //The database gets updated when the app starts
            UpdateDatabase(app);

        }

        private string GetHerokuConnectionString()
        {
            //Get the connection string from the environment variable
            string connURI = Environment.GetEnvironmentVariable("DATABASE_URL");

            //Split the connection string
            string[] splittedConnURI = connURI.Split(new[] { ':', '/', '@' }, StringSplitOptions.RemoveEmptyEntries);

            return $"Server={splittedConnURI[3]};Port={splittedConnURI[4]};Database={splittedConnURI[5]};User Id={splittedConnURI[1]};Password={splittedConnURI[2]};SSL Mode=Require;Trust Server Certificate=true";

        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<WebApplication1Context>())
                {
                    context.Database.Migrate();
                }
            }
        }

    }
}
