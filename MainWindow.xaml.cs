using System;
using System.Windows;
using Microsoft.Win32; // Necesario para guardar archivos
using AdminTallerNenufar.Views;
using AdminTallerNenufar.Data;

namespace AdminTallerNenufar
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Inicializamos la base de datos (crea carpeta y tablas si no existen)
            ConexionDB.InicializarBaseDatos();

            // Al iniciar, mostramos la lista de equipos
            MainFrame.Navigate(new ListaEquiposView());
        }

        private void BtnNuevoIngreso_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RegistroClienteView());
        }

        private void BtnVerEquipos_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListaEquiposView());
        }

        /// <summary>
        /// Abre un cuadro de diálogo para guardar una copia de seguridad de la base de datos
        /// </summary>
        private void BtnRespaldo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Sugerimos un nombre con la fecha de hoy
            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            saveFileDialog.FileName = $"Respaldo_Nenufar_{fecha}.db";
            saveFileDialog.Filter = "Base de Datos SQLite (*.db)|*.db";
            saveFileDialog.Title = "Seleccione dónde guardar el respaldo";

            if (saveFileDialog.ShowDialog() == true)
            {
                // Extraemos la ruta de la carpeta del archivo seleccionado
                string rutaCarpeta = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);

                // Llamamos al método que creamos en ConexionDB
                ConexionDB.CrearRespaldo(rutaCarpeta);
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnInicio_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListaEquiposView());
        }
    }
}