using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs.CreateProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.DetailsProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.ListProjectDTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<CreateProjectDto, ProjectEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<ProjectEntity, ProjectListItemDto>()
                .ForMember(dest => dest.WorkersCount, opt => opt.MapFrom(src => src.Users.Count))
                .ForMember(dest => dest.TasksCount, opt => opt.MapFrom(_ => 0));

            CreateMap<ProjectEntity, ProjectDetailsDto>()
                .ForMember(dest => dest.WorkersCount, opt => opt.MapFrom(src => src.Users.Count))
                .ForMember(dest => dest.TasksCount, opt => opt.MapFrom(_ => 0))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            CreateMap<ProjectUserEntity, ProjectUserListItemDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
        }
    }
}
