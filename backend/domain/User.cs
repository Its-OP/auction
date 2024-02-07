namespace domain;

public class User
{
    public User() {}
    public User(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
    }

    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
}