using backend.ApiContracts;
using domain;
using domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public AuctionsController(IApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateAuction([FromBody] AuctionArguments auctionArguments, CancellationToken token)
    {
        if (auctionArguments.Images.Count(x => x.Metadata.Type == ImageType.Thumbnail) != 1)
            return BadRequest("There must be exactly 1 thumbnail");
        
        var images = auctionArguments.Images.Select(x => new Image(x.Metadata.Type, new ImageBody(x.Base64Body))).ToList();
        await _context.Images.AddRangeAsync(images, token);
        
        var auction = new Auction(auctionArguments.Title, auctionArguments.MinPrice, auctionArguments.MinStakeValue, auctionArguments.Description, images);
        await _context.Auctions.AddAsync(auction, token);
        await _context.SaveChangesAsync(token);

        return Ok();
    }
    
    [HttpPost]
    [Route("close/{auctionId:int}")]
    public async Task<IActionResult> CloseAuction([FromRoute] int auctionId, CancellationToken token)
    {
        var auction = await _context.Auctions.SingleOrDefaultAsync(x => x.Id == auctionId, token);
        if (auction is null)
            return BadRequest("Auction not found");
        
        auction.Close();
        _context.Auctions.Update(auction);
        await _context.SaveChangesAsync(token);

        return Ok();
    }
    
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AuctionContract>))]
    public async Task<IActionResult> GetAuctions(CancellationToken token, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        if (pageSize < 1)
            return BadRequest("Page size is invalid");

        pageSize = Math.Min(pageSize, 100);
        
        if (pageNumber < 1)
            return BadRequest("Page number is invalid");

        var auctions = await _context.Auctions.Select(x => new AuctionContract
        {
            Id = x.Id,
            Description = x.Description,
            MinPrice = x.MinPrice,
            MinStakeValue = x.MinBidValue,
            Title = x.Title,
            Status = x.Status,
            ThumbnailId = x.Images.Single(i => i.Type == ImageType.Thumbnail).Id,
            Images = x.Images.Select(i => new ImageDetails(i.Id, i.Type)).ToList()
        }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(token);

        return Ok(auctions);
    }
}