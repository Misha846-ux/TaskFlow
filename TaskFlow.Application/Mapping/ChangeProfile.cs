using AutoMapper;
using TaskFlow.Application.DTOs.ChangeDTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mapping
{
    public class ChangeProfile : Profile
    {
        public ChangeProfile()
        {
            CreateMap<ChangeEntity, ChangeDto>()
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.NoteId));
        }
    }
}
