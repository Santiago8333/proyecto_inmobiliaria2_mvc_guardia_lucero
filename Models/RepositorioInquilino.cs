using MySql.Data.MySqlClient;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class RepositorioInquilino
{
    private readonly string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_guardia_lucero;SslMode=none";

    /*
        public List<Inquilinos> ObtenerPaginados(string email,int pagina, int tamanoPagina)
        {
            List<Inquilinos> inquilinos = new List<Inquilinos>();
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = @"SELECT Id_inquilino, Dni, Apellido, Nombre, Email, Telefono,Creado_por, Fecha_creacion,Estado
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
                        inquilinos.Add(new Inquilinos
                        {
                            Id_inquilino = reader.GetInt32(nameof(Inquilinos.Id_inquilino)),
                            Dni = reader.GetString(nameof(Inquilinos.Dni)),
                            Apellido = reader.GetString(nameof(Inquilinos.Apellido)),
                            Nombre = reader.GetString(nameof(Inquilinos.Nombre)),
                            Email = reader.GetString(nameof(Inquilinos.Email)),
                            Telefono = reader.GetString(nameof(Inquilinos.Telefono)),
                            Creado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Inquilinos.Creado_por)))
                                ? null
                                : reader.GetString(nameof(Inquilinos.Creado_por)),
                            Fecha_creacion = reader.GetDateTime(nameof(Inquilinos.Fecha_creacion)),
                            Estado = reader.GetBoolean(nameof(Inquilinos.Estado))
                        });
                    }
                }
            }
            return inquilinos;
        }
        */
public List<Inquilinos> ObtenerPaginados(string? email, int pagina, int tamanoPagina)
{
    List<Inquilinos> inquilinos = new List<Inquilinos>();
    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        
        var sqlBuilder = new System.Text.StringBuilder(@"
            SELECT Id_inquilino, Dni, Apellido, Nombre, Email, Telefono,
                   Creado_por, Fecha_creacion, Estado
            FROM inquilinos");

        using (MySqlCommand command = new MySqlCommand())
        {
            
            if (!string.IsNullOrEmpty(email))
            {
               
                sqlBuilder.Append(" WHERE Email LIKE @email");
                command.Parameters.AddWithValue("@email", $"%{email}%"); 
            }

            
            sqlBuilder.Append(" ORDER BY Id_inquilino LIMIT @limit OFFSET @offset");
            command.Parameters.AddWithValue("@limit", tamanoPagina);
            command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);
            
            command.CommandText = sqlBuilder.ToString();
            command.Connection = connection;

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    inquilinos.Add(new Inquilinos
                    {
                        Id_inquilino = reader.GetInt32(nameof(Inquilinos.Id_inquilino)),
                        Dni = reader.GetString(nameof(Inquilinos.Dni)),
                        Apellido = reader.GetString(nameof(Inquilinos.Apellido)),
                        Nombre = reader.GetString(nameof(Inquilinos.Nombre)),
                        Email = reader.GetString(nameof(Inquilinos.Email)),
                        Telefono = reader.GetString(nameof(Inquilinos.Telefono)),
                        Creado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Inquilinos.Creado_por)))
                            ? null
                            : reader.GetString(nameof(Inquilinos.Creado_por)),
                        Fecha_creacion = reader.GetDateTime(nameof(Inquilinos.Fecha_creacion)),
                        Estado = reader.GetBoolean(nameof(Inquilinos.Estado))
                    });
                }
            }
        }
    }
    return inquilinos;
}
    /*
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
    */
public int ContarFiltrados(string? email)
{
    using (var connection = new MySqlConnection(ConectionString))
    {
        var sqlBuilder = new System.Text.StringBuilder("SELECT COUNT(*) FROM inquilinos");
        using (var command = new MySqlCommand())
        {
            if (!string.IsNullOrEmpty(email))
            {
                sqlBuilder.Append(" WHERE Email LIKE @email");
                command.Parameters.AddWithValue("@email", $"%{email}%");
            }

            command.CommandText = sqlBuilder.ToString();
            command.Connection = connection;
            connection.Open();
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
    public void AgregarInquilino(Inquilinos nuevoInquilino)
    {

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"INSERT INTO inquilinos ({nameof(Inquilinos.Dni)}, {nameof(Inquilinos.Apellido)}, {nameof(Inquilinos.Nombre)}, {nameof(Inquilinos.Email)}, {nameof(Inquilinos.Telefono)},{nameof(Inquilinos.Creado_por)})
                    VALUES (@Dni, @Apellido, @Nombre,@Email,@Telefono,@Creado_por)";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega los parámetros
                command.Parameters.AddWithValue("@Dni", nuevoInquilino.Dni);
                command.Parameters.AddWithValue("@Apellido", nuevoInquilino.Apellido);
                command.Parameters.AddWithValue("@Nombre", nuevoInquilino.Nombre);
                command.Parameters.AddWithValue("@Email", nuevoInquilino.Email);
                command.Parameters.AddWithValue("@Telefono", nuevoInquilino.Telefono);
                command.Parameters.AddWithValue("@Creado_por", nuevoInquilino.Creado_por);

                connection.Open();
                command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
                connection.Close();
            }
        }
    }
    public void EliminarInquilino(int id)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = "DELETE FROM inquilinos WHERE Id_inquilino = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
    public Inquilinos? ObtenerPorID(int id)
    {
        Inquilinos? res = null;

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT Id_inquilino, Dni, Apellido, Nombre, Email, Telefono
                      FROM inquilinos
                      WHERE Id_inquilino = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega el parámetro id
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Inquilinos
                        {
                            Id_inquilino = reader.GetInt32(nameof(Inquilinos.Id_inquilino)),
                            Dni = reader.GetString(nameof(Inquilinos.Dni)),
                            Apellido = reader.GetString(nameof(Inquilinos.Apellido)),
                            Nombre = reader.GetString(nameof(Inquilinos.Nombre)),
                            Email = reader.GetString(nameof(Inquilinos.Email)),
                            Telefono = reader.GetString(nameof(Inquilinos.Telefono))
                        };
                    }
                }
            }
        }

        return res; // Retorna el propietario o null si no se encontró
    }
    public void ActualizarInquilino(Inquilinos actualizarInquilino)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"UPDATE inquilinos
               SET 
                   {nameof(Inquilinos.Dni)} = @Dni, 
                   {nameof(Inquilinos.Apellido)} = @Apellido, 
                   {nameof(Inquilinos.Nombre)} = @Nombre, 
                   {nameof(Inquilinos.Email)} = @Email, 
                   {nameof(Inquilinos.Telefono)} = @Telefono
               WHERE Id_inquilino = @Id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega los parámetros
                command.Parameters.AddWithValue("@Dni", actualizarInquilino.Dni);
                command.Parameters.AddWithValue("@Apellido", actualizarInquilino.Apellido);
                command.Parameters.AddWithValue("@Nombre", actualizarInquilino.Nombre);
                command.Parameters.AddWithValue("@Email", actualizarInquilino.Email);
                command.Parameters.AddWithValue("@Telefono", actualizarInquilino.Telefono);
                command.Parameters.AddWithValue("@Id", actualizarInquilino.Id_inquilino);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
    public Inquilinos? ObtenerPorEmail(string email)
    {
        Inquilinos? res = null;

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT Id_inquilino, Apellido, Nombre, Email, Telefono 
                      FROM inquilinos 
                      WHERE Email = @Email";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Inquilinos
                        {
                            Id_inquilino = reader.GetInt32(reader.GetOrdinal("Id_inquilino")),
                            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Telefono = reader.GetString(reader.GetOrdinal("Telefono"))
                        };
                    }
                }
            }
        }

        return res; // Retorna el propietario o null si no se encontró
    }
    public List<Inquilinos> BuscarPorEmail(string emailParcial)
    {
        var lista = new List<Inquilinos>();

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT Id_inquilino, Nombre,Apellido, Email, Telefono,Dni,Fecha_creacion
                            FROM inquilinos
                            WHERE Email LIKE @Email";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("@Email", $"%{emailParcial}%");

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Inquilinos
                        {
                            Id_inquilino = reader.GetInt32(reader.GetOrdinal("Id_inquilino")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                            Dni = reader.GetString(reader.GetOrdinal("Dni")),
                            Fecha_creacion = reader.GetDateTime(reader.GetOrdinal("Fecha_creacion"))
                        });
                    }
                }
            }
        }

        return lista;
    }
            public void DesactivarInquilino(Inquilinos actualizarInquilino)
        {
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = @"UPDATE inquilinos
                      SET Estado = 0
                      WHERE Id_inquilino = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", actualizarInquilino.Id_inquilino);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void ActivarInquilino(Inquilinos actualizarInquilino)
        {
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = @"UPDATE inquilinos
                      SET Estado = 1
                      WHERE Id_inquilino = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", actualizarInquilino.Id_inquilino);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
}