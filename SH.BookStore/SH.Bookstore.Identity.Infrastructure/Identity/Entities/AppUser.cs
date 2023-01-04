using Microsoft.AspNetCore.Identity;

using Throw;

namespace SH.Bookstore.Identity.Infrastructure.Identity.Entities;
/// <summary>
/// Класс сущности пользователя
/// </summary>
public class AppUser : IdentityUser
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string? LastName { get; private set; }

    /// <summary>
    /// Активен ли пользователь
    /// </summary>
    public bool IsActive { get; private set; }
    private AppUser()
    {
    }

    /// <summary>
    /// Создает объект пользователя
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="userName"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public static AppUser Create(string firstName, string? lastName, string email, string userName, string? phoneNumber)
    {
        return new()
        {
            FirstName = firstName.Throw().IfNullOrEmpty(firstName => firstName),
            LastName = lastName,
            IsActive = true,
            Email = email.Throw().IfNullOrEmpty(email => email),
            UserName = userName.Throw().IfNullOrEmpty(email => email),
            PhoneNumber = phoneNumber
        };
    }

    public void ToggleStatus(bool isActive)
    {
        IsActive = isActive;
    }
}
