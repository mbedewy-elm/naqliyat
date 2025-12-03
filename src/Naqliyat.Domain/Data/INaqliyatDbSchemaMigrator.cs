using System.Threading.Tasks;

namespace Naqliyat.Data;

public interface INaqliyatDbSchemaMigrator
{
    Task MigrateAsync();
}
