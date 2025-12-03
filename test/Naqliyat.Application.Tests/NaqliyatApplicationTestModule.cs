using Volo.Abp.Modularity;

namespace Naqliyat;

[DependsOn(
    typeof(NaqliyatApplicationModule),
    typeof(NaqliyatDomainTestModule)
)]
public class NaqliyatApplicationTestModule : AbpModule
{

}
