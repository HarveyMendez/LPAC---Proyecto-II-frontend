// Models/Departamento.cs
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.DTOs; // <--- ¡MUY IMPORTANTE ESTE USING!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Departamento : ViewModelBase
    {
        private string _codDepartamento = string.Empty;
        private string _nombreDepartamento = string.Empty;

        public Departamento() { }

        public Departamento(string codDepartamento, string nombreDepartamento)
        {
            _codDepartamento = codDepartamento;
            _nombreDepartamento = nombreDepartamento;
        }

        public string CodDepartamento
        {
            get => _codDepartamento;
            set
            {
                if (_codDepartamento != value)
                {
                    _codDepartamento = value;
                    OnPropertyChanged(nameof(CodDepartamento));
                }
            }
        }

        public string NombreDepartamento
        {
            get => _nombreDepartamento;
            set
            {
                if (_nombreDepartamento != value)
                {
                    _nombreDepartamento = value;
                    OnPropertyChanged(nameof(NombreDepartamento));
                }
            }
        }

        // ***** ESTE MÉTODO ToDto() DEBE ESTAR AQUÍ *****
        public DepartamentoDTO ToDto()
        {
            return new DepartamentoDTO
            {
                codDepartamento = this.CodDepartamento,
                nombreDepartamento = this.NombreDepartamento
            };
        }

        // Este método FromDto() también es importante para convertir de DTO a Modelo
        public Departamento FromDto(DepartamentoDTO dto)
        {
            this.CodDepartamento = dto.codDepartamento ?? string.Empty;
            this.NombreDepartamento = dto.nombreDepartamento ?? string.Empty;
            return this;
        }
    }
}