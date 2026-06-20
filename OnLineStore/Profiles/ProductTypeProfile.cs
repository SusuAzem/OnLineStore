using OnLineStore.ViewModels;

using AutoMapper;

using Core;

namespace OnLineStore.Profiles
{
    public class ProductTypeProfile : Profile
    {
        public ProductTypeProfile()
        {
            CreateMap<ProductType, ProductTypeViewModel>()
            .ReverseMap();            
        }
    }
}
