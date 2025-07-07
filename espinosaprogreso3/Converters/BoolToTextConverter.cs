using System.Globalization;

namespace espinosaprogreso3.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public static readonly BoolToTextConverter Instance = new BoolToTextConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool enInventario)
                return enInventario ? "✅ Agregar al inventario" : "❌ No agregar al inventario";

            return "Estado desconocido";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}