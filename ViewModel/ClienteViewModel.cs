using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;

using LPAC___Proyecto_II_frontend.Models;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Commands;

namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class ClienteViewModel : ViewModelBase
    {
        public ObservableCollection<Cliente> ClientesEncontrados { get; set; }

        private Cliente _clienteSeleccionado;
        private Cliente _clienteActual;
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
                    if (_clienteSeleccionado != null)
                    {
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
            MensajeColor = Brushes.Black;

            BuscarClientesCommand = new RelayCommand(ExecuteBuscarClientes);
            NuevoClienteCommand = new RelayCommand(ExecuteNuevoCliente);
            GuardarClienteCommand = new RelayCommand(ExecuteGuardarCliente, CanExecuteGuardarCliente);
            EditarClienteCommand = new RelayCommand(ExecuteEditarCliente, CanExecuteEditarCliente);
            EliminarClienteCommand = new RelayCommand(ExecuteEliminarCliente, CanExecuteEliminarCliente);
            LimpiarFormularioCommand = new RelayCommand(ExecuteLimpiarFormulario);

            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                CargarClientesDePrueba();
            }
        }

        private void CargarClientesDePrueba()
        {
            ClientesEncontrados.Add(new Cliente(1, "Empresa A", "Juan", "Pérez", "Gerente", "Calle 1", "San José", "San José", "10101", "CR", "2222-3333", "2222-4444"));
            ClientesEncontrados.Add(new Cliente(2, "Compañía B", "María", "González", "Asistente", "Av. 2", "Heredia", "Heredia", "20202", "CR", "8888-7777", "8888-6666"));
            ClientesEncontrados.Add(new Cliente(3, "Distribuidora C", "Pedro", "Rodríguez", "Vendedor", "Ruta 3", "Alajuela", "Alajuela", "30303", "CR", "9999-1111", "9999-2222"));
        }

        private void ExecuteBuscarClientes(object parameter)
        {
            MensajeEstado = "Buscando clientes...";
            MensajeColor = Brushes.Blue;
            ClientesEncontrados.Clear();

            foreach (var cliente in GetTodosLosClientesDePrueba())
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
            ObservableCollection<Cliente> todos = new ObservableCollection<Cliente>();
            todos.Add(new Cliente(1, "Empresa A", "Juan", "Pérez", "Gerente", "Calle 1", "San José", "San José", "10101", "CR", "2222-3333", "2222-4444"));
            todos.Add(new Cliente(2, "Compañía B", "María", "González", "Asistente", "Av. 2", "Heredia", "Heredia", "20202", "CR", "8888-7777", "8888-6666"));
            todos.Add(new Cliente(3, "Distribuidora C", "Pedro", "Rodríguez", "Vendedor", "Ruta 3", "Alajuela", "Alajuela", "30303", "CR", "9999-1111", "9999-2222"));
            todos.Add(new Cliente(4, "Soluciones S.A.", "Ana", "Díaz", "Directora", "Barrio Norte", "Cartago", "Cartago", "40404", "CR", "7777-5555", "7777-4444"));
            return todos;
        }

        private void ExecuteNuevoCliente(object parameter)
        {
            ClienteActual = new Cliente();
            ClienteSeleccionado = null;
            MensajeEstado = "Listo para crear un nuevo cliente.";
            MensajeColor = Brushes.Black;
        }

        private void ExecuteGuardarCliente(object parameter)
        {
            if (ClienteActual.ClienteId == 0)
            {
                MensajeEstado = "Guardando nuevo cliente...";
                MensajeColor = Brushes.Blue;
                ClienteActual.ClienteId = ClientesEncontrados.Count > 0 ? ClientesEncontrados.Max(c => c.ClienteId) + 1 : 1;
                ClientesEncontrados.Add(ClienteActual);
                MensajeEstado = $"Cliente '{ClienteActual.NombreCompania}' guardado con éxito.";
                MensajeColor = Brushes.Green;
            }
            else
            {
                MensajeEstado = "Actualizando cliente existente...";
                MensajeColor = Brushes.Blue;
                var clienteExistente = ClientesEncontrados.FirstOrDefault(c => c.ClienteId == ClienteActual.ClienteId);
                if (clienteExistente != null)
                {
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
            ExecuteLimpiarFormulario(null);
        }

        private bool CanExecuteGuardarCliente(object parameter)
        {
            return ClienteActual != null && !string.IsNullOrWhiteSpace(ClienteActual.NombreCompania);
        }

        private void ExecuteEditarCliente(object parameter)
        {
            if (ClienteActual != null && ClienteActual.ClienteId != 0)
            {
                MensajeEstado = $"Editando cliente: {ClienteActual.NombreCompania}";
                MensajeColor = Brushes.Black;
            }
            else
            {
                MensajeEstado = "Seleccione un cliente para editar.";
                MensajeColor = Brushes.Red;
            }
        }

        private bool CanExecuteEditarCliente(object parameter)
        {
            return ClienteActual != null && ClienteActual.ClienteId != 0;
        }

        private void ExecuteEliminarCliente(object parameter)
        {
            if (ClienteActual != null && ClienteActual.ClienteId != 0)
            {
                MensajeEstado = $"Eliminando cliente '{ClienteActual.NombreCompania}'...";
                MensajeColor = Brushes.OrangeRed;
                var clienteARemover = ClientesEncontrados.FirstOrDefault(c => c.ClienteId == ClienteActual.ClienteId);
                if (clienteARemover != null)
                {
                    ClientesEncontrados.Remove(clienteARemover);
                    MensajeEstado = $"Cliente '{ClienteActual.NombreCompania}' eliminado con éxito.";
                    MensajeColor = Brushes.Green;
                    ExecuteLimpiarFormulario(null);
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
            return ClienteActual != null && ClienteActual.ClienteId != 0;
        }

        private void ExecuteLimpiarFormulario(object parameter)
        {
            ClienteActual = new Cliente();
            ClienteSeleccionado = null;
            MensajeEstado = "Formulario limpio y listo para un nuevo cliente o búsqueda.";
            MensajeColor = Brushes.Black;
        }
    }
}