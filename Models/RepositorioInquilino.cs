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
    public void AgregarInquilino(Inquilinos nuevoInquilino)
    {

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"INSERT INTO inquilinos ({nameof(Inquilinos.Dni)}, {nameof(Inquilinos.Apellido)}, {nameof(Inquilinos.Nombre)}, {nameof(Inquilinos.Email)}, {nameof(Inquilinos.Telefono)})
                    VALUES (@Dni, @Apellido, @Nombre,@Email,@Telefono)";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega los parámetros
                command.Parameters.AddWithValue("@Dni", nuevoInquilino.Dni);
                command.Parameters.AddWithValue("@Apellido", nuevoInquilino.Apellido);
                command.Parameters.AddWithValue("@Nombre", nuevoInquilino.Nombre);
                command.Parameters.AddWithValue("@Email", nuevoInquilino.Email);
                command.Parameters.AddWithValue("@Telefono", nuevoInquilino.Telefono);


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
                            Id_Inquilino = reader.GetInt32(nameof(Inquilinos.Id_Inquilino)),
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
                command.Parameters.AddWithValue("@Id", actualizarInquilino.Id_Inquilino);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}