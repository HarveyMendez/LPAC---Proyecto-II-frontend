using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Pagos : ViewModelBase
    {
        private int idPago;
        private int idOrden;
        private decimal cantidadPago;
        private DateTime fechaPago;
        private string numTarjetaCredito;
        private int idMetodoPago;

        public Pagos() { }

        public Pagos(int idPago, int idOrden, decimal cantidadPago, DateTime fechaPago, string numTarjetaCredito, int idMetodoPago)
        {
            this.IdPago = idPago;
            this.IdOrden = idOrden;
            this.CantidadPago = cantidadPago;
            this.FechaPago = fechaPago;
            this.NumTarjetaCredito = numTarjetaCredito;
            this.IdMetodoPago = idMetodoPago;
        }

        public int IdPago { get => idPago; set { if (idPago != value) { idPago = value; OnPropertyChanged(nameof(IdPago)); } } }
        public int IdOrden { get => idOrden; set { if (idOrden != value) { idOrden = value; OnPropertyChanged(nameof(IdOrden)); } } }
        public decimal CantidadPago { get => cantidadPago; set { if (cantidadPago != value) { cantidadPago = value; OnPropertyChanged(nameof(CantidadPago)); } } }
        public DateTime FechaPago { get => fechaPago; set { if (fechaPago != value) { fechaPago = value; OnPropertyChanged(nameof(FechaPago)); } } }
        public string NumTarjetaCredito { get => numTarjetaCredito; set { if (numTarjetaCredito != value) { numTarjetaCredito = value; OnPropertyChanged(nameof(NumTarjetaCredito)); } } }
        public int IdMetodoPago { get => idMetodoPago; set { if (idMetodoPago != value) { idMetodoPago = value; OnPropertyChanged(nameof(IdMetodoPago)); } } }
    }

}
