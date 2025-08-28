using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class LicensesOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminLicensesOverview";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        LicenseOverviewModel model = await ModelBinder.Bind<LicenseOverviewModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
            result = View(model);
        }

        await LoadLicenses(model);
        result = View(model);
        return result;
    }

    private static void GenerateFakeDataForEdit(LicenseOverviewModel model)
    {
        model.List.Add(new License(
            Guid.NewGuid())
        {
            AssignedUser = new User(Guid.NewGuid())
            {
                Name = "Zahir Avery"
            },
            ExpirationDate = DateTime.Now.AddYears(1)
        });

        model.List.Add(new License(
           Guid.NewGuid())
        {
            AssignedUser = new User(Guid.NewGuid())
            {
                Name = "Cristian Roberts"
            },
            ExpirationDate = DateTime.Now.AddYears(1)
        });

        model.List.Add(new License(
           Guid.NewGuid())
        {
            AssignedUser = new User(Guid.NewGuid())
            {
                Name = "Augustine Walls"
            },
            ExpirationDate = DateTime.Now.AddYears(1)
        });
    }

    private async Task LoadLicenses(LicenseOverviewModel model)
    {
        Response<IList<License>> licenseResponse = await Client.GetLicensesAsync(page: model.Page, pageSize: model.PageSize);
        if (licenseResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.List.AddRange(licenseResponse.Result);
        }
    }
}