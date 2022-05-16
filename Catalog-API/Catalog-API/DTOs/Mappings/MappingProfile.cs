using AutoMapper;
using Catalog_API.Models;

namespace Catalog_API.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //ReverseMap() means that it is a relations reversed, being able to transform one into the other and vice versa
            CreateMap<Product,ProductDTO>().ReverseMap();//Informs AutoMapper that we will map the Product class to ProductDTO
            CreateMap<Category,CategoryDTO>().ReverseMap();
        }
    }
}
