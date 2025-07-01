using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LPAC___Proyecto_II_frontend.Models;
using LPAC___Proyecto_II_frontend.Services;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Commands;

namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class DepartamentoViewModel : ViewModelBase
    {
        private readonly DepartamentoService _departamentoService;

        public ObservableCollection<Departamento> Departamentos { get; set; } = new();

        private Departamento _selectedDepartamento;
        public Departamento SelectedDepartamento
        {
            get => _selectedDepartamento;
            set
            {
                _selectedDepartamento = value;
                OnPropertyChanged(nameof(SelectedDepartamento));
                OnPropertyChanged(nameof(IsDepartamentoSelected));
                if (value != null)
                    EditDepartamento = new Departamento().FromDto(value.ToDto());
                else
                    EditDepartamento = new Departamento();
            }
        }

        private Departamento _editDepartamento = new Departamento();
        public Departamento EditDepartamento
        {
            get => _editDepartamento;
            set
            {
                _editDepartamento = value;
                OnPropertyChanged(nameof(EditDepartamento));
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
                _ = BuscarDepartamentosAsync();
            }
        }

        public bool IsDepartamentoSelected => SelectedDepartamento != null && !string.IsNullOrEmpty(SelectedDepartamento.CodDepartamento);

        public ICommand LoadDepartamentosCommand { get; }
        public ICommand SaveDepartamentoCommand { get; }
        public ICommand DeleteDepartamentoCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand BuscarDepartamentosCommand { get; }

        public DepartamentoViewModel()
        {
            _departamentoService = new DepartamentoService();
            LoadDepartamentosCommand = new RelayCommand(async _ => await LoadDepartamentosAsync());
            BuscarDepartamentosCommand = new RelayCommand(async _ => await BuscarDepartamentosAsync());
            SaveDepartamentoCommand = new RelayCommand(async _ => await SaveDepartamentoAsync(), _ => CanSaveDepartamento());
            DeleteDepartamentoCommand = new RelayCommand(async _ => await DeleteDepartamentoAsync(), _ => IsDepartamentoSelected);
            CancelEditCommand = new RelayCommand(_ => CancelEdit());
            SelectedDepartamento = new Departamento();
            EditDepartamento = new Departamento();
            _ = LoadDepartamentosAsync();
        }

        private async Task LoadDepartamentosAsync()
        {
            var departamentos = await _departamentoService.GetAllDepartamentosAsync();
            Departamentos.Clear();
            foreach (var dep in departamentos)
                Departamentos.Add(dep);
            if (!IsDepartamentoSelected)
            {
                SelectedDepartamento = new Departamento();
                EditDepartamento = new Departamento();
            }
        }

        private async Task BuscarDepartamentosAsync()
        {
            var departamentos = await _departamentoService.GetAllDepartamentosAsync();
            var filtro = TextoBusqueda?.Trim().ToLower() ?? string.Empty;
            var filtrados = string.IsNullOrEmpty(filtro)
                ? departamentos
                : departamentos.Where(d => d.CodDepartamento.ToLower().Contains(filtro) || d.NombreDepartamento.ToLower().Contains(filtro)).ToList();
            Departamentos.Clear();
            foreach (var dep in filtrados)
                Departamentos.Add(dep);
        }

        private bool CanSaveDepartamento()
        {
            return EditDepartamento != null &&
                   !string.IsNullOrWhiteSpace(EditDepartamento.CodDepartamento) &&
                   !string.IsNullOrWhiteSpace(EditDepartamento.NombreDepartamento);
        }

        private async Task SaveDepartamentoAsync()
        {
            if (Departamentos.Any(d => d.CodDepartamento == EditDepartamento.CodDepartamento))
            {
                await _departamentoService.UpdateDepartamentoAsync(EditDepartamento);
                await LoadDepartamentosAsync();
            }
            else
            {
                var created = await _departamentoService.CreateDepartamentoAsync(EditDepartamento);
                if (created != null)
                    Departamentos.Add(created);
            }
            SelectedDepartamento = new Departamento();
            EditDepartamento = new Departamento();
        }

        private async Task DeleteDepartamentoAsync()
        {
            if (SelectedDepartamento == null || string.IsNullOrEmpty(SelectedDepartamento.CodDepartamento))
                return;
            await _departamentoService.DeleteDepartamentoAsync(SelectedDepartamento.CodDepartamento);
            var toRemove = Departamentos.FirstOrDefault(d => d.CodDepartamento == SelectedDepartamento.CodDepartamento);
            if (toRemove != null)
                Departamentos.Remove(toRemove);
            SelectedDepartamento = new Departamento();
            EditDepartamento = new Departamento();
        }

        private void CancelEdit()
        {
            EditDepartamento = SelectedDepartamento != null ? new Departamento().FromDto(SelectedDepartamento.ToDto()) : new Departamento();
        }
    }
}
