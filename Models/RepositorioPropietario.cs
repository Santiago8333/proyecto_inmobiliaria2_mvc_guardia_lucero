using MySql.Data.MySqlClient;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class RepositorioPropietario
{
    private readonly string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_guardia_lucero;SslMode=none";


    public List<Propietarios> ObtenerTodos()
    {
        List<Propietarios> propietarios = new List<Propietarios>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"SELECT {nameof(Propietarios.Id_propietario)}, {nameof(Propietarios.Dni)}, {nameof(Propietarios.Apellido)}, {nameof(Propietarios.Nombre)}, {nameof(Propietarios.Email)}, {nameof(Propietarios.Telefono)}, {nameof(Propietarios.Fecha_creacion)})
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
    public void AgregarPropietario(Propietarios nuevoPropietario)
    {

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"INSERT INTO propietarios ({nameof(Propietarios.Dni)}, {nameof(Propietarios.Apellido)}, {nameof(Propietarios.Nombre)}, {nameof(Propietarios.Email)}, {nameof(Propietarios.Telefono)})
                    VALUES (@Dni, @Apellido, @Nombre,@Email,@Telefono)";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega los parámetros
                command.Parameters.AddWithValue("@Dni", nuevoPropietario.Dni);
                command.Parameters.AddWithValue("@Apellido", nuevoPropietario.Apellido);
                command.Parameters.AddWithValue("@Nombre", nuevoPropietario.Nombre);
                command.Parameters.AddWithValue("@Email", nuevoPropietario.Email);
                command.Parameters.AddWithValue("@Telefono", nuevoPropietario.Telefono);
                

                connection.Open();
                command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
                connection.Close();
            }
        }
    }

    public List<Propietarios> ObtenerPaginados(int pagina, int tamanoPagina)
    {
        List<Propietarios> propietarios = new List<Propietarios>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT Id_propietario, Dni, Apellido, Nombre, Email, Telefono, Fecha_creacion
                      FROM propietarios
                      ORDER BY Id_propietario
                      LIMIT @limit OFFSET @offset";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@limit", tamanoPagina);
                command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);

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
                        Fecha_creacion = reader.GetDateTime(nameof(Propietarios.Fecha_creacion)),
                    });
                }
            }
        }
        return propietarios;
    }
    public int ContarPropietarios()
    {
        using (var connection = new MySqlConnection(ConectionString))
        {
            var query = "SELECT COUNT(*) FROM propietarios";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
    public Propietarios? ObtenerPorID(int id)
    {
        Propietarios? res = null;

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT Id_propietario, Dni, Apellido, Nombre, Email, Telefono
                      FROM propietarios 
                      WHERE Id_propietario = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega el parámetro id
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Propietarios
                        {
                            Id_propietario = reader.GetInt32(nameof(Propietarios.Id_propietario)),
                            Dni = reader.GetString(nameof(Propietarios.Dni)),
                            Apellido = reader.GetString(nameof(Propietarios.Apellido)),
                            Nombre = reader.GetString(nameof(Propietarios.Nombre)),
                            Email = reader.GetString(nameof(Propietarios.Email)),
                            Telefono = reader.GetString(nameof(Propietarios.Telefono))
                        };
                    }
                }
            }
        }

        return res; // Retorna el propietario o null si no se encontró
    }
    public void ActualizarPropietario(Propietarios actualizarPropietario)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"UPDATE propietarios 
               SET 
                   {nameof(Propietarios.Dni)} = @Dni, 
                   {nameof(Propietarios.Apellido)} = @Apellido, 
                   {nameof(Propietarios.Nombre)} = @Nombre, 
                   {nameof(Propietarios.Email)} = @Email, 
                   {nameof(Propietarios.Telefono)} = @Telefono
               WHERE Id_propietario = @Id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega los parámetros
                command.Parameters.AddWithValue("@Dni", actualizarPropietario.Dni);
                command.Parameters.AddWithValue("@Apellido", actualizarPropietario.Apellido);
                command.Parameters.AddWithValue("@Nombre", actualizarPropietario.Nombre);
                command.Parameters.AddWithValue("@Email", actualizarPropietario.Email);
                command.Parameters.AddWithValue("@Telefono", actualizarPropietario.Telefono);
                command.Parameters.AddWithValue("@Id", actualizarPropietario.Id_propietario);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
    
public void EliminarPropietario(int id)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = "DELETE FROM propietarios WHERE Id_propietario = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}