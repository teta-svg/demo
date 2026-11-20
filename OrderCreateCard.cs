namespace demo
{
    public class OrderCreateCard
    {
        public string? Image { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Productoin { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Composition { get; set; }
        public decimal Price { get; set; }
        public List<PairCount> Pairs { get; set; } = [];

    }

    public class PairCount
    {
        public int IdSize { get; set; }
        public decimal? Size { get; set; }
        public int? QuantityAvailable { get; set; }
    }
}
