using LPAC___Proyecto_II_frontend.Models.Enums;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace LPAC___Proyecto_II_frontend.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null || !(value is UserRole userRole))
                return Visibility.Collapsed;

            var requiredRoles = parameter.ToString()
                .Split('|')
                .Select(r => r.Trim())
                .Where(r => !string.IsNullOrEmpty(r))
                .Select(r => Enum.Parse(typeof(UserRole), r))
                .Cast<UserRole>();

            return requiredRoles.Contains(userRole)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}