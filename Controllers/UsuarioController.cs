using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
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

    [HttpPost]
    public async Task<IActionResult> Login(Usuarios loginusuario)
    {
        Console.WriteLine("Login: " + loginusuario.Email);
        Console.WriteLine("Login: " + loginusuario.Clave);
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
            TempData["Mensaje"] = "Error de credenciales";
            return View();
        }
        if (ModelState.IsValid)
        {
            var saltString = config["Salt"];
            if (string.IsNullOrEmpty(saltString))
            {
                throw new InvalidOperationException("El valor 'Salt' no está configurado en el archivo de configuración.");
            }
            // Aquí iría tu lógica de autenticación
            // Hash de la contraseña ingresada por el usuario durante el login
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginusuario.Clave,
                salt: System.Text.Encoding.ASCII.GetBytes(saltString), // Asegúrate de usar la misma sal que usaste al registrar el usuario
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
            var e = await repo.ObtenerPorEmailAsync(loginusuario.Email);

            if (e == null || e.Clave != hashed)
            {

                TempData["Mensaje"] = "Credenciales ingresadas incorrectas";
                return RedirectToAction("Login");
            }

            var AvatarUrl = e.AvatarUrl;
            if (string.IsNullOrEmpty(AvatarUrl))
            {
                throw new InvalidOperationException("El valor AvatarUrl esta vacio.");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, e.Email),
                new Claim("FullName", e.Nombre + " " + e.Apellido),
                new Claim("AvatarUrl", AvatarUrl),
                new Claim(ClaimTypes.Role, e.RolNombre),
            };
            var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            TempData["Mensaje"] = "Credenciales Correctas";
            return RedirectToAction("", "Home");
        }
        TempData["Mensaje"] = "Error de credenciales";
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
            var saltString = config["Salt"];
            if (string.IsNullOrEmpty(saltString))
            {
                throw new InvalidOperationException("El valor 'Salt' no está configurado en el archivo de configuración.");
            }
            //Console.WriteLine("dentro: " + nuevoUsuario);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: nuevoUsuario.Clave,
                salt: System.Text.Encoding.ASCII.GetBytes(saltString),
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

    public async Task<IActionResult> Actualizar(Usuarios actualizarUsuario)
    {
        if (ModelState.IsValid)
        {
            //verificar rengo
            if (actualizarUsuario.Rol == 1)
            {
                actualizarUsuario.RolNombre = "Administrador";
            }
            else
            {
                actualizarUsuario.RolNombre = "Empleado";
            }
            //verificar contraseña
            var usuario = repo.ObtenerPorID(actualizarUsuario.Id_usuario);
            if (usuario == null)
            {
                TempData["Mensaje"] = "Usuario no encontrado.";
                return RedirectToAction("Index");
            }
            
            //verificar si el email ya esta registrado
            if (usuario.Email == actualizarUsuario.Email)
            {

            }
            else
            {
                var e = await repo.ObtenerPorEmailAsync(actualizarUsuario.Email);
                if (e != null)
                {
                    if (actualizarUsuario.Email == e.Email)
                    {
                        TempData["Mensaje"] = "Error al actualizar: EL email de ese usuario ya esta registrado.";
                        return RedirectToAction("Edicion", new { id = usuario.Id_usuario });
                    }
                }
            }
            repo.ActualizarUsuario(actualizarUsuario);
            
            if (usuario.Email == @User.Identity.Name)
            {
                actualizarUsuario.AvatarUrl = usuario.AvatarUrl;
                await ActualizarClaimsYReautenticarEdicion(actualizarUsuario);

            }

            TempData["Mensaje"] = "Usuario Modificado correctamente.";
            return RedirectToAction("Edicion", new { id = usuario.Id_usuario });
        }
        TempData["Mensaje"] = "Hubo un error al Modificar el Usuario.";
        return RedirectToAction("Index");
    }
    

    public IActionResult Edicion(int id)
    {
        if (id == 0)
        {
            TempData["Mensaje"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }
        else
        {
            var usuario = repo.ObtenerPorID(id);
            if (usuario == null)
            {
                TempData["Mensaje"] = "Usuario no encontrado.";
                return RedirectToAction("Index");
            }

            return View(usuario);
        }
    }
    [HttpPost]
    public IActionResult ActualizarEditarClave(Usuarios actualizarUsuario)
    {
        if (ModelState.IsValid)
        {
            var usuario = repo.ObtenerPorID(actualizarUsuario.Id_usuario);
            if (usuario == null)
            {
                TempData["Mensaje"] = "Usuario no encontrado.";
                return RedirectToAction("Index");
            }
            var saltString = config["Salt"];
            if (string.IsNullOrEmpty(saltString))
            {
                throw new InvalidOperationException("El valor 'Salt' no está configurado en el archivo de configuración.");
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: actualizarUsuario.Clave,
                salt: System.Text.Encoding.ASCII.GetBytes(saltString),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
            actualizarUsuario.Clave = hashed;
            repo.ActualizarEditarClave(actualizarUsuario);
            TempData["Mensaje"] = "Clave Modificado correctamente.";
            return RedirectToAction("Index");
        }
        TempData["Mensaje"] = "Hubo un error al Modificar Clave.";
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> ActualizarEditarAvatar(Usuarios actualizarUsuario,IFormFile? AvatarFile)
    {
        if (ModelState.IsValid)
        {
            var usuario = repo.ObtenerPorID(actualizarUsuario.Id_usuario);
            if (usuario == null)
            {
                TempData["Mensaje"] = "Usuario no encontrado.";
                return RedirectToAction("Index");
            }
            
        if (AvatarFile != null && AvatarFile.Length > 0)
        {
            // Eliminar portada anterior (si existía)
            if (!string.IsNullOrEmpty(actualizarUsuario.AvatarUrl))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", usuario.AvatarUrl);
                if (System.IO.File.Exists(oldFilePath) && oldFilePath != "wwwroot/images/avatars/default-avatar.png")
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            // Guardar nueva portada
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(AvatarFile.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatars", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                AvatarFile.CopyTo(stream);
            }

            actualizarUsuario.AvatarUrl = fileName;
        }
        else
        {
            // Mantener portada actual si no se subió nueva
            actualizarUsuario.AvatarUrl = usuario.AvatarUrl;
        }
            if (usuario.Email == @User.Identity.Name)
            { 
                actualizarUsuario.Email = usuario.Email;
                actualizarUsuario.Nombre = usuario.Nombre;
                actualizarUsuario.Apellido = usuario.Apellido;
                actualizarUsuario.RolNombre = usuario.RolNombre;
                await ActualizarClaimsYReautenticarEdicion(actualizarUsuario);
            }
            repo.ActualizarEditarAvatar(actualizarUsuario);
            TempData["Mensaje"] = "Avatar Modificado correctamente.";
            return RedirectToAction("Edicion", new { id = usuario.Id_usuario });
        }
        TempData["Mensaje"] = "Hubo un error al Modificar Avatar.";
        return RedirectToAction("Index");
    }
    
private async Task ActualizarClaimsYReautenticarEdicion(Usuarios usuarioActualizado)
    {
        // Crear una lista de claims actualizada
        var AvatarUrl = usuarioActualizado.AvatarUrl;
        if (string.IsNullOrEmpty(AvatarUrl))
        {
            throw new InvalidOperationException("El valor 'AvatarUrl' no está.");
        }
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuarioActualizado.Email),
        new Claim("FullName", usuarioActualizado.Nombre + " " + usuarioActualizado.Apellido),
        new Claim(ClaimTypes.Role, usuarioActualizado.RolNombre),// El rol actualizado
        new Claim("AvatarUrl",AvatarUrl)
        // Puedes añadir más claims si es necesario
        };

        // Crear una nueva identidad de usuario con las claims actualizadas
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Crear un nuevo principal
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Re-autenticar al usuario actualizando su cookie de autenticación
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
    }
}