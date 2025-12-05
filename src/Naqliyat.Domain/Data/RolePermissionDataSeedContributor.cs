using System.Threading.Tasks;
using Naqliyat.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Naqliyat.Data
{
    public class RolePermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        public RolePermissionDataSeedContributor(IPermissionDataSeeder permissionDataSeeder)
        {
            _permissionDataSeeder = permissionDataSeeder;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            // Grant trip creation permission to Requester role
            await _permissionDataSeeder.SeedAsync(
                roleName: "Requester",
                permissions: new[]
                {
                    NaqliyatPermissions.Trips.Create
                }
            );

            // Grant open-trips viewing permission to Driver role
            await _permissionDataSeeder.SeedAsync(
                roleName: "Driver",
                permissions: new[]
                {
                    NaqliyatPermissions.Trips.ViewOpen
                }
            );
        }
    }
}
