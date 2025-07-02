using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LPAC___Proyecto_II_frontend.DTOs; // Importante: Usa el DTO de Rol
using LPAC___Proyecto_II_frontend.Models; // Solo si se necesitan los modelos para algo más, sino se puede quitar
using System;
using System.Linq;

namespace LPAC___Proyecto_II_frontend.Services
{
    public class RolService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public RolService()
        {
            _httpClient = AuthService.GetHttpClient();
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl(); // Asumo que AppConfig existe
            _apiEndpoint = $"{baseUrlFromConfig}/api/Rol"; // Endpoint del controlador de Roles
        }

        // CAMBIO CRUCIAL: Ahora devuelve List<RolDTO>
        public async Task<List<RolDTO>> GetAllRolesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode();

                // Directamente lee como List<RolDTO>
                var rolesDto = await response.Content.ReadFromJsonAsync<List<RolDTO>>();
                return rolesDto ?? new List<RolDTO>(); // Devuelve los DTOs directamente
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener roles: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al obtener roles: {ex.Message}");
                throw;
            }
        }

        // Puedes añadir métodos para Crear, Modificar, Eliminar si tu aplicación los necesita para Roles
        // Pero para el CRUD de empleados, solo necesitarías GetAllRolesAsync para los ComboBoxes.
    }
}