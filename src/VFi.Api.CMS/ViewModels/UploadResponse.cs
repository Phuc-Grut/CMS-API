using VFi.NetDevPack.Queries;

namespace VFi.Api.CMS.ViewModels;

public class UploadResponse : BaseResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public int? Size { get; set; }
    public string Path { get; set; }
    public string VirtualPath { get; set; }
    public string Type { get; set; }
}
public class InfoFileResponse
{

    public bool Status { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public int? Size { get; set; }
    public string Path { get; set; }
    public string VirtualPath { get; set; }
    public string Type { get; set; }
}
public class UploadMultiResponse : BaseResponse
{
    public List<InfoFileResponse> infoFiles { get; set; }
}
