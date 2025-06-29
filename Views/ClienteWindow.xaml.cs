using LPAC___Proyecto_II_frontend.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace LPAC___Proyecto_II_frontend.Views 
{

    public partial class ClienteWindow : UserControl
    {
        public ClienteWindow()
        {
            InitializeComponent(); 
            this.DataContext = new ClienteViewModel(); 
        }
    }
}