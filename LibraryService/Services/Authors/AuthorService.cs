using Library.Infrastructure;
using Library.Infrastructure.Dtos;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Interfaces;
using Library.Shared.Api.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Services.Authors
{
    public class AuthorService : IAuthorService
    {
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AuthorService(IGenericRepository<Author> authorRepository, IUnitOfWork unitOfWork)
        {
            _authorRepository= authorRepository;
            _unitOfWork= unitOfWork;
        }
        public async Task<ApiServiceResponse<List<Author>>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepository.Query().ToListAsync();
            if(authors.IsNullOrEmpty())
                return new NotFoundApiServiceResponse<List<Author>>();
            return new SuccessApiServiceResponse<List<Author>>(authors);
        }

        public async Task<ApiServiceResponse<Author>> GetAuthorByIdAsync(int id)
        {
            var author = await _authorRepository.Query().Include(ba => ba.BookAuthors).ThenInclude(b => b.Book).FirstOrDefaultAsync(a => a.Id == id);
            if(author is null)
                return new NotFoundApiServiceResponse<Author>();
            return new SuccessApiServiceResponse<Author>(author);
        }
        public async Task<ApiServiceResponse<List<Author>>> SearchAuthorsAsync(string fullName)
        {
            var authors = (await _authorRepository.Query()
                .ToListAsync())
                .Where(x => $"{x.FirstName} {x.LastName}".ToUpper()
                .Contains(fullName.ToUpper()))
                .ToList();
            if (authors.IsNullOrEmpty())
                return new NotFoundApiServiceResponse<List<Author>>();
            return new SuccessApiServiceResponse<List<Author>>(authors);
        }
        public async Task<ApiServiceResponse<int>> AddAuthorAsync(AuthorAddDto newAuthor)
        {
            var author = new Author
            {
                FirstName = newAuthor.FirstName,
                LastName = newAuthor.LastName,
                YearOfBirth = newAuthor.YearOfBirth,
            };
            await _authorRepository.Add(author);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessApiServiceResponse<int>(author.Id);
        }
        public async Task<ApiServiceResponse<int>> UpdateAuthorAsync(int id, AuthorAddDto newAuthor)
        {
            var author = await _authorRepository.GetSingleOrDefaultAsync(id);
            if (author is null)
                return new NotFoundApiServiceResponse<int>();
            author.FirstName = newAuthor.FirstName;
            author.LastName = newAuthor.LastName;
            author.YearOfBirth = newAuthor.YearOfBirth;
            _authorRepository.Update(author);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessApiServiceResponse<int>(author.Id);
        }
        public async Task<ApiServiceResponse<bool>> RemoveAuthorAsync(int id)
        {
            var book = await _authorRepository.Query()
                .Include(b => b.BookAuthors)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return new NotFoundApiServiceResponse<bool>();

            _authorRepository.Remove(book);

            await _unitOfWork.SaveChangesAsync();

            return new SuccessApiServiceResponse<bool>(true);
        }

    }
}
