namespace domain;

public class Auction
{
    public Auction(int id, string title, decimal minPrice, string description, IEnumerable<Image> images, IEnumerable<Stake> stakes)
    {
        Id = id;
        Title = title;
        MinPrice = minPrice;
        Description = description;
        Status = AuctionStatus.Active;
        Images = images;
        Stakes = stakes;
    }

    public int Id { get; }
    public string Title { get; }
    public decimal MinPrice { get; }
    public string Description { get; }
    public AuctionStatus Status { get; }
    public IEnumerable<Image> Images { get; }
    public IEnumerable<Stake> Stakes { get; }
}

public enum AuctionStatus
{
    Active,
    Closed
}