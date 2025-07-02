using LPAC___Proyecto_II_frontend.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Services
{
    public class CategoriaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public CategoriaService()
        {
            _httpClient = AuthService.GetHttpClient();
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl();
            _apiEndpoint = $"{baseUrlFromConfig}/api/Categoria";
        }

        public async Task<List<CategoriaDTO>> GetCategoriasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode();
                var categorias = await response.Content.ReadFromJsonAsync<List<CategoriaDTO>>();
                return categorias ?? new List<CategoriaDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categorías: {ex.Message}");
                throw;
            }
        }

        public async Task<CategoriaDTO?> GetCategoriaByIdAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<CategoriaDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categoría por ID: {ex.Message}");
                throw;
            }
        }

        public async Task<CategoriaDTO?> CrearCategoriaAsync(CategoriaDTO categoria)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, categoria);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<CategoriaDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear categoría: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ModificarCategoriaAsync(string id, CategoriaDTO categoria)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiEndpoint}/{id}", categoria);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar categoría: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EliminarCategoriaAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiEndpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar categoría: {ex.Message}");
                throw;
            }
        }
    }
}