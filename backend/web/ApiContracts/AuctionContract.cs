using domain;

namespace backend.ApiContracts;

public class AuctionContract
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public string Description { get; set; }
    public AuctionStatus Status { get; set; }
    public string ThumbnailUrl { get; set; }
}