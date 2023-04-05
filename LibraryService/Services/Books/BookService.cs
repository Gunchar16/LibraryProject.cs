using Library.Infrastructure.Entities;
using Library.Infrastructure.Interfaces;
using Library.Shared.Api.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Shared.Api.Response;
using Library.Shared.Api.Exceptions;
using Library.Infrastructure.Dtos;
using Library.Infrastructure;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace Library.Service.Services.Books
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IGenericRepository<Book> bookRepository, 
            IGenericRepository<Author> authorRepository, 
            IGenericRepository<User> userRepository, 
            IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiServiceResponse<List<BookDetailsDto>>> GetBooksAsync()
        {
            var books = await _bookRepository.Query().Include(b => b.BookAuthors).ThenInclude(ba => ba.Author).ToListAsync();
            if (books.IsNullOrEmpty())
                return new ApiServiceResponse<List<BookDetailsDto>>();
            var result = new List<BookDetailsDto>();
            foreach(var book in books)
            {
                result.Add(new BookDetailsDto(
                book.Id,
                book.Title,
                book.Description,
                book.Image,
                book.Rating,
                book.PublicationDate,
                book.IsTaken,
                book.BookAuthors.Select(ba => new AuthorDetailsDto(
                    ba.Author.Id,
                    ba.Author.FirstName,
                    ba.Author.LastName,
                    ba.Author.YearOfBirth)).ToList()));
            }
            return new SuccessApiServiceResponse<List<BookDetailsDto>>(result);
        }

        public async Task<ApiServiceResponse<BookDetailsDto>> GetBookAsync(int id)
        {
            var book = await _bookRepository.Query()
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return new NotFoundApiServiceResponse<BookDetailsDto>();
            var bookDto = new BookDetailsDto(
                book.Id,
                book.Title,
                book.Description,
                book.Image,
                book.Rating,
                book.PublicationDate,
                book.IsTaken,
                book.BookAuthors.Select(ba => new AuthorDetailsDto(
                    ba.Author.Id,
                    ba.Author.FirstName, 
                    ba.Author.LastName, 
                    ba.Author.YearOfBirth)).ToList());

            return new SuccessApiServiceResponse<BookDetailsDto>(bookDto);
        }
        public async Task<ApiServiceResponse<List<BookDetailsDto>>> SearchBooksAsync(string text)
        {
            var books = (await _bookRepository.Query()
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .ToListAsync())
                .Where(x => x.Title.ToUpper().Contains(text.ToUpper()) ||
                       x.BookAuthors.Select(ba => $"{ba.Author.FirstName} {ba.Author.LastName}")
                       .Any(authorName => authorName.ToUpper().Contains(text.ToUpper())))
                .ToList();

            if (books.IsNullOrEmpty())
                return new NotFoundApiServiceResponse<List<BookDetailsDto>>();

            var searchedBooks = new List<BookDetailsDto>();
            foreach (var book in books)
                searchedBooks.Add(new BookDetailsDto(
                    book.Id,
                    book.Title,
                    book.Description,
                    book.Image,
                    book.Rating,
                    book.PublicationDate,
                    book.IsTaken,
                    book.BookAuthors.Select(ba => new AuthorDetailsDto(
                        ba.Author.Id,
                        ba.Author.FirstName,
                        ba.Author.LastName,
                        ba.Author.YearOfBirth)).ToList()));

            return new SuccessApiServiceResponse<List<BookDetailsDto>>(searchedBooks);
        }

        public async Task<ApiServiceResponse<int>> AddBookAsync(BookAddDto bookDto)
        {
            var newBook = new Book()
            {
                Description = bookDto.Description,
                Image = bookDto.Image,
                PublicationDate = bookDto.PublicationDate,
                Rating = bookDto.Rating,
                Title = bookDto.Title,
                IsTaken = false,
                BookAuthors = new List<BookAuthor>()
            };

            foreach (var authorId in bookDto.AuthorIds)
            {
                var author = await _authorRepository.GetSingleOrDefaultAsync(authorId);

                if (author != null)
                {
                    var bookAuthor = new BookAuthor()
                    {
                        Book = newBook,
                        Author = author
                    };

                    newBook.BookAuthors.Add(bookAuthor);
                }
            }

            await _bookRepository.Add(newBook);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessApiServiceResponse<int>(newBook.Id);
        }

        public async Task<ApiServiceResponse<int>> UpdateBookAsync(int id, BookUpdateDto book)
        {
            var existingBook = await _bookRepository.Query().Include(b => b.BookAuthors).SingleOrDefaultAsync(b => b.Id == id);

            if (existingBook is null)
                return new NotFoundApiServiceResponse<int>("Book not found");

            existingBook.Description = book.Description;
            existingBook.Image = book.Image;
            existingBook.PublicationDate = book.PublicationDate;
            existingBook.Rating = book.Rating;
            existingBook.Title = book.Title;

            existingBook.BookAuthors.Clear();

            foreach (var authorId in book.AuthorIds)
            {
                var author = await _authorRepository.GetSingleOrDefaultAsync(authorId);

                if (author != null)
                {
                    var bookAuthor = new BookAuthor()
                    {
                        Book = existingBook,
                        Author = author
                    };

                    existingBook.BookAuthors.Add(bookAuthor);
                }
            }

            _bookRepository.Update(existingBook);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessApiServiceResponse<int>(existingBook.Id);
        }
        public async Task<ApiServiceResponse<bool>> RemoveBookAsync(int id)
        {
            var book = await _bookRepository.Query()
                .Include(b => b.BookAuthors)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return new NotFoundApiServiceResponse<bool>();

            _bookRepository.Remove(book);

            await _unitOfWork.SaveChangesAsync();

            return new SuccessApiServiceResponse<bool>(true);
        }
        public async Task<ApiServiceResponse<bool>> TakeBookAsync(int bookId, int userId)
        {
            var book = await _bookRepository.Query().Include(b => b.TakenBy).FirstOrDefaultAsync(b => b.Id == bookId);
            if (book is null)
                return new NotFoundApiServiceResponse<bool>($"Book with ID {bookId} not found.");

            if (book.TakenBy != null)
                return new BadRequestApiServiceResponse<bool>(false, $"Book with ID {bookId} has already been taken.");

            book.TakenBy = await _userRepository.GetSingleOrDefaultAsync(userId);
            book.IsTaken = true;

            await _unitOfWork.SaveChangesAsync();
            return new SuccessApiServiceResponse<bool>(true);
        }
        public async Task<ApiServiceResponse<bool>> ReturnBookAsync(int bookId, int userId)
        {
            var book = await _bookRepository.Query().Include(b => b.TakenBy).FirstOrDefaultAsync(b => b.Id == bookId);
            if (book is null)
                return new NotFoundApiServiceResponse<bool>($"Book with ID {bookId} not found.");

            if (book.TakenBy is null)
                return new BadRequestApiServiceResponse<bool>(false, $"Book with ID {bookId} has not been taken.");

            book.TakenBy = null;
            book.IsTaken = false;

            await _unitOfWork.SaveChangesAsync();
            return new SuccessApiServiceResponse<bool>(true);
        }
    }
}
