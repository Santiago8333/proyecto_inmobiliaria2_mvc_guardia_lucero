using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        ViewBag.Propietarios = repo.ObtenerTodos();
        return View(listaInquilinos);
    }
    
}