namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class Contratos
{
    public int Id_contrato { get; set; }
    public int Id_inmueble { get; set; }
    public int Id_inquilino { get; set; }
    public Boolean Contrato_completado { get; set; }
    public decimal Monto { get; set; }
    public decimal Monto_total { get; set; }
    public decimal Monto_a_pagar { get; set; }
    public DateTime Fecha_creacion { get; set; }
    public DateTime Fecha_desde { get; set; }
    public DateTime Fecha_hasta { get; set; }
    public DateTime? Fecha_final { get; set; }
    public int Meses { get; set; }
    public string Creado_por { get; set; } = "";
    public string Terminado_por { get; set; } = "";
    public string DireccionInmueble { get; set; } = "";
    public string EmailInquilino { get; set; } = "";
    public Boolean Estado { get; set; }
}