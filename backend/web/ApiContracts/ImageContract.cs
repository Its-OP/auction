using domain;

namespace backend.ApiContracts;

public class ImageContract
{
    public ImageDetails Metadata { get; set; }
    public string Base64Body { get; set; }
}