using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using CustomIdentity.BLL.Extensions;
using CustomIdentity.BLL.Models.Account;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace CustomIdentity.BLL.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            ITokenGeneratorService tokenGeneratorService, 
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGeneratorService = tokenGeneratorService;
            _mapper = mapper;
        }

        public async Task<LoginResultModel> LoginAsync(LoginModel loginModel, bool lockoutOnFailure = false)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);

            if (user is null)
            {
                throw new AuthenticationException();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password,loginModel.RememberMe, lockoutOnFailure);

            if (!signInResult.Succeeded)
            {
                throw new AuthenticationException();
            }

            var jwtToken = _tokenGeneratorService.CreateJwtToken(user);

            var loginResultModel = new LoginResultModel
            {
                AccessToken = jwtToken,
                RefreshToken = string.Empty,
                UserName = user.UserName
            };

            return loginResultModel;
        }

        public async Task RegisterAsync(RegisterModel registerModel)
        {
            var existedUser = await _userManager.FindByEmailAsync(registerModel.Email);

            if (existedUser != null)
            {
                throw new Exception("User with specified credential already exists");
            }

            var user = _mapper.Map<User>(registerModel);

            var createIdentityResult = await _userManager.CreateAsync(user, registerModel.Password);
            createIdentityResult.ThrowExceptionOnFailure();
        }
    }
}