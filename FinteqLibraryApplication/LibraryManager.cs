using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FinteqLibrary
{
    public class LibraryManager
    {
        private List<Book> existingBooks;
        private string bookListPath;

        public LibraryManager(string bookListPath)
        {
            this.bookListPath = bookListPath;
            existingBooks = LoadBooksFromJson(bookListPath);
        }

        public LibraryManager()
        {
            // Initialize with default values or logic if necessary.
        }


        public List<Book> LoadBooks()
        {
            return existingBooks;
        }

        public void SubmitAdd(string title, string author, string year)
        {
            if (IsYearValid(year) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(author))
            {
                int newBookId = GetNextBookId();
                Book newBook = new Book
                {
                    ID = newBookId,
                    Title = title,
                    Author = author,
                    PublicationYear = int.Parse(year),
                    IsCheckedOut = false
                };
                existingBooks.Add(newBook);

                SaveBooksToJson(existingBooks);
            }
            else
            {
                throw new ArgumentException("Invalid input. Please provide valid title, author, and year.");
            }
        }

        public void SubmitUpdate(int bookId, string title, string author, string year)
        {
            if (IsYearValid(year) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(author))
            {
                Book bookToUpdate = existingBooks.FirstOrDefault(book => book.ID == bookId);
                if (bookToUpdate != null)
                {
                    bookToUpdate.Title = title;
                    bookToUpdate.Author = author;
                    bookToUpdate.PublicationYear = int.Parse(year);

                    SaveBooksToJson(existingBooks);
                }
                else
                {
                    throw new ArgumentException("Book not found or could not be updated.");
                }
            }
            else
            {
                throw new ArgumentException("Invalid input. Please provide valid title, author, and year.");
            }
        }

        public void SubmitDelete(int bookId)
        {
            Book bookToDelete = existingBooks.FirstOrDefault(book => book.ID == bookId);
            if (bookToDelete != null)
            {
                existingBooks.Remove(bookToDelete);
                SaveBooksToJson(existingBooks);
            }
            else
            {
                throw new ArgumentException("Book not found or could not be deleted.");
            }
        }

        public void SubmitCheckOut(int bookId)
        {
            Book bookToCheckOut = existingBooks.FirstOrDefault(book => book.ID == bookId);
            if (bookToCheckOut != null)
            {
                if (!bookToCheckOut.IsCheckedOut)
                {
                    bookToCheckOut.IsCheckedOut = true;
                    SaveBooksToJson(existingBooks);
                }
                else
                {
                    throw new InvalidOperationException("This book is already checked out.");
                }
            }
            else
            {
                throw new ArgumentException("Book not found.");
            }
        }

        public void SubmitReturn(int bookId)
        {
            Book bookToReturn = existingBooks.FirstOrDefault(book => book.ID == bookId);
            if (bookToReturn != null)
            {
                if (bookToReturn.IsCheckedOut)
                {
                    bookToReturn.IsCheckedOut = false;
                    SaveBooksToJson(existingBooks);
                }
                else
                {
                    throw new InvalidOperationException("This book is already checked in.");
                }
            }
            else
            {
                throw new ArgumentException("Book not found.");
            }
        }

        private List<Book> LoadBooksFromJson(string bookListPath)
        {
            if (File.Exists(bookListPath))
            {
                string jsonData = File.ReadAllText(bookListPath);
                return JsonSerializer.Deserialize<List<Book>>(jsonData);
            }
            return new List<Book>();
        }

        private void SaveBooksToJson(List<Book> books)
        {
            string jsonData = JsonSerializer.Serialize(books);
            File.WriteAllText(bookListPath, jsonData);
        }

        private int GetNextBookId()
        {
            return existingBooks.Any() ? existingBooks.Max(b => b.ID) + 1 : 1;
        }

        private bool IsYearValid(string input)
        {
            return int.TryParse(input, out int year) && year >= 0 && year <= DateTime.Now.Year;
        }

        public class Book
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public int PublicationYear { get; set; }
            public bool IsCheckedOut { get; set; }
        }
    }
}
