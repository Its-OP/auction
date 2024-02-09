using domain;

namespace backend.ApiContracts;

public class AuctionContract
{
    public AuctionContract(Auction auction)
    {
        Id = auction.Id;
        Description = auction.Description;
        MinPrice = auction.MinPrice;
        MinStakeValue = auction.MinBidValue;
        Title = auction.Title;
        Status = auction.Status;
        ThumbnailId = auction.Images.Single(x => x.Type == ImageType.Thumbnail).Id;
        Images = auction.Images.Select(i => new ImageDetails(i.Id, i.Type)).ToList();
        HostUsername = auction.Host.Username;
    }
    
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MinStakeValue { get; set; }
    public string Description { get; set; }
    public AuctionStatus Status { get; set; }
    public int ThumbnailId { get; set; }
    public List<ImageDetails> Images { get; set; }
    public string HostUsername { get; set; }
}