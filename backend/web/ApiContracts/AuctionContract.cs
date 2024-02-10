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
        ThumbnailId = auction.GetThumbnail().Id;
        Gallery = auction.GetGallery().Select(i => new ImageDetails(i.Id, i.Type)).ToList();
        HostUsername = auction.Host.Username;
        WinningBid = auction.GetWinningBid() is null ? null : new BidContract(auction.GetWinningBid()!);
    }
    
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MinStakeValue { get; set; }
    public string Description { get; set; }
    public AuctionStatus Status { get; set; }
    public int ThumbnailId { get; set; }
    public List<ImageDetails> Gallery { get; set; }
    public string HostUsername { get; set; }
    public BidContract? WinningBid { get; set; }
}