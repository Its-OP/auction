namespace domain;

public class Stake
{
    public Stake() {}
    public Stake(decimal amount, DateTime timestamp, User user)
    {
        Amount = amount;
        Timestamp = timestamp;
        User = user;
    }
    
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public User User { get; set; }
}