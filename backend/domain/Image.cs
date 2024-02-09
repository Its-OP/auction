using System.Text.Json.Serialization;

namespace domain;

public class Image
{
    public Image() { }

    public Image(ImageType type, ImageBody body)
    {
        Type = type;
        Body = body;
    }
    
    public int Id { get; set; }
    public ImageType Type { get; set; }
    public virtual ImageBody Body { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ImageType
{
    Thumbnail,
    Gallery
}