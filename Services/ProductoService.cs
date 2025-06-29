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
    public class ProductoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public ProductoService()
        {
            _httpClient = new HttpClient();
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl(); 
            _apiEndpoint = $"{baseUrlFromConfig}/api/Producto";
            _httpClient.BaseAddress = new Uri(baseUrlFromConfig);
        }

        public async Task<List<Producto>> GetAllProductosAsync(string searchTerm = null)
        {
            try
            {
                string requestUrl = _apiEndpoint;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    
                    requestUrl += $"?nombreProducto={Uri.EscapeDataString(searchTerm)}";
                }

                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var productosDto = await response.Content.ReadFromJsonAsync<List<ProductoDTO>>();
                var productos = productosDto?.Select(dto => new Producto().FromDto(dto)).ToList();
                return productos ?? new List<Producto>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener productos: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al obtener productos: {ex.Message}");
                throw;
            }
        }

        public async Task<Producto> CreateProductoAsync(Producto producto)
        {
            try
            {
                var productoDto = producto.ToDto();
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, productoDto);
                response.EnsureSuccessStatusCode();

                var createdProductoDto = await response.Content.ReadFromJsonAsync<ProductoDTO>();
                return new Producto().FromDto(createdProductoDto);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al crear producto: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al crear producto: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProductoAsync(Producto producto)
        {
            try
            {
                var productoDto = producto.ToDto();
                var response = await _httpClient.PutAsJsonAsync($"{_apiEndpoint}/{producto.idProducto}", productoDto);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al actualizar producto: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al actualizar producto: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteProductoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiEndpoint}/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al eliminar producto: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al eliminar producto: {ex.Message}");
                throw;
            }
        }
    }
}