using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using Exceptionless;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rat.Api.Auth;
using Rat.Api.Observability.Health;
using Rat.Core;
using Rat.Data;

namespace Rat.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private static readonly string[] CORS_ALLOW_ALL = new string[1] { "*" };

        private readonly IWebHostEnvironment _environment;
		private readonly IConfiguration _configuration;

		/// <summary>
		/// Initializes new instance of <see cref="Startup"/>
		/// </summary>
		public Startup(IWebHostEnvironment env, IConfiguration configuration)
		{
			_environment = env;
			_configuration = configuration;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(x =>
            {
                x.Configure(options =>
                    options.ActivityTrackingOptions =
                        ActivityTrackingOptions.SpanId |
                        ActivityTrackingOptions.TraceId |
                        ActivityTrackingOptions.ParentId);
            });

            var healthCheckTimeout = _configuration.GetValue<int>("HealthCheckOptions:TimeoutMs");
            healthCheckTimeout = healthCheckTimeout == default ? 30 : healthCheckTimeout;

            services
                .AddHealthChecks()
                .AddCheck<ReadyHealthCheck>("Readiness probe", tags: new[] { "ready" }, timeout: TimeSpan.FromSeconds(healthCheckTimeout))
                .AddCheck<LiveHealthCheck>("Liveness probe", tags: new[] { "live" }, timeout: TimeSpan.FromSeconds(healthCheckTimeout));

            services.AddCors(options => { options.AddPolicy("AllowAllPolicy", BuildCorsPolicy); });

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rat Api", Version = "v1" });
                c.DescribeAllParametersInCamelCase();
            });

            // Refer to this article if you require more information on CORS
            // https://docs.microsoft.com/en-us/aspnet/core/security/cors
            static void BuildCorsPolicy(CorsPolicyBuilder builder)
            {
                builder
                    .WithOrigins(CORS_ALLOW_ALL)
                    .WithMethods(CORS_ALLOW_ALL)
                    .WithHeaders(CORS_ALLOW_ALL)
                    .Build();
            }

            var domain = $"https://{_configuration["Auth0:Domain"]}";

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = $"https://{_configuration["Auth0:Audience"]}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddControllers();

            services.AddMvc(x => x.EnableEndpointRouting = false);

			var exceptionlessKey = _configuration.GetValue<string>("Exceptionless:Key");

			if (string.IsNullOrWhiteSpace(exceptionlessKey))
				throw new ArgumentException("Exceptionless key is not set");

			services.AddExceptionless(exceptionlessKey);

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserProvider, UserProvider>();

            services.AddCommandsAndQueries();

            services.AddRatDbContext(_configuration);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
			app.UseExceptionless();

			if (_environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAllPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", ReturnOk);

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    AllowCachingResponses = false,
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = HealthReportWriter.WriteResponse
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    AllowCachingResponses = false,
                    Predicate = (check) => check.Tags.Contains("live"),
                    ResponseWriter = HealthReportWriter.WriteResponse
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rat Api V1");
            });
        }

        private static Task ReturnOk(HttpContext context)
        {
            context.Response.StatusCode = 200;

            return Task.CompletedTask;
        }
    }
}
