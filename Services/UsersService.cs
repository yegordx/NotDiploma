using Diploma.Services.Authorization;
using Diploma.Contracts;
using Diploma.Contexts;
using Microsoft.AspNetCore.Identity;
using Diploma.Entities;
using Diploma.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Services;

public class UsersService
{
    private readonly MyJwtProvider _jwtProvider;
    private readonly MyPasswordHasher _passwordHasher;
    private readonly ShopDbContext _context;

    public UsersService(MyPasswordHasher passwordHasher, MyJwtProvider jwtProvider, ShopDbContext context)
    {
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _context = context;
    }

    public async Task<JwtTokenResult> Register(RegisterUserRequest request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existingUser != null)
            throw new Exception("Користувач з таким email вже існує.");

        var hashedPassword = _passwordHasher.Generate(request.Password);

        var newUser = new User(
            name: request.Name,
            email: request.Email,
            hashedPassword: hashedPassword,
            age: request.Age,
            sex: request.Sex,
            weightKg: request.WeightKg,
            heightCm: request.HeightCm,
            goal: request.Goal,
            budgetPerWeek: request.BudgetPerWeek,
            isVegan: request.IsVegan
        );

        if (request.Restrictions is not null)
        {
            newUser.Restrictions = request.Restrictions;
        }

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return _jwtProvider.GenerateToken(newUser.Id.ToString(), "user");
    }

    public async Task<JwtTokenResult> Login(LoginUserRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
            throw new Exception("Користувача не знайдено.");

        if (!_passwordHasher.Verify(request.Password, user.HashedPassword))
            throw new Exception("Невірний пароль.");

        return _jwtProvider.GenerateToken(user.Id.ToString(), "user");
    }

    public async Task UpdateUser(Guid userId, UpdateUserRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("Користувача не знайдено.");

        user.Name = request.Name;
        user.Age = request.Age;
        user.Sex = request.Sex;
        user.WeightKg = request.WeightKg;
        user.HeightCm = request.HeightCm;
        user.Goal = request.Goal;
        user.BudgetPerWeek = request.BudgetPerWeek;
        user.IsVegan = request.IsVegan;
        user.Restrictions = request.Restrictions ?? new List<string>();

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("Користувача не знайдено.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
