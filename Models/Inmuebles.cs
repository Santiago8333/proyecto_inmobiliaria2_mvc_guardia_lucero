namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class Inmuebles
{
    public int Id_inmueble { get; set; }
    public int Id_propietario { get; set; }
    public string EmailPropietario { get; set; } = "";
    public string Uso { get; set; } = "";
    public string Tipo { get; set; } = "";
    public int Ambiente { get; set; }
    public decimal Precio { get; set; }
    public float Longitud { get; set; }
    public float Latitud { get; set; }
    public DateTime Fecha_creacion { get; set; }
    public Boolean Estado { get; set; } 

}