using System;
using System.ComponentModel; 
using System.Windows.Input; 
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Commands;
using LPAC___Proyecto_II_frontend.Services;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using LPAC___Proyecto_II_frontend.Views;

namespace LPAC___Proyecto_II_frontend.ViewModel 
{
    public class LoginViewModel : ViewModelBase 
    {
        private string _usuario;
        private string _contrasena; 
        private string _mensajeError;

        public string Usuario
        {
            get => _usuario;
            set
            {
                if (_usuario != value)
                {
                    _usuario = value;
                    OnPropertyChanged(nameof(Usuario));
                }
            }
        }

        public string Contrasena 
        {
            get => _contrasena;
            set
            {
                if (_contrasena != value)
                {
                    _contrasena = value;
                    OnPropertyChanged(nameof(Contrasena));
                }
            }
        }

        public string MensajeError
        {
            get => _mensajeError;
            set
            {
                if (_mensajeError != value)
                {
                    _mensajeError = value;
                    OnPropertyChanged(nameof(MensajeError));
                }
            }
        }

        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        private async void ExecuteLogin(object parameter)
        {
            try
            {
                bool success = await AuthService.LoginAsync(Usuario, Contrasena);

                if (success)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                    });
                    Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault()?.Close();

                }
                else
                {
                    MensajeError = "Usuario o contraseña incorrectos";
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error: {ex.Message}";
            }
        }
        


        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Usuario) && !string.IsNullOrWhiteSpace(Contrasena);
        }
    }
}