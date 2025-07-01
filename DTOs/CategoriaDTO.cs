using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LPAC___Proyecto_II_frontend.DTOs
{
    public class CategoriaDTO : INotifyPropertyChanged
    {
        private string _codCategoria = string.Empty;
        private string _descripcion = string.Empty;

        public string codCategoria
        {
            get => _codCategoria;
            set { _codCategoria = value; OnPropertyChanged(); }
        }
        public string descripcion
        {
            get => _descripcion;
            set { _descripcion = value; OnPropertyChanged(); }
        }

        public CategoriaDTO Clone()
        {
            return new CategoriaDTO
            {
                codCategoria = this.codCategoria,
                descripcion = this.descripcion
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
