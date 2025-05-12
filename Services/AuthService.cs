﻿using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Core.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<User?> RegisterAsync(string username, string password, string role)
    {
        if (await _db.Users.AnyAsync(u => u.Username == username))
            return null;

        var hashedPassword = HashPassword(password);
        var user = new User { Username = username, PasswordHash = hashedPassword, Role = role };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
            return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiryMinutes"]!));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using var hasher = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256);
        var salt = hasher.Salt;
        var hash = hasher.GetBytes(32);
        return Convert.ToBase64String(salt.Concat(hash).ToArray());
    }

    private bool VerifyPassword(string password, string stored)
    {
        var bytes = Convert.FromBase64String(stored);
        var salt = bytes.Take(16).ToArray();
        var hash = bytes.Skip(16).ToArray();

        using var hasher = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        var testHash = hasher.GetBytes(32);
        return testHash.SequenceEqual(hash);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public static string HashPasswordStatic(string password)
    {
        using var hasher = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256);
        var salt = hasher.Salt;
        var hash = hasher.GetBytes(32);
        return Convert.ToBase64String(salt.Concat(hash).ToArray());
    }

}
