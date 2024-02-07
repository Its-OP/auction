using domain;

namespace backend.ApiContracts;

public class ImageContract
{
    public ImageClass Class { get; set; }
    public string Base64Image { get; set; }
}