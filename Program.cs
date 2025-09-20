using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;
var builder = WebApplication.CreateBuilder(args);
// Registrar el repositorio como servicio
builder.Services.AddScoped<RepositorioPropietario>();
builder.Services.AddScoped<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioContrato>();
builder.Services.AddScoped<ContratoDetalleViewModel>();
// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
