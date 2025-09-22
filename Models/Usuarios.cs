namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class Usuarios
{
    public int Id_usuario { get; set; }
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    public string Email { get; set; } = "";
    public string? AvatarUrl;
    public IFormFile? AvatarFile { get; set; }
    public int Rol { get; set; }
    public string RolNombre { get; set; } = "";
    public Boolean Estado { get; set; }

}