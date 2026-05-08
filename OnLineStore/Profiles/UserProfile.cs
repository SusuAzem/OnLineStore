using OnLineStore.ViewModels;

using AutoMapper;

using Core;

namespace OnLineStore.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<Client, ClientViewModel>();
        }
    }
}
