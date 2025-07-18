﻿using LPAC___Proyecto_II_frontend.DTOs;
using LPAC___Proyecto_II_frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LPAC___Proyecto_II_frontend.Services
{
    public class InformacionDeMiCompaniaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public InformacionDeMiCompaniaService()
        {
            _httpClient = AuthService.GetHttpClient();
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl();
            _apiEndpoint = $"{baseUrlFromConfig}/api/InfoCompania";
        }

        public async Task<InformacionDeMiCompania> GetInfoActual()
        {
            try
            {
                string requestUrl = _apiEndpoint;

                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var infoDTO = await response.Content.ReadFromJsonAsync<InformacionDeMiCompaniaDTO>();

                if(infoDTO != null)
                {
                    var info = InformacionDeMiCompania.FromDTO(infoDTO);

                    return info;
                }

                return new InformacionDeMiCompania
                {
                    Nombre = "No disponible",
                    Direccion = "No disponible",
                    Ciudad = "No disponible",
                    Estado_o_provincia = "No disponible",
                    Codigo_postal = "No disponible",
                    Pais = "No disponible",
                    Telefono = "No disponible",
                    Num_fax = "No disponible",
                    Terminos_pago = "No disponible",
                    Mensaje = "No disponible"
                };
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error al obtener informacion de la compañia httpEX: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado al obtener informacion de la compañia EX: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }


    }
}
