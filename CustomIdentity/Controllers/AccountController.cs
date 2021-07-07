using System.Threading.Tasks;
using AutoMapper;
using CustomIdentity.BLL.Models.Account;
using CustomIdentity.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentity.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IMapper mapper,
            IAccountService accountService) : base(mapper)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var token = await _accountService.LoginAsync(loginModel);

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegisterModel loginModel)
        {
            await _accountService.RegisterAsync(loginModel);

            return Ok();
        }
    }
}
