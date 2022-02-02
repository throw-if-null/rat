using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Rat.Api.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(new Program().GetType().Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(
		JwtBearerDefaults.AuthenticationScheme,
		configureOptions =>
		{
			configureOptions.Authority = $"{builder.Configuration["Auth0:Domain"]}";
			configureOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
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
app.UseSwaggerUI();

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

CreateProjectRoute.Map(app);
GetProjectsForUserRoute.Map(app);
GetProjectRoute.Map(app);
UpdateProjectRoute.Map(app);
DeleteProjectRoute.Map(app);

app.Run();