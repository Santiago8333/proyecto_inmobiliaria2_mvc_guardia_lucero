using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;

public class ContratoController : Controller
{
    private readonly RepositorioContrato repo;
    private readonly IConfiguration config;
    public ContratoController(RepositorioContrato repositorio, IConfiguration config)
    {
        this.repo = repositorio;
        this.config = config;
    }
    public IActionResult Index(int pagina = 1, int tamanoPagina = 5)
    {
        var listaContratos = repo.ObtenerPaginados(pagina, tamanoPagina);

        int totalRegistros = repo.ContarContrato();
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);
        ViewBag.Registros = totalRegistros > 0;
        return View(listaContratos);
    }
    [HttpPost]
    public ActionResult Agregar(Contratos contrato)
    {

        if (ModelState.IsValid)
        {

            TempData["Mensaje"] = "Contrato agregado exitosamente.";
            contrato.Monto_a_pagar = contrato.Monto_total;
            repo.AgregarContrato(contrato);
            return RedirectToAction("Index");

        }
        TempData["Mensaje"] = "Error al agregar.";
        return RedirectToAction("Index");
    }

    public IActionResult Eliminar(int id)
    {
        var propietario = repo.BuscarPorId(id);
        if (propietario == null)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        repo.EliminarContrato(id);
        TempData["Mensaje"] = "Contrato eliminado.";
        return RedirectToAction("Index");

    }
    public IActionResult Detalle(int id)
    {
        var contrato = repo.BuscarPorId(id);
        return View(contrato);
    }
    public IActionResult Edicion(int id)
    {
        if (id == 0)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        else
        {

            var contrato = repo.BuscarPorId(id);
            if (contrato == null)
            {
                TempData["Mensaje"] = "Contrato no encontrado.";
                return RedirectToAction("Index");
            }

            return View(contrato);
        }
    }
    /*
    public IActionResult Pago(int id, int pagina = 1, int tamanoPagina = 5)
    {
        if (id == 0)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        var contrato = repo.BuscarPorId(id);
        if (contrato == null)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        var listaPagos = repo.ObtenerPagosPaginados(id, pagina, tamanoPagina);

        int totalRegistros = repo.ContarPagos(id);
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);
        ViewBag.Registros = totalRegistros > 0;
        ViewBag.contrato = contrato;
        return View(listaPagos);


    }
    */
    // Recibimos parámetros para ambas paginaciones, con valores por defecto
public IActionResult Pago(int id, int paginaPagos = 1, int paginaMultas = 1)
{
    var contrato = repo.BuscarPorId(id);
    if (contrato == null)
    {
        TempData["Mensaje"] = "Contrato no encontrado.";
        return RedirectToAction("Index");
    }

    int tamanoPagina = 5; 

    
    var viewModel = new ContratoDetalleViewModel
    {
        ContratoActual = contrato,
        
        
        ListaDePagos = repo.ObtenerPagosPaginados(id, paginaPagos, tamanoPagina),
        PaginaActualPagos = paginaPagos,
        TotalPaginasPagos = (int)Math.Ceiling((double)repo.ContarPagos(id) / tamanoPagina),

       
        ListaDeMultas = repo.ObtenerMultasPaginados(id, paginaMultas, tamanoPagina),
        PaginaActualMultas = paginaMultas,
        TotalPaginasMultas = (int)Math.Ceiling((double)repo.ContarMultas(id) / tamanoPagina)
    };
    
        
    
    return View(viewModel);
}
    [HttpPost]
    public ActionResult AgregarPago(Pagos pago)
    {

        if (ModelState.IsValid)
        {
            var contrato = repo.BuscarPorId(pago.Id_contrato);
            if (contrato == null)
            {
                TempData["Mensaje"] = "Contrato no encontrado.";
                return RedirectToAction("Index");
            }
            if (contrato.Monto_a_pagar > 0)
            {
                if (0 == contrato.Monto_a_pagar - pago.Monto)
                {
                    repo.ActualizarContratoCompletado(pago.Id_contrato);
                }
                TempData["Mensaje"] = "Pago agregado exitosamente.";
                repo.AgregarPago(pago);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Mensaje"] = "este contrato ya esta pagado.";
                return RedirectToAction("Index");
            }
        }
        TempData["Mensaje"] = "Error al agregar.";
        return RedirectToAction("Index");
    }
    
    public IActionResult AnularPago(int Id_pago, int Id_contrato)
    {
        var pago = repo.BuscarPagoPorId(Id_pago);
        if (pago == null)
        {
            TempData["Mensaje"] = "Pago no encontrado.";
            return RedirectToAction("Index");
        }
        repo.AnularPago(Id_pago, Id_contrato, pago.Monto);
        TempData["Mensaje"] = "Pago Anulado.";
        return RedirectToAction("Index");
    }
    public IActionResult EliminarPago(int id)
    {
        var pago = repo.BuscarPagoPorId(id);
        if (pago == null)
        {
            TempData["Mensaje"] = "Pago no encontrado.";
            return RedirectToAction("Index");
        }
        repo.EliminarPago(id);
        TempData["Mensaje"] = "Pago eliminado.";
        return RedirectToAction("Index");

    }
}