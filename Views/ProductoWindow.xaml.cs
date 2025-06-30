using LPAC___Proyecto_II_frontend.ViewModel;
using System.Windows.Controls;

namespace LPAC___Proyecto_II_frontend.Views
{
    public partial class ProductoWindow : UserControl 
    {
        public ProductoWindow()
        {
            InitializeComponent();
            this.DataContext = new ProductoViewModel();
        }
    }
}