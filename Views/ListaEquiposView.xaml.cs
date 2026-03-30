using System.Windows.Controls;
using AdminTallerNenufar.Data;
using System.Data;

namespace AdminTallerNenufar.Views
{
    public partial class ListaEquiposView : Page
    {
        private EquipoRepository _equipoRepo = new EquipoRepository();

        public ListaEquiposView()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            // Asignamos el DataTable directamente al DataGrid
            DataTable datos = _equipoRepo.ObtenerTodosLosEquipos();
            dgEquipos.ItemsSource = datos.DefaultView;
        }
    }
}