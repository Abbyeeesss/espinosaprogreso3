using SQLite;
using espinosaprogreso3.Models;

namespace espinosaprogreso3.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public async Task Init()
        {
            if (_database != null)
                return;

            var path = Path.Combine(FileSystem.AppDataDirectory, "ropa.db");
            _database = new SQLiteAsyncConnection(path);
            await _database.CreateTableAsync<Prenda>();
        }

        public async Task<List<Prenda>> GetPrendasAsync()
        {
            await Init();
            return await _database.Table<Prenda>().ToListAsync();
        }

        public async Task AddPrendaAsync(Prenda prenda)
        {
            await Init();
            await _database.InsertAsync(prenda);
        }
    }
}
