using System.Windows.Input;
using espinosaprogreso3.Models;
using espinosaprogreso3.Services;
using System.Text;

namespace espinosaprogreso3.ViewModels
{
    public class FormularioViewModel : BaseViewModel
    {
        public string PrendaNombre { get; set; }
        public string Color { get; set; }
        public int Talla { get; set; }
        public bool EnInventario { get; set; }

        public ICommand GuardarCommand { get; }

        private DatabaseService _databaseService = new();

        public FormularioViewModel()
        {
            GuardarCommand = new Command(async () => await Guardar());
        }

        private async Task Guardar()
        {
            // Validación
            if (EnInventario && Talla < 10)
            {
                await Shell.Current.DisplayAlert("Error", "No se puede guardar una prenda en inventario con talla menor a 10", "OK");
                return;
            }

            var nuevaPrenda = new Prenda
            {
                PrendaNombre = PrendaNombre,
                Color = Color,
                Talla = Talla,
                EnInventario = EnInventario
            };

            await _databaseService.AddPrendaAsync(nuevaPrenda);

            // Guardar en archivo Log
            string log = $"Se incluyó el registro {PrendaNombre} el {DateTime.Now:dd/MM/yyyy HH:mm}";
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "Logs_Espinosa.txt");
            File.AppendAllText(filePath, log + Environment.NewLine);

            await Shell.Current.DisplayAlert("Éxito", "Registro guardado", "OK");

            // Limpiar formulario
            PrendaNombre = Color = string.Empty;
            Talla = 0;
            EnInventario = false;
            OnPropertyChanged(nameof(PrendaNombre));
            OnPropertyChanged(nameof(Color));
            OnPropertyChanged(nameof(Talla));
            OnPropertyChanged(nameof(EnInventario));
        }
    }
}
