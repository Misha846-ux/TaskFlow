using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ComapniesDTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mapping;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CompanyEntity, CompanyGetDto>()
           .ForMember(
               dest => dest.EmploeesId,
               opt => opt.MapFrom(src => src.CompanyUsers.Select(u => u.UserId))
           )
           .ForMember(
               dest => dest.ProjectsId,
               opt => opt.MapFrom(src => src.Projects.Select(p => p.Id))
           );

        CreateMap<CompanyPostDto, CompanyEntity>()
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => DateTime.UtcNow)
            )
            .ForMember(dest => dest.Projects, opt => opt.Ignore())
            .ForMember(dest => dest.CompanyUsers, opt => opt.Ignore());

        CreateMap<CompanyUpdateDto, CompanyEntity>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore())
            .ForMember(dest => dest.CompanyUsers, opt => opt.Ignore());
    }
}
