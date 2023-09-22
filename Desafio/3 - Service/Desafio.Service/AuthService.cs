﻿using Desafio.Domain.Interfaces.Services;
using Desafio.Domain.Models;
using Desafio.Infra.CrossCutting.Extensions;
using Desafio.Infra.CrossCutting.Notifications;
using Desafio.Infra.CrossCutting.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Desafio.Service
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly AppSettings _appSettings;

        public AuthService(
            INotifier notifier,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IOptions<AppSettings> appSettings) :
            base(notifier)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<LoginResponseViewModel> Register(User user, string password)
        {
            var resultCreateUser = await _userManager.CreateAsync(user, password);

            if (resultCreateUser.Succeeded)
            {
                await AddClaims(user);
                await _signInManager.SignInAsync(user, false);
                return await GetJwt(user.Email);
            }

            foreach (var error in resultCreateUser.Errors)
            {
                NotifyError(error.Description);
            }

            return await GetJwt(user.Email);
        }

        public async Task<LoginResponseViewModel> Login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {
                return await GetJwt(email);
            }
            else if (result.IsLockedOut)
            {
                NotifyError("Usario bloqueado");
                return null;
            }

            NotifyError("Senha ou e-mail inválido");

            return null;
        }


        private async Task AddClaims(User user)
        {
            await _userManager.AddClaimAsync(user, new Claim("Pessoa", "Create"));
            await _userManager.AddClaimAsync(user, new Claim("Pessoa", "Get"));
            await _userManager.AddClaimAsync(user, new Claim("Pessoa", "Update"));
            await _userManager.AddClaimAsync(user, new Claim("Pessoa", "Delete"));
        }


        public async Task<LoginResponseViewModel> GetJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidOn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var userToken = new UserTokenViewModel(user.Id.ToString(), user.Email, claims);

            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = userToken
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
