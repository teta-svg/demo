using demo.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace demo
{
    public class ProductService
    {
        private readonly DemoDbContext _context;

        public ProductService(DemoDbContext context)
        {
            _context = context;
        }

        private List<Product> GetProductsFromDb(string filter, string? search = null)
        {
            var query = _context.Products
                    .Include(p => p.IdCategoryNavigation)
                    .Include(p => p.IdProductionNavigation)
                    .Include(p => p.StockItems)
                    .Include(p => p.OrderProducts)
                        .ThenInclude(op => op.IdOrderNavigation)
                    .AsQueryable();

            if (!filter.Contains("Все категории"))
            {
                query = query
                    .Where(p => p.IdCategoryNavigation.Name.Contains(filter));
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query
                    .Where(p =>
                        p.ProductName.ToLower().Contains(search.ToLower()) ||
                        (p.Description != null && p.Description.ToLower().Contains(search.ToLower())));
                    
            }

            return query.ToList();
        }

        private List<Category> GetCategoriesFromDb()
        {
            var categories = _context.Categories.ToList();

            return categories;
        }

        public ObservableCollection<ProductCard> GetProductList(string filter, string? search = null)
        {
            var productsFromDb = GetProductsFromDb(filter, search);

            ObservableCollection<ProductCard> products = [];

            DateOnly prevDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1));

            decimal discount = 0.25m;

            foreach (var product in productsFromDb)
            {
                bool hasOrderLastMonth = product.OrderProducts
                    .Any(op => op.IdOrderNavigation.OrderDate.Year == prevDate.Year
                    && op.IdOrderNavigation.OrderDate.Month == prevDate.Month);

                int? totalQuantity = product.StockItems.Sum(p => p.QuantityAvailable);

                products.Add(new ProductCard()
                {
                    ImageSource = $"/Images/{product.Image ?? "picture.png"}",
                    Category = product.IdCategoryNavigation.Name,
                    Name = product.ProductName,
                    Composition = product.Composition,
                    Productoin = product.IdProductionNavigation.Name,
                    PairCount = totalQuantity,
                    PairCountText = totalQuantity > 5 ? "много" : "мало",
                    Price = hasOrderLastMonth ? product.Price : product.Price - product.Price * discount,
                    BackgroundColor = totalQuantity <= 3 ? "#FF8080" : "#D2F6E7",
                });
            }

            return products;
        }

        public ObservableCollection<string> GetCategories()
        {
            var categoriesFromDb = GetCategoriesFromDb();

            ObservableCollection<string> categories = [];

            categories.Add("Все категории");

            foreach (var category in categoriesFromDb)
            {
                if (string.IsNullOrEmpty(category.Name)) continue;

                categories.Add(category.Name);
            }

            return categories;
        }
    }
}
