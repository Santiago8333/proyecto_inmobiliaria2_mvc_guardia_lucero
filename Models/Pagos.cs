namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class Pagos
{
    public int Id_pago { get; set; }
    public int Id_contrato { get; set; }
    public string Detalle { get; set; } = "";
    public decimal Monto { get; set; }
    public string Creado_por { get; set; } = "";
    public string Anulado_por { get; set; } = "";
    public Boolean Estado { get; set; }
 }