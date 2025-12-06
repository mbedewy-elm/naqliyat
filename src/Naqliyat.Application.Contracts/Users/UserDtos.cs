using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Naqliyat.Users;

public class RegisterAsRequesterDto
{
    // No additional info needed for requester role
}

public class RegisterAsOwnerDto
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Identification { get; set; }
    public string EntityNumber { get; set; }
    public int NationalityId { get; set; }
}

public class RegisterAsDriverDto
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Identification { get; set; }
    public string LicenseNumber { get; set; }
    public Guid? PhotoId { get; set; }
    public int NationalityId { get; set; }
}

public class UserRegistrationResultDto
{
    public bool Success { get; set; }
    public string Role { get; set; }
    public Guid? EntityId { get; set; }
    public string Message { get; set; }
}

public interface IUserAppService
{
    Task<UserRegistrationResultDto> RegisterAsRequesterAsync();
    Task<UserRegistrationResultDto> RegisterAsOwnerAsync(RegisterAsOwnerDto input);
    Task<UserRegistrationResultDto> RegisterAsDriverAsync(RegisterAsDriverDto input);
}
