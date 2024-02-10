using backend.ApiContracts;
using domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Authorize]
[Route("api/bids")]
public class BidsController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public BidsController(IApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BidErrorCodes))]
    public async Task<IActionResult> Bid([FromBody] BidArguments bidArguments, CancellationToken token)
    {
        var bidderUsername = User.GetUsername();
        var bidder = await _context.Users.SingleOrDefaultAsync(x => x.Username == bidderUsername, token);
        if (bidder is null)
            return BadRequest("User not found");
        
        if (!await _context.Auctions.AnyAsync(x => x.Id == bidArguments.AuctionId, token))
            return BadRequest("Auction not found");

        var auction = await _context.Auctions.SingleAsync(x => x.Id == bidArguments.AuctionId, token);
        var bidSuccessful = auction.TryBid(bidArguments.Value, out var bid);

        if (bidSuccessful)
        {
            bid.User = bidder;
            _context.Auctions.Update(auction);
            await _context.Bids.AddAsync(bid, token);
            await _context.SaveChangesAsync(token);
            return Ok();
        }

        return BadRequest(BidErrorCodes.BidFailed);
    }
    
    [HttpGet]
    [Route("{auctionId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BidContract>))]
    public async Task<IActionResult> GetStakes(int auctionId, CancellationToken token)
    {
        var auction = await _context
            .Auctions
            .Include(x => x.Bids)
            .ThenInclude(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == auctionId, token);
        
        if (auction is null)
            return BadRequest("Auction does not exist");

        var stakes = auction.Bids.Select(x => new BidContract(x)).ToList();

        return Ok(stakes);
    }
}