using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserGetDto>()
            .ForMember(dest => dest.GlobalRole, opt => opt.MapFrom(src => src.GlobalRole.ToString()))
            .ForMember(dest => dest.CompaniesId, opt => opt.MapFrom(src => src.Companies.Select(u => u.CompanyId)));

        CreateMap<UserPostDto, UserEntity>();

        CreateMap<UserUpdateDto, UserEntity>();

        CreateMap<UserLoginDto, UserEntity>();
    }
}
