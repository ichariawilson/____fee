using Devspaces.Support;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Fee.Mobile.Applying.HttpAggregator.Config;
using Microsoft.Fee.Mobile.Applying.HttpAggregator.Filters.Basket.API.Infrastructure.Filters;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using GrpcApplying;
using GrpcBasket;
using ScholarshipApi;
using Microsoft.Fee.Mobile.Applying.HttpAggregator.Infrastructure;
using Microsoft.Fee.Mobile.Applying.HttpAggregator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Microsoft.Fee.Mobile.Applying.HttpAggregator
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
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddUrlGroup(new Uri(Configuration["ScholarshipUrlHC"]), name: "scholarshipapi-check", tags: new string[] { "scholarshipapi" })
                .AddUrlGroup(new Uri(Configuration["ApplyingUrlHC"]), name: "applyingapi-check", tags: new string[] { "applyingapi" })
                .AddUrlGroup(new Uri(Configuration["ApplyingBasketUrlHC"]), name: "applyingbasketapi-check", tags: new string[] { "applyingbasketapi" })
                .AddUrlGroup(new Uri(Configuration["StudentIdentityUrlHC"]), name: "studentidentityapi-check", tags: new string[] { "studentidentityapi" })
                .AddUrlGroup(new Uri(Configuration["PaymentUrlHC"]), name: "paymentapi-check", tags: new string[] { "paymentapi" });

            services.AddCustomMvc(Configuration)
                 .AddCustomAuthentication(Configuration)
                 .AddDevspaces()
                 .AddHttpServices()
                 .AddGrpcServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var pathBase = Configuration["PATH_BASE"];

            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Apply BFF V1");

                c.OAuthClientId("mobileapplyingaggswaggerui");
                c.OAuthClientSecret(string.Empty);
                c.OAuthRealm(string.Empty);
                c.OAuthAppName("Mobile Applying Aggregattor Swagger UI");
            });

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<UrlsConfig>(configuration.GetSection("urls"));

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Applying Aggregator for Mobile Clients",
                    Version = "v1",
                    Description = "Applying Aggregator for Mobile Clients"
                });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{configuration.GetValue<string>("StudentIdentityUrlExternal")}/connect/authorize"),
                            TokenUrl = new Uri($"{configuration.GetValue<string>("StudentIdentityUrlExternal")}/connect/token"),

                            Scopes = new Dictionary<string, string>()
                            {
                                { "mobileapplyingagg", "Mobile Applying Aggregator" }
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials());
            });

            return services;
        }
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = configuration.GetValue<string>("urls:studentidentity");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "mobileapplyingagg";
            });

            return services;
        }

        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            //register delegating handlers
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //register http services

            services.AddHttpClient<IApplicationApiClient, ApplicationApiClient>()
                   .AddDevspacesSupport();

            return services;
        }

        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddScoped<IBasketService, BasketService>();

            services.AddGrpcClient<Basket.BasketClient>((services, options) =>
            {
                var basketApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcBasket;
                options.Address = new Uri(basketApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddScoped<IScholarshipService, ScholarshipService>();

            services.AddGrpcClient<Scholarship.ScholarshipClient>((services, options) =>
            {
                var scholarshipApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcScholarship;
                options.Address = new Uri(scholarshipApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddScoped<IApplyingService, ApplyingService>();

            services.AddGrpcClient<ApplyingGrpc.ApplyingGrpcClient>((services, options) =>
            {
                var applyingApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcApplying;
                options.Address = new Uri(applyingApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}
