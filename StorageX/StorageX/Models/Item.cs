using System;
using System.IO;
using SQLite;
using Xamarin.Essentials;

namespace StorageX.Models
{
    public class Item
    {

        protected string[] categories;

        public Item()
        {
        }
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CategoriesStr { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal Purchase { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public bool isPath { get; set; }
        public string ImageName { get; set; }
        public string Produser { get; set; }
        public int Amount { get; set; }

        public string Description
        {
            get
            {
                if(isPath)
                    return $"Доріжка {Width}x{Height} — ціна:{Price}";

                return $"{Width}x{Height} — ціна:{Price} - к-ть:{Amount}";
            }
        }
        public void AddCategory(string category)
        {
            if (category == null)
            {
                return;
            }
            if (categories != null)
                Array.Resize(ref categories, categories.Length + 1);
            else
            {
                categories = new string[1];
            }
            categories[categories.Length - 1] = category;
            CategoriesStr = String.Join("$", categories);
        }
        public string[] Categories
        {
            get
            {
                categories = CategoriesStr?.Split('$');
                return categories;
            }
        }
        public string ImageFullPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), (ImageName ?? ""));
            }
        }
    }
}

