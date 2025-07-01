using LPAC___Proyecto_II_frontend.Commands;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Models.Enums;
using LPAC___Proyecto_II_frontend.Services;
using LPAC___Proyecto_II_frontend.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public UserRole CurrentUserRole => AuthService.Rol;
        public string NombreUsuario => AuthService.NombreCompleto;

        public ICommand LogoutCommand { get; }

        public MainWindowViewModel()
        {
            LogoutCommand = new RelayCommand(Logout, CanExecute);
        }



        public void Logout(object parameter)
        {
            AuthService.Logout();

            Application.Current.Dispatcher.Invoke(() =>
            {
                var loaginWindow = new LoginWindow();
                loaginWindow.Show();
            });
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
