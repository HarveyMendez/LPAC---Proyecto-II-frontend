using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Departamento : ViewModelBase
    {
        private int deptoCod;
        private string nombreDepartament;

        public Departamento() { }

        public Departamento(int deptoCod, string nombreDepartament)
        {
            this.DeptoCod = deptoCod;
            this.NombreDepartament = nombreDepartament;
        }

        public int DeptoCod { get => deptoCod; set { if (deptoCod != value) { deptoCod = value; OnPropertyChanged(nameof(DeptoCod)); } } }
        public string NombreDepartament { get => nombreDepartament; set { if (nombreDepartament != value) { nombreDepartament = value; OnPropertyChanged(nameof(NombreDepartament)); } } }
    }
}
