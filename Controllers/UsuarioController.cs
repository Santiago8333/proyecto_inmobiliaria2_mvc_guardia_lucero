using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;

public class UsuarioController : Controller
{
private readonly RepositorioUsuario repo;
private readonly IConfiguration config;
    public UsuarioController(RepositorioUsuario repositorio, IConfiguration config)
    {

        this.repo = repositorio;
        this.config = config;
        this.config = config;
}

public IActionResult Index(int pagina = 1, int tamanoPagina = 5)
{
    var listaUsuarios = repo.ObtenerPaginados(pagina, tamanoPagina);

    int totalRegistros = repo.ContarUsuarios();
    ViewBag.PaginaActual = pagina;
    ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);
    ViewBag.Registros = totalRegistros > 0;
    return View(listaUsuarios);
}

public IActionResult Login()
{
return View();
}
public async Task<IActionResult> Agregar(Usuarios nuevoUsuario, IFormFile AvatarFile)
{
    //verificar si el email ya esta registrado
    var e = await repo.ObtenerPorEmailAsync(nuevoUsuario.Email);
    if (e != null)
    {
        if (nuevoUsuario.Email == e.Email)
        {
            TempData["Mensaje"] = "Error: EL email de ese usuario ya esta registrado.";
            return RedirectToAction("Index");
        }
    }

    if (!ModelState.IsValid)
    {
        Console.WriteLine("El modelo no es válido");
        foreach (var modelState in ModelState.Values)
        {
            foreach (var error in modelState.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }
    }
    ModelState.Remove("AvatarFile");
    if (ModelState.IsValid)
    {
        //Console.WriteLine("dentro: " + nuevoUsuario);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: nuevoUsuario.Clave,
                salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
        nuevoUsuario.Clave = hashed;
        //verificar rengo
        if (nuevoUsuario.Rol == 1)
        {
            nuevoUsuario.RolNombre = "Administrador";
        }
        else
        {
            nuevoUsuario.RolNombre = "Empleado";
        }
        // Verificar si se subió un archivo de avatar
        if (AvatarFile != null && AvatarFile.Length > 0)
        {
            var fileName = Path.GetFileNameWithoutExtension(AvatarFile.FileName);
            var extension = Path.GetExtension(AvatarFile.FileName);
            var newFileName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatars", newFileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await AvatarFile.CopyToAsync(stream);
            }

            Console.WriteLine("Archivo cargado: " + fileName);
            nuevoUsuario.AvatarUrl = $"/avatars/{newFileName}";
        }
        else
        {
            Console.WriteLine("No se subió un archivo de avatar.");
            nuevoUsuario.AvatarUrl = "/avatars/default-avatar.png";  // Asignar avatar por defecto
        }

        repo.AgregarUsuario(nuevoUsuario);
        TempData["Mensaje"] = "Usuario agregado exitosamente.";
        return RedirectToAction("Index");
    }

    TempData["Mensaje"] = "Hubo un error al agregar el Usuario.";

    return RedirectToAction("Index");
}
    //Get logout usuario
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Usuario");
    }
}