using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using MvpSite.Rendering.Models.Any;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Any;

[ViewComponent(Name = ViewComponentName)]
public class MyLicenseDownloadViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client) : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AnyMyLicenseDownload";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        MyLicenseDownload model = await ModelBinder.Bind<MyLicenseDownload>(ViewContext);

        if (Request.Method == "POST")
        {
            if (Request.Form.ContainsKey("downloadLicense"))
            {
                try
                {
                    var (response, fileName) = await Client.GetLicenseByUserAsync();
                    if (response.StatusCode == HttpStatusCode.OK && response.Result != null)
                    {
                        using var ms = new MemoryStream();
                        await response.Result.CopyToAsync(ms);
                        model.Base64Content = Convert.ToBase64String(ms.ToArray());
                        model.FileName = fileName;
                    }
                    else
                    {
                        model.ErrorMessage = response.Message ?? "Failed to download license.";
                    }
                }
                catch (Exception)
                {
                    model.ErrorMessage = "An error occurred while downloading the license.";
                }
            }
        }

        return View(model);
    }
}