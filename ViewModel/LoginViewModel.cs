using System;
using System.ComponentModel; 
using System.Windows.Input; 
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Commands;

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

        private void ExecuteLogin(object parameter)
        {
            if (Usuario == "admin" && Contrasena == "123")
            {
                MensajeError = "¡Inicio de sesión exitoso! Pura vida.";
            }
            else
            {
                MensajeError = "Usuario o contraseña incorrectos. ¡Intente de nuevo, mae!";
            }
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Usuario) && !string.IsNullOrWhiteSpace(Contrasena);
        }
    }
}