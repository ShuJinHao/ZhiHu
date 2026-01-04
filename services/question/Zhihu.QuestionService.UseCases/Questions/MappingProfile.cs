using AutoMapper;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.QuestionService.UseCases.Questions.Commands;

namespace Zhihu.QuestionService.UseCases.Questions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateQuestionCommand, Question>();
        CreateMap<UpdateQuestionCommand, Question>();
    }
}