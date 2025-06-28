using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Producto : ViewModelBase
    {
        private int idProducto;
        private string nombreProducto;
        private decimal precio;
        private int codCategoria;
        private int cantidadExistencias;
        private int puntoReorden;

        public Producto() { }

        public Producto(int idProducto, string nombreProducto, decimal precio, int codCategoria, int cantidadExistencias, int puntoReorden)
        {
            this.IdProducto = idProducto;
            this.NombreProducto = nombreProducto;
            this.Precio = precio;
            this.CodCategoria = codCategoria;
            this.CantidadExistencias = cantidadExistencias;
            this.PuntoReorden = puntoReorden;
        }

        public int IdProducto { get => idProducto; set { if (idProducto != value) { idProducto = value; OnPropertyChanged(nameof(IdProducto)); } } }
        public string NombreProducto { get => nombreProducto; set { if (nombreProducto != value) { nombreProducto = value; OnPropertyChanged(nameof(NombreProducto)); } } }
        public decimal Precio { get => precio; set { if (precio != value) { precio = value; OnPropertyChanged(nameof(Precio)); } } }
        public int CodCategoria { get => codCategoria; set { if (codCategoria != value) { codCategoria = value; OnPropertyChanged(nameof(CodCategoria)); } } }
        public int CantidadExistencias { get => cantidadExistencias; set { if (cantidadExistencias != value) { cantidadExistencias = value; OnPropertyChanged(nameof(CantidadExistencias)); } } }
        public int PuntoReorden { get => puntoReorden; set { if (puntoReorden != value) { puntoReorden = value; OnPropertyChanged(nameof(PuntoReorden)); } } }
    }
}
