using Naqliyat.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Naqliyat.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NaqliyatController : AbpControllerBase
{
    protected NaqliyatController()
    {
        LocalizationResource = typeof(NaqliyatResource);
    }
}
