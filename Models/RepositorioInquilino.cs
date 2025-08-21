using MySql.Data.MySqlClient;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class RepositorioInquilino
{
    private readonly string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_guardia_lucero;SslMode=none";


    public List<Inquilinos> ObtenerPaginados(int pagina, int tamanoPagina)
    {
        List<Inquilinos> propietarios = new List<Inquilinos>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT Id_inquilino, Dni, Apellido, Nombre, Email, Telefono, Fecha_creacion
                      FROM inquilinos
                      ORDER BY Id_inquilino
                      LIMIT @limit OFFSET @offset";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@limit", tamanoPagina);
                command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    propietarios.Add(new Inquilinos
                    {
                        Id_Inquilino = reader.GetInt32(nameof(Inquilinos.Id_Inquilino)),
                        Dni = reader.GetString(nameof(Inquilinos.Dni)),
                        Apellido = reader.GetString(nameof(Inquilinos.Apellido)),
                        Nombre = reader.GetString(nameof(Inquilinos.Nombre)),
                        Email = reader.GetString(nameof(Inquilinos.Email)),
                        Telefono = reader.GetString(nameof(Inquilinos.Telefono)),
                        Fecha_creacion = reader.GetDateTime(nameof(Inquilinos.Fecha_creacion)),
                    });
                }
            }
        }
        return propietarios;
    }
    public int ContarInquilinos()
    {
        using (var connection = new MySqlConnection(ConectionString))
        {
            var query = "SELECT COUNT(*) FROM inquilinos";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
   
}