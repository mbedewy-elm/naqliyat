using System;
using System.Linq;
using System.Threading.Tasks;
using Naqliyat.Trucks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Naqliyat.Users;

public class UserAppService : NaqliyatAppService, IUserAppService
{
    private readonly IdentityUserManager _userManager;
    private readonly IRepository<Owner, Guid> _ownerRepository;
    private readonly IRepository<Driver, Guid> _driverRepository;

    public UserAppService(
        IdentityUserManager userManager,
        IRepository<Owner, Guid> ownerRepository,
        IRepository<Driver, Guid> driverRepository)
    {
        _userManager = userManager;
        _ownerRepository = ownerRepository;
        _driverRepository = driverRepository;
    }

    [UnitOfWork]
    public virtual async Task<UserRegistrationResultDto> RegisterAsRequesterAsync()
    {
        var currentUserId = CurrentUser.Id;
        if (!currentUserId.HasValue)
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                Message = "User not authenticated."
            };
        }

        var user = await _userManager.GetByIdAsync(currentUserId.Value);

        // Check if user already has the role
        if (await _userManager.IsInRoleAsync(user, "Requester"))
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                Role = "Requester",
                Message = "User is already registered as Requester."
            };
        }

        var result = await _userManager.AddToRoleAsync(user, "Requester");

        if (!result.Succeeded)
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                Message = "Failed to add Requester role."
            };
        }

        return new UserRegistrationResultDto
        {
            Success = true,
            Role = "Requester",
            Message = "Successfully registered as Requester."
        };
    }

    [UnitOfWork]
    public virtual async Task<UserRegistrationResultDto> RegisterAsOwnerAsync(RegisterAsOwnerDto input)
    {
        var currentUserId = CurrentUser.Id;
        if (!currentUserId.HasValue)
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                Message = "User not authenticated."
            };
        }

        var user = await _userManager.GetByIdAsync(currentUserId.Value);

        // Check if user already has the role
        if (await _userManager.IsInRoleAsync(user, "Owner"))
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                Role = "Owner",
                Message = "User is already registered as Owner."
            };
        }

        // Check if owner entity already exists for this user
        var ownerQueryable = await _ownerRepository.GetQueryableAsync();
        var existingOwner = await AsyncExecuter.FirstOrDefaultAsync(
            ownerQueryable.Where(o => o.UserId == currentUserId.Value));

        if (existingOwner != null)
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                Role = "Owner",
                EntityId = existingOwner.Id,
                Message = "Owner entity already exists for this user."
            };
        }

        // Create Owner entity
        var owner = new Owner(
            Guid.NewGuid(),
            input.Name,
            input.Phone,
            input.Identification,
            input.EntityNumber,
            input.NationalityId,
            currentUserId.Value);

        owner = await _ownerRepository.InsertAsync(owner, autoSave: true);

        // Add role to user
        var result = await _userManager.AddToRoleAsync(user, "Owner");

        if (!result.Succeeded)
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                EntityId = owner.Id,
                Message = "Owner entity created but failed to add Owner role."
            };
        }

        return new UserRegistrationResultDto
        {
            Success = true,
            Role = "Owner",
            EntityId = owner.Id,
            Message = "Successfully registered as Owner."
        };
    }

    [UnitOfWork]
    public virtual async Task<UserRegistrationResultDto> RegisterAsDriverAsync(RegisterAsDriverDto input)
    {
        var currentUserId = CurrentUser.Id;
        if (!currentUserId.HasValue)
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                Message = "User not authenticated."
            };
        }

        var user = await _userManager.GetByIdAsync(currentUserId.Value);

        // Check if user already has the role
        if (await _userManager.IsInRoleAsync(user, "Driver"))
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                Role = "Driver",
                Message = "User is already registered as Driver."
            };
        }

        // Check if driver entity already exists for this user
        var driverQueryable = await _driverRepository.GetQueryableAsync();
        var existingDriver = await AsyncExecuter.FirstOrDefaultAsync(
            driverQueryable.Where(d => d.UserId == currentUserId.Value));

        if (existingDriver != null)
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                Role = "Driver",
                EntityId = existingDriver.Id,
                Message = "Driver entity already exists for this user."
            };
        }

        // Create Driver entity
        Driver driver;
        if (input.PhotoId.HasValue)
        {
            driver = new Driver(
                Guid.NewGuid(),
                input.Name,
                input.Phone,
                input.Identification,
                input.LicenseNumber,
                input.NationalityId,
                currentUserId.Value,
                input.PhotoId.Value);
        }
        else
        {
            driver = new Driver(
                Guid.NewGuid(),
                input.Name,
                input.Phone,
                input.Identification,
                input.LicenseNumber,
                input.NationalityId,
                currentUserId.Value);
        }

        driver = await _driverRepository.InsertAsync(driver, autoSave: true);

        // Add role to user
        var result = await _userManager.AddToRoleAsync(user, "Driver");

        if (!result.Succeeded)
        {
            return new UserRegistrationResultDto
            {
                Success = false,
                EntityId = driver.Id,
                Message = "Driver entity created but failed to add Driver role."
            };
        }

        return new UserRegistrationResultDto
        {
            Success = true,
            Role = "Driver",
            EntityId = driver.Id,
            Message = "Successfully registered as Driver."
        };
    }
}
