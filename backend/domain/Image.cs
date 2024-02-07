namespace domain;

public class Image
{
    public int Id { get; }
    public string Url { get; }
    public ImageClass Class { get; }
}

public enum ImageClass
{
    Thumbnail,
    Gallery
}