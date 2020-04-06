using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ColoursAPI.Services;


namespace ColoursAPI
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
            services.AddSingleton<ColoursService>(new ColoursService(Configuration));
            services.AddControllers();
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Mark Harrison Colours API",
                    Version = "v1",
                    Description = "Colours API",
                    TermsOfService = new Uri("https://github.com/markharrison"),
                    Contact = new OpenApiContact
                    {
                        Name = "Mark Harrison",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/markharrison"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT License",
                        Url = new Uri("https://github.com/markharrison/ColourAPI/blob/master/LICENSE"),
                    }
                }
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(builder =>
                        builder.WithOrigins("http://localhost")
                                .AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod());

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mark Harrison Colours API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
