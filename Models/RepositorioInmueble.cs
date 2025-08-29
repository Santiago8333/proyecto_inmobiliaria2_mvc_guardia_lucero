using MySql.Data.MySqlClient;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class RepositorioInmueble : RepositorioBase
{

    //private readonly string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_guardia_lucero;SslMode=none";
    public RepositorioInmueble(IConfiguration configuration) : base(configuration)
    {

    }
    public List<Inmuebles> ObtenerPaginados(int pagina, int tamanoPagina)
    {
        List<Inmuebles> propietarios = new List<Inmuebles>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT Id_inmueble,Id_propietario, Uso, Tipo, Ambiente,Precio,Longitud, Latitud,Fecha_creacion, Estado
                      FROM inmuebles
                      ORDER BY Id_inmueble
                      LIMIT @limit OFFSET @offset";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@limit", tamanoPagina);
                command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    propietarios.Add(new Inmuebles
                    {
                        Id_inmueble = reader.GetInt32(nameof(Inmuebles.Id_inmueble)),
                        Id_propietario = reader.GetInt32(nameof(Inmuebles.Id_propietario)),
                        Uso = reader.GetString(nameof(Inmuebles.Uso)),
                        Tipo = reader.GetString(nameof(Inmuebles.Tipo)),
                        Ambiente = reader.GetInt32(nameof(Inmuebles.Ambiente)),
                        Precio = reader.GetDecimal(nameof(Inmuebles.Precio)),
                        Longitud = reader.GetFloat(nameof(Inmuebles.Longitud)),
                        Latitud = reader.GetFloat(nameof(Inmuebles.Latitud)),
                        Fecha_creacion = reader.GetDateTime(nameof(Inmuebles.Fecha_creacion)),
                        Estado = reader.GetBoolean(nameof(Inmuebles.Estado))
                    });
                }
            }
        }
        return propietarios;
    }
     public int ContarInmuebles()
    {
        using (var connection = new MySqlConnection(ConectionString))
        {
            var query = "SELECT COUNT(*) FROM inmuebles";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}