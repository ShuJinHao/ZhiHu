using AutoMapper;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.QuestionService.UseCases.Answers.Commands;

namespace Zhihu.QuestionService.UseCases.Answers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateAnswerCommand, Answer>();
        CreateMap<UpdateAnswerCommand, Answer>();
    }
}