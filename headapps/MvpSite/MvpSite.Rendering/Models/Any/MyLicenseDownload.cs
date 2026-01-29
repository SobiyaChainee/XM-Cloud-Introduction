using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Any;

public class MyLicenseDownload : BaseModel
{
    public TextField? TitleLabel { get; set; }

    public TextField? SuccessLabel { get; set; }

    public TextField? DownloadLabel { get; set; }

    public string FileName { get; set; } = "license.xml";

    public string? Base64Content { get; set; }

    public string? ErrorMessage { get; set; }
}
