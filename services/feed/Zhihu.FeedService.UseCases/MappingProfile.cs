using Zhihu.FeedService.Core.Entities;
using Zhihu.FeedService.UseCases.Commands;

namespace Zhihu.FeedService.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateInboxFeedsCommand, Inbox>();

        CreateMap<CreateOutboxFeedCommand, Outbox>()
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.SenderId));
    }
}