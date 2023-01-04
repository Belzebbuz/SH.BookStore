using System.Security.Claims;

namespace SH.Bookstore.Shared.Identity.RequestCurrentUser;

/// <summary>
/// <see cref="ICurrentUser"/> Позволяет получать информацию о пользователе совершившего запрос
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Получить уникальный идентификатор пользователя в виде <see cref="Guid"/>
    /// </summary>
    /// <returns>Уникальный идентификатор пользователя</returns>
    public Guid GetUserId();

    /// <summary>
    /// Получить <see cref="ClaimTypes.Email"/> текущего пользователя
    /// </summary>
    /// <returns>Email пользователя</returns>
    public string? GetUserEmail();

    /// <summary>
    /// Аутентифицирован ли пользователь
    /// </summary>
    /// <returns></returns>
    public bool IsAuthenticated();


    /// <summary>
    /// Получить список <see cref="Claim"/> пользователя
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Claim>? GetUserClaims();
}

/// <summary>
/// <see cref="ICurrentUserInitializer"/> позволяет инициализировать пользователя, совершившего запрос
/// </summary>
public interface ICurrentUserInitializer
{
    /// <summary>
    /// Установить пользователя, совершившего запрос
    /// </summary>
    /// <param name="user"></param>
    public void SetCurrentUser(ClaimsPrincipal user);

    /// <summary>
    /// Установить Id пользователя, совершившего запрос
    /// </summary>
    /// <param name="userId"></param>
    public void SetCurrentUserId(string userId);
}