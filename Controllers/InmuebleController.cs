using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;

public class InmuebleController : Controller
{
    private readonly RepositorioInmueble repo;
    private readonly IConfiguration config;
    public InmuebleController(RepositorioInmueble repositorio, IConfiguration config)
    {
        this.repo = repositorio;
        this.config = config;
    }
    public IActionResult Index(int pagina = 1, int tamanoPagina = 5)
    {
        var listaInquilinos = repo.ObtenerPaginados(pagina, tamanoPagina);

        int totalRegistros = repo.ContarInmuebles();
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);
        ViewBag.Registros = totalRegistros > 0;
        ViewBag.Propietarios = repo.ObtenerTodosPropietarios();
        return View(listaInquilinos);
    }
    [HttpPost]
    public ActionResult Agregar(Inmuebles inmueble)
    {
        if (ModelState.IsValid)
        {
            TempData["Mensaje"] = "Inmuebles agregado exitosamente.";
            repo.AgregarInmueble(inmueble);
            return RedirectToAction("Index");
        }
        TempData["Mensaje"] = "Error al agregar.";
        return RedirectToAction("Index");
    }
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
    public IActionResult Actualizar(Inmuebles actualizarInmuebles)
    {
        if (ModelState.IsValid)
        {
            repo.ActualizarInmueble(actualizarInmuebles);
            TempData["Mensaje"] = "Inmueble Modificado correctamente.";
            return RedirectToAction("Index");
        }
        TempData["Mensaje"] = "Hubo un error al Modificar el Inmueble.";
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
            repo.DesactivarInmueble(inmueble.Id_inmueble);
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