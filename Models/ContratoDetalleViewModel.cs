namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;
public class ContratoDetalleViewModel
{
    public Contratos ContratoActual { get; set; }

    // Datos de Pagos
    public List<Pagos> ListaDePagos { get; set; }
    public int PaginaActualPagos { get; set; }
    public int TotalPaginasPagos { get; set; }

    // Datos de Multas
    public List<Multas> ListaDeMultas { get; set; }
    public int PaginaActualMultas { get; set; }
    public int TotalPaginasMultas { get; set; }
}