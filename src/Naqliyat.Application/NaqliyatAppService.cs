using Naqliyat.Localization;
using Volo.Abp.Application.Services;

namespace Naqliyat;

/* Inherit your application services from this class.
 */
public abstract class NaqliyatAppService : ApplicationService
{
    protected NaqliyatAppService()
    {
        LocalizationResource = typeof(NaqliyatResource);
    }
}
