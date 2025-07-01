using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.DTOs; // Usamos el DTO para el mapeo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Rol : ViewModelBase
    {
        private int _idRol;
        private string _nombreRol = string.Empty; // Inicialización consistente

        public Rol() { }

        public Rol(int idRol, string nombreRol)
        {
            _idRol = idRol;
            _nombreRol = nombreRol;
        }

        public int IdRol
        {
            get => _idRol;
            set { if (_idRol != value) { _idRol = value; OnPropertyChanged(nameof(IdRol)); } }
        }

        public string NombreRol
        {
            get => _nombreRol;
            set { if (_nombreRol != value) { _nombreRol = value; OnPropertyChanged(nameof(NombreRol)); } }
        }

        // Método para convertir este modelo a su DTO correspondiente
        public RolDTO ToDto()
        {
            return new RolDTO
            {
                idRol = this.IdRol,
                nombreRol = this.NombreRol
            };
        }

        // Método para popular este modelo desde un DTO
        public Rol FromDto(RolDTO dto)
        {
            this.IdRol = dto.idRol;
            this.NombreRol = dto.nombreRol ?? string.Empty; // Manejar posible null del DTO
            return this;
        }
    }
}