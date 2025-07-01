// DepartamentoService.cs
using LPAC___Proyecto_II_frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using LPAC___Proyecto_II_frontend.DTOs;

namespace LPAC___Proyecto_II_frontend.Services
{
    public class DepartamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public DepartamentoService()
        {
            _httpClient = new HttpClient();
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl();
            _apiEndpoint = $"{baseUrlFromConfig}/api/Departamento";
            _httpClient.BaseAddress = new Uri(baseUrlFromConfig);
        }

        public async Task<List<Departamento>> GetAllDepartamentosAsync(string searchTerm = null)
        {
            try
            {
                string requestUrl = _apiEndpoint;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    // Aquí tu backend controller "VerDepartamentos" no tiene un parámetro searchTerm
                    // Si el backend lo tuviera, sería:
                    // requestUrl += $"?nombreDepartamento={Uri.EscapeDataString(searchTerm)}";
                }

                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var departamentosDto = await response.Content.ReadFromJsonAsync<List<DepartamentoDTO>>();

                // Convierte DTOs a Modelos antes de devolverlos
                var departamentos = departamentosDto?.Select(dto => new Departamento().FromDto(dto)).ToList();
                return departamentos ?? new List<Departamento>();
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

        public async Task<Departamento> CreateDepartamentoAsync(Departamento departamento)
        {
            try
            {
                var departamentoDto = departamento.ToDto();
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, departamentoDto);
                response.EnsureSuccessStatusCode();

                var createdDepartamentoDto = await response.Content.ReadFromJsonAsync<DepartamentoDTO>();
                return new Departamento().FromDto(createdDepartamentoDto);
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

        public async Task UpdateDepartamentoAsync(Departamento departamento)
        {
            try
            {
                var departamentoDto = departamento.ToDto();
                // FIX: Usar 'departamento.CodDepartamento' (mayúscula de tu Model)
                var response = await _httpClient.PutAsJsonAsync($"{_apiEndpoint}/{departamento.CodDepartamento}", departamentoDto);
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