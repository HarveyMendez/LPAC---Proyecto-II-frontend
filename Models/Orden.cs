using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Orden : ViewModelBase
    {
        private int idOrden;
        private int clienteId;
        private int idEmpleado;
        private DateTime fechaOrden;
        private string direccionViaje;
        private string ciudadViaje;
        private string provinciaViaje;
        private string paisViaje;
        private string telefonoViaje;
        private DateTime fechaViaje;

        public Orden() { }

        public Orden(int idOrden, int clienteId, int idEmpleado, DateTime fechaOrden, string direccionViaje, string ciudadViaje, string provinciaViaje, string paisViaje, string telefonoViaje, DateTime fechaViaje)
        {
            this.IdOrden = idOrden;
            this.ClienteId = clienteId;
            this.IdEmpleado = idEmpleado;
            this.FechaOrden = fechaOrden;
            this.DireccionViaje = direccionViaje;
            this.CiudadViaje = ciudadViaje;
            this.ProvinciaViaje = provinciaViaje;
            this.PaisViaje = paisViaje;
            this.TelefonoViaje = telefonoViaje;
            this.FechaViaje = fechaViaje;
        }

        public int IdOrden { get => idOrden; set { if (idOrden != value) { idOrden = value; OnPropertyChanged(nameof(IdOrden)); } } }
        public int ClienteId { get => clienteId; set { if (clienteId != value) { clienteId = value; OnPropertyChanged(nameof(ClienteId)); } } }
        public int IdEmpleado { get => idEmpleado; set { if (idEmpleado != value) { idEmpleado = value; OnPropertyChanged(nameof(IdEmpleado)); } } }
        public DateTime FechaOrden { get => fechaOrden; set { if (fechaOrden != value) { fechaOrden = value; OnPropertyChanged(nameof(FechaOrden)); } } }
        public string DireccionViaje { get => direccionViaje; set { if (direccionViaje != value) { direccionViaje = value; OnPropertyChanged(nameof(DireccionViaje)); } } }
        public string CiudadViaje { get => ciudadViaje; set { if (ciudadViaje != value) { ciudadViaje = value; OnPropertyChanged(nameof(CiudadViaje)); } } }
        public string ProvinciaViaje { get => provinciaViaje; set { if (provinciaViaje != value) { provinciaViaje = value; OnPropertyChanged(nameof(ProvinciaViaje)); } } }
        public string PaisViaje { get => paisViaje; set { if (paisViaje != value) { paisViaje = value; OnPropertyChanged(nameof(PaisViaje)); } } }
        public string TelefonoViaje { get => telefonoViaje; set { if (telefonoViaje != value) { telefonoViaje = value; OnPropertyChanged(nameof(TelefonoViaje)); } } }
        public DateTime FechaViaje { get => fechaViaje; set { if (fechaViaje != value) { fechaViaje = value; OnPropertyChanged(nameof(FechaViaje)); } } }
    }
}
