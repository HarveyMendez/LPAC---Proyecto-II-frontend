using LPAC___Proyecto_II_frontend.ViewModel;
using System.Windows.Controls;

namespace LPAC___Proyecto_II_frontend.Views
{
    public partial class CategoriaWindow : UserControl
    {
        public CategoriaWindow()
        {
            InitializeComponent();
            this.DataContext = new CategoriaViewModel();
        }
    }
}
