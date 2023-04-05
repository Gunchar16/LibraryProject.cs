using Library.Infrastructure.Dtos;
using Library.Infrastructure.Entities;
using Library.Shared.Api.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Services.Authors
{
    public interface IAuthorService
    {
        Task<ApiServiceResponse<List<Author>>> GetAllAuthorsAsync();
        Task<ApiServiceResponse<Author>> GetAuthorByIdAsync(int id);
        Task<ApiServiceResponse<List<Author>>> SearchAuthorsAsync(string fullName);
        Task<ApiServiceResponse<int>> AddAuthorAsync(AuthorAddDto author);
        Task<ApiServiceResponse<int>> UpdateAuthorAsync(int id, AuthorAddDto newAuthor);
        Task<ApiServiceResponse<bool>> RemoveAuthorAsync(int id);
    }
}
