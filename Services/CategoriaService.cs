// En LPAC---Proyecto_II_frontend/Services/CategoriaService.cs
using LPAC___Proyecto_II_frontend.DTOs; // Asegúrate de usar el namespace correcto para tus DTOs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; // Para ReadFromJsonAsync
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
            // Asegúrate de que AppConfig.GetApiBaseUrl() devuelva la URL base de tu API,
            // por ejemplo, "https://localhost:XXXXX" donde XXXXX es el puerto de tu API.
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl();
            _apiEndpoint = $"{baseUrlFromConfig}/api/Categoria"; // La ruta base para tu CategoriaController
            _httpClient.BaseAddress = new Uri(baseUrlFromConfig);
        }

        public async Task<List<CategoriaDTO>> GetCategoriasAsync()
        {
            try
            {
                // Realiza la llamada GET a la API
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode(); // Lanza una excepción si el código de estado no es de éxito (2xx)

                // Lee el contenido de la respuesta como una lista de CategoriaDTO
                var categorias = await response.Content.ReadFromJsonAsync<List<CategoriaDTO>>();
                return categorias ?? new List<CategoriaDTO>(); // Devuelve una lista vacía si es null
            }
            catch (HttpRequestException e)
            {
                // Manejo de errores de HTTP (ej. API no disponible, error 404, 500, etc.)
                Console.WriteLine($"Error de solicitud HTTP al obtener categorías: {e.Message}");
                // Puedes lanzar la excepción o devolver una lista vacía/null, dependiendo de tu manejo de errores.
                throw;
            }
            catch (Exception e)
            {
                // Otros errores (ej. deserialización)
                Console.WriteLine($"Error inesperado al obtener categorías: {e.Message}");
                throw;
            }
        }
    }
}