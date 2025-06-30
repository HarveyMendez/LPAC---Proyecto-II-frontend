using LPAC___Proyecto_II_frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LPAC___Proyecto_II_frontend.Views
{
    /// <summary>
    /// Lógica de interacción para OrdenWindow.xaml
    /// </summary>
    public partial class OrdenWindow : UserControl
    {
        public OrdenWindow()
        {
            InitializeComponent();
            this.DataContext = new OrdenViewModel();
        }
    }
}
