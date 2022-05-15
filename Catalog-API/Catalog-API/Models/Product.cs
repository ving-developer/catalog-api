namespace Catalog_API.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string  Description { get; set; }

        public float Price { get; set; }

        public string ImageUrl { get; set; }

        public float Stock { get; set; }

        public DateTime RegisterDate { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
