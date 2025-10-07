using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;
using Microsoft.AspNetCore.Authorization;
namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;
[Authorize]
public class InmuebleController : Controller
{
    private readonly RepositorioInmueble repo;
    private readonly IConfiguration config;
    public InmuebleController(RepositorioInmueble repositorio, IConfiguration config)
    {
        this.repo = repositorio;
        this.config = config;
    }
    public IActionResult Index(string? email, bool? estado,DateTime? fechaInicio, DateTime? fechaFin,int pagina = 1, int tamanoPagina = 5)
    {
        ViewBag.FechaInicioFilter = fechaInicio;
        ViewBag.FechaFinFilter = fechaFin;
        ViewBag.EmailFilter = email;
        ViewBag.EstadoFilter = estado;
        //var listaInmuebles = repo.ObtenerPaginados(pagina, tamanoPagina);
        var listaInmuebles = repo.ObtenerPaginadosFiltrados(email, estado,fechaInicio,fechaFin, pagina, tamanoPagina);
        //int totalRegistros = repo.ContarInmuebles();
        var totalRegistros = repo.ContarFiltrados(email, estado,fechaInicio,fechaFin);
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);
        ViewBag.Registros = totalRegistros > 0;
        return View(listaInmuebles);
    }
    [HttpPost]
public IActionResult Agregar(Inmuebles inmueble)
{
    if (ModelState.IsValid)
    {
        if (inmueble.PortadaFile != null && inmueble.PortadaFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/inmuebles");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(inmueble.PortadaFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                inmueble.PortadaFile.CopyTo(stream);
            }

            inmueble.Portada = uniqueFileName;
        }
        inmueble.Creado_por = User.Identity?.Name ?? "Sistema"; 
        repo.AgregarInmueble(inmueble);
        TempData["Mensaje"] = "Inmueble agregado exitosamente.";
        return RedirectToAction("Index");
    }

    TempData["Mensaje"] = "Error al agregar el inmueble.";
    return View(inmueble);
}
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        var inmueble = repo.ObtenerPorID(id);
        if (inmueble == null)
        {
            TempData["Mensaje"] = "Inmueble no encontrado.";
            return RedirectToAction("Index");
        }
        repo.EliminarInmueble(id);
        TempData["Mensaje"] = "Inmueble eliminado.";
        return RedirectToAction("Index");

    }
    public IActionResult Edicion(int id)
    {
        if (id == 0)
        {
            TempData["Mensaje"] = "Inmueble no encontrado.";
            return RedirectToAction("Index");
        }
        else
        {

            var inmueble = repo.ObtenerPorID(id);
            if (inmueble == null)
            {
                TempData["Mensaje"] = "Inmueble no encontrado.";
                return RedirectToAction("Index");
            }

            return View(inmueble);
        }
    }
    [HttpPost]
public IActionResult Actualizar(Inmuebles actualizarInmuebles, IFormFile? PortadaFile)
{
    if (ModelState.IsValid)
    {
        var inmuebleExistente = repo.ObtenerPorID(actualizarInmuebles.Id_inmueble);

        if (PortadaFile != null && PortadaFile.Length > 0)
        {
            // Eliminar portada anterior (si existía)
            if (!string.IsNullOrEmpty(inmuebleExistente.Portada))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/inmuebles", inmuebleExistente.Portada);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            // Guardar nueva portada
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(PortadaFile.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/inmuebles", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                PortadaFile.CopyTo(stream);
            }

            actualizarInmuebles.Portada = fileName;
        }
        else
        {
            // Mantener portada actual si no se subió nueva
            actualizarInmuebles.Portada = inmuebleExistente.Portada;
        }

        repo.ActualizarInmueble(actualizarInmuebles);
        TempData["Mensaje"] = "Inmueble modificado correctamente.";
        return RedirectToAction("Index");
    }

    TempData["Mensaje"] = "Hubo un error al modificar el inmueble.";
    return RedirectToAction("Index");
}

    public IActionResult Detalle(int id)
    {
        var inmueble = repo.ObtenerPorID(id);
        return View(inmueble);
    }
    [HttpGet]
    public IActionResult Buscar(string term)
    {
        var resultados = repo.BuscarPorDireccion(term ?? "");
        return Json(resultados);
    }
    public IActionResult Desactivar(int id)
    {
        if (id == 0)
        {
            TempData["Mensaje"] = "Inmueble no encontrado.";
            return RedirectToAction("Index");
        }
        else
        {

            var inmueble = repo.ObtenerPorID(id);
            if (inmueble == null)
            {
                TempData["Mensaje"] = "Inmueble no encontrado.";
                return RedirectToAction("Index");
            }
            var Name = User.Identity?.Name ?? "Sistema"; 
            repo.DesactivarInmueble(inmueble.Id_inmueble,Name);
            TempData["Mensaje"] = "Inmueble desactivado.";
            return RedirectToAction("Index");
        }
    }
    public IActionResult Activar(int id)
    {
        if (id == 0)
        {
            TempData["Mensaje"] = "Inmueble no encontrado.";
            return RedirectToAction("Index");
        }
        else
        {

            var inmueble = repo.ObtenerPorID(id);
            if (inmueble == null)
            {
                TempData["Mensaje"] = "Inmueble no encontrado.";
                return RedirectToAction("Index");
            }
            repo.ActivarInmueble(inmueble.Id_inmueble);
            TempData["Mensaje"] = "Inmueble activar.";
            return RedirectToAction("Index");
        }
    }
}