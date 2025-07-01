// Models/Rol.cs
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.DTOs; // <--- ¡MUY IMPORTANTE ESTE USING!
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
        private string _nombreRol = string.Empty;

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

        // ***** ESTE MÉTODO ToDto() DEBE ESTAR AQUÍ *****
        public RolDTO ToDto()
        {
            return new RolDTO
            {
                idRol = this.IdRol,
                nombreRol = this.NombreRol
            };
        }

        // Este método FromDto() también es importante para convertir de DTO a Modelo
        public Rol FromDto(RolDTO dto)
        {
            this.IdRol = dto.idRol;
            this.NombreRol = dto.nombreRol ?? string.Empty;
            return this;
        }
    }
}