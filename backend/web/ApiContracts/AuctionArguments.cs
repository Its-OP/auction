namespace backend.ApiContracts;

public class AuctionArguments
{
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MinBidValue { get; set; }
    public string Description { get; set; }
    public List<ImageContract> Images { get; set; }
}