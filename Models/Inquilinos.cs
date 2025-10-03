namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class Inquilinos
{
    public int Id_inquilino {get;set;}
    public string Dni {get;set;} = "";

    public string Apellido {get;set;} = "";

    public string Nombre {get;set;} = "";

    public string Email {get;set;} = "";

    public string Telefono {get;set;} = "";
    public string? Creado_por { get; set; } = "";
    public DateTime Fecha_creacion { get; set; }

}