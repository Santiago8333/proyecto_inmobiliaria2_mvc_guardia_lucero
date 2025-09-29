using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;
using Microsoft.AspNetCore.Authorization;
namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;
[Authorize]
public class InquilinoController : Controller
{ 
 private readonly ILogger<InquilinoController> _logger;
    private RepositorioInquilino repo;
    public InquilinoController(ILogger<InquilinoController> logger)
    {
        _logger = logger;
        repo = new RepositorioInquilino();

    }
    public IActionResult Index(int pagina = 1, int tamanoPagina = 5)
    {
        var listaInquilinos = repo.ObtenerPaginados(pagina, tamanoPagina);

        int totalRegistros = repo.ContarInquilinos();
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);
        ViewBag.Registros = totalRegistros > 0;
        return View(listaInquilinos);
    }

     [HttpPost]
    public ActionResult Agregar(Inquilinos inquilino)
    {
        if (ModelState.IsValid)
        {
            var inquilinoExistente = repo.ObtenerPorEmail(inquilino.Email);
            if (inquilinoExistente != null)
            {
                TempData["Mensaje"] = "ya hay un Inquilino Registrado con ese email.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Mensaje"] = "Inquilino agregado exitosamente.";
                repo.AgregarInquilino(inquilino);
                return RedirectToAction("Index");
            }
        }
        TempData["Mensaje"] = "Error al agregar.";
        return View(inquilino);
    }

public IActionResult Eliminar(int id)
{
  var inquilino = repo.ObtenerPorID(id);
        if (inquilino == null)
        {
            TempData["Mensaje"] = "Inquilino no encontrado.";
            return RedirectToAction("Index");
        }
        repo.EliminarInquilino(id);
        TempData["Mensaje"] = "Inquilino eliminado.";
        return RedirectToAction("Index");
    
}
    [HttpPost]
    public IActionResult Actualizar(Inquilinos actualizarInquilino)
    {
        if (ModelState.IsValid)
        {
            repo.ActualizarInquilino(actualizarInquilino);
            TempData["Mensaje"] = "Inquilino Modificado correctamente.";
            return RedirectToAction("Index");

        }
        TempData["Mensaje"] = "Hubo un error al Modificar el Inquilino.";
        return RedirectToAction("Index");
}

    public IActionResult Edicion(int id)
    {
        if (id == 0)
        {
            TempData["Mensaje"] = "Inquilino no encontrado.";
            return RedirectToAction("Index");
        }
        else
        {
            var inquilino = repo.ObtenerPorID(id);
            if (inquilino == null)
            {
                TempData["Mensaje"] = "Inquilino no encontrado.";
                return RedirectToAction("Index");
            }

            return View(inquilino);
        }
    }

[HttpGet]
public IActionResult Buscar(string term)
{
    var resultados = repo.BuscarPorEmail(term ?? "");
    return Json(resultados);
}



}