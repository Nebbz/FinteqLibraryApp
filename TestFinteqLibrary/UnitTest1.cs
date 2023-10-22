using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FinteqLibrary;


namespace TestFinteqLibrary
{
    [TestClass]
    public class LibraryManagerTests
    {
        private readonly string TestBookListPath = "testBookList.json";
        private LibraryManager libraryManager;

        [TestInitialize]
        public void TestInitialize()
        {
            File.WriteAllText(TestBookListPath, "[]");
            libraryManager = new LibraryManager(TestBookListPath);
        }

        [TestMethod]
        public void TestLoadBooks()
        {
            var existingBooks = libraryManager.LoadBooks();

            Assert.IsNotNull(existingBooks);
            Assert.AreEqual(0, existingBooks.Count);
        }

        [TestMethod]
        public void TestSubmitAdd_ValidInput()
        {
            string title = "Sample Book";
            string author = "Sample Author";
            string year = "2023";

            libraryManager.SubmitAdd(title, author, year);
            var existingBooks = libraryManager.LoadBooks();

            Assert.AreEqual(1, existingBooks.Count);
            Assert.AreEqual(title, existingBooks[0].Title);
            Assert.AreEqual(author, existingBooks[0].Author);
            Assert.AreEqual(int.Parse(year), existingBooks[0].PublicationYear);
            Assert.IsFalse(existingBooks[0].IsCheckedOut);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubmitAdd_InvalidInput()
        {
            string title = "Sample Book";
            string author = "";
            string year = "InvalidYear";

            libraryManager.SubmitAdd(title, author, year);
        }

        [TestMethod]
        public void TestSubmitUpdate_ValidInput()
        {
            string title = "Updated Title";
            string author = "Updated Author";
            string year = "2023";

            // Add a book first
            libraryManager.SubmitAdd("Sample Book", "Sample Author", "2022");

            // Find the book's ID
            var bookToUpdate = libraryManager.LoadBooks()[0];

            libraryManager.SubmitUpdate(bookToUpdate.ID, title, author, year);
            var existingBooks = libraryManager.LoadBooks();

            Assert.AreEqual(1, existingBooks.Count);
            Assert.AreEqual(title, existingBooks[0].Title);
            Assert.AreEqual(author, existingBooks[0].Author);
            Assert.AreEqual(int.Parse(year), existingBooks[0].PublicationYear);
            Assert.IsFalse(existingBooks[0].IsCheckedOut);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubmitUpdate_InvalidInput()
        {
            string title = "";
            string author = "Updated Author";
            string year = "InvalidYear";

            // Add a book first
            libraryManager.SubmitAdd("Sample Book", "Sample Author", "2022");

            // Find the book's ID
            var bookToUpdate = libraryManager.LoadBooks()[0];

            libraryManager.SubmitUpdate(bookToUpdate.ID, title, author, year);
        }

        [TestMethod]
        public void TestSubmitDelete_BookExists()
        {
            // Add a book first
            libraryManager.SubmitAdd("Sample Book", "Sample Author", "2023");

            // Find the book's ID
            var bookToDelete = libraryManager.LoadBooks()[0];

            libraryManager.SubmitDelete(bookToDelete.ID);
            var existingBooks = libraryManager.LoadBooks();

            Assert.AreEqual(0, existingBooks.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubmitDelete_BookNotFound()
        {
            // Add a book first
            libraryManager.SubmitAdd("Sample Book", "Sample Author", "2023");

            // Attempt to delete a book that doesn't exist
            libraryManager.SubmitDelete(2); // ID that doesn't exist
        }

        [TestMethod]
        public void TestSubmitCheckOut_BookExists()
        {
            // Add a book first
            libraryManager.SubmitAdd("Sample Book", "Sample Author", "2023");

            // Find the book's ID
            var bookToCheckOut = libraryManager.LoadBooks()[0];

            libraryManager.SubmitCheckOut(bookToCheckOut.ID);
            var existingBooks = libraryManager.LoadBooks();

            Assert.AreEqual(1, existingBooks.Count);
            Assert.IsTrue(existingBooks[0].IsCheckedOut);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestSubmitCheckOut_BookAlreadyCheckedOut()
        {
            // Add a book first
            libraryManager.SubmitAdd("Sample Book", "Sample Author", "2023");

            // Find the book's ID
            var bookToCheckOut = libraryManager.LoadBooks()[0];

            libraryManager.SubmitCheckOut(bookToCheckOut.ID);
            libraryManager.SubmitCheckOut(bookToCheckOut.ID); // Try to check out the already checked-out book
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubmitCheckOut_BookNotFound()
        {
            // Attempt to check out a book that doesn't exist
            libraryManager.SubmitCheckOut(2); // ID that doesn't exist
        }

        [TestMethod]
        public void TestSubmitReturn_BookExists()
        {
            // Add a book first
            libraryManager.SubmitAdd("Sample Book", "Sample Author", "2023");

            // Find the book's ID
            var bookToReturn = libraryManager.LoadBooks()[0];

            // Check out the book before attempting to return it
            libraryManager.SubmitCheckOut(bookToReturn.ID);

            // Attempt to return the book
            libraryManager.SubmitReturn(bookToReturn.ID);

            var existingBooks = libraryManager.LoadBooks();

            Assert.AreEqual(1, existingBooks.Count);
            Assert.IsFalse(existingBooks[0].IsCheckedOut);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestSubmitReturn_BookAlreadyCheckedIn()
        {
            // Add a book first
            libraryManager.SubmitAdd("Sample Book", "Sample Author", "2023");

            // Find the book's ID
            var bookToReturn = libraryManager.LoadBooks()[0];

            libraryManager.SubmitReturn(bookToReturn.ID); // Try to return a book that's already checked in
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubmitReturn_BookNotFound()
        {
            // Attempt to return a book that doesn't exist
            libraryManager.SubmitReturn(2); // ID that doesn't exist
        }
    }
}
