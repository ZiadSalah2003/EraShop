using EraShop.API.Contracts.Authentication;
using EraShop.API.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity.Data;

namespace EraShop.API.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SignUpRequest, ApplicationUser>()
                .Map(dest => dest.UserName, dest => dest.Email);
        }
    }
}
