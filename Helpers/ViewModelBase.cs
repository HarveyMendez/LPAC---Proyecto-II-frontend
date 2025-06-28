using System.ComponentModel; // Esto es para INotifyPropertyChanged

namespace LPAC___Proyecto_II_frontend.Helpers // Cambie el namespace según la ubicación de su proyecto
{
    // Clase base que implementa INotifyPropertyChanged.
    // Todas sus ViewModels y sus Models del frontend (si necesita que avisen cambios)
    // heredarán de esta para que no tenga que escribir el mismo código una y otra vez.
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // Esto es lo que avisa cuando una propiedad cambia.
        public event PropertyChangedEventHandler PropertyChanged;

        // Este método lo llamamos dentro de cada 'set' de una propiedad
        // para que la interfaz sepa que algo nuevo pasó y se actualice.
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}