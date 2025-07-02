using LPAC___Proyecto_II_frontend.Commands;
using LPAC___Proyecto_II_frontend.DTOs;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Models;
using LPAC___Proyecto_II_frontend.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class OrdenViewModel : ViewModelBase
    {
        private readonly ClienteService _clienteService;
        private readonly ProductoService _productoService;
        private readonly InformacionDeMiCompaniaService _infoCompaniaService;
        private readonly OrdenService _ordenService;

        // Comandos
        public ICommand AgregarProductoCommand { get; private set; }
        public ICommand EliminarProductoCommand { get; private set; }
        public ICommand GenerarFacturaCommand { get; private set; }
        public ICommand BuscarClientesCommand { get; private set; }
        public ICommand BuscarProductosCommand { get; private set; }

        // Colecciones
        private ObservableCollection<Cliente> _clientes;
        public ObservableCollection<Cliente> Clientes
        {
            get => _clientes;
            set
            {
                _clientes = value;
                OnPropertyChanged(nameof(Clientes));
            }
        }

        private ObservableCollection<Producto> _productosDisponibles;
        public ObservableCollection<Producto> ProductosDisponibles
        {
            get => _productosDisponibles;
            set
            {
                _productosDisponibles = value;
                OnPropertyChanged(nameof(ProductosDisponibles));
            }
        }

        // Propiedades seleccionadas
        private Cliente _clienteSeleccionado;
        public Cliente ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set
            {
                if (SetProperty(ref _clienteSeleccionado, value) && value != null)
                {
                    // Copiar datos del cliente a los campos de viaje
                    DireccionViaje = value.Direccion;
                    CiudadViaje = value.Ciudad;
                    ProvinciaViaje = value.Provincia;
                    PaisViaje = value.Pais;
                    TelefonoViaje = value.Telefono;
                }
            }
        }

        private Producto _productoSeleccionado;
        public Producto ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set => SetProperty(ref _productoSeleccionado, value);
        }

        private OrderItemViewModel _itemOrdenSeleccionado;
        public OrderItemViewModel ItemOrdenSeleccionado
        {
            get => _itemOrdenSeleccionado;
            set => SetProperty(ref _itemOrdenSeleccionado, value);
        }

        // Campos de la orden
        private DateTime _fechaOrden = DateTime.Now;
        public DateTime FechaOrden
        {
            get => _fechaOrden;
            set => SetProperty(ref _fechaOrden, value);
        }

        // Campos de viaje (copiados del cliente)
        private string _direccionViaje;
        public string DireccionViaje
        {
            get => _direccionViaje;
            set => SetProperty(ref _direccionViaje, value);
        }

        private string _ciudadViaje;
        public string CiudadViaje
        {
            get => _ciudadViaje;
            set => SetProperty(ref _ciudadViaje, value);
        }

        private string _provinciaViaje;
        public string ProvinciaViaje
        {
            get => _provinciaViaje;
            set => SetProperty(ref _provinciaViaje, value);
        }

        private string _paisViaje;
        public string PaisViaje
        {
            get => _paisViaje;
            set => SetProperty(ref _paisViaje, value);
        }

        private string _telefonoViaje;
        public string TelefonoViaje
        {
            get => _telefonoViaje;
            set => SetProperty(ref _telefonoViaje, value);
        }

        // Items de la orden
        private ObservableCollection<OrderItemViewModel> _itemsOrden;
        public ObservableCollection<OrderItemViewModel> ItemsOrden
        {
            get => _itemsOrden;
            set
            {
                _itemsOrden = value;
                OnPropertyChanged(nameof(ItemsOrden));
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(Impuestos));
                OnPropertyChanged(nameof(Total));
            }
        }

        // Mensajes de estado
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

        // Totales calculados
        public decimal Subtotal => ItemsOrden?.Sum(item => item.Subtotal) ?? 0;
        public decimal Impuestos => ItemsOrden?.Sum(item => item.Impuestos) ?? 0;
        public decimal Total => Subtotal + Impuestos;

        // Tasa de impuestos
        private double _tasaImpuestos;

        public OrdenViewModel()
        {
            _clienteService = new ClienteService();
            _productoService = new ProductoService();
            _infoCompaniaService = new InformacionDeMiCompaniaService();
            _ordenService = new OrdenService();

            ItemsOrden = new ObservableCollection<OrderItemViewModel>();
            Clientes = new ObservableCollection<Cliente>();
            ProductosDisponibles = new ObservableCollection<Producto>();

            // Inicializar comandos
            AgregarProductoCommand = new RelayCommand(ExecuteAgregarProducto, CanExecuteAgregarProducto);
            EliminarProductoCommand = new RelayCommand(ExecuteEliminarProducto, CanExecuteEliminarProducto);
            GenerarFacturaCommand = new RelayCommand(ExecuteGenerarFactura, CanExecuteGenerarFactura);
            BuscarClientesCommand = new RelayCommand(async (obj) => await ExecuteBuscarClientes());
            BuscarProductosCommand = new RelayCommand(async (obj) => await ExecuteBuscarProductos());

            // Cargar datos iniciales
            _ = LoadInitialDataAsync();
        }

        private async Task LoadInitialDataAsync()
        {
            try
            {
                MensajeEstado = "Cargando datos iniciales...";
                MensajeColor = "Blue";

                // Cargar tasa de impuestos
                var infoCompania = await _infoCompaniaService.GetInfoActual();
                _tasaImpuestos = infoCompania.ImpuestoVenta / 100.0;

                // Cargar clientes y productos
                await ExecuteBuscarClientes();
                await ExecuteBuscarProductos();

                MensajeEstado = "Datos cargados correctamente";
                MensajeColor = "Green";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al cargar datos: {ex.Message}";
                MensajeColor = "Red";
            }
        }

        private async Task ExecuteBuscarClientes()
        {
            try
            {
                MensajeEstado = "Buscando clientes...";
                MensajeColor = "Blue";

                var clientes = await _clienteService.GetClientesAsync();
                Clientes.Clear();
                foreach (var cliente in clientes)
                {
                    Clientes.Add(cliente);
                }

                MensajeEstado = $"Clientes encontrados: {Clientes.Count}";
                MensajeColor = "Green";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al buscar clientes: {ex.Message}";
                MensajeColor = "Red";
            }
        }

        private async Task ExecuteBuscarProductos()
        {
            try
            {
                MensajeEstado = "Buscando productos...";
                MensajeColor = "Blue";

                var productos = await _productoService.GetAllProductosAsync();
                ProductosDisponibles.Clear();
                foreach (var producto in productos)
                {
                    ProductosDisponibles.Add(producto);
                }

                MensajeEstado = $"Productos encontrados: {ProductosDisponibles.Count}";
                MensajeColor = "Green";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al buscar productos: {ex.Message}";
                MensajeColor = "Red";
            }
        }

        private void ExecuteAgregarProducto(object obj)
        {
            if (ProductoSeleccionado == null) return;

            // Verificar si el producto ya está en la orden
            var itemExistente = ItemsOrden.FirstOrDefault(i => i.Producto.idProducto == ProductoSeleccionado.idProducto);

            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
            }
            else
            {
                ItemsOrden.Add(new OrderItemViewModel(ProductoSeleccionado, _tasaImpuestos));
            }

            ActualizarTotales();
        }

        private bool CanExecuteAgregarProducto(object obj) => ProductoSeleccionado != null;

        private void ExecuteEliminarProducto(object obj)
        {
            if (ItemOrdenSeleccionado != null)
            {
                ItemsOrden.Remove(ItemOrdenSeleccionado);
                ActualizarTotales();
            }
        }

        private bool CanExecuteEliminarProducto(object obj) => ItemOrdenSeleccionado != null;

        private void ActualizarTotales()
        {
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Impuestos));
            OnPropertyChanged(nameof(Total));
        }

        private async void ExecuteGenerarFactura(object obj)
        {
            if (ClienteSeleccionado == null)
            {
                MensajeEstado = "Seleccione un cliente antes de generar la factura";
                MensajeColor = "Red";
                return;
            }

            if (ItemsOrden.Count == 0)
            {
                MensajeEstado = "Agregue al menos un producto a la orden";
                MensajeColor = "Red";
                return;
            }

            try
            {
                MensajeEstado = "Generando factura...";
                MensajeColor = "Blue";

                var ordenDto = ToOrdenDTO();

                if (ordenDto == null || ordenDto.Cliente == null || ordenDto.Cliente.IdCliente == 0)
                {
                    MessageBox.Show("Error al preparar los datos de la orden", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var ordenCreada = await _ordenService.CreateOrdenAsync(ordenDto);

                if(ordenCreada == null)
                {
                    MessageBox.Show("Error al crear la orden. Por favor, intente nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var pdfBytes = await _ordenService.GenerateInvoicePdfAsync(ordenDto);

                string fileName = $"Factura_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                await File.WriteAllBytesAsync(fileName, pdfBytes);

                Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });

                MensajeEstado = $"Factura generada: {fileName}";
                MensajeColor = "Green";
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al generar factura: {ex.Message}";
                MensajeColor = "Red";
            }
        }

        private bool CanExecuteGenerarFactura(object obj) =>
            ClienteSeleccionado != null && ItemsOrden.Count > 0;

        public OrdenDTO ToOrdenDTO()
        {
            if (ClienteSeleccionado == null)
            {
                MessageBox.Show("No hay cliente seleccionado al generar el DTO", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            var clienteDto = ClienteSeleccionado.ToDto();


            return new OrdenDTO
            {
                fecha_orden = DateTime.Now,
                direccion_viaje = DireccionViaje,
                cuidad_viaje = CiudadViaje,
                provincia_viaje = ProvinciaViaje,
                pais_viaje = PaisViaje,
                telefono_viaje = TelefonoViaje,
                fecha_viaje = FechaOrden,
                Cliente = clienteDto,
                Empleado = new EmpleadoDTO { idEmpleado = 11},
                Detalles = ItemsOrden.Select(item => new DetalleOrdenDTO
                {
                    cantidad = (float)item.Cantidad,
                    precioUnitario = (float)item.PrecioUnitario,
                    impuestoAplicado = item.AplicaImpuesto ? (int)(_tasaImpuestos * 100) : 0,
                    Producto = item.Producto.ToDto()
                }).ToList()
            };
        }

        // Add the following method to the ViewModelBase class to fix the CS0103 error.  
        protected bool SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    // ViewModel para items de la orden
    public class OrderItemViewModel : ViewModelBase
    {
        private readonly double _tasaImpuestos;

        public Producto Producto { get; }
        public string Nombre => Producto.nombreProducto;
        public decimal PrecioUnitario => (decimal)Producto.precio;
        public bool AplicaImpuesto => Producto.aplicaImpuesto;

        private int _cantidad = 1;
        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (SetProperty(ref _cantidad, value))
                {
                    OnPropertyChanged(nameof(Subtotal));
                    OnPropertyChanged(nameof(Impuestos));
                }
            }
        }

        public decimal Subtotal => PrecioUnitario * Cantidad;
        public decimal Impuestos => AplicaImpuesto ? Subtotal * (decimal)_tasaImpuestos : 0;
        public decimal Total => Subtotal + Impuestos;

        public OrderItemViewModel(Producto producto, double tasaImpuestos)
        {
            Producto = producto;
            _tasaImpuestos = tasaImpuestos;
        }

        // Add the following method to the ViewModelBase class to fix the CS0103 error.  
        protected bool SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}