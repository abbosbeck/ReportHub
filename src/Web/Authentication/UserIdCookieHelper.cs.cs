namespace Web.Authentication;

public static class UserIdCookieHelper
{
    private const string CookieName = "X-User-ID";

    public static string GetOrCreateUserId(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue(CookieName, out var userId))
            return userId;

        userId = Guid.NewGuid().ToString();
        context.Response.Cookies.Append(CookieName, userId, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return userId;
    }
}
