namespace demo
{
    public class ProductCard
    {
        public string ImageSource { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Productoin {  get; set; } = string.Empty;
        public string? Composition { get; set; }
        public decimal Price { get; set; }
        public int? PairCount { get; set; }
        public string PairCountText { get; set; } = string.Empty;
        public string BackgroundColor {  get; set; } = string.Empty;
    }
}