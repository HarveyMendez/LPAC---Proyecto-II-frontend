using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Empleado : ViewModelBase
    {
        private int idEmpleado;
        private string nombreEmpleado;
        private string apellidosEmpleado;
        private string puesto;
        private string extension;
        private string telefonoTrabajo;
        private int deptoCod;
        private int idRol;

        public Empleado() { }

        public Empleado(int idEmpleado, string nombreEmpleado, string apellidosEmpleado, string puesto, string extension, string telefonoTrabajo, int deptoCod, int idRol)
        {
            this.IdEmpleado = idEmpleado;
            this.NombreEmpleado = nombreEmpleado;
            this.ApellidosEmpleado = apellidosEmpleado;
            this.Puesto = puesto;
            this.Extension = extension;
            this.TelefonoTrabajo = telefonoTrabajo;
            this.DeptoCod = deptoCod;
            this.IdRol = idRol;
        }

        public int IdEmpleado { get => idEmpleado; set { if (idEmpleado != value) { idEmpleado = value; OnPropertyChanged(nameof(IdEmpleado)); } } }
        public string NombreEmpleado { get => nombreEmpleado; set { if (nombreEmpleado != value) { nombreEmpleado = value; OnPropertyChanged(nameof(NombreEmpleado)); } } }
        public string ApellidosEmpleado { get => apellidosEmpleado; set { if (apellidosEmpleado != value) { apellidosEmpleado = value; OnPropertyChanged(nameof(ApellidosEmpleado)); } } }
        public string Puesto { get => puesto; set { if (puesto != value) { puesto = value; OnPropertyChanged(nameof(Puesto)); } } }
        public string Extension { get => extension; set { if (extension != value) { extension = value; OnPropertyChanged(nameof(Extension)); } } }
        public string TelefonoTrabajo { get => telefonoTrabajo; set { if (telefonoTrabajo != value) { telefonoTrabajo = value; OnPropertyChanged(nameof(TelefonoTrabajo)); } } }
        public int DeptoCod { get => deptoCod; set { if (deptoCod != value) { deptoCod = value; OnPropertyChanged(nameof(DeptoCod)); } } }
        public int IdRol { get => idRol; set { if (idRol != value) { idRol = value; OnPropertyChanged(nameof(IdRol)); } } }
    }
}
