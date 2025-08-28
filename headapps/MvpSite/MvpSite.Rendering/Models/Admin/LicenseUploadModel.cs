using Microsoft.AspNetCore.Http;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Admin
{
    public class LicenseUploadModel : BaseModel
    {
        public TextField? TitleLabel { get; set; }

        public TextField? DescriptionLabel { get; set; }

        public TextField? ChooseFileLabel { get; set; }

        public TextField? SubmitLabel { get; set; }

        public TextField? SuccessLabel { get; set; }

        public HyperLinkField? OverviewLink { get; set; }

        public IFormFile? LicenseFile { get; set; }

        public string? ErrorMessage { get; set; }
    }
}