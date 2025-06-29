using LPAC___Proyecto_II_frontend.Commands;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Models;
using LPAC___Proyecto_II_frontend.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System; // Agregado para Exception

namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class ProductoViewModel : ViewModelBase
    {
        private readonly ProductoService _productoService;

        // Comandos
        public ICommand BuscarProductosCommand { get; private set; }
        public ICommand NuevoProductoCommand { get; private set; }
        public ICommand GuardarProductoCommand { get; private set; }
        public ICommand EliminarProductoCommand { get; private set; }
        public ICommand LimpiarFormularioCommand { get; private set; } // Nuevo comando para limpiar

        // Colección para el DataGrid
        private ObservableCollection<Producto> _productosEncontrados;
        public ObservableCollection<Producto> ProductosEncontrados
        {
            get => _productosEncontrados;
            set
            {
                _productosEncontrados = value;
                OnPropertyChanged(nameof(ProductosEncontrados));
            }
        }

        // Producto actual para el formulario de edición/creación
        private Producto _productoActual;
        public Producto ProductoActual
        {
            get => _productoActual;
            set
            {
                _productoActual = value;
                OnPropertyChanged(nameof(ProductoActual));
                if (GuardarProductoCommand is RelayCommand guardarCommand)
                {
                    guardarCommand.RaiseCanExecuteChanged();
                }
                if (EliminarProductoCommand is RelayCommand eliminarCommand)
                {
                    eliminarCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // Campo de búsqueda
        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                _textoBusqueda = value;
                OnPropertyChanged(nameof(TextoBusqueda));
            }
        }

        // Mensaje de estado para el usuario
        private string _mensajeEstado = string.Empty;
        public string MensajeEstado
        {
            get => _mensajeEstado;
            set
            {
                _mensajeEstado = value;
                OnPropertyChanged(nameof(MensajeEstado));
                OnPropertyChanged(nameof(MensajeColor)); // Notificar cambio de color
            }
        }

        // Propiedad para el color del mensaje de estado (para errores)
        private string _mensajeColor = "Green"; // Default a verde o un color neutro
        public string MensajeColor
        {
            get => _mensajeColor;
            set
            {
                _mensajeColor = value;
                OnPropertyChanged(nameof(MensajeColor));
            }
        }

        // Constructor
        public ProductoViewModel()
        {
            _productoService = new ProductoService();
            ProductosEncontrados = new ObservableCollection<Producto>();

            // Inicializa los comandos ANTES que ProductoActual
            GuardarProductoCommand = new RelayCommand(ExecuteGuardarProducto, CanExecuteGuardarProducto);
            EliminarProductoCommand = new RelayCommand(ExecuteEliminarProducto, CanExecuteEliminarProducto);
            NuevoProductoCommand = new RelayCommand(ExecuteNuevoProducto);
            BuscarProductosCommand = new RelayCommand(async (obj) => await ExecuteBuscarProductos());
            LimpiarFormularioCommand = new RelayCommand(ExecuteNuevoProducto); // El botón Limpiar usa la misma lógica que Nuevo

            // Inicializa ProductoActual después de los comandos
            ProductoActual = new Producto();

            _ = ExecuteBuscarProductos(); // Iniciar la carga de productos al abrir la ventana
        }

        // Métodos de los comandos

        private async Task ExecuteBuscarProductos()
        {
            try
            {
                MensajeEstado = "Buscando productos...";
                MensajeColor = "Blue"; // Color para estado de carga
                // Pasa el TextoBusqueda al servicio
                var productos = await _productoService.GetAllProductosAsync(TextoBusqueda);
                ProductosEncontrados.Clear();
                foreach (var p in productos)
                {
                    ProductosEncontrados.Add(p);
                }
                MensajeEstado = $"Productos encontrados: {ProductosEncontrados.Count}";
                MensajeColor = "Green"; // Color para éxito
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al buscar productos: {ex.Message}";
                MensajeColor = "Red"; // Color para error
            }
        }

        private void ExecuteNuevoProducto(object obj)
        {
            ProductoActual = new Producto();
            MensajeEstado = "Ingrese los datos del nuevo producto.";
            MensajeColor = "Blue";
        }

        private async void ExecuteGuardarProducto(object obj)
        {
            if (ProductoActual == null) return;

            try
            {
                MensajeEstado = "Guardando producto...";
                MensajeColor = "Blue";
                if (ProductoActual.idProducto == 0)
                {
                    await _productoService.CreateProductoAsync(ProductoActual);
                    MensajeEstado = "Producto creado exitosamente.";
                }
                else
                {
                    await _productoService.UpdateProductoAsync(ProductoActual);
                    MensajeEstado = "Producto actualizado exitosamente.";
                }
                await ExecuteBuscarProductos(); // Recargar la lista después de guardar
                ProductoActual = new Producto(); // Limpiar el formulario
                MensajeColor = "Green";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al guardar producto: {ex.Message}";
                MensajeColor = "Red";
            }
        }

        private bool CanExecuteGuardarProducto(object obj)
        {
            // Lógica para habilitar/deshabilitar el botón Guardar
            // Asume que nombreProducto es un campo obligatorio
            return ProductoActual != null && !string.IsNullOrWhiteSpace(ProductoActual.nombreProducto);
        }

        private async void ExecuteEliminarProducto(object obj)
        {
            if (ProductoActual == null || ProductoActual.idProducto == 0) return;

            // Opcional: Puedes añadir aquí una confirmación al usuario antes de eliminar
            // var result = MessageBox.Show("¿Está seguro de que desea eliminar este producto?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            // if (result != MessageBoxResult.Yes) return;

            try
            {
                MensajeEstado = "Eliminando producto...";
                MensajeColor = "Blue";
                await _productoService.DeleteProductoAsync(ProductoActual.idProducto);
                MensajeEstado = "Producto eliminado exitosamente.";
                await ExecuteBuscarProductos(); // Recargar la lista
                ProductoActual = new Producto(); // Limpiar el formulario
                MensajeColor = "Green";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al eliminar producto: {ex.Message}";
                MensajeColor = "Red";
            }
        }

        private bool CanExecuteEliminarProducto(object obj)
        {
            // Lógica para habilitar/deshabilitar el botón Eliminar
            // Solo se puede eliminar si hay un producto seleccionado con un ID válido
            return ProductoActual != null && ProductoActual.idProducto != 0;
        }
    }
}