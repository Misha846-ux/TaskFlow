using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectPostDto, ProjectEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.Tasks, opt => opt.Ignore());

            CreateMap<ProjectEntity, ProjectGetDto>()
                .ForMember(dest => dest.UsersId, opt => opt.MapFrom(src => src.Users.Select(u => u.UserId)))
                .ForMember(dest => dest.TasksId, opt => opt.MapFrom(src => src.Tasks.Select(t => t.Id)));
            CreateMap<ProjectUpdateDto, ProjectEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.Tasks, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore());
        }
    }
}
