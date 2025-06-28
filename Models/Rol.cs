using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Rol : ViewModelBase
    {
        private int idRol;
        private string nombreRol;

        public Rol() { }

        public Rol(int idRol, string nombreRol)
        {
            this.IdRol = idRol;
            this.NombreRol = nombreRol;
        }

        public int IdRol { get => idRol; set { if (idRol != value) { idRol = value; OnPropertyChanged(nameof(IdRol)); } } }
        public string NombreRol { get => nombreRol; set { if (nombreRol != value) { nombreRol = value; OnPropertyChanged(nameof(NombreRol)); } } }
    }
}
