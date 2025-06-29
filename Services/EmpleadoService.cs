using LPAC___Proyecto_II_frontend.DTOs; 
using LPAC___Proyecto_II_frontend.Models; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json; 
using System.Threading.Tasks;

namespace LPAC___Proyecto_II_frontend.Services
{
   
    public class EmpleadoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public EmpleadoService()
        {
            _httpClient = new HttpClient();
            
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl();
            _apiEndpoint = $"{baseUrlFromConfig}/api/Empleado"; 
            _httpClient.BaseAddress = new Uri(baseUrlFromConfig);
        }

      
        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            try
            {
           
                var response = await _httpClient.GetAsync(_apiEndpoint);

           
                response.EnsureSuccessStatusCode();

          
                var empleadosDto = await response.Content.ReadFromJsonAsync<List<EmpleadoDTO>>();

           
        
                var empleados = empleadosDto?.Select(dto => new Empleado().FromDto(dto)).ToList();
                return empleados ?? new List<Empleado>();
            }
            catch (HttpRequestException e)
            {
                
                Console.WriteLine($"Error de solicitud HTTP al obtener empleados: {e.Message}");
                throw; 
            }
            catch (Exception e)
            {
         
                Console.WriteLine($"Error inesperado al obtener empleados: {e.Message}");
                throw; // Relanza la excepción
            }
        }
    }
}