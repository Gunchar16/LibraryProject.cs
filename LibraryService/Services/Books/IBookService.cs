using Library.Infrastructure.Dtos;
using Library.Infrastructure.Entities;
using Library.Shared.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Services.Books
{

    public interface IBookService
    {
        Task<ApiServiceResponse<List<BookDetailsDto>>> GetBooksAsync();
        Task<ApiServiceResponse<BookDetailsDto>> GetBookAsync(int id);
        Task<ApiServiceResponse<List<BookDetailsDto>>> SearchBooksAsync(string text);
        Task<ApiServiceResponse<int>> AddBookAsync(BookAddDto book);
        Task<ApiServiceResponse<int>> UpdateBookAsync(int id, BookUpdateDto book);
        Task<ApiServiceResponse<bool>> RemoveBookAsync(int id);
        Task<ApiServiceResponse<bool>> TakeBookAsync(int bookId, int userId);
        Task<ApiServiceResponse<bool>> ReturnBookAsync(int bookId, int userId);

    }
}
