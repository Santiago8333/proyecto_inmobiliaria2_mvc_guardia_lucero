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
            var query = @"SELECT i.Id_inmueble,
                            i.Id_propietario,
                            p.Email,
                            i.Uso,
                            i.Tipo,
                            i.Ambiente,
                            i.Precio,
                            i.Longitud,
                            i.Latitud,
                            i.Fecha_creacion,
                            i.Estado
                        FROM inmuebles i
                        JOIN propietarios p ON i.Id_propietario = p.Id_propietario
                        ORDER BY i.Id_inmueble
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
                        EmailPropietario = reader.GetString("Email"),
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
    public List<Propietarios> ObtenerTodos()
    {
        List<Propietarios> propietarios = new List<Propietarios>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"SELECT {nameof(Propietarios.Id_propietario)}, {nameof(Propietarios.Dni)}, {nameof(Propietarios.Apellido)}, {nameof(Propietarios.Nombre)}, {nameof(Propietarios.Email)}, {nameof(Propietarios.Telefono)}, {nameof(Propietarios.Fecha_creacion)}
                      FROM propietarios WHERE 1";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    propietarios.Add(new Propietarios
                    {
                        Id_propietario = reader.GetInt32(nameof(Propietarios.Id_propietario)),
                        Dni = reader.GetString(nameof(Propietarios.Dni)),
                        Apellido = reader.GetString(nameof(Propietarios.Apellido)),
                        Nombre = reader.GetString(nameof(Propietarios.Nombre)),
                        Email = reader.GetString(nameof(Propietarios.Email)),
                        Telefono = reader.GetString(nameof(Propietarios.Telefono)),
                        Fecha_creacion = reader.GetDateTime(nameof(Propietarios.Fecha_creacion))

                    });
                }
                connection.Close();
            }
            return propietarios;
        }
    }
    public void AgregarInmueble(Inmuebles nuevoInmuebles)
    {

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"INSERT INTO inmuebles ({nameof(Inmuebles.Id_propietario)}, {nameof(Inmuebles.Uso)}, {nameof(Inmuebles.Tipo)}, {nameof(Inmuebles.Ambiente)}, {nameof(Inmuebles.Precio)}, {nameof(Inmuebles.Longitud)}, {nameof(Inmuebles.Latitud)})
                    VALUES (@Id_propietario, @Uso, @Tipo,@Ambiente,@Precio,@Longitud,@Latitud)";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega los parámetros
                command.Parameters.AddWithValue("@Id_propietario", nuevoInmuebles.Id_propietario);
                command.Parameters.AddWithValue("@Uso", nuevoInmuebles.Uso);
                command.Parameters.AddWithValue("@Tipo", nuevoInmuebles.Tipo);
                command.Parameters.AddWithValue("@Ambiente", nuevoInmuebles.Ambiente);
                command.Parameters.AddWithValue("@Precio", nuevoInmuebles.Precio);
                command.Parameters.AddWithValue("@Longitud", nuevoInmuebles.Longitud);
                command.Parameters.AddWithValue("@Latitud", nuevoInmuebles.Latitud);

                connection.Open();
                command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
                connection.Close();
            }
        }
    }
}