using Naqliyat.Samples;
using Xunit;

namespace Naqliyat.EntityFrameworkCore.Applications;

[Collection(NaqliyatTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<NaqliyatEntityFrameworkCoreTestModule>
{

}
