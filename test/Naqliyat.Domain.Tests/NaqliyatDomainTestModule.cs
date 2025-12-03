using Volo.Abp.Modularity;

namespace Naqliyat;

[DependsOn(
    typeof(NaqliyatDomainModule),
    typeof(NaqliyatTestBaseModule)
)]
public class NaqliyatDomainTestModule : AbpModule
{

}
