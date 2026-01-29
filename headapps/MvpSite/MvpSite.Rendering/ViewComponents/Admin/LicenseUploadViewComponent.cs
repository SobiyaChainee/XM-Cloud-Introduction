using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class LicenseUploadViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminLicenseUpload";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        LicenseUploadModel model = await ModelBinder.Bind<LicenseUploadModel>(ViewContext);

        if (!model.IsEditing && Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            if (model.LicenseFile != null && model.LicenseFile.Length > 0)
            {
                string fileName = model.LicenseFile.FileName;
                if (!fileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("LicenseFile", "Only .zip files are allowed.");
                }
                else
                {
                    using MemoryStream fileStream = new();
                    await model.LicenseFile.CopyToAsync(fileStream);
                    fileStream.Position = 0;

                    try
                    {
                        Response<IList<License>> uploadResponse = await Client.UploadLicensesAsync(fileStream, fileName);

                        if (uploadResponse.StatusCode == HttpStatusCode.OK)
                        {
                            ModelState.Clear();
                            return View("Success", model);
                        }
                        else
                        {
                            ModelState.AddModelError("LicenseFile", uploadResponse.Message ?? "Failed to upload licenses");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("LicenseFile", $"Error processing license file: {ex.Message}");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("LicenseFile", "No file was selected or the file is empty");
            }
        }

        return View(model);
    }
}