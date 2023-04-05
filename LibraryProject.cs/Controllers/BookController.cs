using Library.Infrastructure.Dtos;
using Library.Infrastructure.Entities;
using Library.Service.Services.Books;
using Library.Shared.Api.ApplicationContext;
using Library.Shared.Api.Extensions;
using Library.Shared.Api.Filters;
using Library.Shared.Api.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ApplicationContext _applicationContext;
        public BookController(IBookService bookService, ApplicationContext applicationContext)
        {
            _bookService = bookService;
            _applicationContext = applicationContext;
        }

        [HttpGet]
        public async Task<ActionResult<ApiServiceResponse<List<BookDetailsDto>>>> GetBooks()
            => this.ResponseResult(await _bookService.GetBooksAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiServiceResponse<BookDetailsDto>>> GetBook(int id)
            => this.ResponseResult(await _bookService.GetBookAsync(id));
        [HttpGet("search")]
        public async Task<ActionResult<ApiServiceResponse<List<BookDetailsDto>>>> SearchBooks(string text)
            => this.ResponseResult(await _bookService.SearchBooksAsync(text));
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<ApiServiceResponse<int>>> AddBook([FromBody] BookAddDto book)
            => this.ResponseResult(await _bookService.AddBookAsync(book));
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<ActionResult<ApiServiceResponse<int>>> UpdateBook(int id, [FromBody] BookUpdateDto book)
            => this.ResponseResult(await _bookService.UpdateBookAsync(id, book));
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiServiceResponse<bool>>> RemoveBook(int id)
            => this.ResponseResult(await _bookService.RemoveBookAsync(id));
        [HttpPut("{bookId}/take")]
        public async Task<ActionResult<ApiServiceResponse<bool>>> TakeBook(int bookId)
        {
            var userId = _applicationContext.UserId.Value;
            return this.ResponseResult(await _bookService.TakeBookAsync(bookId, userId));
        }
        [HttpPut("{bookId}/return")]
        public async Task<ActionResult<ApiServiceResponse<bool>>> ReturnBook(int bookId)
        {
            var userId = _applicationContext.UserId.Value;
            return this.ResponseResult(await _bookService.ReturnBookAsync(bookId, userId));
        }
    }
}