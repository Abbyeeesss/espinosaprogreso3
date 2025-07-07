using SQLite;
using espinosaprogreso3.Models;

namespace espinosaprogreso3.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;
        private readonly string _databasePath;

        public DatabaseService()
        {
            _databasePath = Path.Combine(FileSystem.AppDataDirectory, "inventario.db3");
        }

        private async Task InitializeAsync()
        {
            if (_database is not null)
                return;

            _database = new SQLiteAsyncConnection(_databasePath);
            await _database.CreateTableAsync<Prenda>();
        }

        public async Task<List<Prenda>> GetPrendasAsync()
        {
            await InitializeAsync();
            return await _database.Table<Prenda>().ToListAsync();
        }

        public async Task<List<Prenda>> GetPrendasEnInventarioAsync()
        {
            await InitializeAsync();
            return await _database.Table<Prenda>().Where(p => p.EnInventario).ToListAsync();
        }

        public async Task<int> SavePrendaAsync(Prenda prenda)
        {
            await InitializeAsync();

            // REGLA DE NEGOCIO: No prendas en inventario con talla < 10
            if (prenda.EnInventario && prenda.Talla < 10)
            {
                throw new InvalidOperationException("No se pueden registrar prendas en inventario con talla menor a 10");
            }

            if (prenda.Id != 0)
                return await _database.UpdateAsync(prenda);
            else
                return await _database.InsertAsync(prenda);
        }

        public async Task<int> DeletePrendaAsync(Prenda prenda)
        {
            await InitializeAsync();
            return await _database.DeleteAsync(prenda);
        }

        // Para el manejo de archivos - exportar datos
        public async Task<string> ExportarDatosAsync()
        {
            var prendas = await GetPrendasAsync();
            var json = System.Text.Json.JsonSerializer.Serialize(prendas, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });

            var fileName = $"inventario_backup_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            await File.WriteAllTextAsync(filePath, json);
            return filePath;
        }
    }
}
