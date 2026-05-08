using OnLineStore.ViewModels;

using AutoMapper;

using Core;

namespace OnLineStore.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductItemViewModel>()
            .ReverseMap();            
        }
    }
}
