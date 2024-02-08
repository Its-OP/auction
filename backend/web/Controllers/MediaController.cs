using backend.ApiContracts;
using domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/media")]
public class MediaController: ControllerBase
{
    private readonly IApplicationDbContext _context;

    public MediaController(IApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [ResponseCache(VaryByQueryKeys = new [] { "imageId" }, Duration = 300)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageContract))]
    [Route("{imageId:int}")]
    public async Task<IActionResult> GetImage(int imageId, CancellationToken token)
    {
        var image = await _context.Images.Include(x => x.Body).SingleOrDefaultAsync(x => x.Id == imageId, token);
        if (image is null)
            return NotFound($"Image {imageId} does not exist");
        
        return Ok(new ImageContract { Base64Body = image.Body.Base64Body, Metadata = new ImageDetails(image.Id, image.Type) });
    }
}