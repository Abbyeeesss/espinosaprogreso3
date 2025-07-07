using System.Globalization;

namespace espinosaprogreso3.Converters
{
    public class LogToColorConverter : IValueConverter
    {
        public static readonly LogToColorConverter Instance = new LogToColorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string log)
            {
                if (log.Contains("ERROR") || log.Contains("❌"))
                    return Colors.Red;
                else if (log.Contains("ADVERTENCIA") || log.Contains("⚠️"))
                    return Colors.Orange;
                else if (log.Contains("Sistema iniciado") || log.Contains("✅"))
                    return Colors.Green;
                else
                    return Colors.Blue;
            }

            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}