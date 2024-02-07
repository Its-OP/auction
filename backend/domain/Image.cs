namespace domain;

public class Image
{
    public Image() {}
    public int Id { get; set; }
    public string Url { get; set; }
    public ImageClass Class { get; set; }
}

public enum ImageClass
{
    Thumbnail,
    Gallery
}