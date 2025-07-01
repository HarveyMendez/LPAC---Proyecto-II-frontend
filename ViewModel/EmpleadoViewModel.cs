// EmpleadoViewModel.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LPAC___Proyecto_II_frontend.Models;      // Para Empleado, Departamento, Rol Models
using LPAC___Proyecto_II_frontend.DTOs;        // Para DTOs como EmpleadoDTO, DepartamentoDTO, RolDTO
using LPAC___Proyecto_II_frontend.Services;    // Para los servicios
using LPAC___Proyecto_II_frontend.Helpers;     // Para ViewModelBase (asumiendo que sigue allí)
using LPAC___Proyecto_II_frontend.Commands;    // Para tu RelayCommand

namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class EmpleadoViewModel : ViewModelBase
    {
        private readonly EmpleadoService _empleadoService;
        private readonly DepartamentoService _departamentoService;
        private readonly RolService _rolService;

        private Empleado _selectedEmpleado = new Empleado();
        public Empleado SelectedEmpleado
        {
            get => _selectedEmpleado;
            set
            {
                if (_selectedEmpleado != value)
                {
                    _selectedEmpleado = value;
                    OnPropertyChanged(nameof(SelectedEmpleado));
                    OnPropertyChanged(nameof(IsEmpleadoSelected));

                    // Al seleccionar un empleado, actualizamos la selección en los ComboBoxes.
                    // Departamento: _selectedEmpleado.Departamento es DTO, SelectedDepartamento es Modelo. Convertir DTO a Modelo.
                    SelectedDepartamento = _selectedEmpleado?.Departamento != null
                                            ? new Departamento().FromDto(_selectedEmpleado.Departamento)
                                            : null;
                    // Rol: _selectedEmpleado.Rol es DTO, SelectedRol es DTO. Asignación directa.
                    SelectedRol = _selectedEmpleado?.Rol;
                }
            }
        }
        public bool IsEmpleadoSelected => SelectedEmpleado != null && SelectedEmpleado.IdEmpleado != 0;


        public ObservableCollection<Empleado> Empleados { get; set; } = new ObservableCollection<Empleado>();

        // Departamentos: Se mantiene como Modelo porque DepartamentoService devuelve Modelos.
        public ObservableCollection<Departamento> Departamentos { get; set; } = new ObservableCollection<Departamento>();

        // CAMBIO: Roles ahora es de DTOs, consistente con RolService.GetAllRolesAsync()
        public ObservableCollection<RolDTO> Roles { get; set; } = new ObservableCollection<RolDTO>();


        // Se mantiene como Modelo, consistente con la colección Departamentos.
        private Departamento _selectedDepartamento;
        public Departamento SelectedDepartamento
        {
            get => _selectedDepartamento;
            set
            {
                if (_selectedDepartamento != value)
                {
                    _selectedDepartamento = value;
                    OnPropertyChanged(nameof(SelectedDepartamento));
                    if (SelectedEmpleado != null)
                    {
                        // Cuando se selecciona un Departamento en el ComboBox,
                        // asignamos su DTO (convertido del Modelo) a la propiedad del Empleado.
                        SelectedEmpleado.Departamento = value?.ToDto();
                    }
                }
            }
        }

        // CAMBIO: SelectedRol ahora es de DTOs, consistente con la colección Roles.
        private RolDTO _selectedRol;
        public RolDTO SelectedRol
        {
            get => _selectedRol;
            set
            {
                if (_selectedRol != value)
                {
                    _selectedRol = value;
                    OnPropertyChanged(nameof(SelectedRol));
                    if (SelectedEmpleado != null)
                    {
                        // Cuando se selecciona un Rol en el ComboBox,
                        // asignamos directamente (ya es DTO).
                        SelectedEmpleado.Rol = value;
                    }
                }
            }
        }


        public ICommand LoadEmpleadosCommand { get; }
        public ICommand SaveEmpleadoCommand { get; }
        public ICommand DeleteEmpleadoCommand { get; }
        public ICommand NewEmpleadoCommand { get; }
        public ICommand CancelEditCommand { get; }

        public EmpleadoViewModel()
        {
            _empleadoService = new EmpleadoService();
            _departamentoService = new DepartamentoService();
            _rolService = new RolService();

            LoadEmpleadosCommand = new RelayCommand(async (parameter) => await LoadEmpleadosAsync());
            // FIX: Ahora SI se convierte SelectedEmpleado a DTO porque EmpleadoService espera DTO.
            SaveEmpleadoCommand = new RelayCommand(async (parameter) => await SaveEmpleadoAsync(), (parameter) => CanSaveEmpleado());
            DeleteEmpleadoCommand = new RelayCommand(async (parameter) => await DeleteEmpleadoAsync(), (parameter) => IsEmpleadoSelected);
            NewEmpleadoCommand = new RelayCommand((parameter) => NewEmpleado());
            CancelEditCommand = new RelayCommand((parameter) => CancelEdit());

            _ = LoadInitialDataAsync();
        }

        private async Task LoadInitialDataAsync()
        {
            await LoadDepartamentosAsync();
            await LoadRolesAsync();
            await LoadEmpleadosAsync();
        }

        private async Task LoadEmpleadosAsync()
        {
            try
            {
                var empleados = await _empleadoService.GetEmpleadosAsync(); // Retorna List<Empleado> (Modelos)
                Empleados.Clear();
                foreach (var empleado in empleados)
                {
                    Empleados.Add(empleado);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar empleados: {ex.Message}");
                // TODO: Implementar lógica de notificación al usuario (ej. MessageBox.Show)
            }
        }

        // Se añaden Modelos directamente, ya que DepartamentoService.GetAllDepartamentosAsync() los devuelve.
        private async Task LoadDepartamentosAsync()
        {
            try
            {
                var departamentos = await _departamentoService.GetAllDepartamentosAsync(); // Retorna List<Departamento>
                Departamentos.Clear();
                foreach (var deptoModel in departamentos)
                {
                    Departamentos.Add(deptoModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar departamentos: {ex.Message}");
                // TODO: Implementar lógica de notificación al usuario
            }
        }

        // CAMBIO: Ahora se añaden DTOs directamente, ya que RolService.GetAllRolesAsync() los devuelve.
        private async Task LoadRolesAsync()
        {
            try
            {
                var roles = await _rolService.GetAllRolesAsync(); // Retorna List<RolDTO>
                Roles.Clear();
                foreach (var rolDto in roles)
                {
                    Roles.Add(rolDto); // Añade el DTO directamente
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar roles: {ex.Message}");
                // TODO: Implementar lógica de notificación al usuario
            }
        }

        private async Task SaveEmpleadoAsync()
        {
            if (SelectedEmpleado == null) return;

            try
            {
                // CAMBIO CLAVE: Convertir SelectedEmpleado (Modelo) a EmpleadoDTO
                var empleadoDtoToSave = SelectedEmpleado.ToDto();

                if (SelectedEmpleado.IdEmpleado == 0) // Nuevo empleado
                {
                    await _empleadoService.CreateEmpleadoAsync(empleadoDtoToSave);
                }
                else // Actualizar empleado existente
                {
                    await _empleadoService.UpdateEmpleadoAsync(empleadoDtoToSave);
                }
                await LoadEmpleadosAsync();
                NewEmpleado();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar empleado: {ex.Message}");
                // TODO: Implementar lógica de notificación al usuario
            }
        }

        private bool CanSaveEmpleado()
        {
            return SelectedEmpleado != null &&
                   !string.IsNullOrWhiteSpace(SelectedEmpleado.NombreEmpleado) &&
                   !string.IsNullOrWhiteSpace(SelectedEmpleado.ApellidosEmpleado) &&
                   !string.IsNullOrWhiteSpace(SelectedEmpleado.Puesto) &&
                   SelectedEmpleado.Departamento != null &&
                   SelectedEmpleado.Rol != null;
        }

        private async Task DeleteEmpleadoAsync()
        {
            if (SelectedEmpleado == null || SelectedEmpleado.IdEmpleado == 0) return;

            try
            {
                await _empleadoService.DeleteEmpleadoAsync(SelectedEmpleado.IdEmpleado);
                await LoadEmpleadosAsync();
                NewEmpleado();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar empleado: {ex.Message}");
                // TODO: Implementar lógica de notificación al usuario
            }
        }

        private void NewEmpleado()
        {
            SelectedEmpleado = new Empleado();
            SelectedDepartamento = null;
            SelectedRol = null;
        }

        private void CancelEdit()
        {
            NewEmpleado();
        }
    }
}