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
    [Route("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionContract))]
    public async Task<IActionResult> CreateAuction([FromBody] AuctionArguments auctionArguments, int userId, CancellationToken token)
    {
        if (!AuctionArgumentsAreValid(auctionArguments))
            return BadRequest("Arguments are invalid");
        
        if (auctionArguments.Images.Count(x => x.Metadata.Type == ImageType.Thumbnail) != 1)
            return BadRequest("There must be exactly 1 thumbnail");
        
        var images = auctionArguments.Images.Select(x => new Image(x.Metadata.Type, new ImageBody(x.Base64Body))).ToList();

        var host = await _context.Users.SingleAsync(x => x.Id == userId, token);
        var auction = new Auction(auctionArguments.Title, auctionArguments.MinPrice, auctionArguments.MinBidValue, auctionArguments.Description, images, host);
        await _context.Auctions.AddAsync(auction, token);
        await _context.SaveChangesAsync(token);

        return Ok(new AuctionContract(auction));
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

        var auctions = await _context
            .Auctions
            .Include(x => x.Images)
            .Include(x => x.Bids)
            .Include(x => x.Host)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);
        
        var contracts = auctions.Select(x => new AuctionContract(x)).ToList();
        return Ok(contracts);
    }
    
    [HttpPut]
    [Route("update/{auctionId:int}")]
    public async Task<IActionResult> UpdateAuctionDetails([FromBody] AuctionArguments auctionArguments, int auctionId, CancellationToken token)
    {
        if (!AuctionArgumentsAreValid(auctionArguments))
            return BadRequest("Arguments are invalid");
        
        var auction = await _context.Auctions.Include(x => x.Images).Include(x => x.Host).SingleOrDefaultAsync(x => x.Id == auctionId, token);
        if (auction is null)
            return BadRequest("Auction does not exist");

        if (auction.IsReadOnly())
            return BadRequest(UpdateAuctionErrorCodes.AuctionIsClosed);

        auction.Description = auctionArguments.Description;
        auction.MinPrice = auctionArguments.MinPrice;
        auction.Title = auctionArguments.Title;
        auction.MinBidValue = auctionArguments.MinBidValue;
        
        _context.Auctions.Update(auction);
        await _context.SaveChangesAsync(token);

        return Ok(new AuctionContract(auction));
    }
    
    [HttpPut]
    [Route("gallery/{auctionId:int}/thumbnail")]
    public async Task<IActionResult> UpdateAuctionThumbnail([FromBody] ImageContract newThumbnail, int auctionId, CancellationToken token)
    {
        var auction = await _context.Auctions.Include(x => x.Images).SingleOrDefaultAsync(x => x.Id == auctionId, token);
        if (auction is null)
            return BadRequest("Auction does not exist");

        if (auction.IsReadOnly())
            return BadRequest(UpdateAuctionErrorCodes.AuctionIsClosed);

        var thumbnail = auction.Images.Single(x => x.Type == ImageType.Thumbnail);
        thumbnail.Body.Base64Body = newThumbnail.Base64Body;
        
        _context.Images.Update(thumbnail);
        await _context.SaveChangesAsync(token);

        return Ok(new AuctionContract(auction));
    }
    
    [HttpDelete]
    [Route("gallery/{auctionId:int}/image/{imageId:int}")]
    public async Task<IActionResult> DeleteGalleryImage(int auctionId, int imageId, CancellationToken token)
    {
        var auction = await _context.Auctions.Include(x => x.Images).SingleOrDefaultAsync(x => x.Id == auctionId, token);
        if (auction is null)
            return BadRequest("Auction does not exist");

        if (auction.IsReadOnly())
            return BadRequest(UpdateAuctionErrorCodes.AuctionIsClosed);

        var image = auction.Images.SingleOrDefault(x => x.Id == imageId);
        if (image is null)
            return BadRequest("Image does not exist");
        
        _context.Images.Remove(image);
        await _context.SaveChangesAsync(token);

        return Ok(new AuctionContract(auction));
    }
    
    [HttpPut]
    [Route("gallery/{auctionId:int}")]
    public async Task<IActionResult> AddGalleryImage([FromBody] ImageContract newThumbnail, int auctionId, CancellationToken token)
    {
        var auction = await _context.Auctions.SingleOrDefaultAsync(x => x.Id == auctionId, token);
        if (auction is null)
            return BadRequest("Auction does not exist");

        if (auction.IsReadOnly())
            return BadRequest(UpdateAuctionErrorCodes.AuctionIsClosed);

        var image = new Image(ImageType.Gallery, new ImageBody(newThumbnail.Base64Body));
        auction.Images.Add(image);
        
        await _context.SaveChangesAsync(token);

        return Ok(new AuctionContract(auction));
    }

    private static bool AuctionArgumentsAreValid(AuctionArguments args)
    {
        return args is { MinBidValue: > 0, MinPrice: > 0 };
    }
}