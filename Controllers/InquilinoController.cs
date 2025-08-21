using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;

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

        return View(listaInquilinos);
    }













}