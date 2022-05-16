namespace Catalog_API.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public ICollection<ProductDTO> Products { get; set; }
}
