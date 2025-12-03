using Xunit;

namespace Naqliyat.EntityFrameworkCore;

[CollectionDefinition(NaqliyatTestConsts.CollectionDefinitionName)]
public class NaqliyatEntityFrameworkCoreCollection : ICollectionFixture<NaqliyatEntityFrameworkCoreFixture>
{

}
