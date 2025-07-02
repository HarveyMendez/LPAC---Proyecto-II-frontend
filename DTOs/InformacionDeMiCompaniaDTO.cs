using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.DTOs
{
    public class InformacionDeMiCompaniaDTO
    {
        public int setupid { get; set; }

        public double impuestoVenta { get; set; }

        public string nombre { get; set; } = string.Empty;

        public string direccion { get; set; } = string.Empty;

        public string ciudad { get; set; } = string.Empty;

        public string estadoOProvincia { get; set; } = string.Empty;

        public string codigoPostal { get; set; } = string.Empty;

        public string pais { get; set; } = string.Empty;

        public string telefono { get; set; } = string.Empty;

        public string numFax { get; set; } = string.Empty;

        public string terminosPago { get; set; } = string.Empty;

        public string mensaje { get; set; } = string.Empty;
    }
}
