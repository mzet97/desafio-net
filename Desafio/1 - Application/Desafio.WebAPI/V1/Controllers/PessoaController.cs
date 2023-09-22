using AutoMapper;
using Desafio.Domain.Interfaces;
using Desafio.Domain.Interfaces.Services;
using Desafio.Domain.Models;
using Desafio.Infra.CrossCutting.Filters;
using Desafio.Infra.CrossCutting.Notifications;
using Desafio.WebAPI.Controllers;
using Desafio.WebAPI.Extensions;
using Desafio.WebAPI.ViewModels.Pessoa;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.WebAPI.V1.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v{version:apiVersion}/pessoa")]
    public class PessoaController : MainController
    {
        private readonly IPessoaService _pessoaService;
        private readonly IMapper _mapper;

        public PessoaController(
            INotifier notifier, 
            IUser appUser, 
            IPessoaService pessoaService, 
            IMapper mapper) : base(notifier, appUser)
        {
            _pessoaService = pessoaService;
            _mapper = mapper;
        }

        [HttpPost]
        [ClaimsAuthorize("Pessoa", "Create")]
        public async Task<ActionResult<PessoaViewModel>> Create(PessoaCreatedViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var categoryProduct = await _pessoaService.Create(_mapper.Map<Pessoa>(viewModel));

            var resp = _mapper.Map<PessoaViewModel>(categoryProduct);

            return CustomResponse(resp);
        }

        [HttpGet]
        [ClaimsAuthorize("Pessoa", "Get")]
        public async Task<IEnumerable<PessoaViewModel>> GetAllWithFilter([FromQuery] PessoaFilter filter)
        {
            return _mapper.Map<IEnumerable<PessoaViewModel>>(await _pessoaService.GetAllWithFilter(filter));
        }

        [HttpGet("{id}")]
        [ClaimsAuthorize("Pessoa", "Get")]
        public async Task<PessoaViewModel> GetById(int id)
        {
            return _mapper.Map<PessoaViewModel>(await _pessoaService.GetById(id));
        }

        [HttpPut("{id}")]
        [ClaimsAuthorize("Pessoa", "Update")]
        public async Task<ActionResult<PessoaViewModel>> Update([FromRoute] int id, [FromBody] PessoaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            if (id != viewModel.Id) return NotFound();

            var categoryProduct = await _pessoaService.Update(_mapper.Map<Pessoa>(viewModel));

            var resp = _mapper.Map<PessoaViewModel>(categoryProduct);

            return CustomResponse(resp);
        }

        [HttpDelete("{id}")]
        [ClaimsAuthorize("Pessoa", "Delete")]
        public async Task<PessoaViewModel> Delete(int id)
        {
            return _mapper.Map<PessoaViewModel>(await _pessoaService.Delete(id));
        }
    }
}
