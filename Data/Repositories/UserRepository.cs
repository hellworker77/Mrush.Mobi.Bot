using AutoMapper;
using Data.Abstraction.Interfaces;
using Data.Implementation;
using Domain.Models;
using Entities.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _applicationContext;
    private readonly DbSet<UserEntity> _dbSet;
    private readonly IMapper _mapper;
    public UserRepository(ApplicationContext applicationContext,
    IMapper mapper)
    {
        _applicationContext = applicationContext;
        _dbSet = applicationContext.Set<UserEntity>();
        _mapper = mapper;
    }
    public async Task<User> GetByTelegramIdAsync(long telegramId)
    {
        var entity = await _applicationContext.Users.Include(x => x.Accounts).AsNoTracking().FirstOrDefaultAsync(x=>x.TelegramId == telegramId);
        var dto = _mapper.Map<User>(entity);
        return dto;
    }

    public async Task AddAsync(User user)
    {
        var entity = _mapper.Map<UserEntity>(user);
        await _dbSet.AddAsync(entity);
        await _applicationContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity != null)
        {
            _dbSet.Remove(entity);
        }

        await _applicationContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var entity = _mapper.Map<UserEntity>(user);

        var trackedEntity = await _dbSet.FindAsync(user.Id);
        
        if (trackedEntity != null)
        {
            trackedEntity.Accounts = entity.Accounts;
            _dbSet.Update(trackedEntity);

            await _applicationContext.SaveChangesAsync();
        }
    }
}