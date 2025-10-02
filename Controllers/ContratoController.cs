using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using proyecto_inmobiliaria2_mvc_guardia_lucero.Models;
using Microsoft.AspNetCore.Authorization;
namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Controllers;

[Authorize]
public class ContratoController : Controller
{
    private readonly RepositorioContrato repo;
    private readonly RepositorioInmueble repoInmuebles;
    private readonly IConfiguration config;
    public ContratoController(RepositorioContrato repositorio, RepositorioInmueble repoInmuebles, IConfiguration config)
    {
        this.repo = repositorio;
        this.config = config;
        this.repoInmuebles = repoInmuebles;
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
            //validar si el inmueble esta activo
            var inmueble = repoInmuebles.ObtenerPorID(contrato.Id_inmueble);
            if (inmueble == null)
            {
                TempData["Mensaje"] = "El inmueble seleccionado no existe.";
                return View(contrato);
            }
            if (inmueble.Estado == false)
            {
                TempData["Mensaje"] = "El inmueble seleccionado Esta desactivado.";
                return View(contrato);
            }
            //validar que el inmueble no este ocupado dentro de Fecha_desde Fecha_hasta debe decir que esta ocupado
            var contratosExistentes = repo.ObtenerContratosActivosPorInmueble(contrato.Id_inmueble);
            foreach (var existente in contratosExistentes)
            {
                if ((contrato.Fecha_desde <= existente.Fecha_hasta) && (contrato.Fecha_hasta >= existente.Fecha_desde))
                {

                    TempData["Mensaje"] = $"El inmueble ya se encuentra alquilado entre {existente.Fecha_desde:dd/MM/yyyy} y {existente.Fecha_hasta:dd/MM/yyyy}.";
                    return RedirectToAction("Index");
                }
            }
            TempData["Mensaje"] = "Contrato agregado exitosamente.";
            contrato.Monto_a_pagar = contrato.Monto_total;
            contrato.Creado_por = User.Identity?.Name ?? "Sistema";
            repo.AgregarContrato(contrato);
            return RedirectToAction("Index");

        }
        TempData["Mensaje"] = "Error al agregar.";
        return RedirectToAction("Index");
    }
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        var contrato = repo.BuscarPorId(id);
        if (contrato == null)
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
                pago.Creado_por = User.Identity?.Name ?? "Sistema";
                repo.AgregarPago(pago);
                return RedirectToAction("Pago", "Contrato", new { id = pago.Id_contrato });
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
        pago.Anulado_por = User.Identity?.Name ?? "Sistema";
        repo.AnularPago(Id_pago, Id_contrato, pago.Monto, pago.Anulado_por);
        TempData["Mensaje"] = "Pago Anulado.";
        return RedirectToAction("Pago", "Contrato", new { id = pago.Id_contrato });
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
    public IActionResult Cancelar(int id)
    {
        var contrato = repo.BuscarPorId(id);
        var multa = new Multas();
        if (contrato == null)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        if (contrato.Contrato_completado)
        {
            TempData["Mensaje"] = "Contrato completados no se pueden cancelar.";
            return RedirectToAction("Index");
        }
        var monto_total = contrato.Monto * contrato.Meses;
        if (contrato.Estado)
        {
            if (contrato.Monto_a_pagar > (monto_total / 2))
            {
                Console.WriteLine("Se pagó menos de la mitad del contrato.");
                contrato.Meses += 2;
                contrato.Contrato_completado = false;
                contrato.Monto_a_pagar += contrato.Monto * 2;
                contrato.Monto_total = contrato.Monto * contrato.Meses;
                contrato.Estado = false;
                contrato.Fecha_final = DateTime.Now;
                contrato.Terminado_por = User.Identity?.Name ?? "Sistema";
                //logica multa
                multa.Monto = contrato.Monto * 2;
                multa.Razon_multa = "Falto Pagar Mas de la mitad del contrato.";
                repo.AnularContrato(contrato, multa);
                TempData["Mensaje"] = "Contrato Cancelado y aplicando multa.";
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("Se pagó más de la mitad del contrato.");
                contrato.Meses += 1;
                contrato.Contrato_completado = false;
                contrato.Monto_a_pagar += contrato.Monto * 1;
                contrato.Monto_total = contrato.Monto * contrato.Meses;
                contrato.Estado = false;
                contrato.Fecha_final = DateTime.Now;
                contrato.Terminado_por = User.Identity?.Name ?? "Sistema";
                //logica multa
                multa.Monto = contrato.Monto * 2;
                multa.Razon_multa = "Falto Pagar menos de la mitad del contrato.";
                repo.AnularContrato(contrato, multa);
                TempData["Mensaje"] = "Contrato Cancelado y aplicando multa.";
                return RedirectToAction("Index");
            }
        }
        TempData["Mensaje"] = "Este contrato ya esta Cancelado.";
        return RedirectToAction("Index");

    }
    public IActionResult Renovar(int id)
    {
        var contrato = repo.BuscarPorId(id);
        if (contrato == null)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        if (contrato.Estado)
        {
            TempData["Mensaje"] = "Contrato desactivado no se pueden Renovar.";
            return RedirectToAction("Index");
        }
        if (contrato.Contrato_completado)
        {
            TempData["Mensaje"] = "Contrato completados no se pueden cancelar.";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ObtenerDatosParaRenovar(int id)
    {
        var contrato = repo.BuscarPorId(id);
        if (contrato == null)
        {
            return NotFound();
        }

        return Json(new
        {
            contrato.Id_inquilino,
            contrato.Id_inmueble,
            contrato.Monto,
            contrato.Fecha_hasta
        });
    }
[HttpPost]
public IActionResult Renovar(Contratos nuevoContrato, int Id_contrato_original)
{
    if (ModelState.IsValid)
    {
       
        var contratoOriginal = repo.BuscarPorId(Id_contrato_original);
        if (contratoOriginal == null)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        if (contratoOriginal.Contrato_completado == false)
        {
                TempData["Mensaje"] = "Contrato no esta completado.";
            return RedirectToAction("Index");
        }
        // 2. Crear y guardar el nuevo contrato (renovación)
            nuevoContrato.Estado = true;
        nuevoContrato.Monto_a_pagar = nuevoContrato.Monto_total; 
        repo.AgregarContrato(nuevoContrato);

        TempData["Mensaje"] = "Contrato renovado exitosamente.";
        return RedirectToAction("Index");
    }

    TempData["Mensaje"] = "Hubo un error al renovar el contrato.";
    return RedirectToAction("Index"); // O devolver a la vista con errores
}
}