namespace domain;

public class Bid
{
    public Bid() {}
    public Bid(decimal amount, DateTime timestamp, Auction auction)
    {
        Amount = amount;
        Timestamp = timestamp;
        Auction = auction;
    }
    
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public User User { get; set; }
    public Auction Auction { get; set; }
}