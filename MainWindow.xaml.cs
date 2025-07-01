using System;
using System.Windows;
using System.Windows.Controls;
using LPAC___Proyecto_II_frontend.ViewModel;
using LPAC___Proyecto_II_frontend.Views; 

namespace LPAC___Proyecto_II_frontend
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        /// <summary>
        /// Manejador de eventos para el botón "Empleados".
        /// </summary>
        private void Empleados_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Crea una nueva instancia de la vista de Empleados y la asigna al ContentControl.
                // Esta clase (EmpleadoWindow) debe ser un UserControl, no una Window.
                MainContentArea.Content = new EmpleadoWindow();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir al crear la vista.
                MessageBox.Show($"Error al cargar la vista de Empleados: {ex.Message}", "Error de Carga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Manejador de eventos para el botón "Clientes".
        /// </summary>
        private void Clientes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Crea una nueva instancia de la vista de Clientes y la asigna al ContentControl.
                // Basado en tu código, esta clase debería ser ClienteView (o ClienteWindow convertida a UserControl).
                MainContentArea.Content = new ClienteWindow();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir al crear la vista.
                MessageBox.Show($"Error al cargar la vista de Clientes: {ex.Message}", "Error de Carga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Manejador de eventos para el botón "Productos".
        /// </summary>
        private void Productos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Crea una nueva instancia de la vista de Productos y la asigna al ContentControl.
                // Esta clase (ProductoWindow) debe ser un UserControl, no una Window.
                MainContentArea.Content = new ProductoWindow();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir al crear la vista.
                MessageBox.Show($"Error al cargar la vista de Productos: {ex.Message}", "Error de Carga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Manejador de eventos para el botón "Productos".
        /// </summary>
        private void Ordenes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Crea una nueva instancia de la vista de Productos y la asigna al ContentControl.
                // Esta clase (ProductoWindow) debe ser un UserControl, no una Window.
                MainContentArea.Content = new OrdenWindow();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir al crear la vista.
                MessageBox.Show($"Error al cargar la vista de Productos: {ex.Message}", "Error de Carga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}