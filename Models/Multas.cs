namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class Multas
{
    public int Id_multa { get; set; }
    public int Id_contrato { get; set; }
    public string Razon_multa { get; set; } = "";
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }

}