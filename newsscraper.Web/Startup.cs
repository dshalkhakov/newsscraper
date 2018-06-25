using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace newsscraper.Web
{
    using Microsoft.EntityFrameworkCore;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
    using newsscraper.DAL;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString = Configuration.GetConnectionString("ScraperContext");

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<ScraperContext>(options => options.UseNpgsql(connectionString, opts => opts.MigrationsAssembly("newsscraper.Migrations")));

            //
            // application services
            //
            services.AddScoped(typeof(DAL.IArticlesRepository), typeof(DAL.ArticlesRepository));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
