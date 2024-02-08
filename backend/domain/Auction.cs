namespace domain;

public class Auction
{
    public Auction() {}
    public Auction(string title, decimal minPrice, decimal minBidValue, string description, ICollection<Image> images)
    {
        Title = title;
        MinPrice = minPrice;
        MinBidValue = minBidValue;
        CurrentPrice = 0;
        Description = description;
        Status = AuctionStatus.Active;
        Images = images;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal MinBidValue { get; set; }
    public string Description { get; set; }
    public AuctionStatus Status { get; set; }
    public ICollection<Image> Images { get; set; }
    public ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public bool TryBid(decimal value, out Bid bid)
    {
        bid = new Bid();
        if (Status == AuctionStatus.Closed)
            return false;

        if (CurrentPrice == 0 && value >= MinPrice)
        {
            bid = Bid(value);
            return true;
        }

        if (CurrentPrice > 0 && value - CurrentPrice >= MinBidValue)
        {
            bid = Bid(value);
            return true;
        }

        return false;
    }

    private Bid Bid(decimal value)
    {
        var bid = new Bid(value, DateTime.UtcNow, this);
        CurrentPrice = value;
        Bids.Add(bid);
        return bid;
    }
}

public enum AuctionStatus
{
    Active,
    Closed
}