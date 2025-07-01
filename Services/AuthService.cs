using LPAC___Proyecto_II_frontend.DTOs.Autenticacion;
using LPAC___Proyecto_II_frontend.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LPAC___Proyecto_II_frontend.Services
{
    public static class AuthService
    {
        private static readonly HttpClient _httpClient;
        private static string _authUrl;

        // Propiedades para almacenar el estado de autenticación
        public static string Token { get; private set; }
        public static DateTime TokenExpiration { get; private set; } = DateTime.MinValue;
        public static string NombreCompleto { get; private set; }
        public static UserRole Rol { get; private set; }
        public static bool IsAuthenticated => !string.IsNullOrEmpty(Token);

        static AuthService()
        {
            _httpClient = new HttpClient();
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl();
            _authUrl = $"{baseUrlFromConfig}/api/Auth/login";
            _httpClient.BaseAddress = new Uri(baseUrlFromConfig);
        }

        public static async Task<bool> LoginAsync(string nombreUsuario, string contrasena)
        {
            try
            {
                var loginDto = new EmpleadoLoginDTO
                {
                    NombreUsuario = nombreUsuario,
                    Contrasena = contrasena
                };

                var response = await _httpClient.PostAsJsonAsync(_authUrl, loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<RespuestaAutenticacionDTO>();

                    Token = authResponse.Token;
                    TokenExpiration = authResponse.Expiracion;
                    NombreCompleto = authResponse.NombreCompleto;
                    Rol = Enum.Parse<UserRole>(authResponse.Rol);

                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", Token);

                    return true;
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static void Logout()
        {
            Token = null;
            TokenExpiration = DateTime.MinValue;
            NombreCompleto = null;
            Rol = UserRole.Ninguno;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }


    }
}
