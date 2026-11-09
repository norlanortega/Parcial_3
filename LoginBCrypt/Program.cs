using System;
using System.Text;
using Microsoft.Data.SqlClient;

namespace LoginBCrypt
{
    class Program
    {
        const string CadenaConexion =
            "Server=.\\SQLEXPRESS;Database=LoginBCryptDB;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n===== SISTEMA DE LOGIN (BCrypt) =====");
                Console.WriteLine("1. Registrar usuario");
                Console.WriteLine("2. Iniciar sesión");
                Console.WriteLine("3. Ver historial de intentos");
                Console.WriteLine("0. Salir");
                Console.Write("Opción: ");

                switch (Console.ReadLine())
                {
                    case "1": RegistrarUsuario(); break;
                    case "2": IniciarSesion(); break;
                    case "3": VerIntentos(); break;
                    case "0": return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
        }

        // Generar el hash y guardarlo en la BD (5 pts)
        static void RegistrarUsuario()
        {
            Console.Write("Nombre completo: ");
            string nombreCompleto = Console.ReadLine() ?? "";
            Console.Write("Usuario: ");
            string usuario = Console.ReadLine() ?? "";
            Console.Write("Contraseña: ");
            string password = LeerPassword();

            string hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);
            Console.WriteLine($"Hash generado: {hash}");

            try
            {
                using var cn = new SqlConnection(CadenaConexion);
                cn.Open();
                string sql = @"INSERT INTO Usuarios (NombreUsuario, NombreCompleto, PasswordHash)
                               VALUES (@usuario, @nombre, @hash)";
                using var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@nombre", nombreCompleto);
                cmd.Parameters.AddWithValue("@hash", hash);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Usuario registrado correctamente.");
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                Console.WriteLine("Ese nombre de usuario ya existe.");
            }
        }

        // Verificar con BCrypt.Verify (5 pts) y mostrar el nombre (5 pts)
        static void IniciarSesion()
        {
            Console.Write("Usuario: ");
            string usuario = Console.ReadLine() ?? "";
            Console.Write("Contraseña: ");
            string password = LeerPassword();

            using var cn = new SqlConnection(CadenaConexion);
            cn.Open();

            string? nombreCompleto = null;
            string? hashGuardado = null;

            string sql = "SELECT NombreCompleto, PasswordHash FROM Usuarios WHERE NombreUsuario = @usuario";
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@usuario", usuario);
                using var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    nombreCompleto = dr.GetString(0);
                    hashGuardado = dr.GetString(1);
                }
            }

            bool exitoso = hashGuardado != null && BCrypt.Net.BCrypt.Verify(password, hashGuardado);

            RegistrarIntento(cn, usuario, exitoso);

            if (exitoso)
                Console.WriteLine($"\n¡Acceso concedido! Bienvenido(a), {nombreCompleto}.");
            else
                Console.WriteLine("\nUsuario o contraseña incorrectos.");
        }

        // Guardar intento exitoso/fallido con usuario y fecha
        static void RegistrarIntento(SqlConnection cn, string usuario, bool exitoso)
        {
            string sql = "INSERT INTO IntentosLogin (NombreUsuario, Exitoso) VALUES (@usuario, @exitoso)";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@exitoso", exitoso);
            cmd.ExecuteNonQuery();
        }

        static void VerIntentos()
        {
            using var cn = new SqlConnection(CadenaConexion);
            cn.Open();
            string sql = "SELECT NombreUsuario, Exitoso, Fecha FROM IntentosLogin ORDER BY Fecha DESC";
            using var cmd = new SqlCommand(sql, cn);
            using var dr = cmd.ExecuteReader();

            Console.WriteLine($"\n{"Usuario",-20}{"Resultado",-12}Fecha");
            while (dr.Read())
            {
                string r = dr.GetBoolean(1) ? "EXITOSO" : "FALLIDO";
                Console.WriteLine($"{dr.GetString(0),-20}{r,-12}{dr.GetDateTime(2)}");
            }
        }

        // Oculta la contraseña con *
        static string LeerPassword()
        {
            var sb = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return sb.ToString();
        }
    }
}