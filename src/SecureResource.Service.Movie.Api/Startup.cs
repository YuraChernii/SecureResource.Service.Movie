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
using Microsoft.EntityFrameworkCore;
using Movies.API.Data;
using Microsoft.IdentityModel.Tokens;
using Azure.Storage.Blobs;
using Sentry;
using Newtonsoft.Json;
using SecureResource.Service.Movie.Api.Models;
using System.Net;
using Microsoft.AspNetCore.Http;
using SecureResource.Service.Movie.Api.Models.Constants;
using Microsoft.AspNetCore.Diagnostics;

namespace Movies.API
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

            services.AddDbContext<MoviesContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SQLServerDB")));

            services.AddSingleton(x => new BlobServiceClient(Configuration.GetConnectionString("AzureBlobStorage")));

            services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = "https://localhost:5005";
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false
                        };
                    });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id", "movieClient", "movies_mvc_client"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler(errorApp =>
            //    {
            //        errorApp.Run(async context =>
            //        {
            //            context.Response.StatusCode = 500;
            //            context.Response.ContentType = "application/json";
            //            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            //            var exceptionHandlerPathFeature =
            //                context.Features.Get<IExceptionHandlerPathFeature>();

            //            var exception = exceptionHandlerPathFeature.Error;
            //            SentrySdk.CaptureException(exception);

            //            while (exception?.InnerException != null)
            //            {
            //                exception = exception.InnerException;
            //            }

            //            var responseText = JsonConvert.SerializeObject(new ErrorResponseModel(ErrorTypes.InternalServerError, exception?.Message, HttpStatusCode.InternalServerError, exceptionHandlerPathFeature.Error?.StackTrace, exceptionHandlerPathFeature.Error?.Data));
            //            await context.Response.WriteAsync(responseText);
            //        });
            //    });
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable automatic tracing integration.
            // If running with .NET 5 or below, make sure to put this middleware
            // right after `UseRouting()`.
            app.UseSentryTracing();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
