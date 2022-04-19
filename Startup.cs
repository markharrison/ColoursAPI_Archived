using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColoursAPI.Services;
using Microsoft.AspNetCore.Http;

namespace ColoursAPI
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            config = configuration;
        }

        public IConfiguration config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<AppConfig>(op =>
            {
                AppConfig appconfig = new AppConfig(config);

                services.AddSingleton(new ColoursService(appconfig));

                return appconfig;
            });

            services.AddControllers();
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Mark Harrison Colours API",
                    Version = "v1",
                    Description = "Colours API",
                    TermsOfService = new Uri("https://github.com/markharrison/ColourAPI/blob/master/LICENSE"),
                    Contact = new OpenApiContact
                    {
                        Name = "Mark Harrison",
                        Email = "mark.coloursapi@harrison.ws",
                        Url = new Uri("https://github.com/markharrison/coloursapi"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT License",
                        Url = new Uri("https://github.com/markharrison/ColourAPI/blob/master/LICENSE"),
                    }
                }
                );

                c.EnableAnnotations();

                string strURL = config.GetValue<string>("ServerURL");
                if (strURL != null && strURL != "")
                {
                    c.AddServer(new OpenApiServer()
                    {
                        Url = strURL
                    });
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppConfig appconfig)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/appconfiginfo", async context => await context.Response.WriteAsync(appconfig.GetAppConfigInfo()));
                endpoints.MapControllers();
            });
        }
    }
}
