using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Naqliyat.Data
{
    public class RoleDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IdentityRoleManager _roleManager;

        public RoleDataSeedContributor(
            IIdentityRoleRepository roleRepository,
            IdentityRoleManager roleManager)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await CreateRoleIfNotExistsAsync("Requester");
            await CreateRoleIfNotExistsAsync("Owner");
            await CreateRoleIfNotExistsAsync("Driver");
            await CreateRoleIfNotExistsAsync("Admin");
        }

        private async Task CreateRoleIfNotExistsAsync(string roleName)
        {
            var existing = await _roleManager.FindByNameAsync(roleName);

            if (existing != null)
            {
                return;
            }

            var result = await _roleManager.CreateAsync(
                new IdentityRole(
                    Guid.NewGuid(),
                    roleName
                )
            );

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors ?? Array.Empty<Microsoft.AspNetCore.Identity.IdentityError>());
                throw new AbpException($"Failed to create role '{roleName}': {errors}");
            }
        }
    }
}
