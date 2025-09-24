using MySql.Data.MySqlClient;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models
{
    public class RepositorioUsuario : RepositorioBase
    { 
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {

        }

public List<Usuarios> ObtenerPaginados(int pagina, int tamanoPagina)
{
    List<Usuarios> usuarios = new List<Usuarios>();
    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT Id_usuario, Nombre, Apellido, Email, AvatarUrl, Rol, RolNombre, Estado
                FROM usuarios
                ORDER BY Id_usuario
                LIMIT @limit OFFSET @offset";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@limit", tamanoPagina);
            command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);

            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                usuarios.Add(new Usuarios
                {
                    Id_usuario = reader.GetInt32(nameof(Usuarios.Id_usuario)),
                    Nombre = reader.GetString(nameof(Usuarios.Nombre)),
                    Apellido = reader.GetString(nameof(Usuarios.Apellido)),
                    Email = reader.GetString(nameof(Usuarios.Email)),
                    AvatarUrl = reader.GetString(nameof(Usuarios.AvatarUrl)),
                    Rol = reader.GetInt32(nameof(Usuarios.RolNombre)),
                    Estado = reader.GetBoolean(nameof(Usuarios.Estado))
                });
            }
        }
    }
    return usuarios;
}

public int ContarUsuarios()
{
    using (var connection = new MySqlConnection(ConectionString))
    {
        var query = "SELECT COUNT(*) FROM usuarios";
        using (var command = new MySqlCommand(query, connection))
        {
            connection.Open();
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}


public async Task<Usuarios?> ObtenerPorEmailAsync(string email)
{
    Usuarios? res = null;

    using (var connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT Id_usuario, Nombre, Apellido, Email, RolNombre, Clave , AvatarUrl
                      FROM usuarios
                      WHERE Email = @Email AND Estado = true";

        using (var command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro Email
            command.Parameters.AddWithValue("@Email", email);

            await connection.OpenAsync(); // Abre la conexión de manera asincrónica

            using (var reader = await command.ExecuteReaderAsync()) // Ejecuta el lector asincrónicamente
            {
                if (await reader.ReadAsync()) // Lee de forma asincrónica
                {
                    res = new Usuarios
                    {
                        Id_usuario = reader.GetInt32(reader.GetOrdinal("Id_usuario")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        RolNombre = reader.GetString(reader.GetOrdinal("RolNombre")),
                        Clave = reader.GetString(reader.GetOrdinal("Clave")),
                        AvatarUrl = reader.GetString(reader.GetOrdinal("AvatarUrl"))
                    };
                }
            }
        }
    }

    return res; // Retorna el usuario o null si no se encontró
}


public void AgregarUsuario(Usuarios nuevoUsuario)
{
     using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"INSERT INTO usuarios ({nameof(Usuarios.Nombre)}, {nameof(Usuarios.Apellido)}, {nameof(Usuarios.Email)}, {nameof(Usuarios.Clave)}, {nameof(Usuarios.Rol)}, {nameof(Usuarios.RolNombre)},{nameof(Usuarios.Estado)},{nameof(Usuarios.AvatarUrl)})
                    VALUES (@Nombre, @Apellido, @Email,@Clave,@Rol, @RolNombre, @Estado,@AvatarFile)";
         using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            
            command.Parameters.AddWithValue("@Nombre", nuevoUsuario.Nombre);
            command.Parameters.AddWithValue("@Apellido", nuevoUsuario.Apellido);
            command.Parameters.AddWithValue("@Email", nuevoUsuario.Email);
            command.Parameters.AddWithValue("@Clave", nuevoUsuario.Clave);
            command.Parameters.AddWithValue("@Rol", nuevoUsuario.Rol);
            command.Parameters.AddWithValue("@RolNombre", nuevoUsuario.RolNombre);
            command.Parameters.AddWithValue("@AvatarFile",nuevoUsuario.AvatarUrl);
            command.Parameters.AddWithValue("@Estado", true); 

            connection.Open();
            command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
            connection.Close();
        }
    }
}






    }
}