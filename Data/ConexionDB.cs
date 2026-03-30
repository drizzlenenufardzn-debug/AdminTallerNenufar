using System;
using System.Data.SQLite;
using System.IO;
using System.Windows; // Necesario para los MessageBox

namespace AdminTallerNenufar.Data
{
    public static class ConexionDB
    {
        private const string NombreBaseDatos = "tallerNenufar.db";

        // 1. Definimos la ruta en LocalAppData para que funcione en cualquier PC sin errores de permisos
        private static readonly string CarpetaApp = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "NenufarMarket"
        );

        // 2. Ruta completa del archivo .db
        private static readonly string RutaBaseDatos = Path.Combine(CarpetaApp, NombreBaseDatos);

        // Cadena de conexión usando la nueva ruta segura
        private static readonly string CadenaConexion = $"Data Source={RutaBaseDatos};Version=3;";

        /// <summary>
        /// Retorna una nueva instancia de conexión a SQLite
        /// </summary>
        public static SQLiteConnection ObtenerConexion()
        {
            return new SQLiteConnection(CadenaConexion);
        }

        /// <summary>
        /// Asegura que la carpeta, la base de datos y las tablas existan al iniciar la app
        /// </summary>
        public static void InicializarBaseDatos()
        {
            try
            {
                // CRÍTICO: Creamos la carpeta si no existe antes de intentar conectar
                if (!Directory.Exists(CarpetaApp))
                {
                    Directory.CreateDirectory(CarpetaApp);
                }

                using (var conexion = ObtenerConexion())
                {
                    conexion.Open();

                    string tablaClientes = @"CREATE TABLE IF NOT EXISTS Clientes (
                                                IdCliente INTEGER PRIMARY KEY AUTOINCREMENT,
                                                Cedula TEXT UNIQUE NOT NULL,
                                                Nombre TEXT NOT NULL,
                                                Apellido TEXT NOT NULL,
                                                Telefono TEXT
                                            );";

                    string tablaEquipos = @"CREATE TABLE IF NOT EXISTS Equipos (
                                                IdEquipo INTEGER PRIMARY KEY AUTOINCREMENT,
                                                IdCliente INTEGER,
                                                NombreEquipo TEXT,
                                                Marca TEXT,
                                                Modelo TEXT,
                                                Categoria TEXT,
                                                Subcategoria TEXT,
                                                Descripcion TEXT,
                                                FechaIngreso DATETIME DEFAULT CURRENT_TIMESTAMP,
                                                FOREIGN KEY(IdCliente) REFERENCES Clientes(IdCliente)
                                            );";

                    using (var comando = new SQLiteCommand(conexion))
                    {
                        comando.CommandText = tablaClientes;
                        comando.ExecuteNonQuery();

                        comando.CommandText = tablaEquipos;
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la base de datos: {ex.Message}", "Error de Sistema", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Crea una copia de seguridad de la base de datos en la ruta especificada
        /// </summary>
        public static void CrearRespaldo(string rutaDestino)
        {
            try
            {
                if (File.Exists(RutaBaseDatos))
                {
                    string fecha = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
                    string nombreRespaldo = $"Respaldo_Nenufar_{fecha}.db";
                    string destinoCompleto = Path.Combine(rutaDestino, nombreRespaldo);

                    File.Copy(RutaBaseDatos, destinoCompleto, true);
                    MessageBox.Show($"Respaldo guardado con éxito en:\n{destinoCompleto}", "Copia de Seguridad", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se encontró la base de datos original para respaldar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el respaldo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}