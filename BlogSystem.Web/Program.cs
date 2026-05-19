using BlogSystem.Application;
using BlogSystem.Infrastructure;
using BlogSystem.Application.Validators.User;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

// Authentication / Authorization (Cookie for now)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.LogoutPath = "/auth/logout";
        options.AccessDeniedPath = "/auth/login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// API controllers
app.MapControllers();

app.Run();
