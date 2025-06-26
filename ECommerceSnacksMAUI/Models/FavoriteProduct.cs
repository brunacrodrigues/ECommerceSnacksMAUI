using SQLite;

namespace ECommerceSnacksMAUI.Models
{
    public class FavoriteProduct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }
        public decimal Price { get; set; }
        public string? UrlImage { get; set; }
        public bool IsFavorite { get; set; }
    }
}
