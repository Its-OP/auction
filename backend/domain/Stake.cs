namespace domain;

public class Stake
{
    public Stake(decimal amount, DateTime timestamp, User user)
    {
        Amount = amount;
        Timestamp = timestamp;
        User = user;
    }
    
    public int Id { get; }
    public decimal Amount { get; }
    public DateTime Timestamp { get; }
    public User User { get; }
}