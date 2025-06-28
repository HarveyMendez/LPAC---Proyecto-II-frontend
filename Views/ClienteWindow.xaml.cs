using System.Windows; // Necesario para la clase Window

namespace LPAC___Proyecto_II_frontend.Views // Asegúrese que este namespace sea el mismo que usa para sus Views
{
    /// <summary>
    /// Lógica de interacción para ClienteWindow.xaml
    /// </summary>
    public partial class ClienteWindow : Window // 'partial' significa que parte de la clase está en el XAML
    {
        public ClienteWindow()
        {
            InitializeComponent(); // Este método es llamado automáticamente para cargar la interfaz del XAML
        }
    }
}