using LPAC___Proyecto_II_frontend.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LPAC___Proyecto_II_frontend.DTOs;
using System.Linq; // Necesario para el .Select() y .ToList()

namespace LPAC___Proyecto_II_frontend.Services
{
    public class ClienteService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        /// <summary>
        /// Constructor que inicializa el HttpClient y la URL del endpoint de la API.
        /// </summary>
        public ClienteService()
        {
            _httpClient = AuthService.GetHttpClient();

            // Obtiene la URL base de la API desde la configuración de la aplicación.
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl();
            _apiEndpoint = $"{baseUrlFromConfig}/api/Cliente"; // Asume que tu endpoint de clientes es /api/Cliente
        }

        /// <summary>
        /// Obtiene una lista de clientes de la API, opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda para filtrar por nombre de compañía o contacto.</param>
        /// <returns>Una lista de objetos Cliente.</returns>
        public async Task<List<Cliente>> GetClientesAsync(string searchTerm = null)
        {
            try
            {
                string requestUrl = _apiEndpoint;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    // Asume que tu API puede filtrar por un parámetro de consulta 'searchTerm'.
                    // Ajusta el nombre del parámetro si es diferente en tu backend (ej. "?search=...")
                    requestUrl += $"?searchTerm={Uri.EscapeDataString(searchTerm)}";
                }

                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode(); // Lanza una excepción si el código de estado no es de éxito.

                // Lee la respuesta JSON y la deserializa en una lista de DTOs.
                var clientesDto = await response.Content.ReadFromJsonAsync<List<ClienteDTO>>();

                // Convierte la lista de DTOs a la lista de modelos Cliente usando el método FromDto().
                var clientes = clientesDto?.Select(dto => new Cliente().FromDto(dto)).ToList();

                // Devuelve la lista o una lista vacía si la deserialización resulta en null.
                return clientes ?? new List<Cliente>();
            }
            catch (HttpRequestException ex)
            {
                // Maneja errores específicos de la solicitud HTTP (ej. 404 Not Found, 500 Internal Server Error).
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                throw; // Relanza la excepción para que pueda ser manejada en el ViewModel.
            }
            catch (Exception ex)
            {
                // Maneja otros errores inesperados (ej. problemas de red, deserialización).
                Console.WriteLine($"Error inesperado al obtener clientes: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo cliente enviando una solicitud POST a la API.
        /// </summary>
        /// <param name="cliente">El objeto Cliente a crear.</param>
        /// <returns>El objeto Cliente creado con su nuevo ID.</returns>
        public async Task<Cliente> CreateClienteAsync(Cliente cliente)
        {
            try
            {
                // Convierte el modelo Cliente a un DTO para enviarlo a la API.
                var clienteDto = cliente.ToDto();
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, clienteDto);
                response.EnsureSuccessStatusCode();

                // Lee la respuesta para obtener el DTO del cliente creado (con el ID de la base de datos).
                var createdClienteDto = await response.Content.ReadFromJsonAsync<ClienteDTO>();

                // Convierte el DTO de respuesta de nuevo a un modelo Cliente y lo devuelve.
                return new Cliente().FromDto(createdClienteDto);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al crear cliente: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al crear cliente: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza un cliente existente enviando una solicitud PUT a la API.
        /// </summary>
        /// <param name="cliente">El objeto Cliente con los datos actualizados.</param>
        public async Task UpdateClienteAsync(Cliente cliente)
        {
            try
            {
                var clienteDto = cliente.ToDto();
                // Asume que la URL de actualización usa el ID del cliente.
                var response = await _httpClient.PutAsJsonAsync($"{_apiEndpoint}/{cliente.ClienteId}", clienteDto);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al actualizar cliente: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al actualizar cliente: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Elimina un cliente por su ID enviando una solicitud DELETE a la API.
        /// </summary>
        /// <param name="clienteId">El ID del cliente a eliminar.</param>
        public async Task DeleteClienteAsync(int clienteId)
        {
            try
            {
                // Asume que la URL de eliminación usa el ID del cliente.
                var response = await _httpClient.DeleteAsync($"{_apiEndpoint}/{clienteId}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al eliminar cliente: {ex.Message}");
                throw;
            }
        }
    }
}