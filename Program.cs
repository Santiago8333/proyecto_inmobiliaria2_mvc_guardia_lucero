using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
// Registrar el repositorio como servicio
builder.Services.AddScoped<RepositorioPropietario>();
builder.Services.AddScoped<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioContrato>();
builder.Services.AddScoped<ContratoDetalleViewModel>();
builder.Services.AddScoped<RepositorioUsuario>();
// Add services to the container.
builder.Services.AddControllersWithViews();
//configurar atentificacion
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>//el sitio web valida con cookie
	{
		options.LoginPath = "/Usuario/Login";
		options.LogoutPath = "/Usuario/Logout";
		options.AccessDeniedPath = "/Home/Registringido";
		options.ExpireTimeSpan = TimeSpan.FromMinutes(10);//Tiempo de expiración
	});
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Empleado"));
	options.AddPolicy("Administrador", policy => policy.RequireRole(ClaimTypes.Role,"Administrador"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//habilitar autentificacion
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
