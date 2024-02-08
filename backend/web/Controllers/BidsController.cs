using backend.ApiContracts;
using domain;
using domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
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
    public async Task<IActionResult> Bid([FromBody] StakeArguments stakeArguments, CancellationToken token)
    {
        if (!await _context.Users.AnyAsync(x => x.Username == stakeArguments.BidderUsername, token))
            return BadRequest("User not found");
        
        if (!await _context.Auctions.AnyAsync(x => x.Id == stakeArguments.AuctionId, token))
            return BadRequest("Auction not found");

        var auction = await _context.Auctions.SingleAsync(x => x.Id == stakeArguments.AuctionId, token);
        var user = await _context.Users.SingleAsync(x => x.Username == stakeArguments.BidderUsername, token);
        var bidSuccessful = auction.TryBid(stakeArguments.Value, out var bid);

        if (bidSuccessful)
        {
            bid.User = user;
            _context.Auctions.Update(auction);
            await _context.Bids.AddAsync(bid, token);
            await _context.SaveChangesAsync(token);
            return Ok();
        }

        return BadRequest(BidErrorCodes.BidFailed);
    }
    
    [HttpGet]
    [Route("{auctionId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StakeContract>))]
    public async Task<IActionResult> GetStakes(int auctionId, CancellationToken token)
    {
        var auction = await _context
            .Auctions
            .Include(x => x.Bids)
            .ThenInclude(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == auctionId, token);
        
        if (auction is null)
            return BadRequest("Auction does not exist");

        var stakes = auction.Bids.Select(x => new StakeContract
        {
            Timestamp = x.Timestamp,
            Username = x.User.Username,
            Value = x.Amount
        }).ToList();

        return Ok(stakes);
    }
}