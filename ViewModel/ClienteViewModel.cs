using System;
using System.Collections.ObjectModel; // Para ObservableCollection
using System.ComponentModel; // Para INotifyPropertyChanged y DesignerProperties
using System.Linq; // Para el .FirstOrDefault() y .Max() en los ejemplos
using System.Windows.Input; // Para ICommand
using System.Windows.Media; // Para SolidColorBrush (el color del mensaje)
using System.Windows.Threading; // Necesario para DependencyObject si no está ya presente
using System.Windows; // Necesario para DependencyObject si no está ya presente

// Necesitará referenciar sus modelos del frontend. Asumo que están en una carpeta 'Models'.
// y sus Helpers/Commands.
using LPAC___Proyecto_II_frontend.Models; // Asegúrese que este namespace sea correcto para sus Models
using LPAC___Proyecto_II_frontend.Helpers; // Asegúrese que este namespace sea correcto para ViewModelBase
using LPAC___Proyecto_II_frontend.Commands; // Asegúrese que este namespace sea correcto para RelayCommand

namespace LPAC___Proyecto_II_frontend.ViewModel // ¡Este namespace debe coincidir con el del XAML!
{
    public class ClienteViewModel : ViewModelBase
    {
        // Colección para mostrar los clientes en el DataGrid
        public ObservableCollection<Cliente> ClientesEncontrados { get; set; }

        private Cliente _clienteSeleccionado;
        private Cliente _clienteActual; // Para los campos del formulario de CRUD
        private string _textoBusqueda;
        private string _mensajeEstado;
        private SolidColorBrush _mensajeColor;

        public Cliente ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set
            {
                if (_clienteSeleccionado != value)
                {
                    _clienteSeleccionado = value;
                    OnPropertyChanged(nameof(ClienteSeleccionado));
                    // Cuando se selecciona un cliente en el DataGrid, lo cargamos en el formulario
                    if (_clienteSeleccionado != null)
                    {
                        // Clonar para no modificar directamente el objeto de la lista en el DataGrid
                        // Esto crea una nueva instancia con los mismos valores para editar.
                        ClienteActual = (Cliente)Activator.CreateInstance(typeof(Cliente),
                            _clienteSeleccionado.ClienteId,
                            _clienteSeleccionado.NombreCompania,
                            _clienteSeleccionado.NombreContacto,
                            _clienteSeleccionado.ApellidoContacto,
                            _clienteSeleccionado.PuestoContacto,
                            _clienteSeleccionado.Direccion,
                            _clienteSeleccionado.Ciudad,
                            _clienteSeleccionado.Provincia,
                            _clienteSeleccionado.CodigoPostal,
                            _clienteSeleccionado.Pais,
                            _clienteSeleccionado.Telefono,
                            _clienteSeleccionado.NumFax);
                    }
                    else
                    {
                        ClienteActual = new Cliente(); // Si se deselecciona, limpiar el formulario
                    }
                }
            }
        }

        public Cliente ClienteActual
        {
            get => _clienteActual;
            set
            {
                if (_clienteActual != value)
                {
                    _clienteActual = value;
                    OnPropertyChanged(nameof(ClienteActual));
                    // Esto para asegurar que los botones de acción se actualicen
                    ((RelayCommand)GuardarClienteCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)EditarClienteCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)EliminarClienteCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                if (_textoBusqueda != value)
                {
                    _textoBusqueda = value;
                    OnPropertyChanged(nameof(TextoBusqueda));
                }
            }
        }

        public string MensajeEstado
        {
            get => _mensajeEstado;
            set
            {
                if (_mensajeEstado != value)
                {
                    _mensajeEstado = value;
                    OnPropertyChanged(nameof(MensajeEstado));
                }
            }
        }

        public SolidColorBrush MensajeColor
        {
            get => _mensajeColor;
            set
            {
                if (_mensajeColor != value)
                {
                    _mensajeColor = value;
                    OnPropertyChanged(nameof(MensajeColor));
                }
            }
        }

        // Comandos
        public ICommand BuscarClientesCommand { get; private set; }
        public ICommand NuevoClienteCommand { get; private set; }
        public ICommand GuardarClienteCommand { get; private set; }
        public ICommand EditarClienteCommand { get; private set; }
        public ICommand EliminarClienteCommand { get; private set; }
        public ICommand LimpiarFormularioCommand { get; private set; }


        public ClienteViewModel()
        {
            ClientesEncontrados = new ObservableCollection<Cliente>();
            ClienteActual = new Cliente();
            MensajeColor = Brushes.Black; // Color por defecto

            // Inicializar comandos
            BuscarClientesCommand = new RelayCommand(ExecuteBuscarClientes);
            NuevoClienteCommand = new RelayCommand(ExecuteNuevoCliente);
            GuardarClienteCommand = new RelayCommand(ExecuteGuardarCliente, CanExecuteGuardarCliente);
            EditarClienteCommand = new RelayCommand(ExecuteEditarCliente, CanExecuteEditarCliente);
            EliminarClienteCommand = new RelayCommand(ExecuteEliminarCliente, CanExecuteEliminarCliente);
            LimpiarFormularioCommand = new RelayCommand(ExecuteLimpiarFormulario);

            // *** CAMBIO APLICADO AQUÍ: Verificación para el modo diseño ***
            // Solo cargamos datos de prueba si NO estamos en modo diseño.
            // En modo de diseño, queremos un constructor simple que no falle.
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                // Este código solo se ejecutará cuando la aplicación corra de verdad,
                // no cuando el diseñador de Visual Studio intente renderizar la ventana.
                CargarClientesDePrueba();
            }
            // *** FIN DEL CAMBIO ***
        }

        private void CargarClientesDePrueba()
        {
            // Esto es solo para que vea algo en el DataGrid al inicio.
            // En un proyecto real, esto llamaría a su ClienteService para obtener los datos.
            ClientesEncontrados.Add(new Cliente(1, "Empresa A", "Juan", "Pérez", "Gerente", "Calle 1", "San José", "San José", "10101", "CR", "2222-3333", "2222-4444"));
            ClientesEncontrados.Add(new Cliente(2, "Compañía B", "María", "González", "Asistente", "Av. 2", "Heredia", "Heredia", "20202", "CR", "8888-7777", "8888-6666"));
            ClientesEncontrados.Add(new Cliente(3, "Distribuidora C", "Pedro", "Rodríguez", "Vendedor", "Ruta 3", "Alajuela", "Alajuela", "30303", "CR", "9999-1111", "9999-2222"));
        }

        private void ExecuteBuscarClientes(object parameter)
        {
            MensajeEstado = "Buscando clientes...";
            MensajeColor = Brushes.Blue;
            ClientesEncontrados.Clear();

            // Aquí iría la lógica para llamar a su ClienteService.GetClientes(TextoBusqueda)
            // y llenar la colección ClientesEncontrados con los resultados de su backend.
            // Por ahora, un filtro simple de ejemplo:
            foreach (var cliente in GetTodosLosClientesDePrueba()) // Supongamos que esta función trae todos los clientes
            {
                if (string.IsNullOrWhiteSpace(TextoBusqueda) ||
                    cliente.NombreCompania.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    cliente.NombreContacto.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    cliente.ApellidoContacto.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase))
                {
                    ClientesEncontrados.Add(cliente);
                }
            }

            MensajeEstado = $"Búsqueda completada. {ClientesEncontrados.Count} clientes encontrados.";
            MensajeColor = Brushes.Green;
        }

        private ObservableCollection<Cliente> GetTodosLosClientesDePrueba()
        {
            // Simula la base de datos completa de clientes
            ObservableCollection<Cliente> todos = new ObservableCollection<Cliente>();
            todos.Add(new Cliente(1, "Empresa A", "Juan", "Pérez", "Gerente", "Calle 1", "San José", "San José", "10101", "CR", "2222-3333", "2222-4444"));
            todos.Add(new Cliente(2, "Compañía B", "María", "González", "Asistente", "Av. 2", "Heredia", "Heredia", "20202", "CR", "8888-7777", "8888-6666"));
            todos.Add(new Cliente(3, "Distribuidora C", "Pedro", "Rodríguez", "Vendedor", "Ruta 3", "Alajuela", "Alajuela", "30303", "CR", "9999-1111", "9999-2222"));
            todos.Add(new Cliente(4, "Soluciones S.A.", "Ana", "Díaz", "Directora", "Barrio Norte", "Cartago", "Cartago", "40404", "CR", "7777-5555", "7777-4444"));
            return todos;
        }


        private void ExecuteNuevoCliente(object parameter)
        {
            ClienteActual = new Cliente(); // Limpia el formulario para un nuevo cliente
            ClienteSeleccionado = null; // Deselecciona cualquier cliente en el DataGrid
            MensajeEstado = "Listo para crear un nuevo cliente.";
            MensajeColor = Brushes.Black;
        }

        private void ExecuteGuardarCliente(object parameter)
        {
            if (ClienteActual.ClienteId == 0) // Es un nuevo cliente
            {
                MensajeEstado = "Guardando nuevo cliente...";
                MensajeColor = Brushes.Blue;
                // Lógica real: Llamar a su ClienteService.CreateCliente(ClienteActual)
                // y esperar la respuesta de su API REST.
                // Simulación de guardado:
                ClienteActual.ClienteId = ClientesEncontrados.Count > 0 ? ClientesEncontrados.Max(c => c.ClienteId) + 1 : 1; // Asignar un ID temporal para la simulación
                ClientesEncontrados.Add(ClienteActual);
                MensajeEstado = $"Cliente '{ClienteActual.NombreCompania}' guardado con éxito.";
                MensajeColor = Brushes.Green;
            }
            else // Es un cliente existente (editar)
            {
                MensajeEstado = "Actualizando cliente existente...";
                MensajeColor = Brushes.Blue;
                // Lógica real: Llamar a su ClienteService.UpdateCliente(ClienteActual)
                // y esperar la respuesta de su API REST.
                // Simulación de actualización: encontrar y reemplazar en la colección
                var clienteExistente = ClientesEncontrados.FirstOrDefault(c => c.ClienteId == ClienteActual.ClienteId);
                if (clienteExistente != null)
                {
                    // Copiar propiedades del ClienteActual al clienteExistente para actualizar la lista
                    clienteExistente.NombreCompania = ClienteActual.NombreCompania;
                    clienteExistente.NombreContacto = ClienteActual.NombreContacto;
                    clienteExistente.ApellidoContacto = ClienteActual.ApellidoContacto;
                    clienteExistente.PuestoContacto = ClienteActual.PuestoContacto;
                    clienteExistente.Direccion = ClienteActual.Direccion;
                    clienteExistente.Ciudad = ClienteActual.Ciudad;
                    clienteExistente.Provincia = ClienteActual.Provincia;
                    clienteExistente.CodigoPostal = ClienteActual.CodigoPostal;
                    clienteExistente.Pais = ClienteActual.Pais;
                    clienteExistente.Telefono = ClienteActual.Telefono;
                    clienteExistente.NumFax = ClienteActual.NumFax;
                    // Notificar cambios al ObservableCollection para que el DataGrid se actualice
                    // ClientesEncontrados.Remove(clienteExistente); // Podría hacer esto si el binding no se actualiza,
                    // ClientesEncontrados.Add(clienteExistente);   // o una mejor práctica es notificar a nivel de la propiedad en Cliente.
                }
                MensajeEstado = $"Cliente '{ClienteActual.NombreCompania}' actualizado con éxito.";
                MensajeColor = Brushes.Green;
            }
            ExecuteLimpiarFormulario(null); // Limpiar el formulario después de guardar/actualizar
        }

        private bool CanExecuteGuardarCliente(object parameter)
        {
            // Se puede guardar si el nombre de la compañía no está vacío.
            // Aquí podría agregar más validaciones del formulario (ej. otros campos requeridos).
            return ClienteActual != null && !string.IsNullOrWhiteSpace(ClienteActual.NombreCompania);
        }

        private void ExecuteEditarCliente(object parameter)
        {
            // La lógica de "editar" se maneja principalmente por la selección en el DataGrid
            // y la carga automática en ClienteActual. Este botón podría usarse para
            // habilitar campos si fueran deshabilitados por defecto, o simplemente
            // para que el usuario tenga una acción explícita de "Editar".
            if (ClienteActual != null && ClienteActual.ClienteId != 0)
            {
                MensajeEstado = $"Editando cliente: {ClienteActual.NombreCompania}";
                MensajeColor = Brushes.Black;
                // Si la UI deshabilita campos al seleccionar, aquí los re-habilitaría.
            }
            else
            {
                MensajeEstado = "Seleccione un cliente para editar.";
                MensajeColor = Brushes.Red;
            }
        }

        private bool CanExecuteEditarCliente(object parameter)
        {
            // Se puede "editar" si hay un cliente seleccionado en el formulario.
            return ClienteActual != null && ClienteActual.ClienteId != 0;
        }

        private void ExecuteEliminarCliente(object parameter)
        {
            if (ClienteActual != null && ClienteActual.ClienteId != 0)
            {
                MensajeEstado = $"Eliminando cliente '{ClienteActual.NombreCompania}'...";
                MensajeColor = Brushes.OrangeRed;
                // Lógica real: Llamar a su ClienteService.DeleteCliente(ClienteActual.ClienteId)
                // y esperar la respuesta de su API REST.
                // Simulación de eliminación:
                var clienteARemover = ClientesEncontrados.FirstOrDefault(c => c.ClienteId == ClienteActual.ClienteId);
                if (clienteARemover != null)
                {
                    ClientesEncontrados.Remove(clienteARemover); // Lo quita de la lista mostrada
                    MensajeEstado = $"Cliente '{ClienteActual.NombreCompania}' eliminado con éxito.";
                    MensajeColor = Brushes.Green;
                    ExecuteLimpiarFormulario(null); // Limpia el formulario después de eliminar
                }
                else
                {
                    MensajeEstado = "No se encontró el cliente para eliminar en la lista actual.";
                    MensajeColor = Brushes.Red;
                }
            }
            else
            {
                MensajeEstado = "Seleccione un cliente para eliminar.";
                MensajeColor = Brushes.Red;
            }
        }

        private bool CanExecuteEliminarCliente(object parameter)
        {
            // Se puede eliminar si hay un cliente seleccionado en el formulario.
            return ClienteActual != null && ClienteActual.ClienteId != 0;
        }

        private void ExecuteLimpiarFormulario(object parameter)
        {
            ClienteActual = new Cliente(); // Reinicia el objeto ClienteActual (limpia los campos)
            ClienteSeleccionado = null; // Deselecciona cualquier elemento en el DataGrid
            MensajeEstado = "Formulario limpio y listo para un nuevo cliente o búsqueda.";
            MensajeColor = Brushes.Black;
        }
    }
}