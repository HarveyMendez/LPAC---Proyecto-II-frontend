using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.DTOs
{

    public class EmpleadoDTO
    {
        public int idEmpleado { get; set; }
        public string nombreEmpleado { get; set; } = string.Empty;
        public string apellidosEmpleado { get; set; } = string.Empty;
        public string puesto { get; set; } = string.Empty;
        public string extension { get; set; } = string.Empty;
        public string telefonoTrabajo { get; set; } = string.Empty;
        public int deptoCod { get; set; }
        public int idRol { get; set; }
    }
}