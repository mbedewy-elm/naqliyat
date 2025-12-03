using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Naqliyat.Data;

/* This is used if database provider does't define
 * INaqliyatDbSchemaMigrator implementation.
 */
public class NullNaqliyatDbSchemaMigrator : INaqliyatDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
