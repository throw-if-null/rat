using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rat.Api.Auth;
using Rat.Api.CreateConfigurationEntries;
using Rat.Api.Observability.Health;
using Rat.Api.Routes;
using Rat.Api.Routes.CreateConfiguration;
using Rat.Api.Routes.CreateConfigurationEntry;
using Rat.Api.Routes.DeleteConfiguration;
using Rat.Api.Routes.DeleteConfigurationEntry;
using Rat.Api.Routes.Empty;
using Rat.Api.Routes.GetConfiguration;
using Rat.Api.Routes.Health;
using Rat.Api.Routes.PatchConfiguration;
using Rat.Api.Routes.PatchConfigurationEntry;
using Rat.Core;
using Rat.Sql;

namespace Rat.Api
{
	public partial class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

			builder.Services.Configure<SqlConnectionFactoryOptions>(builder.Configuration.GetSection($"{nameof(SqlConnectionFactoryOptions)}"));

			builder.Services.AddLogging(x =>
			{
				x.Configure(options =>
					options.ActivityTrackingOptions =
						ActivityTrackingOptions.SpanId |
						ActivityTrackingOptions.TraceId |
						ActivityTrackingOptions.ParentId);
			});

			var healthCheckTimeout = builder.Configuration.GetValue<int>("HealthCheckOptions:TimeoutMs");
			healthCheckTimeout = healthCheckTimeout == default ? 30 : healthCheckTimeout;

			builder.Services
				.AddHealthChecks()
				.AddCheck<ReadyHealthCheck>("Readiness probe", tags: new[] { "ready" }, timeout: TimeSpan.FromSeconds(healthCheckTimeout))
				.AddCheck<LiveHealthCheck>("Liveness probe", tags: new[] { "live" }, timeout: TimeSpan.FromSeconds(healthCheckTimeout));

			builder.Services.AddCors(options => { options.AddPolicy("AllowAllPolicy", BuildCorsPolicy); });

			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			builder.Services.AddSingleton<IMemberProvider, MemberProvider>();
			builder.Services.AddSingleton<RouteExecutor>();
			builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

			builder.Services.AddCommands();
			builder.Services.AddQueries();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "Rat API", Version = "v1" });

				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "bearer"
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = "Bearer",
								Type = ReferenceType.SecurityScheme
							}
						},
						Array.Empty<string>()
					}
				});
			});

			builder.Services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(
					JwtBearerDefaults.AuthenticationScheme,
					configureOptions =>
					{
						configureOptions.Authority = $"{builder.Configuration["Auth0:Domain"]}";
						configureOptions.TokenValidationParameters = new TokenValidationParameters
						{
							ValidAudience = builder.Configuration["Auth0:Audience"],
							ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
						};
					});

			builder.Services.AddAuthorization(configureOptions =>
			{
				configureOptions.AddPolicy(
					"MustHaveAuthenticatedUser",
					policy => policy.RequireAuthenticatedUser());
			});

			WebApplication? app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.MapSwagger();
			app.UseSwaggerUI(options =>
			{
				options.EnableTryItOutByDefault();
			});

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.Use(async (ctx, next) =>
			{
				try
				{
					await next();
				}
				catch (BadHttpRequestException ex)
				{
					ctx.Response.StatusCode = ex.StatusCode;
					await ctx.Response.WriteAsync(ex.Message);
				}
			});

			EmptyRoute.Map(app);
			LivenessRoute.Map(app);
			ReadinessRoute.Map(app);
			CreateProjectRoute.Map(app);
			GetProjectsForUserRoute.Map(app);
			GetProjectRoute.Map(app);
			PatchProjectRoute.Map(app);
			DeleteProjectRoute.Map(app);
			CreateConfigurationRoute.Map(app);
			GetConfigurationRoute.Map(app);
			PatchConfigurationRoute.Map(app);
			DeleteConfigurationRoute.Map(app);
			CreateConfigurationEntryRoute.Map(app);
			CreateConfigurationEntriesRoute.Map(app);
			PatchConfigurationEntryRoute.Map(app);
			DeleteConfigurationEntryRoute.Map(app);

			app.Run();

			// Refer to this article if you require more information on CORS
			// https://docs.microsoft.com/en-us/aspnet/core/security/cors
			static void BuildCorsPolicy(CorsPolicyBuilder builder)
			{
				builder
					.SetIsOriginAllowed(x => true)
					.AllowCredentials()
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyMethod()
					.Build();
			}
		}
	}

}
