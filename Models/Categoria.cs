using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Categoria : ViewModelBase
    {
        private int codCategoria;
        private string descripcion;

        public Categoria() { }

        public Categoria(int codCategoria, string descripcion)
        {
            this.CodCategoria = codCategoria;
            this.Descripcion = descripcion;
        }

        public int CodCategoria { get => codCategoria; set { if (codCategoria != value) { codCategoria = value; OnPropertyChanged(nameof(CodCategoria)); } } }
        public string Descripcion { get => descripcion; set { if (descripcion != value) { descripcion = value; OnPropertyChanged(nameof(Descripcion)); } } }
    }
}
