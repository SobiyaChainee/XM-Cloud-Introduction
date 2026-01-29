using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Admin;

public class LicenseOverviewModel : ListModel<License>
{
    public TextField? TitleLabel { get; set; }

    public TextField? AssignedUserNameHeader { get; set; }

    public TextField? ExpirationDateHeader { get; set; }

    public HyperLinkField? EditLink { get; set; }

    public TextField? ConfirmMessageLabelFormat { get; set; }

    public TextField? ConfirmLabel { get; set; }

    public TextField? UnAssignedUserLabel { get; set; }

    public HyperLinkField? UploadLicensesLink { get; set; }
}