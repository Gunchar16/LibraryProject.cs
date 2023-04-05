using Library.Infrastructure.Dtos;
using Library.Infrastructure.Entities;
using Library.Service.Services.Authors;
using Library.Shared.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Library.Shared.Api.Response;
using Microsoft.AspNetCore.Authorization;
using Library.Shared.Api.Extensions;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiServiceResponse<List<Author>>>> GetAllAuthors()
            => this.ResponseResult(await _authorService.GetAllAuthorsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiServiceResponse<Author>>> GetAuthorById(int id)
            => this.ResponseResult(await _authorService.GetAuthorByIdAsync(id));

        [HttpGet("search")]
        public async Task<ActionResult<ApiServiceResponse<List<Author>>>> SearchAuthors(string fullName)
            => this.ResponseResult(await _authorService.SearchAuthorsAsync(fullName));

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<ApiServiceResponse<int>>> AddAuthor([FromBody] AuthorAddDto authorAddDto)
            => this.ResponseResult(await _authorService.AddAuthorAsync(authorAddDto));

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<ActionResult<ApiServiceResponse<int>>> UpdateAuthor(int id, [FromBody] AuthorAddDto authorAddDto)
            => this.ResponseResult(await _authorService.UpdateAuthorAsync(id, authorAddDto));

        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<ActionResult<ApiServiceResponse<bool>>> RemoveAuthor(int id)
            => this.ResponseResult(await _authorService.RemoveAuthorAsync(id));
    }
}
