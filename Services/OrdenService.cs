using LPAC___Proyecto_II_frontend.DTOs;
using LPAC___Proyecto_II_frontend.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows;

namespace LPAC___Proyecto_II_frontend.Services
{
    public class OrdenService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public OrdenService()
        {
            _httpClient = AuthService.GetHttpClient();
            string baseUrlFromConfig = AppConfig.GetApiBaseUrl();
            _apiEndpoint = $"{baseUrlFromConfig}/api/Orden";
        }

        /// <summary>
        /// Envía una nueva orden al backend
        /// </summary>
        public async Task<OrdenDTO> CreateOrdenAsync(OrdenDTO ordenDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, ordenDto);
                response.EnsureSuccessStatusCode();

                var createdOrdenDto = await response.Content.ReadFromJsonAsync<OrdenDTO>();

                MessageBox.Show($"Orden creada exitosamente {createdOrdenDto.idOrden}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                return createdOrdenDto;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error al crear la orden: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception($"Error al crear la orden: {ex.Message}");
            }
            catch (Exception ex)
            {
            MessageBox.Show($"Error inesperado al crear la orden: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception($"Error inesperado al crear la orden: {ex.Message}");
            }
        }

        /// <summary>
        /// Genera el PDF de la factura en el backend
        /// </summary>
        public async Task<byte[]> GenerateInvoicePdfAsync(OrdenDTO ordenDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"" +_apiEndpoint + "/generar-factura", ordenDto);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception($"Error al generar el PDF: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado al generar el PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception($"Error inesperado al generar el PDF: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene todas las órdenes del sistema
        /// </summary>
        public async Task<List<Orden>> GetAllOrdenesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode();

                var ordenesDto = await response.Content.ReadFromJsonAsync<List<OrdenDTO>>();
                return ordenesDto?.ConvertAll(dto => new Orden().FromDto(dto)) ?? new List<Orden>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error al obtener órdenes: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener órdenes: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene una orden específica por su ID
        /// </summary>
        public async Task<Orden> GetOrdenByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode();

                var ordenDto = await response.Content.ReadFromJsonAsync<OrdenDTO>();
                return new Orden().FromDto(ordenDto);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error al obtener la orden: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener la orden: {ex.Message}");
            }
        }
    }
}