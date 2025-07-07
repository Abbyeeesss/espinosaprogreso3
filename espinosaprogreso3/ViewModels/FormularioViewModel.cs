using System.Windows.Input;
using espinosaprogreso3.Models;
using espinosaprogreso3.Services;

namespace espinosaprogreso3.ViewModels
{
    public class FormularioViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private string _nombrePrenda = string.Empty;
        private string _color = string.Empty;
        private int _talla = 10;
        private bool _enInventario;

        public string NombrePrenda
        {
            get => _nombrePrenda;
            set => SetProperty(ref _nombrePrenda, value);
        }

        public string Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public int Talla
        {
            get => _talla;
            set => SetProperty(ref _talla, value);
        }

        public bool EnInventario
        {
            get => _enInventario;
            set => SetProperty(ref _enInventario, value);
        }

        public ICommand GuardarCommand { get; }
        public ICommand LimpiarCommand { get; }

        public FormularioViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Registrar Prenda";

            GuardarCommand = new Command(async () => await GuardarPrendaAsync());
            LimpiarCommand = new Command(LimpiarFormulario);
        }

        private async Task GuardarPrendaAsync()
        {
            if (IsBusy) return;

            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(NombrePrenda))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El nombre de la prenda es requerido", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Color))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El color es requerido", "OK");
                    return;
                }

                if (Talla <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "La talla debe ser mayor a 0", "OK");
                    return;
                }

                IsBusy = true;

                var prenda = new Prenda
                {
                    NombrePrenda = NombrePrenda.Trim(),
                    Color = Color.Trim(),
                    Talla = Talla,
                    EnInventario = EnInventario
                };

                await _databaseService.SavePrendaAsync(prenda);

                await Application.Current.MainPage.DisplayAlert("Éxito", "Prenda guardada correctamente", "OK");
                LimpiarFormulario();

                // QUITÉ LA NAVEGACIÓN AUTOMÁTICA QUE CAUSABA EL ERROR
            }
            catch (InvalidOperationException ex)
            {
                // Esta es la validación de la regla de negocio (talla < 10 en inventario)
                await Application.Current.MainPage.DisplayAlert("Error de Validación", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al guardar: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void LimpiarFormulario()
        {
            NombrePrenda = string.Empty;
            Color = string.Empty;
            Talla = 10;
            EnInventario = false;
        }
    }
}