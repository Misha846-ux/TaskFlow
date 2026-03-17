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
               opt => opt.MapFrom(src => src.Users.Select(u => u.Id))
           )
           .ForMember(
               dest => dest.ProjectsId,
               opt => opt.MapFrom(src => src.Projects.Select(p => p.Id))
           );

        CreateMap<CompanyPostDto, CompanyEntity>();

        CreateMap<CompanyUpdateDto, CompanyEntity>();
    }
}
