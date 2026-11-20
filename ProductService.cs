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

        private DateOnly PrevDate { get; } = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1));

        private decimal Discount { get; } = 0.25m;

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
                    .Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                        (p.Description != null && p.Description.ToLower().Contains(search.ToLower())));
            }

            return [.. query];
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

            foreach (var product in productsFromDb)
            {
                bool hasOrderLastMonth = product.OrderProducts
                    .Any(op => op.IdOrderNavigation.OrderDate.Year == PrevDate.Year
                    && op.IdOrderNavigation.OrderDate.Month == PrevDate.Month);

                int? totalQuantity = product.StockItems.Sum(p => p.QuantityAvailable);

                products.Add(new ProductCard()
                {
                    Id = product.IdProduct,
                    ImageSource = $"/Images/{product.Image ?? "picture.png"}",
                    Category = product.IdCategoryNavigation.Name,
                    Name = product.ProductName,
                    Composition = product.Composition,
                    Productoin = product.IdProductionNavigation.Name,
                    PairCount = totalQuantity,
                    PairCountText = totalQuantity > 5 ? "много" : "мало",
                    Price = hasOrderLastMonth ? product.Price : product.Price - product.Price * Discount,
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

        private Product? GetProductByIdFromDb(int id)
        {
            return _context.Products
                .Include(p => p.OrderProducts)
                    .ThenInclude(op => op.IdOrderNavigation)
                .Include(p => p.IdCategoryNavigation)
                .Include(p => p.IdProductionNavigation)
                .Include(p => p.StockItems)
                    .ThenInclude(si => si.IdSizeNavigation)
                .FirstOrDefault(p => p.IdProduct == id);
        }

        public OrderCreateCard? GetProductById(int id)
        {
            var product = GetProductByIdFromDb(id);

            if (product == null) return null;

            List<PairCount> pairs = [];

            foreach(var pair in product.StockItems)
            {
                pairs.Add(new PairCount()
                {
                    IdSize = pair.IdSize,
                    Size = pair.IdSizeNavigation.Size1,
                    QuantityAvailable = pair.QuantityAvailable
                });
            }

            bool hasOrderLastMonth = product.OrderProducts
                   .Any(op => op.IdOrderNavigation.OrderDate.Year == PrevDate.Year
                   && op.IdOrderNavigation.OrderDate.Month == PrevDate.Month);

            return new OrderCreateCard
            {
                Image = $"/Images/{product.Image ?? "picture.png"}",
                Category = product.IdCategoryNavigation.Name,
                Name = product.ProductName,
                Productoin = product.IdProductionNavigation.Name,
                Description = product.Description,
                Composition = product.Composition,
                Price = hasOrderLastMonth ? product.Price : product.Price - product.Price * Discount,
                Pairs = pairs
            };
        }

        private void QuantityAvailableUpdate(int idSize, int count, int idProduct)
        {
            var stockItem = _context.StockItems
                .FirstOrDefault(si => si.IdSize == idSize && si.IdProduct == idProduct);
            if (stockItem != null)
            {
                stockItem.QuantityAvailable -= count;
                _context.SaveChanges();
            }
        }

        public void CreateOrder(int userId, int productId, int count, int idSize)
        {
            int maxOrderId = _context.Orders.Max(o => o.IdOrder);
            int maxOrderNumber = _context.Orders.Max(o => o.OrderNumber);
            int maxOrderProductId = _context.OrderProducts.Max(op => op.IdOrderProduct);

            var order = new Order()
            {
                IdOrder = maxOrderId + 1,
                OrderNumber = maxOrderNumber + 1,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                IdUser = userId,
                Count = count,
                IdSize = idSize,
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        IdOrderProduct = maxOrderProductId + 1,
                        IdProduct = productId
                    }
                }

            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            QuantityAvailableUpdate(idSize, count, productId);
        }
    }
}
