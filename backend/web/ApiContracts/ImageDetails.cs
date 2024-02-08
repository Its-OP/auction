using domain;

namespace backend.ApiContracts;

public class ImageDetails
{
    public ImageDetails(int id, ImageType type)
    {
        Id = id;
        Type = type;
    }
    
    public int Id { get; set; }
    public ImageType Type { get; set; }
}