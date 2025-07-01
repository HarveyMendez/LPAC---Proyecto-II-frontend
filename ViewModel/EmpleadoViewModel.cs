using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using LPAC___Proyecto_II_frontend.Models; // Models namespace for Empleado
using LPAC___Proyecto_II_frontend.Services;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Commands;
using System.Linq;
using LPAC___Proyecto_II_frontend.DTOs; // DTOs namespace for DepartamentoDTO, RolDTO, EmpleadoDTO
using System;

namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class EmpleadoViewModel : ViewModelBase
    {
        private readonly EmpleadoService _empleadoService;
        private readonly DepartamentoService _departamentoService;
        private readonly RolService _rolService;

        // Collection of Empleado Models for the DataGrid
        private ObservableCollection<Empleado> _empleados;
        public ObservableCollection<Empleado> Empleados
        {
            get => _empleados;
            set
            {
                _empleados = value;
                OnPropertyChanged(nameof(Empleados));
            }
        }

        // The currently selected Empleado Model in the form
        private Empleado _selectedEmpleado;
        public Empleado SelectedEmpleado
        {
            get => _selectedEmpleado;
            set
            {
                _selectedEmpleado = value ?? new Empleado(); // Ensure it's never null
                OnPropertyChanged(nameof(SelectedEmpleado));

                // Logic to synchronize ComboBoxes with SelectedEmpleado
                if (_selectedEmpleado != null)
                {
                    // Department: SelectedEmpleado.Departamento is EmpleadoDTO, Departamentos is ObservableCollection<DepartamentoDTO>
                    if (Departamentos != null && _selectedEmpleado.Departamento != null)
                    {
                        SelectedDepartamento = Departamentos.FirstOrDefault(d => d.codDepartamento == _selectedEmpleado.Departamento.codDepartamento);
                    }
                    else
                    {
                        SelectedDepartamento = null;
                    }

                    // Rol: SelectedEmpleado.Rol is RolDTO, Roles is ObservableCollection<RolDTO>
                    if (Roles != null && _selectedEmpleado.Rol != null)
                    {
                        SelectedRol = Roles.FirstOrDefault(r => r.idRol == _selectedEmpleado.Rol.idRol);
                    }
                    else
                    {
                        SelectedRol = null;
                    }
                }
                else
                {
                    SelectedDepartamento = null;
                    SelectedRol = null;
                }

                if (UpdateEmpleadoCommand is RelayCommand updateCommand)
                {
                    updateCommand.RaiseCanExecuteChanged();
                }
                if (DeleteEmpleadoCommand is RelayCommand deleteCommand)
                {
                    deleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // Collection of DepartamentoDTOs for the Department ComboBox
        private ObservableCollection<DepartamentoDTO> _departamentos;
        public ObservableCollection<DepartamentoDTO> Departamentos
        {
            get => _departamentos;
            set
            {
                _departamentos = value;
                OnPropertyChanged(nameof(Departamentos));
            }
        }

        // Selected DepartamentoDTO from the ComboBox
        private DepartamentoDTO? _selectedDepartamento;
        public DepartamentoDTO? SelectedDepartamento
        {
            get => _selectedDepartamento;
            set
            {
                _selectedDepartamento = value;
                OnPropertyChanged(nameof(SelectedDepartamento));
                // Assign the selected DTO directly to SelectedEmpleado.Departamento (which is already a DTO)
                if (SelectedEmpleado != null)
                {
                    SelectedEmpleado.Departamento = value ?? new DepartamentoDTO();
                }
            }
        }

        // Collection of RolDTOs for the Rol ComboBox
        private ObservableCollection<RolDTO> _roles;
        public ObservableCollection<RolDTO> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }

        // Selected RolDTO from the ComboBox
        private RolDTO? _selectedRol;
        public RolDTO? SelectedRol
        {
            get => _selectedRol;
            set
            {
                _selectedRol = value;
                OnPropertyChanged(nameof(SelectedRol));
                // Assign the selected DTO directly to SelectedEmpleado.Rol (which is already a DTO)
                if (SelectedEmpleado != null)
                {
                    SelectedEmpleado.Rol = value ?? new RolDTO();
                }
            }
        }

        // Commands
        public ICommand LoadEmpleadosCommand { get; }
        public ICommand AddEmpleadoCommand { get; }
        public ICommand UpdateEmpleadoCommand { get; }
        public ICommand DeleteEmpleadoCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        public EmpleadoViewModel()
        {
            _empleadoService = new EmpleadoService();
            _departamentoService = new DepartamentoService();
            _rolService = new RolService();

            _empleados = new ObservableCollection<Empleado>(); // Collection of Models.Empleado
            _departamentos = new ObservableCollection<DepartamentoDTO>(); // Collection of DTOs
            _roles = new ObservableCollection<RolDTO>();             // Collection of DTOs

            _selectedEmpleado = new Empleado(); // Initialize Empleado model

            LoadEmpleadosCommand = new RelayCommand(async (param) => await LoadEmpleados());
            AddEmpleadoCommand = new RelayCommand(async (param) => await AddEmpleado());
            UpdateEmpleadoCommand = new RelayCommand(async (param) => await UpdateEmpleado(), (param) => CanModifyOrDelete());
            DeleteEmpleadoCommand = new RelayCommand(async (param) => await DeleteEmpleado(), (param) => CanModifyOrDelete());
            ClearSelectionCommand = new RelayCommand((param) => ClearSelection());

            _ = LoadInitialData();
        }

        private async Task LoadInitialData()
        {
            await LoadDepartamentos();
            await LoadRoles();
            await LoadEmpleados();
        }

        private async Task LoadEmpleados()
        {
            try
            {
                // ASSUMPTION: _empleadoService.GetEmpleadosAsync() returns IEnumerable<EmpleadoDTO>
                var empleadosDto = await _empleadoService.GetEmpleadosAsync();
                Empleados.Clear();
                foreach (var empDto in empleadosDto)
                {
                    // CONVERSION: Convert EmpleadoDTO to Models.Empleado using FromDto()
                    Empleados.Add(new Empleado().FromDto(empDto));
                }

                if (Empleados.Any())
                {
                    SelectedEmpleado = Empleados.First();
                }
                else
                {
                    SelectedEmpleado = new Empleado(); // Ensures form has an object to bind to
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar empleados: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadDepartamentos()
        {
            try
            {
                // ASSUMPTION: _departamentoService.GetAllDepartamentosAsync() returns IEnumerable<DepartamentoDTO>
                var departamentos = await _departamentoService.GetAllDepartamentosAsync();
                Departamentos.Clear();
                foreach (var depto in departamentos)
                {
                    Departamentos.Add(depto);
                }

                if (SelectedEmpleado != null && SelectedEmpleado.Departamento != null)
                {
                    SelectedDepartamento = Departamentos.FirstOrDefault(d => d.codDepartamento == SelectedEmpleado.Departamento.codDepartamento);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar departamentos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadRoles()
        {
            try
            {
                // ASSUMPTION: _rolService.GetAllRolesAsync() returns IEnumerable<RolDTO>
                var roles = await _rolService.GetAllRolesAsync();
                Roles.Clear();
                foreach (var rol in roles)
                {
                    Roles.Add(rol);
                }

                if (SelectedEmpleado != null && SelectedEmpleado.Rol != null)
                {
                    SelectedRol = Roles.FirstOrDefault(r => r.idRol == SelectedEmpleado.Rol.idRol);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar roles: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddEmpleado()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedEmpleado.NombreEmpleado) ||
                    string.IsNullOrWhiteSpace(SelectedEmpleado.ApellidosEmpleado) ||
                    SelectedEmpleado.Departamento == null || string.IsNullOrWhiteSpace(SelectedEmpleado.Departamento.codDepartamento) ||
                    SelectedEmpleado.Rol == null || SelectedEmpleado.Rol.idRol == 0)
                {
                    MessageBox.Show("Por favor, completá todos los campos obligatorios (Nombre, Apellidos, Departamento, Rol).", "Datos Incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // CONVERSION: Convert Models.Empleado to EmpleadoDTO for the service call
                var nuevoEmpleadoDto = await _empleadoService.CreateEmpleadoAsync(SelectedEmpleado.ToDto());
                if (nuevoEmpleadoDto != null)
                {
                    ClearSelection();
                    MessageBox.Show("Empleado agregado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadEmpleados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar empleado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateEmpleado()
        {
            if (!CanModifyOrDelete())
            {
                MessageBox.Show("Seleccioná un empleado para modificar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(SelectedEmpleado.NombreEmpleado) ||
                    string.IsNullOrWhiteSpace(SelectedEmpleado.ApellidosEmpleado) ||
                    SelectedEmpleado.Departamento == null || string.IsNullOrWhiteSpace(SelectedEmpleado.Departamento.codDepartamento) ||
                    SelectedEmpleado.Rol == null || SelectedEmpleado.Rol.idRol == 0)
                {
                    MessageBox.Show("Por favor, completá todos los campos obligatorios (Nombre, Apellidos, Departamento, Rol).", "Datos Incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // CONVERSION: Convert Models.Empleado to EmpleadoDTO for the service call
                await _empleadoService.UpdateEmpleadoAsync(SelectedEmpleado.ToDto());
                MessageBox.Show("Empleado modificado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearSelection();
                await LoadEmpleados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar empleado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteEmpleado()
        {
            if (!CanModifyOrDelete())
            {
                MessageBox.Show("Seleccioná un empleado para eliminar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Corrected MessageBox.Show arguments
            if (MessageBox.Show($"¿Estás seguro de eliminar a {SelectedEmpleado.NombreEmpleado} {SelectedEmpleado.ApellidosEmpleado}?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _empleadoService.DeleteEmpleadoAsync(SelectedEmpleado.IdEmpleado);
                    Empleados.Remove(Empleados.FirstOrDefault(e => e.IdEmpleado == SelectedEmpleado.IdEmpleado));
                    ClearSelection();
                    MessageBox.Show("Empleado eliminado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar empleado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanModifyOrDelete()
        {
            return SelectedEmpleado != null && SelectedEmpleado.IdEmpleado != 0;
        }

        private void ClearSelection()
        {
            SelectedEmpleado = new Empleado();
            SelectedDepartamento = null;
            SelectedRol = null;
        }
    }
}