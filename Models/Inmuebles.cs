namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class Inmuebles
{
    public int Id_inmueble { get; set; }
    public int Id_propietario { get; set; }
    public string EmailPropietario { get; set; } = "";
    public string Uso { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Direccion { get; set; } = "";
    public int Ambiente { get; set; }
    public decimal Precio { get; set; }
    public double Longitud { get; set; }
    public double Latitud { get; set; }
    public DateTime Fecha_creacion { get; set; }
    public string? Portada { get; set; }
    public IFormFile? PortadaFile { get; set; }

    public Boolean Estado { get; set; } 

}