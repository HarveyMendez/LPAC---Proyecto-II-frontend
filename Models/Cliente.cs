using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Cliente : ViewModelBase
    {
        private int clienteId;
        private string nombreCompania;
        private string nombreContacto;
        private string apellidoContacto;
        private string puestoContacto;
        private string direccion;
        private string ciudad;
        private string provincia;
        private string codigoPostal;
        private string pais;
        private string telefono;
        private string numFax;

        public Cliente() { }

        public Cliente(int clienteId, string nombreCompania, string nombreContacto, string apellidoContacto, string puestoContacto, string direccion, string ciudad, string provincia, string codigoPostal, string pais, string telefono, string numFax)
        {
            this.ClienteId = clienteId;
            this.NombreCompania = nombreCompania;
            this.NombreContacto = nombreContacto;
            this.ApellidoContacto = apellidoContacto;
            this.PuestoContacto = puestoContacto;
            this.Direccion = direccion;
            this.Ciudad = ciudad;
            this.Provincia = provincia;
            this.CodigoPostal = codigoPostal;
            this.Pais = pais;
            this.Telefono = telefono;
            this.NumFax = numFax;
        }

        public int ClienteId { get => clienteId; set { if (clienteId != value) { clienteId = value; OnPropertyChanged(nameof(ClienteId)); } } }
        public string NombreCompania { get => nombreCompania; set { if (nombreCompania != value) { nombreCompania = value; OnPropertyChanged(nameof(NombreCompania)); } } }
        public string NombreContacto { get => nombreContacto; set { if (nombreContacto != value) { nombreContacto = value; OnPropertyChanged(nameof(NombreContacto)); } } }
        public string ApellidoContacto { get => apellidoContacto; set { if (apellidoContacto != value) { apellidoContacto = value; OnPropertyChanged(nameof(ApellidoContacto)); } } }
        public string PuestoContacto { get => puestoContacto; set { if (puestoContacto != value) { puestoContacto = value; OnPropertyChanged(nameof(PuestoContacto)); } } }
        public string Direccion { get => direccion; set { if (direccion != value) { direccion = value; OnPropertyChanged(nameof(Direccion)); } } }
        public string Ciudad { get => ciudad; set { if (ciudad != value) { ciudad = value; OnPropertyChanged(nameof(Ciudad)); } } }
        public string Provincia { get => provincia; set { if (provincia != value) { provincia = value; OnPropertyChanged(nameof(Provincia)); } } }
        public string CodigoPostal { get => codigoPostal; set { if (codigoPostal != value) { codigoPostal = value; OnPropertyChanged(nameof(CodigoPostal)); } } }
        public string Pais { get => pais; set { if (pais != value) { pais = value; OnPropertyChanged(nameof(Pais)); } } }
        public string Telefono { get => telefono; set { if (telefono != value) { telefono = value; OnPropertyChanged(nameof(Telefono)); } } }
        public string NumFax { get => numFax; set { if (numFax != value) { numFax = value; OnPropertyChanged(nameof(NumFax)); } } }
    }
}
