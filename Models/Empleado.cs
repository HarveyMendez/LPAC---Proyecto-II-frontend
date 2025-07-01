// Models/Empleado.cs
using LPAC___Proyecto_II_frontend.Helpers;
using LPAC___Proyecto_II_frontend.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Models
{
    public class Empleado : ViewModelBase
    {
        private int _idEmpleado;
        private string _nombreEmpleado = string.Empty;
        private string _apellidosEmpleado = string.Empty;
        private string _puesto = string.Empty;
        private string _extension = string.Empty;
        private string _telefonoTrabajo = string.Empty;
        private DepartamentoDTO _departamento = new DepartamentoDTO(); // Cambiado a DTO
        private RolDTO _rol = new RolDTO(); // Cambiado a DTO
        private string? _nombreUsuario;
        private string? _contrasenaHash;
        private string? _email;

        public Empleado() { }

        // Constructor con todos los campos para facilitar la inicialización
        public Empleado(int idEmpleado, string nombreEmpleado, string apellidosEmpleado, string puesto,
                        string extension, string telefonoTrabajo, DepartamentoDTO departamento,
                        RolDTO rol, string? nombreUsuario, string? contrasenaHash, string? email)
        {
            _idEmpleado = idEmpleado;
            _nombreEmpleado = nombreEmpleado;
            _apellidosEmpleado = apellidosEmpleado;
            _puesto = puesto;
            _extension = extension;
            _telefonoTrabajo = telefonoTrabajo;
            _departamento = departamento;
            _rol = rol;
            _nombreUsuario = nombreUsuario;
            _contrasenaHash = contrasenaHash;
            _email = email;
        }

        public int IdEmpleado
        {
            get => _idEmpleado;
            set { if (_idEmpleado != value) { _idEmpleado = value; OnPropertyChanged(nameof(IdEmpleado)); } }
        }

        public string NombreEmpleado
        {
            get => _nombreEmpleado;
            set { if (_nombreEmpleado != value) { _nombreEmpleado = value; OnPropertyChanged(nameof(NombreEmpleado)); } }
        }

        public string ApellidosEmpleado
        {
            get => _apellidosEmpleado;
            set { if (_apellidosEmpleado != value) { _apellidosEmpleado = value; OnPropertyChanged(nameof(ApellidosEmpleado)); } }
        }

        public string Puesto
        {
            get => _puesto;
            set { if (_puesto != value) { _puesto = value; OnPropertyChanged(nameof(Puesto)); } }
        }

        public string Extension
        {
            get => _extension;
            set { if (_extension != value) { _extension = value; OnPropertyChanged(nameof(Extension)); } }
        }

        public string TelefonoTrabajo
        {
            get => _telefonoTrabajo;
            set { if (_telefonoTrabajo != value) { _telefonoTrabajo = value; OnPropertyChanged(nameof(TelefonoTrabajo)); } }
        }

        // Cambiado a DepartamentoDTO
        public DepartamentoDTO Departamento
        {
            get => _departamento;
            set { if (_departamento != value) { _departamento = value; OnPropertyChanged(nameof(Departamento)); } }
        }

        // Cambiado a RolDTO
        public RolDTO Rol
        {
            get => _rol;
            set { if (_rol != value) { _rol = value; OnPropertyChanged(nameof(Rol)); } }
        }

        public string? Nombre_Usuario
        {
            get => _nombreUsuario;
            set { if (_nombreUsuario != value) { _nombreUsuario = value; OnPropertyChanged(nameof(Nombre_Usuario)); } }
        }

        public string? Contrasena_Hash
        {
            get => _contrasenaHash;
            set { if (_contrasenaHash != value) { _contrasenaHash = value; OnPropertyChanged(nameof(Contrasena_Hash)); } }
        }

        public string? Email
        {
            get => _email;
            set { if (_email != value) { _email = value; OnPropertyChanged(nameof(Email)); } }
        }

        // Mapea este modelo a un DTO para enviar al backend
        public EmpleadoDTO ToDto()
        {
            return new EmpleadoDTO
            {
                idEmpleado = this.IdEmpleado,
                nombreEmpleado = this.NombreEmpleado,
                apellidosEmpleado = this.ApellidosEmpleado,
                puesto = this.Puesto,
                extension = this.Extension,
                telefonoTrabajo = this.TelefonoTrabajo,
                departamento = this.Departamento, // Usa el objeto DepartamentoDTO directamente
                rol = this.Rol,                 // Usa el objeto RolDTO directamente
                nombre_usuario = this.Nombre_Usuario,
                contrasena_hash = this.Contrasena_Hash,
                email = this.Email
            };
        }

        // Mapea un DTO recibido del backend a este modelo
        public Empleado FromDto(EmpleadoDTO dto)
        {
            this.IdEmpleado = dto.idEmpleado;
            this.NombreEmpleado = dto.nombreEmpleado;
            this.ApellidosEmpleado = dto.apellidosEmpleado;
            this.Puesto = dto.puesto;
            this.Extension = dto.extension;
            this.TelefonoTrabajo = dto.telefonoTrabajo;
            this.Departamento = dto.departamento ?? new DepartamentoDTO(); // Maneja posibles nulos
            this.Rol = dto.rol ?? new RolDTO(); // Maneja posibles nulos
            this.Nombre_Usuario = dto.nombre_usuario;
            this.Contrasena_Hash = dto.contrasena_hash;
            this.Email = dto.email;
            return this;
        }
    }
}