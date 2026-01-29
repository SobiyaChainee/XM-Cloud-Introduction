using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class LicenseEditViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
        : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminLicenseEdit";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        LicenseEditModel model = await ModelBinder.Bind<LicenseEditModel>(ViewContext);

        if (!model.IsEditing)
        {
            Response<License>? licenseResponse = null;

            if (model.IsEdit && ModelState.IsValid && Request.Method == "POST")
            {
                User? user = null;
                if (!model.Email.IsNullOrEmpty())
                {
                    Response<IList<User>> users = await Client.GetUsersAsync(email: model.Email);
                    user = users.Result?.FirstOrDefault();
                }

                if (user != null)
                {
                    License updateLicense = new(model.Id)
                    {
                        AssignedUser = user
                    };

                    Response<License> updatedResponse = await Client.UpdateLicenseAsync(updateLicense);

                    if (updatedResponse is { StatusCode: HttpStatusCode.OK })
                    {
                        ModelState.Clear();
                    }
                    else if (updatedResponse != null)
                    {
                        ModelState.TryAddModelError(string.Empty, updatedResponse.Message);
                    }
                }
                else
                {
                    ModelState.TryAddModelError(string.Empty, "No user found with provided email.");
                }
            }
            else if (!model.IsEdit || model.Id != Guid.Empty)
            {
                licenseResponse = await Client.GetLicenseAsync(model.Id);
            }

            if (licenseResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                License license = licenseResponse.Result;

                model.Email = license.AssignedUser != null
                    ? (await Client.GetUserAsync(license.AssignedUser.Id))?.Result?.Email ?? string.Empty
                    : string.Empty;

                ModelState.Clear();
            }
            else if (licenseResponse != null && licenseResponse.StatusCode != HttpStatusCode.OK)
            {
                ModelState.TryAddModelError(nameof(model.Email), licenseResponse.Message);
            }

            if (model.IsEdit && ModelState.IsValid && Request.Method == "POST")
            {
                result = View("Updated", model);
            }
            else
            {
                result = View(model);
            }
        }
        else
        {
            result = View(model);
        }

        return result;
    }
}