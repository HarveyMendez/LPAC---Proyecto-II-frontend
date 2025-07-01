using LPAC___Proyecto_II_frontend.ViewModel;
using System.Windows.Controls;

namespace LPAC___Proyecto_II_frontend.Views
{
    public partial class DepartamentoWindow : UserControl
    {
        public DepartamentoWindow()
        {
            InitializeComponent();
            this.DataContext = new DepartamentoViewModel();
        }
    }
}
