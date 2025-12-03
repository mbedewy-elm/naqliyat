using Microsoft.Extensions.Localization;
using Naqliyat.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Naqliyat;

[Dependency(ReplaceServices = true)]
public class NaqliyatBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<NaqliyatResource> _localizer;

    public NaqliyatBrandingProvider(IStringLocalizer<NaqliyatResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
