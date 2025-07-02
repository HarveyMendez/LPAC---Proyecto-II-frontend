using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.DTOs
{
    public class OrdenDTO
    {
        public int idOrden { get; set; }
        public DateTime fecha_orden { get; set; }
        public string direccion_viaje { get; set; } = string.Empty;
        public string cuidad_viaje { get; set; } = string.Empty;
        public string provincia_viaje { get; set; } = string.Empty;
        public string pais_viaje { get; set; } = string.Empty;
        public string telefono_viaje { get; set; } = string.Empty;
        public DateTime fecha_viaje { get; set; }
        public List<DetalleOrdenDTO> Detalles { get; set; } = new List<DetalleOrdenDTO>();
        public ClienteDTO Cliente { get; set; } = new ClienteDTO();
        public EmpleadoDTO Empleado { get; set; } = new EmpleadoDTO();

    }

    public class DetalleOrdenDTO
    {
        public int idOrden { get; set; }
        public float cantidad { get; set; }
        public float precioUnitario { get; set; }
        public int impuestoAplicado { get; set; }
        public ProductoDTO Producto { get; set; } = new ProductoDTO();
    }

}
