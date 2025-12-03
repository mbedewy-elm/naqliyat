using Naqliyat.Samples;
using Xunit;

namespace Naqliyat.EntityFrameworkCore.Domains;

[Collection(NaqliyatTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<NaqliyatEntityFrameworkCoreTestModule>
{

}
