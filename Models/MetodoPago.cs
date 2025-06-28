using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class MetodoPago : ViewModelBase
    {
        private int idMetodoPago;
        private string metodoPagoNombre;

        public MetodoPago() { }

        public MetodoPago(int idMetodoPago, string metodoPagoNombre)
        {
            this.IdMetodoPago = idMetodoPago;
            this.MetodoPagoNombre = metodoPagoNombre;
        }

        public int IdMetodoPago { get => idMetodoPago; set { if (idMetodoPago != value) { idMetodoPago = value; OnPropertyChanged(nameof(IdMetodoPago)); } } }
        public string MetodoPagoNombre { get => metodoPagoNombre; set { if (metodoPagoNombre != value) { metodoPagoNombre = value; OnPropertyChanged(nameof(MetodoPagoNombre)); } } }
    }
}
