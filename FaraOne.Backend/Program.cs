using FaraOne.Application;
using FaraOne.Application.Context;
using FaraOne.Application.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using FaraOne.Infraustructor;
// TODO  ایجاد دیتابیس و ادامه ساخت ها 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ITokenValidator, TokenValidator>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(configureOptions =>
{
    configureOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["JwtConfigs:issuer"],
        ValidAudience = builder.Configuration["JwtConfigs:audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfigs:key"])),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
    configureOptions.SaveToken = true; //httpcontext.GetTokenAsync()
    configureOptions.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = context =>
        {
            //logs
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {//logs
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {//logs
            var tokenValidator = context.HttpContext.RequestServices.GetRequiredService<ITokenValidator>();
            return tokenValidator.Execute(context);
        },
        OnMessageReceived = context =>
        {
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {

            return Task.CompletedTask;
        }
    };
});
 
 
builder.Services.AddScoped<UserRepository, UserRepository>();
builder.Services.AddScoped<UserTokenRepository, UserTokenRepository>();
builder.Services.AddIdnetityMain( builder.Configuration);


builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
