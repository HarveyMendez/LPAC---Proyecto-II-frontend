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
            _httpClient = new HttpClient();
            
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl();
            _apiEndpoint = $"{baseUrlFromConfig}/api/Categoria"; 
            _httpClient.BaseAddress = new Uri(baseUrlFromConfig);
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
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error de solicitud HTTP al obtener categorías: {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error inesperado al obtener categorías: {e.Message}");
                throw;
            }
        }
    }
}