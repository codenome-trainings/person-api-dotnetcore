using System;
using System.Collections.Generic;
using System.Linq;
using PersonApi.Models;
using PersonApi.Models.Context;

namespace PersonApi.Repository.Implementations
{
    public class BookRepositoryImpl : IBookRepository
    {
        
        private readonly MySQLContext _context;

        public BookRepositoryImpl(MySQLContext context)
        {
            _context = context;
        }


        public Book Create(Book book)
        {
            try
            {
                _context.Add(book);
                _context.SaveChanges();
                return book;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Book FindById(long id)
        {
            try
            {
                var book = _context.Books.SingleOrDefault(
                    p => p.Id.Equals(id)
                );
                return book;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public List<Book> FindAll()
        {
            try
            {
                var books = _context.Books.ToList();
                return books;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Book Update(Book book)
        {
            if (!Exists(book.Id)) return null;

            try
            {
                var result = _context.Books.SingleOrDefault(
                    p => p.Id.Equals(book.Id)
                );
                _context.Entry(result).CurrentValues.SetValues(book);
                _context.SaveChanges();
                return book;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public void Delete(long id)
        {
            try
            {
                var result = _context.Books.SingleOrDefault(
                    p => p.Id.Equals(id)
                );

                if (result != null) _context.Books.Remove(result);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool Exists(long? id)
        {
            return _context.Books.Any(
                p => p.Id.Equals(id)
            );
        }
    }
}