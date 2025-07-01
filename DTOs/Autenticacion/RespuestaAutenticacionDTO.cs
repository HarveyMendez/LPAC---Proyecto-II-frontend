using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.DTOs.Autenticacion
{
    public class RespuestaAutenticacionDTO
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
    }
}
