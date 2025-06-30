using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using System.Threading.Tasks; // Required for Task and async/await
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using LPAC___Proyecto_II_frontend.Models;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Commands;
using LPAC___Proyecto_II_frontend.Services;
namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class ClienteViewModel : ViewModelBase
    {

        // Collection to display clients in the DataGrid
        public ObservableCollection<Cliente> ClientesEncontrados { get; set; }

        private Cliente _clienteSeleccionado;
        private Cliente _clienteActual; // Object bound to the input form
        private string _textoBusqueda = string.Empty;
        private string _mensajeEstado = string.Empty;
        private SolidColorBrush _mensajeColor = Brushes.Green; // Default to Green or a neutral color

        // Instance of the service to interact with the backend
        private readonly ClienteService _clienteService;


        // Public properties with change notification
        public Cliente ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set
            {
                if (_clienteSeleccionado != value)
                {
                    _clienteSeleccionado = value;
                    OnPropertyChanged(nameof(ClienteSeleccionado));


                    // Logic to load the selected client into the form for editing.
                    // This is done by assigning a clone to ClienteActual.
                    if (_clienteSeleccionado != null)
                    {
                        // Creates a new instance and copies the properties. This prevents editing the item in the collection directly.
                        ClienteActual = new Cliente
                        {
                            ClienteId = _clienteSeleccionado.ClienteId,
                            NombreCompania = _clienteSeleccionado.NombreCompania,
                            NombreContacto = _clienteSeleccionado.NombreContacto,
                            ApellidoContacto = _clienteSeleccionado.ApellidoContacto,
                            PuestoContacto = _clienteSeleccionado.PuestoContacto,
                            Direccion = _clienteSeleccionado.Direccion,
                            Ciudad = _clienteSeleccionado.Ciudad,
                            Provincia = _clienteSeleccionado.Provincia,
                            CodigoPostal = _clienteSeleccionado.CodigoPostal,
                            Pais = _clienteSeleccionado.Pais,
                            Telefono = _clienteSeleccionado.Telefono,
                            NumFax = _clienteSeleccionado.NumFax
                        };
                        MensajeEstado = $"Cliente '{_clienteSeleccionado.NombreCompania}' seleccionado para editar.";
                        MensajeColor = Brushes.Black;
                    }
                    else
                    {
                        // If deselected, clear the form.

                        ClienteActual = new Cliente();
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

                    // Trigger the CanExecute logic for all commands when ClienteActual changes
                    UpdateCommandStates();

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
                    // Explicitly notify change of color to ensure UI updates
                    OnPropertyChanged(nameof(MensajeColor));
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


        public ICommand BuscarClientesCommand { get; private set; }
        public ICommand NuevoClienteCommand { get; private set; }
        public ICommand GuardarClienteCommand { get; private set; }
        public ICommand EliminarClienteCommand { get; private set; }
        public ICommand LimpiarFormularioCommand { get; private set; }


        public ClienteViewModel()
        {
            _clienteService = new ClienteService();
            ClientesEncontrados = new ObservableCollection<Cliente>();


            // Initialize commands first to avoid null checks in CanExecute
            BuscarClientesCommand = new RelayCommand(async (p) => await ExecuteBuscarClientes(p));

            NuevoClienteCommand = new RelayCommand(ExecuteNuevoCliente);
            GuardarClienteCommand = new RelayCommand(async (p) => await ExecuteGuardarCliente(p), CanExecuteGuardarCliente);
            EliminarClienteCommand = new RelayCommand(async (p) => await ExecuteEliminarCliente(p), CanExecuteEliminarCliente);
            LimpiarFormularioCommand = new RelayCommand(ExecuteLimpiarFormulario);


            // Initialize ClienteActual after commands
            ClienteActual = new Cliente();
            MensajeColor = Brushes.Green;

            // Load clients on startup (fire-and-forget) if not in design mode
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                _ = LoadClientesAsync();

            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                CargarClientesDePrueba();

            }
        }

        /// <summary>
        /// Loads all clients on application startup.
        /// </summary>
        private async Task LoadClientesAsync()
        {
            await ExecuteBuscarClientes(null);

        }

        /// <summary>
        /// Method to update the state of the commands (enable/disable buttons).
        /// </summary>
        private void UpdateCommandStates()
        {
            ((RelayCommand)GuardarClienteCommand).RaiseCanExecuteChanged();
            ((RelayCommand)EliminarClienteCommand).RaiseCanExecuteChanged();
        }

        // --- Command implementations (now with asynchronous calls) ---

        private async Task ExecuteBuscarClientes(object parameter)
        {
            MensajeEstado = "Buscando clientes...";
            MensajeColor = Brushes.Blue;


            try
            {
                // Call the service to get data from the database.
                var clientes = await _clienteService.GetClientesAsync(TextoBusqueda);

                // Clear the collection and fill it with the service results.
                ClientesEncontrados.Clear();
                foreach (var cliente in clientes)
                {
                    ClientesEncontrados.Add(cliente);
                }


                MensajeEstado = $"Búsqueda completada. {ClientesEncontrados.Count} clientes encontrados.";
                MensajeColor = Brushes.Green;
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al buscar clientes: {ex.Message}";
                MensajeColor = Brushes.Red;
            }
        }

        private void ExecuteNuevoCliente(object parameter)
        {

            ExecuteLimpiarFormulario(null);
            MensajeEstado = "Listo para crear un nuevo cliente.";
            MensajeColor = Brushes.Black;
        }

        private async Task ExecuteGuardarCliente(object parameter)
        {

            // Basic validation before saving
            if (string.IsNullOrWhiteSpace(ClienteActual.NombreCompania))
            {
                MensajeEstado = "Por favor, ingrese el nombre de la compañía.";
                MensajeColor = Brushes.Red;
                return;
            }

            try
            {
                if (ClienteActual.ClienteId == 0) // It's a new client
                {
                    MensajeEstado = "Guardando nuevo cliente...";
                    MensajeColor = Brushes.Blue;

                    // Call the service to create the client in the database.
                    var nuevoCliente = await _clienteService.CreateClienteAsync(ClienteActual);

                    // If the operation was successful, add the returned client (with the ID assigned by the backend) to the collection.
                    ClientesEncontrados.Add(nuevoCliente);
                    MensajeEstado = $"Cliente '{nuevoCliente.NombreCompania}' guardado con éxito.";
                    MensajeColor = Brushes.Green;
                }
                else // It's an existing client
                {
                    MensajeEstado = "Actualizando cliente existente...";
                    MensajeColor = Brushes.Blue;

                    // Call the service to update the client.
                    await _clienteService.UpdateClienteAsync(ClienteActual);

                    // Find the client in the list and update its properties with the form data.
                    var clienteExistente = ClientesEncontrados.FirstOrDefault(c => c.ClienteId == ClienteActual.ClienteId);
                    if (clienteExistente != null)
                    {
                        // Copy the updated properties from the form (ClienteActual).
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
                    }

                    MensajeEstado = $"Cliente '{ClienteActual.NombreCompania}' actualizado con éxito.";
                    MensajeColor = Brushes.Green;
                }
                // Recargar la lista después de guardar.
                await ExecuteBuscarClientes(null);
                // Clean the form after saving.
                ExecuteLimpiarFormulario(null);

            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al guardar el cliente: {ex.Message}";
                MensajeColor = Brushes.Red;
            }

        }

        private bool CanExecuteGuardarCliente(object parameter)
        {

            return ClienteActual != null && !string.IsNullOrWhiteSpace(ClienteActual.NombreCompania);
        }

        private async Task ExecuteEliminarCliente(object parameter)
        {

            if (ClienteActual == null || ClienteActual.ClienteId == 0)

            {
                MensajeEstado = "Seleccione un cliente para eliminar.";
                MensajeColor = Brushes.Red;
                return;
            }


            MensajeEstado = $"Eliminando cliente '{ClienteActual.NombreCompania}'...";
            MensajeColor = Brushes.OrangeRed;


            try
            {

                // Call the service to delete the client.
                await _clienteService.DeleteClienteAsync(ClienteActual.ClienteId);

                // Remove the client from the local collection.

                var clienteARemover = ClientesEncontrados.FirstOrDefault(c => c.ClienteId == ClienteActual.ClienteId);
                if (clienteARemover != null)
                {
                    ClientesEncontrados.Remove(clienteARemover);
                    MensajeEstado = $"Cliente '{ClienteActual.NombreCompania}' eliminado con éxito.";
                    MensajeColor = Brushes.Green;
                    ExecuteLimpiarFormulario(null);

                }
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al eliminar el cliente: {ex.Message}";
                MensajeColor = Brushes.Red;
            }
        }

        private bool CanExecuteEliminarCliente(object parameter)
        {

            return ClienteActual != null && ClienteActual.ClienteId != 0;
        }

        private void ExecuteLimpiarFormulario(object parameter)
        {

            // This method creates a new instance of Cliente to clear the form fields.
            ClienteActual = new Cliente();
            ClienteSeleccionado = null; // Also clear the selection in the DataGrid

            MensajeEstado = "Formulario limpio y listo para un nuevo cliente o búsqueda.";
            MensajeColor = Brushes.Black;
        }
    }
}