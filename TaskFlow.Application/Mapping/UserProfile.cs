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

        CreateMap<UserPostDto, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.GlobalRole, opt => opt.Ignore())
            .ForMember(dest => dest.PassToIcon, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RecoveryTokenHash, opt => opt.Ignore())
            .ForMember(dest => dest.RecoveryTokenLifeTime, opt => opt.Ignore())
            .ForMember(dest => dest.Companies, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore())
            .ForMember(dest => dest.Tasks, opt => opt.Ignore())
            .ForMember(dest => dest.Changes, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        CreateMap<UserUpdateDto, UserEntity>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.GlobalRole, opt => opt.MapFrom(src => src.GlobaleRole.ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RecoveryTokenHash, opt => opt.Ignore())
            .ForMember(dest => dest.RecoveryTokenLifeTime, opt => opt.Ignore())
            .ForMember(dest => dest.Tasks, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore())
            .ForMember(dest => dest.Companies, opt => opt.Ignore())
            .ForMember(dest => dest.Changes, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore());

    }
}
