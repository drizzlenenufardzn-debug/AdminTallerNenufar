using System.Windows;
using System.Windows.Controls;
using AdminTallerNenufar.Models;
using AdminTallerNenufar.Data;

namespace AdminTallerNenufar.Views
{
    public partial class RegistroEquipoView : Page
    {
        private int _idCliente;
        private EquipoRepository _equipoRepo = new EquipoRepository();

        public RegistroEquipoView(int idCliente)
        {
            InitializeComponent();
            _idCliente = idCliente;
            lblClienteID.Text = $"Registrando equipo para el cliente ID: {_idCliente}";
        }

        private void BtnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreEq.Text))
            {
                MessageBox.Show("El nombre del equipo es obligatorio.");
                return;
            }

            var nuevoEquipo = new Equipo
            {
                IdCliente = _idCliente,
                NombreEquipo = txtNombreEq.Text,
                Marca = txtMarca.Text,
                Modelo = txtModelo.Text,
                Categoria = cbCategoria.Text,
                Subcategoria = txtSubCat.Text,
                Descripcion = txtDesc.Text
            };

            if (_equipoRepo.GuardarEquipo(nuevoEquipo))
            {
                MessageBox.Show("¡Registro completado con éxito!", "Nenúfar Market", MessageBoxButton.OK, MessageBoxImage.Information);

                // Regresar al inicio o limpiar para un nuevo cliente
                this.NavigationService.Navigate(new RegistroClienteView());
            }
        }
    }
}