using Volo.Abp.Modularity;

namespace Naqliyat;

public abstract class NaqliyatApplicationTestBase<TStartupModule> : NaqliyatTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
