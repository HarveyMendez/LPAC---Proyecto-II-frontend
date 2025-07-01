using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LPAC___Proyecto_II_frontend.DTOs;
using LPAC___Proyecto_II_frontend.Services;
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.Commands;

namespace LPAC___Proyecto_II_frontend.ViewModel
{
    public class CategoriaViewModel : ViewModelBase
    {
        private readonly CategoriaService _categoriaService;

        public ObservableCollection<CategoriaDTO> Categorias { get; set; } = new();

        private CategoriaDTO _selectedCategoria;
        public CategoriaDTO SelectedCategoria
        {
            get => _selectedCategoria;
            set
            {
                _selectedCategoria = value;
                OnPropertyChanged(nameof(SelectedCategoria));
                OnPropertyChanged(nameof(IsCategoriaSelected));
                if (value != null)
                    EditCategoria = value.Clone();
                else
                    EditCategoria = new CategoriaDTO();
            }
        }

        private CategoriaDTO _editCategoria = new CategoriaDTO();
        public CategoriaDTO EditCategoria
        {
            get => _editCategoria;
            set
            {
                _editCategoria = value;
                OnPropertyChanged(nameof(EditCategoria));
            }
        }

        public bool IsCategoriaSelected => SelectedCategoria != null && !string.IsNullOrEmpty(SelectedCategoria.codCategoria);

        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                _textoBusqueda = value;
                OnPropertyChanged(nameof(TextoBusqueda));
                _ = BuscarCategoriasAsync();
            }
        }

        public ICommand LoadCategoriasCommand { get; }
        public ICommand BuscarCategoriasCommand { get; }
        public ICommand SaveCategoriaCommand { get; }
        public ICommand DeleteCategoriaCommand { get; }
        public ICommand CancelEditCommand { get; }

        public CategoriaViewModel()
        {
            _categoriaService = new CategoriaService();
            LoadCategoriasCommand = new RelayCommand(async _ => await LoadCategoriasAsync());
            BuscarCategoriasCommand = new RelayCommand(async _ => await BuscarCategoriasAsync());
            SaveCategoriaCommand = new RelayCommand(async _ => await SaveCategoriaAsync(), _ => CanSaveCategoria());
            DeleteCategoriaCommand = new RelayCommand(async _ => await DeleteCategoriaAsync(), _ => IsCategoriaSelected);
            CancelEditCommand = new RelayCommand(_ => CancelEdit());
            SelectedCategoria = new CategoriaDTO();
            EditCategoria = new CategoriaDTO();
            _ = LoadCategoriasAsync();
        }

        private async Task LoadCategoriasAsync()
        {
            var categorias = await _categoriaService.GetCategoriasAsync();
            Categorias.Clear();
            foreach (var cat in categorias)
                Categorias.Add(cat);
            if (!IsCategoriaSelected)
                SelectedCategoria = new CategoriaDTO();
        }

        private async Task BuscarCategoriasAsync()
        {
            var categorias = await _categoriaService.GetCategoriasAsync();
            var filtro = TextoBusqueda?.Trim().ToLower() ?? string.Empty;
            var filtrados = string.IsNullOrEmpty(filtro)
                ? categorias
                : categorias.Where(c => c.codCategoria.ToLower().Contains(filtro) || c.descripcion.ToLower().Contains(filtro)).ToList();
            Categorias.Clear();
            foreach (var cat in filtrados)
                Categorias.Add(cat);
        }

        private bool CanSaveCategoria()
        {
            return EditCategoria != null &&
                   !string.IsNullOrWhiteSpace(EditCategoria.codCategoria) &&
                   !string.IsNullOrWhiteSpace(EditCategoria.descripcion);
        }

        private async Task SaveCategoriaAsync()
        {
            if (Categorias.Any(c => c.codCategoria == EditCategoria.codCategoria))
            {
                var result = await _categoriaService.ModificarCategoriaAsync(EditCategoria.codCategoria, EditCategoria);
                if (result)
                {
                    await LoadCategoriasAsync();
                }
            }
            else
            {
                var created = await _categoriaService.CrearCategoriaAsync(EditCategoria);
                if (created != null)
                    Categorias.Add(created);
            }
            SelectedCategoria = new CategoriaDTO();
            EditCategoria = new CategoriaDTO();
        }

        private async Task DeleteCategoriaAsync()
        {
            if (SelectedCategoria == null || string.IsNullOrEmpty(SelectedCategoria.codCategoria))
                return;
            var result = await _categoriaService.EliminarCategoriaAsync(SelectedCategoria.codCategoria);
            if (result)
            {
                var toRemove = Categorias.FirstOrDefault(c => c.codCategoria == SelectedCategoria.codCategoria);
                if (toRemove != null)
                    Categorias.Remove(toRemove);
                SelectedCategoria = new CategoriaDTO();
                EditCategoria = new CategoriaDTO();
            }
        }

        private void CancelEdit()
        {
            EditCategoria = SelectedCategoria != null ? SelectedCategoria.Clone() : new CategoriaDTO();
        }
    }
}
