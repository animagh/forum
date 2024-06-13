using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Topic, TopicDto>()
                    .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"))
                    .ForMember(dest => dest.CommentNum, opt => opt.MapFrom(src => src.CommentNum));

        CreateMap<Topic, TopicWithContentDto>()
            .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

        CreateMap<CreateTopicDto, Topic>();

        CreateMap<CreateCommentDto, Comment>();

        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"));

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));
    }

}

