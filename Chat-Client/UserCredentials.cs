namespace Chat_Client;

public record UserCredentials(string login, string password);

public record AuthResponse(string access_token, string login, string id);