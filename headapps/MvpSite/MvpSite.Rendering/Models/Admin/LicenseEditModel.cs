using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Admin
{
    public class LicenseEditModel : BaseModel
    {
        [FromQuery(Name = "id")]

        public Guid Id { get; set; } = Guid.Empty;

        public TextField? EmailLabel { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public TextField? ExpirationDateLabel { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public TextField? SubmitLabel { get; set; }

        public bool IsEdit { get; set; } = true;

        public TextField? SuccessLabel { get; set; }

        public HyperLinkField? OverviewLink { get; set; }
    }
}