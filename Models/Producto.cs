
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPAC___Proyecto_II_frontend.DTOs;
using LPAC___Proyecto_II_frontend.ViewModel;
using LPAC___Proyecto_II_frontend.Helpers;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Producto : ViewModelBase
    {
        private int _idProducto;
        public int idProducto
        {
            get { return _idProducto; }
            set
            {
                _idProducto = value;
                OnPropertyChanged(nameof(idProducto));
            }
        }

        private string _nombreProducto = string.Empty;
        public string nombreProducto
        {
            get { return _nombreProducto; }
            set
            {
                _nombreProducto = value;
                OnPropertyChanged(nameof(nombreProducto));
            }
        }

        private float _precio;
        public float precio
        {
            get { return _precio; }
            set
            {
                _precio = value;
                OnPropertyChanged(nameof(precio));
            }
        }

        // Esta propiedad es para manejar el cod_categoria directamente en el modelo del front
        // y simplificar el binding en el formulario. Al enviar al backend, se mapea a CategoriaDTO.
        private string? _codCategoria;
        public string? codCategoria
        {
            get { return _codCategoria; }
            set
            {
                _codCategoria = value;
                OnPropertyChanged(nameof(codCategoria));
            }
        }

        // Si necesitas mostrar la descripción de la categoría en el DataGrid,
        // puedes agregar una propiedad aquí y llenarla al cargar los productos
        // o si tu API ya te la devuelve.
        private string _descripcionCategoria = string.Empty;
        public string descripcionCategoria
        {
            get { return _descripcionCategoria; }
            set
            {
                _descripcionCategoria = value;
                OnPropertyChanged(nameof(descripcionCategoria));
            }
        }


        private int _cantidadExistencias;
        public int cantidadExistencias
        {
            get { return _cantidadExistencias; }
            set
            {
                _cantidadExistencias = value;
                OnPropertyChanged(nameof(cantidadExistencias));
            }
        }

        private int _puntoReorden;
        public int puntoReorden
        {
            get { return _puntoReorden; }
            set
            {
                _puntoReorden = value;
                OnPropertyChanged(nameof(puntoReorden));
            }
        }

        private bool _aplicaImpuesto = false;
        public bool aplicaImpuesto
        {
            get { return _aplicaImpuesto; }
            set
            {
                _aplicaImpuesto = value;
                OnPropertyChanged(nameof(aplicaImpuesto));
            }
        }

        private string _talla = string.Empty;
        public string talla
        {
            get { return _talla; }
            set
            {
                _talla = value;
                OnPropertyChanged(nameof(talla));
            }
        }

        // Constructor para mapear de ProductoDTO del backend a tu modelo de frontend
        public Producto FromDto(ProductoDTO dto)
        {
            idProducto = dto.idProducto;
            nombreProducto = dto.nombreProducto;
            precio = dto.precio;
            cantidadExistencias = dto.cantidadExistencias;
            puntoReorden = dto.puntoReorden;
            aplicaImpuesto = dto.aplicaImpuesto;
            talla = dto.talla;
            codCategoria = dto.categoria?.codCategoria;
            descripcionCategoria = dto.categoria?.descripcion ?? string.Empty; // Asegurarse de manejar nulls
            return this;
        }

        // Método para convertir tu modelo de frontend a ProductoDTO para enviar al backend
        public ProductoDTO ToDto()
        {
            return new ProductoDTO
            {
                idProducto = idProducto,
                nombreProducto = nombreProducto,
                precio = precio,
                cantidadExistencias = cantidadExistencias,
                puntoReorden = puntoReorden,
                aplicaImpuesto = aplicaImpuesto,
                talla = talla,
                categoria = new CategoriaDTO // Asegurarse de que categoria no sea nula
                {
                    codCategoria = codCategoria,
                    // Si el backend espera una descripción al crear/modificar, podrías enviarla aquí
                    // o dejarla vacía si solo el código es suficiente.
                    descripcion = descripcionCategoria // Usar la descripción actual
                }
            };
        }
    }
}