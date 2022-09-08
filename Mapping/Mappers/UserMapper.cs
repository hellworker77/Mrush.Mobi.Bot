using AutoMapper;
using Domain.Models;
using Entities.Implementation;

namespace Mapping.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserEntity, User>().ReverseMap();
    }
}