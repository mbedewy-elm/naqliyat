using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naqliyat.Permissions;
using Naqliyat.Users;

namespace Naqliyat.Controllers.Users;

[Route("api/app/users")]
public class UserController : NaqliyatController
{
    private readonly IUserAppService _userAppService;

    public UserController(IUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    [HttpPost("register/requester")]
    [Authorize(NaqliyatPermissions.Users.RegisterAsRequester)]
    public virtual Task<UserRegistrationResultDto> RegisterAsRequesterAsync()
    {
        return _userAppService.RegisterAsRequesterAsync();
    }

    [HttpPost("register/owner")]
    [Authorize(NaqliyatPermissions.Users.RegisterAsOwner)]
    public virtual Task<UserRegistrationResultDto> RegisterAsOwnerAsync([FromBody] RegisterAsOwnerDto input)
    {
        return _userAppService.RegisterAsOwnerAsync(input);
    }

    [HttpPost("register/driver")]
    [Authorize(NaqliyatPermissions.Users.RegisterAsDriver)]
    public virtual Task<UserRegistrationResultDto> RegisterAsDriverAsync([FromBody] RegisterAsDriverDto input)
    {
        return _userAppService.RegisterAsDriverAsync(input);
    }
}
