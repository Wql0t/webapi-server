namespace pr.Models;

public class RegisterReq {
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class LoginReq {
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
public class RefreshReq {
    public string RefreshToken { get; set; } = string.Empty;
}
public class AuthRes {
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Email { get; set;} = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
