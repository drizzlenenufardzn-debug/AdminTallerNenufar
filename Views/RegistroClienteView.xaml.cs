using System;
using System.Windows;
using System.Windows.Controls;
using AdminTallerNenufar.Models;
using AdminTallerNenufar.Data;

namespace AdminTallerNenufar.Views
{
    public partial class RegistroClienteView : Page
    {
        private readonly ClienteRepository _clienteRepo;

        public RegistroClienteView()
        {
            InitializeComponent();
            _clienteRepo = new ClienteRepository();

            // Suscribimos el evento LostFocus para buscar al cliente automáticamente
            TxtCedula.LostFocus += TxtCedula_LostFocus;
        }

        /// <summary>
        /// Se activa cuando el usuario termina de escribir la cédula y sale del campo.
        /// </summary>
        private void TxtCedula_LostFocus(object sender, RoutedEventArgs e)
        {
            string cedula = TxtCedula.Text.Trim();
            if (string.IsNullOrEmpty(cedula)) return;

            try
            {
                var clienteExistente = _clienteRepo.BuscarPorCedula(cedula);

                if (clienteExistente != null)
                {
                    // Rellenamos los campos automáticamente
                    TxtNombre.Text = clienteExistente.Nombre;
                    TxtApellido.Text = clienteExistente.Apellido;
                    TxtTelefono.Text = clienteExistente.Telefono;

                    // Opcional: Mostrar un aviso visual discreto
                    TxtCedula.ToolTip = "Cliente ya registrado";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar cliente: {ex.Message}");
            }
        }

        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validar datos mínimos
            if (string.IsNullOrWhiteSpace(TxtCedula.Text) || string.IsNullOrWhiteSpace(TxtNombre.Text))
            {
                MessageBox.Show("Por favor, rellene al menos la Cédula y el Nombre.", "Campos Faltantes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int idClienteFinal;

                // 2. Verificar si el cliente ya existe para no crear un duplicado
                var clienteExistente = _clienteRepo.BuscarPorCedula(TxtCedula.Text.Trim());

                if (clienteExistente != null)
                {
                    // Si ya existe, usamos su ID actual
                    idClienteFinal = clienteExistente.IdCliente;
                }
                else
                {
                    // 3. Si es nuevo, lo creamos
                    Cliente nuevoCliente = new Cliente
                    {
                        Cedula = TxtCedula.Text.Trim(),
                        Nombre = TxtNombre.Text.Trim(),
                        Apellido = TxtApellido.Text.Trim(),
                        Telefono = TxtTelefono.Text.Trim()
                    };
                    idClienteFinal = _clienteRepo.GuardarCliente(nuevoCliente);
                }

                // 4. Navegar al formulario de Equipos pasando el ID del cliente
                if (idClienteFinal > 0)
                {
                    this.NavigationService.Navigate(new RegistroEquipoView(idClienteFinal));
                }
                else
                {
                    MessageBox.Show("No se pudo procesar el registro del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en el proceso: {ex.Message}", "Error de Base de Datos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}