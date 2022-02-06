using Avalonia.Web.Blazor;

namespace PackageReferenceEditor.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        WebAppBuilder.Configure<PackageReferenceEditor.App>()
            .SetupWithSingleViewLifetime();
    }
}
