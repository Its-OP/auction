namespace backend.ApiContracts;

public class StakeArguments
{
    public int AuctionId { get; set; }
    public decimal Value { get; set; }
    public string BidderUsername { get; set; }
}