using System.Windows;
using System.Windows.Controls;
using LPAC___Proyecto_II_frontend.ViewModel; // Asegúrese que este namespace sea correcto

namespace LPAC___Proyecto_II_frontend.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();

            // Esto es para pasar la contraseña al ViewModel, ya que PasswordBox no tiene Binding directo.
            // Una opción más limpia es usar Attached Properties o un Behavior, pero esta es sencilla para empezar.
            PasswordBox.PasswordChanged += (sender, e) =>
            {
                if (DataContext is LoginViewModel vm)
                {
                    vm.Contrasena = PasswordBox.Password;
                }
            };
        }
    }
}