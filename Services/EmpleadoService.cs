using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LPAC___Proyecto_II_frontend.DTOs; // MUY IMPORTANTE: Usamos los DTOs para la comunicación con la API
using LPAC___Proyecto_II_frontend.Models; // Usamos los Models para devolver al ViewModel (después de mapeo)
using System;
using System.Linq;

namespace LPAC___Proyecto_II_frontend.Services
{
    public class EmpleadoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public EmpleadoService()
        {
            _httpClient = AuthService.GetHttpClient();
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl(); // Asumo que tienes una clase AppConfig
            _apiEndpoint = $"{baseUrlFromConfig}/api/Empleado";
        }

        // CAMBIO CLAVE: Este método ahora devuelve una lista de MODELOS (Empleado),
        // haciendo la conversión interna para que el ViewModel reciba lo que espera.
        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode();

                var empleadosDto = await response.Content.ReadFromJsonAsync<List<EmpleadoDTO>>();
                // Realiza la conversión de DTO a Modelo aquí en el servicio.
                return empleadosDto?.Select(dto => new Empleado().FromDto(dto)).ToList() ?? new List<Empleado>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener empleados: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al obtener empleados: {ex.Message}");
                throw;
            }
        }

        public async Task<Empleado?> GetEmpleadoByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
                response.EnsureSuccessStatusCode();

                var empleadoDto = await response.Content.ReadFromJsonAsync<EmpleadoDTO>();
                return empleadoDto != null ? new Empleado().FromDto(empleadoDto) : null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener empleado por ID: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al obtener empleado por ID: {ex.Message}");
                throw;
            }
        }

        // CAMBIO CLAVE: Este método ACEPTA EmpleadoDTO como parámetro
        // Y devuelve EmpleadoDTO (lo que confirma la API que se creó/recibió)
        public async Task<EmpleadoDTO> CreateEmpleadoAsync(EmpleadoDTO empleadoDto)
        {
            try
            {
                // Ya recibimos un DTO, no necesitamos llamar a ToDto()
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, empleadoDto);
                response.EnsureSuccessStatusCode();

                var createdEmpleadoDto = await response.Content.ReadFromJsonAsync<EmpleadoDTO>();
                return createdEmpleadoDto; // Devuelve el DTO creado
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al crear empleado: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al crear empleado: {ex.Message}");
                throw;
            }
        }

        // CAMBIO CLAVE: Este método ACEPTA EmpleadoDTO como parámetro
        public async Task UpdateEmpleadoAsync(EmpleadoDTO empleadoDto)
        {
            try
            {
                // Ya recibimos un DTO, no necesitamos llamar a ToDto()
                var response = await _httpClient.PutAsJsonAsync($"{_apiEndpoint}/{empleadoDto.idEmpleado}", empleadoDto);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al actualizar empleado: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al actualizar empleado: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteEmpleadoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiEndpoint}/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al eliminar empleado: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al eliminar empleado: {ex.Message}");
                throw;
            }
        }
    }
}