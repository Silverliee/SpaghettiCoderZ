using Microsoft.EntityFrameworkCore;
using ReservationAPI.Infrastructure.Database;
using ReservationAPI.Models;

namespace ReservationAPI.Repositories;

public class UserRepository(SqLiteDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await dbContext.Users.FindAsync(userId).AsTask();
    }

    public Task<User> GetUserByEmailAsync(string email)
    {
        return Task.FromResult(dbContext.Users.First(u => u.Email == email));
    }

    public Task<User> RegisterUserAsync(User user)
    {
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
        return Task.FromResult(user);
    }

    public Task<User?> LoginUserAsync(string email, string password)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        return Task.FromResult(user);
    }

    public Task<User> UpdateUserAsync(User user)
    {
        dbContext.Users.Update(user);
        dbContext.SaveChanges();
        return Task.FromResult(user);
    }

    public Task<bool> DeleteUserAsync(int userId)
    {
    var user = dbContext.Users.Find(userId);
        if (user == null)
        {
            return Task.FromResult(false);
        }
        dbContext.Users.Remove(user);
        dbContext.SaveChanges();
        return Task.FromResult(true);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await dbContext.Users.ToListAsync();
    }
}