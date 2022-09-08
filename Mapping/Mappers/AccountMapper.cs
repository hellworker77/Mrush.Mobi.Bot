using AutoMapper;
using Domain.Models;
using Entities.Implementation;

namespace Mapping.Mappers;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<AccountEntity, Account>().ReverseMap();
    }
}