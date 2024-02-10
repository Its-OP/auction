using domain;

namespace backend.ApiContracts;

public class BidContract
{
    public BidContract(Bid bid)
    {
        Username = bid.User.Username;
        Value = bid.Amount;
        Timestamp = bid.Timestamp;
    }
    
    public string Username { get; set; }
    public decimal Value { get; set; }
    public DateTime Timestamp { get; set; }
}