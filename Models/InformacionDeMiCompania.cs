using LPAC___Proyecto_II_frontend.DTOs;
using LPAC___Proyecto_II_frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class InformacionDeMiCompania : ViewModelBase
    {
        private int _setupid;
        private double _impuestoVenta;
        private string _nombre = string.Empty;
        private string _direccion = string.Empty;
        private string _ciudad = string.Empty;
        private string _estado_o_provincia = string.Empty;
        private string _codigo_postal = string.Empty;
        private string _pais = string.Empty;
        private string _telefono = string.Empty;
        private string _num_fax = string.Empty;
        private string _terminos_pago = string.Empty;
        private string _mensaje = string.Empty;

        public InformacionDeMiCompania() { }

        public InformacionDeMiCompania(
            int setupid,
            double impuestoVenta,
            string nombre,
            string direccion,
            string ciudad,
            string estado_o_provincia,
            string codigo_postal,
            string pais,
            string telefono,
            string num_fax,
            string terminos_pago,
            string mensaje)
        {
            Setupid = setupid;
            ImpuestoVenta = impuestoVenta;
            Nombre = nombre;
            Direccion = direccion;
            Ciudad = ciudad;
            Estado_o_provincia = estado_o_provincia;
            Codigo_postal = codigo_postal;
            Pais = pais;
            Telefono = telefono;
            Num_fax = num_fax;
            Terminos_pago = terminos_pago;
            Mensaje = mensaje;
        }

        public int Setupid
        {
            get => _setupid;
            set { if (_setupid != value) { _setupid = value; OnPropertyChanged(nameof(Setupid)); } }
        }

        public double ImpuestoVenta
        {
            get => _impuestoVenta;
            set { if (_impuestoVenta != value) { _impuestoVenta = value; OnPropertyChanged(nameof(ImpuestoVenta)); } }
        }

        public string Nombre
        {
            get => _nombre;
            set { if (_nombre != value) { _nombre = value; OnPropertyChanged(nameof(Nombre)); } }
        }

        public string Direccion
        {
            get => _direccion;
            set { if (_direccion != value) { _direccion = value; OnPropertyChanged(nameof(Direccion)); } }
        }

        public string Ciudad
        {
            get => _ciudad;
            set { if (_ciudad != value) { _ciudad = value; OnPropertyChanged(nameof(Ciudad)); } }
        }

        public string Estado_o_provincia
        {
            get => _estado_o_provincia;
            set { if (_estado_o_provincia != value) { _estado_o_provincia = value; OnPropertyChanged(nameof(Estado_o_provincia)); } }
        }

        public string Codigo_postal
        {
            get => _codigo_postal;
            set { if (_codigo_postal != value) { _codigo_postal = value; OnPropertyChanged(nameof(Codigo_postal)); } }
        }

        public string Pais
        {
            get => _pais;
            set { if (_pais != value) { _pais = value; OnPropertyChanged(nameof(Pais)); } }
        }

        public string Telefono
        {
            get => _telefono;
            set { if (_telefono != value) { _telefono = value; OnPropertyChanged(nameof(Telefono)); } }
        }

        public string Num_fax
        {
            get => _num_fax;
            set { if (_num_fax != value) { _num_fax = value; OnPropertyChanged(nameof(Num_fax)); } }
        }

        public string Terminos_pago
        {
            get => _terminos_pago;
            set { if (_terminos_pago != value) { _terminos_pago = value; OnPropertyChanged(nameof(Terminos_pago)); } }
        }

        public string Mensaje
        {
            get => _mensaje;
            set { if (_mensaje != value) { _mensaje = value; OnPropertyChanged(nameof(Mensaje)); } }
        }

        public static InformacionDeMiCompania FromDTO(InformacionDeMiCompaniaDTO dto)
        {
            return new InformacionDeMiCompania
            {
                Setupid = dto.setupid,
                ImpuestoVenta = dto.impuestoVenta,
                Nombre = dto.nombre,
                Direccion = dto.direccion,
                Ciudad = dto.ciudad,
                Estado_o_provincia = dto.estadoOProvincia,
                Codigo_postal = dto.codigoPostal,
                Pais = dto.pais,
                Telefono = dto.telefono,
                Num_fax = dto.numFax,
                Terminos_pago = dto.terminosPago,
                Mensaje = dto.mensaje
            };
        }

    }

}
