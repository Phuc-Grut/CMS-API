namespace VFi.Api.CMS.Models.Base;

public class ErrorDetails
{
    public string Code { get; set; }

    public string Message { get; set; }

    public IEnumerable<string> FileExists { get; set; }
}
public class ImageSize
{
    public int Height { get; set; }

    public int Width { get; set; }
}
