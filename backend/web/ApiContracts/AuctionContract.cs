using domain;

namespace backend.ApiContracts;

public class AuctionContract
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MinStakeValue { get; set; }
    public string Description { get; set; }
    public AuctionStatus Status { get; set; }
    public int ThumbnailId { get; set; }
    public List<ImageDetails> Images { get; set; }
}