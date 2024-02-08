namespace domain;

public class ImageBody
{
    public ImageBody() {}
    
    public ImageBody(string base64Body)
    {
        Base64Body = base64Body;
    }

    public int Id { get; set; }
    public string Base64Body { get; set; }
    public int ImageId { get; set; }
}