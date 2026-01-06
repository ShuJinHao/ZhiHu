using AutoMapper;
using Zhihu.AgentService.Core.Entities;
using Zhihu.AgentService.UseCases.Commands;

namespace Zhihu.AgentService.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateRobotCommand, Robot>();
    }
}