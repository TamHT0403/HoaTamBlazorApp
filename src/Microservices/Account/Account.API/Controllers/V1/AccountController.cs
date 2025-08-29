using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Account.API
{
    [ApiController]
    [Route("api/v1/account")]
    public class AccountController : ControllerBase
    {
        public AccountController(

        )
        {
            
        }
        [Authorize]
        [HttpGet("test")]
        public async Task<string> Test()
        {
            return await Task.FromResult<string>(result: "Xin chào Việt Nam");
        }
    }
}