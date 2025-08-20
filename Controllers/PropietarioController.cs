using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private RepositorioPropietario repo;
    public PropietarioController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
        repo = new RepositorioPropietario();

    }
    public IActionResult Index(int pagina = 1, int tamanoPagina = 5)
    {
        var listaPropietarios = repo.ObtenerPaginados(pagina, tamanoPagina);

        int totalRegistros = repo.ContarPropietarios();
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);

        return View(listaPropietarios);
    }

    public IActionResult Detalle(int id)
    {

        return View();
    }
    [HttpPost]
    public ActionResult Agregar(Propietario propietario)
    {
        if (ModelState.IsValid)
        {
            TempData["Mensaje"] = "Propietario agregado exitosamente.";
            repo.AgregarPropietario(propietario);
            return RedirectToAction("Index");
        }
        TempData["Mensaje"] = "Error.";
        return View(propietario);
    }

    public IActionResult Edicion(int id)
    {
        if (id == 0)
        {
            TempData["Mensaje"] = "Propietario no encontrado.";
            return RedirectToAction("Index");
        }
        else
        {
            var propietario = repo.ObtenerPorID(id);
            if (propietario == null)
            {
                TempData["Mensaje"] = "Propietario no encontrado.";
                return RedirectToAction("Index");
            }

            return View(propietario);
        }
    }
[HttpPost]
public IActionResult Actualizar(Propietario actualizarPropietario)
{
if (ModelState.IsValid)
    {
        repo.ActualizarPropietario(actualizarPropietario);
        TempData["Mensaje"] = "Propietario Modificado correctamente.";
        return RedirectToAction("Index");
    }
TempData["Mensaje"] = "Hubo un error al Modificar el Propietario.";
return RedirectToAction("Index");
}
}