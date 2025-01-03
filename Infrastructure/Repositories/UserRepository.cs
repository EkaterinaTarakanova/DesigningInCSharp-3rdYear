﻿using SchedulePlanner.Domain.Entities;
using SchedulePlanner.Domain.Interfaces;
using System.Data.Entity;

namespace SchedulePlanner.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext context;

    public UserRepository(AppDbContext context) => this.context = context;

    public async Task CreateAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var users = context.Users.Where(user => user.Id == id);
        foreach (var user in users)
        {
            context.Remove(user);
        }
        await context.SaveChangesAsync();
    }

    public async Task<User?> GetByIDAsync(Guid id)
    {
        return await context.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await context.Users.Where(user => user.Username == username).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(User user)
    {
        await DeleteAsync(user.Id);
        await CreateAsync(user);
    }
}
