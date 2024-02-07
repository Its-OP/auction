namespace domain;

public class User
{
    public User(int id, string username, string passwordHash)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
    }

    public int Id { get; }
    public string Username { get; }
    public string PasswordHash { get; }
}