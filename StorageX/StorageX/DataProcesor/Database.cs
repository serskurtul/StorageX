using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using StorageX.Models;
using System.Linq;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace StorageX
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly string _databasePath;
        private const string TablePath = "items.db3";
        private const string InvoicePath = "Invoice.txt";
        private const string CategoriesPath = "Categories.txt";
        public Database(string databasePath)
        {
            _databasePath = databasePath;
            _database = new SQLiteAsyncConnection(Path.Combine(_databasePath, TablePath));
            _database.CreateTableAsync<Item>();
            if (!File.Exists(Path.Combine(_databasePath, CategoriesPath)))
            {
                var file = File.Create(Path.Combine(_databasePath, CategoriesPath));
                file.Close();
            }
            if (!File.Exists(Path.Combine(_databasePath, InvoicePath)))
            {
                var file = File.Create(Path.Combine(_databasePath, InvoicePath));
                file.Close();
            }
        }

        public Task<List<Item>> GetCarpetsAsync() => _database.Table<Item>().ToListAsync();

        public Task<Item> GetCarpetAsync(int id) => _database.Table<Item>().FirstOrDefaultAsync(x => x.Id == id);

        public Task<int> PostCarpetAsync(Item carpet) => _database.InsertAsync(carpet);

        public Task<int> DeleteCarpetAsync(Item carpet) => _database.DeleteAsync(carpet);

        public Task<int> PutCarpetAsync(Item carpet) => _database.UpdateAsync(carpet);

        public string[] GetCategories()
        {
            var result = File.ReadAllText(Path.Combine(_databasePath, CategoriesPath));
            return result.Split('$');
        }
        public void PostCategories(string newCategory)
        {
            var result = File.ReadAllText(Path.Combine(_databasePath, CategoriesPath));
            result += "$" + newCategory;
            File.WriteAllText(Path.Combine(_databasePath, CategoriesPath), result);
        }
        private void PostCategories(string[] categories)
        {
            var categoriesStr = String.Join("$", categories);
            File.WriteAllText(Path.Combine(_databasePath, CategoriesPath), categoriesStr);
        }
        public void DeleteCategory(string category)
        {
            var result = GetCategories();
            result = result.Where(x => x != category).ToArray();
            PostCategories(result);
        }

        public List<Invoice> GetData()
        {
            var json = File.ReadAllText(Path.Combine(_databasePath, InvoicePath));
            return JsonConvert.DeserializeObject<List<Invoice>>(json) ?? new List<Invoice>();
        }
        public List<Invoice> GetData(DateTime date)
        {
            var json = File.ReadAllText(Path.Combine(_databasePath, InvoicePath));
            var res = JsonConvert.DeserializeObject<List<Invoice>>(json) ?? new List<Invoice>();
            res.RemoveAll(x => x.DateTime.Date != date.Date);
            return res;
        }
        private void WriteData(List<Invoice> data)
        {
            if (File.Exists(Path.Combine(_databasePath, InvoicePath)))
            {
                File.WriteAllText(Path.Combine(_databasePath, InvoicePath), JsonConvert.SerializeObject(data));
            }
        }
        public void WriteData(Invoice obj)
        {
            var list = GetData();
            if (obj.Id == null)
                obj.Id = Guid.NewGuid();
            list.Add(obj);
            WriteData(list);
        }
        public bool DeleteData(Guid id)
        {
            var list = GetData();
            var obj = list.FirstOrDefault(x => x.Id == id);
            if (obj != null)
            {
                list.Remove(obj);
                WriteData(list);
                return true;
            }
            return false;
        }
    }
}

