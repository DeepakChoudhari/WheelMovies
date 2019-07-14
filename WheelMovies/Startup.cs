using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WheelMovies.Business.Implementation;
using WheelMovies.Business.Interfaces;
using WheelMovies.Repository;

namespace WheelMovies
{
    public class Startup
    {
        static readonly string AllowTrustedOriginsPolicy = "WheelMovies";

        static readonly string[] TrustedOrigins = new string[] { "*" };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(options =>
            {
                options.AddPolicy(AllowTrustedOriginsPolicy,
                    policyBuilder =>
                    {
                        policyBuilder.WithOrigins(TrustedOrigins);
                    }
                );
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Wheel Movies", Version = "v1" });
            });

            services.AddDbContext<WheelMoviesContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("WheelMoviesDbConnection"));
            });

            services.AddScoped(typeof(IMoviesRepository), typeof(MoviesRepository));
            services.AddScoped(typeof(IMovieService), typeof(MovieService));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                });
            }
            app.UseMvc();
            app.UseCors(AllowTrustedOriginsPolicy);
        }
    }
}
