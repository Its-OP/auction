using backend.ApiContracts;
using Microsoft.AspNetCore.SignalR;

namespace infrastructure;

public class BidsHub : Hub<IBidsClient>
{
    public const string BidsGroupPrefix = "bids-";

    public override async Task OnConnectedAsync()
    {
        var context = Context.GetHttpContext();
        var auctionId = context.Request.Query["auctionId"];
        
        if (!string.IsNullOrEmpty(auctionId))
        {
            var groupName = BidsGroupPrefix + auctionId;
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        await base.OnConnectedAsync();
    }
}

public interface IBidsClient
{
    public Task OnNewBid(BidContract bid);
}
