using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;
using Microsoft.AspNetCore.Authorization;
namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;

[Authorize]
public class PropietarioController : Controller
{

    private readonly RepositorioPropietario repo;
    private readonly IConfiguration config;
    public PropietarioController(RepositorioPropietario repositorio, IConfiguration config)
    {

        this.repo = repositorio;
        this.config = config;
    }
    public IActionResult Index(string? email,int pagina = 1, int tamanoPagina = 5)
    {
         ViewBag.EmailFilter = email;
        var listaPropietarios = repo.ObtenerPaginados(email,pagina, tamanoPagina);

        int totalRegistros = repo.ContarFiltrados(email);
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);
        ViewBag.Registros = totalRegistros > 0;
        return View(listaPropietarios);
    }

    public IActionResult Detalle(int id)
    {

        return View();
    }
    [HttpPost]
    public ActionResult Agregar(Propietarios propietario)
    {
        if (ModelState.IsValid)
        {
            var propietarioExistente = repo.ObtenerPorEmail(propietario.Email);
            if (propietarioExistente != null)
            {
                TempData["Mensaje"] = "ya hay un Propietario Registrado con ese email.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Mensaje"] = "Propietario agregado exitosamente.";
                propietario.Creado_por = User.Identity?.Name ?? "Sistema";
                repo.AgregarPropietario(propietario);
                return RedirectToAction("Index");
            }
        }
        TempData["Mensaje"] = "Error al agregar.";
        return RedirectToAction("Index");
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
    public IActionResult Actualizar(Propietarios actualizarPropietario)
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
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        var propietario = repo.ObtenerPorID(id);
        if (propietario == null)
        {
            TempData["Mensaje"] = "Propietario no encontrado.";
            return RedirectToAction("Index");
        }
        repo.EliminarPropietario(id);
        TempData["Mensaje"] = "Propietario eliminado.";
        return RedirectToAction("Index");

    }
    [HttpGet]
    public IActionResult Buscar(string term)
    {
        var resultados = repo.BuscarPorEmail(term ?? "");
        return Json(resultados);
    }
        public IActionResult Desactivar(int id)
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
            repo.DesactivarPropietario(propietario);
            TempData["Mensaje"] = "Propietario desactivado.";
            return RedirectToAction("Index");
        }
    }
    public IActionResult Activar(int id)
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
            repo.ActivarPropietario(propietario);
            TempData["Mensaje"] = "Propietario activar.";
            return RedirectToAction("Index");
        }
    }
}