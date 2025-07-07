using System.Collections.ObjectModel;
using System.Windows.Input;
using espinosaprogreso3.Services;

namespace espinosaprogreso3.ViewModels
{
    public class LogsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private string _logContent = string.Empty;

        public ObservableCollection<string> Logs { get; }

        public string LogContent
        {
            get => _logContent;
            set => SetProperty(ref _logContent, value);
        }

        public ICommand LoadLogsCommand { get; }
        public ICommand ClearLogsCommand { get; }
        public ICommand ExportLogsCommand { get; }

        public LogsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Logs del Sistema";

            Logs = new ObservableCollection<string>();

            LoadLogsCommand = new Command(async () => await LoadLogsAsync());
            ClearLogsCommand = new Command(ClearLogs);
            ExportLogsCommand = new Command(async () => await ExportLogsAsync());

            // Cargar logs iniciales
            _ = Task.Run(async () => await LoadLogsAsync());
        }

        private async Task LoadLogsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // Obtener estadísticas de la base de datos
                var todasLasPrendas = await _databaseService.GetPrendasAsync();
                var prendasEnInventario = await _databaseService.GetPrendasEnInventarioAsync();

                Logs.Clear();

                // Agregar logs del sistema
                Logs.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Sistema iniciado");
                Logs.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Total de prendas: {todasLasPrendas.Count}");
                Logs.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Prendas en inventario: {prendasEnInventario.Count}");

                // Logs por talla
                var prendasPorTalla = todasLasPrendas.GroupBy(p => p.Talla).OrderBy(g => g.Key);
                foreach (var grupo in prendasPorTalla)
                {
                    Logs.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Talla {grupo.Key}: {grupo.Count()} prendas");
                }

                // Logs de validación
                var prendasInvalidasEnInventario = todasLasPrendas.Where(p => p.EnInventario && p.Talla < 10).ToList();
                if (prendasInvalidasEnInventario.Any())
                {
                    Logs.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ⚠️ ADVERTENCIA: {prendasInvalidasEnInventario.Count} prendas en inventario con talla < 10");
                }

                // Crear contenido de log como texto
                LogContent = string.Join("\n", Logs);
            }
            catch (Exception ex)
            {
                Logs.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ❌ ERROR: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar logs: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ClearLogs()
        {
            Logs.Clear();
            LogContent = string.Empty;
            Logs.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Logs limpiados");
            LogContent = string.Join("\n", Logs);
        }

        private async Task ExportLogsAsync()
        {
            try
            {
                IsBusy = true;

                var fileName = $"logs_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                await File.WriteAllTextAsync(filePath, LogContent);

                await Application.Current.MainPage.DisplayAlert("Éxito", $"Logs exportados a: {fileName}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al exportar logs: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void AddLog(string message)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            Logs.Add(logEntry);
            LogContent = string.Join("\n", Logs);
        }
    }
}