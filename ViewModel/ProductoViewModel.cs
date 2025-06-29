using LPAC___Proyecto_II_frontend.Commands;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Models;
using LPAC___Proyecto_II_frontend.Services;
using LPAC___Proyecto_II_frontend.DTOs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class ProductoViewModel : ViewModelBase
    {
        private readonly ProductoService _productoService;
        private readonly CategoriaService _categoriaService;

        public ICommand BuscarProductosCommand { get; private set; }
        public ICommand NuevoProductoCommand { get; private set; }
        public ICommand GuardarProductoCommand { get; private set; }
        public ICommand EliminarProductoCommand { get; private set; }
        public ICommand LimpiarFormularioCommand { get; private set; }

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

        private CategoriaDTO _selectedCategoria;
        public CategoriaDTO SelectedCategoria
        {
            get => _selectedCategoria;
            set
            {
                _selectedCategoria = value;
                OnPropertyChanged(nameof(SelectedCategoria));
                if (ProductoActual != null && _selectedCategoria != null)
                {
                    ProductoActual.codCategoria = _selectedCategoria.codCategoria;
                }
            }
        }

        private Producto _productoActual;
        public Producto ProductoActual
        {
            get => _productoActual;
            set
            {
                _productoActual = value;
                OnPropertyChanged(nameof(ProductoActual));
                if (_productoActual != null && Categorias != null && !string.IsNullOrEmpty(_productoActual.codCategoria))
                {
                    SelectedCategoria = Categorias.FirstOrDefault(c => c.codCategoria == _productoActual.codCategoria);
                }
                else if (_productoActual != null && string.IsNullOrEmpty(_productoActual.codCategoria))
                {
                    SelectedCategoria = null;
                }

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

        private string _mensajeEstado = string.Empty;
        public string MensajeEstado
        {
            get => _mensajeEstado;
            set
            {
                _mensajeEstado = value;
                OnPropertyChanged(nameof(MensajeEstado));
                OnPropertyChanged(nameof(MensajeColor));
            }
        }

        private string _mensajeColor = "Green";
        public string MensajeColor
        {
            get => _mensajeColor;
            set
            {
                _mensajeColor = value;
                OnPropertyChanged(nameof(MensajeColor));
            }
        }

        public ProductoViewModel()
        {
            _productoService = new ProductoService();
            _categoriaService = new CategoriaService();
            ProductosEncontrados = new ObservableCollection<Producto>();
            Categorias = new ObservableCollection<CategoriaDTO>();

            GuardarProductoCommand = new RelayCommand(ExecuteGuardarProducto, CanExecuteGuardarProducto);
            EliminarProductoCommand = new RelayCommand(ExecuteEliminarProducto, CanExecuteEliminarProducto);
            NuevoProductoCommand = new RelayCommand(ExecuteNuevoProducto);
            BuscarProductosCommand = new RelayCommand(async (obj) => await ExecuteBuscarProductos());
            LimpiarFormularioCommand = new RelayCommand(ExecuteNuevoProducto);

            ProductoActual = new Producto();

            _ = LoadInitialData();
        }

        private async Task LoadInitialData()
        {
            await LoadCategorias();
            await ExecuteBuscarProductos();
        }

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

                if (ProductoActual != null && !string.IsNullOrEmpty(ProductoActual.codCategoria))
                {
                    SelectedCategoria = Categorias.FirstOrDefault(c => c.codCategoria == ProductoActual.codCategoria);
                }
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al cargar categorías: {ex.Message}";
                MensajeColor = "Red";
            }
        }

        private async Task ExecuteBuscarProductos()
        {
            try
            {
                MensajeEstado = "Buscando productos...";
                MensajeColor = "Blue";
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
            ProductoActual = new Producto();
            SelectedCategoria = null;
            MensajeEstado = "Ingrese los datos del nuevo producto.";
            MensajeColor = "Blue";
        }

        private async void ExecuteGuardarProducto(object obj)
        {
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
                await ExecuteBuscarProductos();
                ExecuteNuevoProducto(null);
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
            return ProductoActual != null &&
                   !string.IsNullOrWhiteSpace(ProductoActual.nombreProducto) &&
                   ProductoActual.precio > 0 &&
                   !string.IsNullOrEmpty(ProductoActual.codCategoria);
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
                await ExecuteBuscarProductos();
                ExecuteNuevoProducto(null);
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
            return ProductoActual != null && ProductoActual.idProducto != 0;
        }

        public void SeleccionarProductoDesdeDataGrid(Producto productoSeleccionado)
        {
            if (productoSeleccionado != null)
            {
                ProductoActual = new Producto
                {
                    idProducto = productoSeleccionado.idProducto,
                    nombreProducto = productoSeleccionado.nombreProducto,
                    precio = productoSeleccionado.precio,
                    codCategoria = productoSeleccionado.codCategoria,
                };
            }
        }
    }
}