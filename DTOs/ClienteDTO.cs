using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.DTOs
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) para la entidad Cliente.
    /// Se utiliza para enviar y recibir datos del cliente a/desde la API.
    /// Las propiedades deben coincidir con la estructura JSON del backend.
    /// </summary>
    public class ClienteDTO
    {
        public int ClienteId { get; set; }
        public string NombreCompania { get; set; } = string.Empty;
        public string NombreContacto { get; set; } = string.Empty;
        public string ApellidoContacto { get; set; } = string.Empty;
        public string PuestoContacto { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string NumFax { get; set; } = string.Empty;
    }
}