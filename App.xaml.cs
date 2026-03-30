using System;
using System.Windows;
using AdminTallerNenufar.Data; // Importante para acceder a ConexionDB

namespace AdminTallerNenufar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Esto crea el archivo .db y las tablas si no existen
                ConexionDB.InicializarBaseDatos();
            }
            catch (Exception ex)
            {
                // Si hay un error al crear la base de datos, lo avisamos de inmediato
                MessageBox.Show($"Error crítico al inicializar la base de datos: {ex.Message}",
                                "Error de Inicio",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                // Opcional: Cerrar la app si no puede conectar a la DB
                // Shutdown(); 
            }
        }
    }
}