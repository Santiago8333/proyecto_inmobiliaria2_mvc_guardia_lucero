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
        // Ejemplo: mostrar un propietario específico
        return View();
    }
}