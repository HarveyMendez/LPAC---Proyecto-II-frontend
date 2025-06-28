using System;
using System.Windows.Input; // Esto es para ICommand

namespace LPAC___Proyecto_II_frontend.Commands // Este namespace debe ser correcto
{
    // Esta clase nos ayuda a crear comandos de manera sencilla para los botones.
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute; // Lo que el comando va a hacer
        private readonly Predicate<object> _canExecute; // Si el comando se puede ejecutar o no

        // Esto avisa al sistema de WPF si el estado de habilitación del botón cambió
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Constructor: Le decimos qué hacer (execute) y opcionalmente cuándo puede hacerlo (canExecute)
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Dice si el comando se puede ejecutar en este momento
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // Ejecuta la acción del comando
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        // ¡ESTE ES EL NUEVO MÉTODO QUE FALTABA!
        // Sirve para forzar a que WPF re-evalúe si el comando puede ejecutarse.
        public void RaiseCanExecuteChanged()
        {
            // CommandManager.InvalidateRequerySuggested() fuerza una re-evaluación de todos los comandos
            CommandManager.InvalidateRequerySuggested();
        }
    }
}