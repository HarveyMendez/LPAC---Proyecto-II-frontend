using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class DetalleOrden : ViewModelBase
    {
        private int idOrden;
        private int idProducto;
        private decimal cantidad;
        private decimal precioLinea;

        public DetalleOrden() { }

        public DetalleOrden(int idOrden, int idProducto, decimal cantidad, decimal precioLinea)
        {
            this.IdOrden = idOrden;
            this.IdProducto = idProducto;
            this.Cantidad = cantidad;
            this.PrecioLinea = precioLinea;
        }

        public int IdOrden { get => idOrden; set { if (idOrden != value) { idOrden = value; OnPropertyChanged(nameof(IdOrden)); } } }
        public int IdProducto { get => idProducto; set { if (idProducto != value) { idProducto = value; OnPropertyChanged(nameof(IdProducto)); } } }
        public decimal Cantidad { get => cantidad; set { if (cantidad != value) { cantidad = value; OnPropertyChanged(nameof(Cantidad)); } } }
        public decimal PrecioLinea { get => precioLinea; set { if (precioLinea != value) { precioLinea = value; OnPropertyChanged(nameof(PrecioLinea)); } } }
    }
}
