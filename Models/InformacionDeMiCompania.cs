using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class InformacionDeMiCompania : ViewModelBase // Hereda del "chismoso"
    {
        private int setupId;
        private decimal impuestoVenta;
        private string nombre;
        private string direccion;
        private string ciudad;
        private string estadoOProvincia;
        private string codigoPostal;
        private string pais;
        private string telefono;
        private string numFax;
        private string terminosPago;
        private string mensaje;

        public InformacionDeMiCompania()
        {
            // Constructor vacío: Siempre es bueno tener uno
        }

        public InformacionDeMiCompania(int setupId, decimal impuestoVenta, string nombre, string direccion, string ciudad, string estadoOProvincia, string codigoPostal, string pais, string telefono, string numFax, string terminosPago, string mensaje)
        {
            // Constructor con todos los datos: Para cuando cree un objeto con información ya existente
            this.SetupId = setupId;
            this.ImpuestoVenta = impuestoVenta;
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Ciudad = ciudad;
            this.EstadoOProvincia = estadoOProvincia;
            this.CodigoPostal = codigoPostal;
            this.Pais = pais;
            this.Telefono = telefono;
            this.NumFax = numFax;
            this.TerminosPago = terminosPago;
            this.Mensaje = mensaje;
        }

        // Propiedades públicas con getters y setters que avisan cambios
        public int SetupId { get => setupId; set { if (setupId != value) { setupId = value; OnPropertyChanged(nameof(SetupId)); } } }
        public decimal ImpuestoVenta { get => impuestoVenta; set { if (impuestoVenta != value) { impuestoVenta = value; OnPropertyChanged(nameof(ImpuestoVenta)); } } }
        public string Nombre { get => nombre; set { if (nombre != value) { nombre = value; OnPropertyChanged(nameof(Nombre)); } } }
        public string Direccion { get => direccion; set { if (direccion != value) { direccion = value; OnPropertyChanged(nameof(Direccion)); } } }
        public string Ciudad { get => ciudad; set { if (ciudad != value) { ciudad = value; OnPropertyChanged(nameof(Ciudad)); } } }
        public string EstadoOProvincia { get => estadoOProvincia; set { if (estadoOProvincia != value) { estadoOProvincia = value; OnPropertyChanged(nameof(EstadoOProvincia)); } } }
        public string CodigoPostal { get => codigoPostal; set { if (codigoPostal != value) { codigoPostal = value; OnPropertyChanged(nameof(CodigoPostal)); } } }
        public string Pais { get => pais; set { if (pais != value) { pais = value; OnPropertyChanged(nameof(Pais)); } } }
        public string Telefono { get => telefono; set { if (telefono != value) { telefono = value; OnPropertyChanged(nameof(Telefono)); } } }
        public string NumFax { get => numFax; set { if (numFax != value) { numFax = value; OnPropertyChanged(nameof(NumFax)); } } }
        public string TerminosPago { get => terminosPago; set { if (terminosPago != value) { terminosPago = value; OnPropertyChanged(nameof(TerminosPago)); } } }
        public string Mensaje { get => mensaje; set { if (mensaje != value) { mensaje = value; OnPropertyChanged(nameof(Mensaje)); } } }
    }
}
