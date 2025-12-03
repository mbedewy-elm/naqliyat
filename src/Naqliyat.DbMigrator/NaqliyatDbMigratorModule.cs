using Naqliyat.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Naqliyat.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(NaqliyatEntityFrameworkCoreModule),
    typeof(NaqliyatApplicationContractsModule)
)]
public class NaqliyatDbMigratorModule : AbpModule
{
}
