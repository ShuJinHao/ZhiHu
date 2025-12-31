using AutoMapper;
using Zhihu.UserService.Core.Entities;

namespace Zhihu.UserService.UseCases;

public record CreatedAppUserDto(int Id, string Nickname);

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AppUser, CreatedAppUserDto>();
    }
}