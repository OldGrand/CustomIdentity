using AutoMapper;
using CustomIdentity.BLL.Models.Account;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.AutoMapper
{
    public class BusinessLogicLayerModelsProfile : Profile
    {
        public BusinessLogicLayerModelsProfile()
        {
            CreateMap<User, RegisterModel>().ForMember(_ => _.Password, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
