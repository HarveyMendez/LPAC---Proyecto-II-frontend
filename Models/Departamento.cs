using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.DTOs; // Usamos el DTO para el mapeo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Departamento : ViewModelBase
    {
        private string _codDepartamento = string.Empty; // Cambiado a string y inicializado
        private string _nombreDepartamento = string.Empty; // Corregido el nombre y inicializado

        public Departamento() { }

        public Departamento(string codDepartamento, string nombreDepartamento) // Cambiado el tipo del código
        {
            _codDepartamento = codDepartamento;
            _nombreDepartamento = nombreDepartamento;
        }

        public string CodDepartamento // Cambiado el tipo de la propiedad
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

        public string NombreDepartamento // Corregido el nombre de la propiedad
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

        // Método para convertir este modelo a su DTO correspondiente
        public DepartamentoDTO ToDto()
        {
            return new DepartamentoDTO
            {
                codDepartamento = this.CodDepartamento,
                nombreDepartamento = this.NombreDepartamento
            };
        }

        // Método para popular este modelo desde un DTO
        public Departamento FromDto(DepartamentoDTO dto)
        {
            this.CodDepartamento = dto.codDepartamento ?? string.Empty; // Manejar posible null del DTO
            this.NombreDepartamento = dto.nombreDepartamento ?? string.Empty; // Manejar posible null del DTO
            return this;
        }
    }
}