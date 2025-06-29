using LPAC___Proyecto_II_frontend.Commands;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Models;
using LPAC___Proyecto_II_frontend.Services;
using LPAC___Proyecto_II_frontend.DTOs; // Agregado para CategoriaDTO
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
        private readonly CategoriaService _categoriaService; // Instancia del servicio de categorías

        // Comandos
        public ICommand BuscarProductosCommand { get; private set; }
        public ICommand NuevoProductoCommand { get; private set; }
        public ICommand GuardarProductoCommand { get; private set; }
        public ICommand EliminarProductoCommand { get; private set; }
        public ICommand LimpiarFormularioCommand { get; private set; }

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

        // Colección para el ComboBox de categorías
        private ObservableCollection<CategoriaDTO> _categorias;
        public ObservableCollection<CategoriaDTO> Categorias
        {
            get => _categorias;
            set
            {
                _categorias = value;
                OnPropertyChanged(nameof(Categorias));
            }
        }

        // Propiedad para la categoría seleccionada en el ComboBox
        private CategoriaDTO _selectedCategoria;
        public CategoriaDTO SelectedCategoria
        {
            get => _selectedCategoria;
            set
            {
                _selectedCategoria = value;
                OnPropertyChanged(nameof(SelectedCategoria));
                // Cuando se selecciona una categoría, actualiza el codCategoria en ProductoActual
                if (ProductoActual != null && _selectedCategoria != null)
                {
                    ProductoActual.codCategoria = _selectedCategoria.codCategoria;
                }
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
                // Asegurarse de que el ComboBox de categoría refleje el producto actual
                if (_productoActual != null && Categorias != null && !string.IsNullOrEmpty(_productoActual.codCategoria))
                {
                    // Seleccionar la categoría en el ComboBox si coincide con el codCategoria del producto
                    SelectedCategoria = Categorias.FirstOrDefault(c => c.codCategoria == _productoActual.codCategoria);
                }
                else if (_productoActual != null && string.IsNullOrEmpty(_productoActual.codCategoria))
                {
                    // Si el producto actual no tiene categoría, deseleccionar del ComboBox
                    SelectedCategoria = null;
                }

                // Notificar cambios para que los botones se actualicen
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
            _categoriaService = new CategoriaService(); // Inicializa el servicio de categorías
            ProductosEncontrados = new ObservableCollection<Producto>();
            Categorias = new ObservableCollection<CategoriaDTO>(); // Inicializa la colección de categorías

            // Inicializa los comandos ANTES que ProductoActual
            GuardarProductoCommand = new RelayCommand(ExecuteGuardarProducto, CanExecuteGuardarProducto);
            EliminarProductoCommand = new RelayCommand(ExecuteEliminarProducto, CanExecuteEliminarProducto);
            NuevoProductoCommand = new RelayCommand(ExecuteNuevoProducto);
            BuscarProductosCommand = new RelayCommand(async (obj) => await ExecuteBuscarProductos());
            LimpiarFormularioCommand = new RelayCommand(ExecuteNuevoProducto);

            // Inicializa ProductoActual después de los comandos
            ProductoActual = new Producto();

            // Carga inicial de datos
            _ = LoadInitialData(); // Llama a un método asíncrono para cargar todo al inicio
        }

        // Nuevo método para cargar datos iniciales (productos y categorías)
        private async Task LoadInitialData()
        {
            await LoadCategorias();
            await ExecuteBuscarProductos(); // Carga todos los productos al inicio
        }

        // Método para cargar categorías desde la API
        private async Task LoadCategorias()
        {
            try
            {
                MensajeEstado = "Cargando categorías...";
                MensajeColor = "Blue";
                var categoriasList = await _categoriaService.GetCategoriasAsync();
                Categorias.Clear();
                foreach (var cat in categoriasList)
                {
                    Categorias.Add(cat);
                }
                MensajeEstado = "Categorías cargadas.";
                MensajeColor = "Green";

                // Una vez cargadas las categorías, si hay un producto actual, intenta seleccionar su categoría
                if (ProductoActual != null && !string.IsNullOrEmpty(ProductoActual.codCategoria))
                {
                    SelectedCategoria = Categorias.FirstOrDefault(c => c.codCategoria == ProductoActual.codCategoria);
                }
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al cargar categorías: {ex.Message}";
                MensajeColor = "Red";
                // Considera mostrar un MessageBox.Show aquí para errores críticos
            }
        }

        // Métodos de los comandos

        private async Task ExecuteBuscarProductos()
        {
            try
            {
                MensajeEstado = "Buscando productos...";
                MensajeColor = "Blue";
                // Pasa el TextoBusqueda al servicio
                var productos = await _productoService.GetAllProductosAsync(TextoBusqueda);
                ProductosEncontrados.Clear();
                foreach (var p in productos)
                {
                    ProductosEncontrados.Add(p);
                }
                MensajeEstado = $"Productos encontrados: {ProductosEncontrados.Count}";
                MensajeColor = "Green";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al buscar productos: {ex.Message}";
                MensajeColor = "Red";
            }
        }

        private void ExecuteNuevoProducto(object obj)
        {
            ProductoActual = new Producto(); // Crea una nueva instancia de Producto
            SelectedCategoria = null; // Deselecciona la categoría en el ComboBox
            MensajeEstado = "Ingrese los datos del nuevo producto.";
            MensajeColor = "Blue";
        }

        private async void ExecuteGuardarProducto(object obj)
        {
            // Validaciones básicas antes de guardar
            if (ProductoActual == null || string.IsNullOrWhiteSpace(ProductoActual.nombreProducto) || ProductoActual.precio <= 0 || string.IsNullOrEmpty(ProductoActual.codCategoria))
            {
                MensajeEstado = "Por favor, complete todos los campos obligatorios (Nombre, Precio, Categoría).";
                MensajeColor = "Red";
                return;
            }

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
                ExecuteNuevoProducto(null); // Limpiar el formulario y preparar para nuevo producto
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
            // Ahora también depende de que haya una categoría seleccionada
            return ProductoActual != null &&
                   !string.IsNullOrWhiteSpace(ProductoActual.nombreProducto) &&
                   ProductoActual.precio > 0 && // Asumiendo que el precio debe ser positivo
                   !string.IsNullOrEmpty(ProductoActual.codCategoria); // Asegura que se ha seleccionado una categoría
        }

        private async void ExecuteEliminarProducto(object obj)
        {
            if (ProductoActual == null || ProductoActual.idProducto == 0) return;

            try
            {
                MensajeEstado = "Eliminando producto...";
                MensajeColor = "Blue";
                await _productoService.DeleteProductoAsync(ProductoActual.idProducto);
                MensajeEstado = "Producto eliminado exitosamente.";
                await ExecuteBuscarProductos(); // Recargar la lista
                ExecuteNuevoProducto(null); // Limpiar el formulario
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
            // Solo se puede eliminar si hay un producto seleccionado con un ID válido (no es un producto nuevo)
            return ProductoActual != null && ProductoActual.idProducto != 0;
        }

        // Método para manejar la selección de un producto en el DataGrid
        // No está en tu código actual, pero es común. Asume que se enlaza a SelectedItem del DataGrid
        public void SeleccionarProductoDesdeDataGrid(Producto productoSeleccionado)
        {
            if (productoSeleccionado != null)
            {
                // Clona el producto para no editar directamente el objeto en la colección
                ProductoActual = new Producto // Puedes usar un método de clonación o un constructor de copia si es más complejo
                {
                    idProducto = productoSeleccionado.idProducto,
                    nombreProducto = productoSeleccionado.nombreProducto,
                    precio = productoSeleccionado.precio,
                    codCategoria = productoSeleccionado.codCategoria,
                    // Copia otras propiedades si las tienes
                };
            }
        }
    }
}