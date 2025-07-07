using System.Collections.ObjectModel;
using System.Windows.Input;
using espinosaprogreso3.Models;
using espinosaprogreso3.Services;

namespace espinosaprogreso3.ViewModels
{
    public class ListaPrendasViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<Prenda> Prendas { get; }
        public ObservableCollection<Prenda> PrendasEnInventario { get; }

        public ICommand LoadPrendasCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ExportarCommand { get; }
        public ICommand DeletePrendaCommand { get; }

        public ListaPrendasViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Lista de Prendas";

            Prendas = new ObservableCollection<Prenda>();
            PrendasEnInventario = new ObservableCollection<Prenda>();

            LoadPrendasCommand = new Command(async () => await LoadPrendasAsync());
            RefreshCommand = new Command(async () => await RefreshPrendasAsync());
            ExportarCommand = new Command(async () => await ExportarDatosAsync());
            DeletePrendaCommand = new Command<Prenda>(async (prenda) => await DeletePrendaAsync(prenda));
        }

        public async Task LoadPrendasAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var todasLasPrendas = await _databaseService.GetPrendasAsync();
                var prendasInventario = await _databaseService.GetPrendasEnInventarioAsync();

                Prendas.Clear();
                PrendasEnInventario.Clear();

                foreach (var prenda in todasLasPrendas)
                    Prendas.Add(prenda);

                foreach (var prenda in prendasInventario)
                    PrendasEnInventario.Add(prenda);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar prendas: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshPrendasAsync()
        {
            await LoadPrendasAsync();
        }

        private async Task ExportarDatosAsync()
        {
            try
            {
                IsBusy = true;
                var filePath = await _databaseService.ExportarDatosAsync();
                await Application.Current.MainPage.DisplayAlert("Éxito", $"Datos exportados a: {Path.GetFileName(filePath)}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al exportar: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeletePrendaAsync(Prenda prenda)
        {
            if (prenda == null) return;

            try
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert(
                    "Confirmar",
                    $"¿Eliminar la prenda '{prenda.NombrePrenda}'?",
                    "Sí", "No");

                if (confirm)
                {
                    await _databaseService.DeletePrendaAsync(prenda);
                    await LoadPrendasAsync();
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Prenda eliminada", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al eliminar: {ex.Message}", "OK");
            }
        }
    }
}