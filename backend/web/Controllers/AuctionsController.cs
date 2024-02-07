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
        if (auctionArguments.Images.Count(x => x.Class == ImageClass.Thumbnail) != 1)
            return BadRequest(ErrorCodes.MissingThumbnail);

        var images = auctionArguments.Images.Select(x => new Image(x.Base64Image, x.Class)).ToList();
        await _context.Images.AddRangeAsync(images, token);
        
        var auction = new Auction(auctionArguments.Title, auctionArguments.MinPrice, auctionArguments.Description, images, Enumerable.Empty<Stake>());
        await _context.Auctions.AddAsync(auction, token);
        await _context.SaveChangesAsync(token);

        return Ok();
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAuctions([FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken token)
    {
        if (pageSize < 1)
            return BadRequest(ErrorCodes.InvalidPageSize);

        pageSize = Math.Min(pageSize, 100);
        
        if (pageNumber < 1)
            return BadRequest(ErrorCodes.InvalidPageNumber);

        var auctions = await _context.Auctions.Select(x => new AuctionContract
        {
            Id = x.Id,
            Description = x.Description,
            MinPrice = x.MinPrice,
            Title = x.Title,
            Status = x.Status,
            ThumbnailUrl = x.Images.Single(i => i.Class == ImageClass.Thumbnail).Url
        }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(token);

        return Ok(auctions);
    }
}