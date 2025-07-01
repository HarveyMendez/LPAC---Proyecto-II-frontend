using LPAC___Proyecto_II_frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using LPAC___Proyecto_II_frontend.DTOs; // Importante: para DepartamentoDTO

namespace LPAC___Proyecto_II_frontend.Services
{
    public class DepartamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public DepartamentoService()
        {
            _httpClient = new HttpClient();
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl(); // Asumo que AppConfig.GetApiBaseUrl() existe
            _apiEndpoint = $"{baseUrlFromConfig}/api/Departamento";
            _httpClient.BaseAddress = new Uri(baseUrlFromConfig);
        }

        // CAMBIO CRUCIAL: Ahora devuelve List<DepartamentoDTO>
        public async Task<List<DepartamentoDTO>> GetAllDepartamentosAsync(string searchTerm = null)
        {
            try
            {
                string requestUrl = _apiEndpoint;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    // Si el backend tuviera un parámetro searchTerm para nombreDepartamento
                    // requestUrl += $"?nombreDepartamento={Uri.EscapeDataString(searchTerm)}";
                }

                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                // Directamente lee como List<DepartamentoDTO>
                var departamentosDto = await response.Content.ReadFromJsonAsync<List<DepartamentoDTO>>();

                // No hay necesidad de mapear a Models.Departamento aquí; el ViewModel lo manejará.
                return departamentosDto ?? new List<DepartamentoDTO>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener departamentos: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al obtener departamentos: {ex.Message}");
                throw;
            }
        }

        // CAMBIO CRUCIAL: Ahora acepta DepartamentoDTO y devuelve DepartamentoDTO
        public async Task<DepartamentoDTO> CreateDepartamentoAsync(DepartamentoDTO departamentoDto)
        {
            try
            {
                // Ya recibimos un DTO, no necesitamos llamar a ToDto()
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, departamentoDto);
                response.EnsureSuccessStatusCode();

                var createdDepartamentoDto = await response.Content.ReadFromJsonAsync<DepartamentoDTO>();
                return createdDepartamentoDto; // Devuelve el DTO creado
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al crear departamento: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al crear departamento: {ex.Message}");
                throw;
            }
        }

        // CAMBIO CRUCIAL: Ahora acepta DepartamentoDTO
        public async Task UpdateDepartamentoAsync(DepartamentoDTO departamentoDto)
        {
            try
            {
                // Ya recibimos un DTO, no necesitamos llamar a ToDto()
                // Usamos departamentoDto.codDepartamento para la URL, que es el ID
                var response = await _httpClient.PutAsJsonAsync($"{_apiEndpoint}/{departamentoDto.codDepartamento}", departamentoDto);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al actualizar departamento: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al actualizar departamento: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteDepartamentoAsync(string deptoCod)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiEndpoint}/{deptoCod}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al eliminar departamento: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al eliminar departamento: {ex.Message}");
                throw;
            }
        }
    }
}