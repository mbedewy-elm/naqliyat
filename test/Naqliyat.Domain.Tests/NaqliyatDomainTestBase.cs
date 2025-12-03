using Volo.Abp.Modularity;

namespace Naqliyat;

/* Inherit from this class for your domain layer tests. */
public abstract class NaqliyatDomainTestBase<TStartupModule> : NaqliyatTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
