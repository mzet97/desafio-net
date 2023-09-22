﻿using Desafio.Domain.Interfaces.Services;
using Desafio.Domain.Interfaces;
using Desafio.Infra.CrossCutting.Notifications;
using Desafio.WebAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Desafio.WebAPI.ViewModels.User;

namespace Desafio.WebAPI.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : MainController
    {
        private readonly IAuthService _authService;

        public AuthController(
            INotifier notifier,
            IAuthService authService,
            IUser user) : base(notifier, user)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = viewModel.ToDomain();

            var resp = await _authService.Register(user, viewModel.Password);

            return CustomResponse(resp);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _authService.Login(viewModel.Email, viewModel.Password);

            return CustomResponse(result);
        }
    }
}
