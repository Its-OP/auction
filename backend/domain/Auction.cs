﻿using System.Text.Json.Serialization;

namespace domain;

public class Auction
{
    public Auction() {}
    public Auction(string title, decimal minPrice, decimal minBidValue, string description, ICollection<Image> images, User host)
    {
        Title = title;
        MinPrice = minPrice;
        MinBidValue = minBidValue;
        CurrentPrice = 0;
        Description = description;
        Status = AuctionStatus.Active;
        Images = images;
        Host = host;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal MinBidValue { get; set; }
    public string Description { get; set; }
    public AuctionStatus Status { get; set; }
    public virtual ICollection<Image> Images { get; set; }
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public virtual User Host { get; set; }

    public Bid? GetWinningBid() => Bids.LastOrDefault();
    public Image GetThumbnail() => Images.Single(x => x.Type == ImageType.Thumbnail);
    public IEnumerable<Image> GetGallery() => Images.Where(x => x.Type != ImageType.Thumbnail);

    public bool TryBid(decimal value, out Bid bid)
    {
        bid = new Bid();
        if (IsReadOnly())
            return false;

        if (CurrentPrice == 0 && value >= MinPrice)
        {
            bid = PlaceBid(value);
            return true;
        }

        if (CurrentPrice > 0 && value - CurrentPrice >= MinBidValue)
        {
            bid = PlaceBid(value);
            return true;
        }

        return false;
    }

    public void Close()
    {
        Status = AuctionStatus.Closed;
    }

    public bool IsReadOnly()
    {
        return Status == AuctionStatus.Closed;
    }

    private Bid PlaceBid(decimal value)
    {
        var bid = new Bid(value, DateTime.UtcNow, this);
        CurrentPrice = value;
        Bids.Add(bid);
        return bid;
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AuctionStatus
{
    Active,
    Closed
}