using MySql.Data.MySqlClient;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class RepositorioPropietario
{
    private readonly string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_guardia_lucero;SslMode=none";


    public List<Propietario> ObtenerTodos()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"SELECT {nameof(Propietario.Id_propietario)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Email)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Fecha_creacion)})
                      FROM propietarios WHERE 1";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    propietarios.Add(new Propietario
                    {
                        Id_propietario = reader.GetInt32(nameof(Propietario.Id_propietario)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
                        Fecha_creacion = reader.GetDateTime(nameof(Propietario.Fecha_creacion))
                        
                    });
                }
                connection.Close();
            }
            return propietarios;
        }
    }
    public void AgregarPropietario(Propietario nuevoPropietario)
    {

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"INSERT INTO propietarios ({nameof(Propietario.Dni)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Email)}, {nameof(Propietario.Telefono)})
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

    public List<Propietario> ObtenerPaginados(int pagina, int tamanoPagina)
    {
        List<Propietario> propietarios = new List<Propietario>();
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
                    propietarios.Add(new Propietario
                    {
                        Id_propietario = reader.GetInt32(nameof(Propietario.Id_propietario)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
                        Fecha_creacion = reader.GetDateTime(nameof(Propietario.Fecha_creacion)),
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
    public Propietario? ObtenerPorID(int id)
    {
        Propietario? res = null;

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
                        res = new Propietario
                        {
                            Id_propietario = reader.GetInt32(nameof(Propietario.Id_propietario)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono))
                        };
                    }
                }
            }
        }

        return res; // Retorna el propietario o null si no se encontró
    }
    public void ActualizarPropietario(Propietario actualizarPropietario)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"UPDATE propietarios 
               SET 
                   {nameof(Propietario.Dni)} = @Dni, 
                   {nameof(Propietario.Apellido)} = @Apellido, 
                   {nameof(Propietario.Nombre)} = @Nombre, 
                   {nameof(Propietario.Email)} = @Email, 
                   {nameof(Propietario.Telefono)} = @Telefono
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
            //var query = "DELETE FROM propietarios WHERE Id_propietarios = @Id";
            var query = $@"DELETE propietarios
                       WHERE Id_propietario = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}