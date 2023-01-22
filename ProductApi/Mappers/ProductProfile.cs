using AutoMapper;
using ProductApi.Models;
using ProductApi.ViewModels;

namespace ProductApi.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile() {
            CreateMap<ProductVM, Product>();
        }
    }
}
