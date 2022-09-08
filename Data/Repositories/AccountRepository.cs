using AutoMapper;
using Data.Abstraction.Interfaces;
using Data.Implementation;
using Domain.Models;
using Entities.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationContext _applicationContext;
    private readonly DbSet<AccountEntity> _dbSet;
    private readonly IMapper _mapper;
    public AccountRepository(ApplicationContext applicationContext,
        IMapper mapper)
    {
        _applicationContext = applicationContext;
        _dbSet = applicationContext.Set<AccountEntity>();
        _mapper = mapper;
    }
    public async Task<Account> GetAccountByIdAsync(Guid accountId)
    {
        var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == accountId);
        var dto = _mapper.Map<Account>(entity);
        return dto;
    }

    public async Task AddAccountAsync(Account account)
    {
        var entity = _mapper.Map<AccountEntity>(account);
        await _dbSet.AddAsync(entity);
        await _applicationContext.SaveChangesAsync();
    }

    public async Task UpdateAccountAsync(Account account)
    {
        var entity = _mapper.Map<AccountEntity>(account);

        var existsEntity = await _applicationContext.Accounts.Include(x=>x.User).AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);

        if (existsEntity != null)
        {
            entity.User = existsEntity.User;

            _dbSet.Update(entity);

            await _applicationContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAccountAsync(Guid accountId)
    {
        var entity = await _dbSet.FindAsync(accountId);

        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _applicationContext.SaveChangesAsync();
        }
    }
}