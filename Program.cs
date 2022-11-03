using ManejoTareas;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var politicaUsuarioAutenticados = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                                  .Build();

// Add services to the container.
builder.Services.AddControllersWithViews(opc => {
    opc.Filters.Add(new AuthorizeFilter(politicaUsuarioAutenticados));
});

/* Configurando el DbContext */
builder.Services.AddDbContext<ApplicationDbContext>(opc => opc.UseSqlServer("name=DefaultConnection"));

builder.Services.AddAuthentication().AddMicrosoftAccount(opc => {
    opc.ClientId = builder.Configuration["MicrosoftClientId"];
    opc.ClientSecret = builder.Configuration["MicrosoftSecretId"];
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opc => { 
    opc.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

/* Evitamos las vistas por defecto */
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opc => {
    opc.LoginPath = "/Usuarios/Login";
    opc.AccessDeniedPath = "/Usuarios/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
