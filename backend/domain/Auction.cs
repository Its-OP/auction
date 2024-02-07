namespace domain;

public class Auction
{
    public Auction() {}
    public Auction(string title, decimal minPrice, string description, IEnumerable<Image> images, IEnumerable<Stake> stakes)
    {
        Title = title;
        MinPrice = minPrice;
        Description = description;
        Status = AuctionStatus.Active;
        Images = images;
        Stakes = stakes;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public string Description { get; set; }
    public AuctionStatus Status { get; set; }
    public IEnumerable<Image> Images { get; set; }
    public IEnumerable<Stake> Stakes { get; set; }
}

public enum AuctionStatus
{
    Active,
    Closed
}