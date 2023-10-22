using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Path = System.IO.Path;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

namespace FinteqLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int giSearchMethod;
        private int giFoundBook;
        private List<Book> existingBooks = new List<Book>();
        private LibraryManager libraryManager;

        public MainWindow()
        {
            InitializeComponent();
            giFoundBook = -1;
            btnBack.Visibility = Visibility.Collapsed;
            existingBooks = LoadBooks();
            Closing += MainWindow_Closing;
        }

        

            private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Close the application properly.
            Application.Current.Shutdown();
        }

        public class Book
        {
            //This is the book class that holds the properties about each book
            public int ID { get; set; } // Automatically incrementing ID.
            public string Title { get; set; }
            public string Author { get; set; }
            public int publicationYear { get; set; }
            public bool isCheckedOut { get; set; }


        }

 


        // Generate a new unique ID for a
        public static class ModeType
        {
            // This string variable will store the current mode ie Adding/Updating etc.
            public static string Mode { get; set; }
        }

        //This Method loads the list of books from the JSON file
        public List<Book> LoadBooks()
        {
            // Defining the path of where the data is stored.
            string relativePath = "bookList.json";
            string bookListPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);



            // Create or load the collection.
            List<Book> bookListCollection = new List<Book>();

            // Check if the data file exists and load data if it does.
            if (File.Exists(bookListPath))
            {
                string jsonData = File.ReadAllText(bookListPath);
                bookListCollection = JsonSerializer.Deserialize<List<Book>>(jsonData);

            }
            return bookListCollection; // Return the deserialized data.
        }

        //This is an event for when the Add Book button is clicked.
        //This event will allow the entry off a new book, using the set methods for all of the books properties. 

        public void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            ModeType.Mode = "Add";
            Button clickedButton = (Button)sender;
            grdAdd.Visibility = Visibility.Visible;
            btnSubmit.Visibility = Visibility.Visible;
            HideOtherButtons(clickedButton);
        }
        //the update button event
        public void btnUpdateBooks_Click(object sender, RoutedEventArgs e)
        {
            ModeType.Mode = "Update";
            Button clickedButton = (Button)sender;
            HideOtherButtons(clickedButton);
            grdFind.Visibility = Visibility.Visible;

        }
        //the view books button event 
        public void btnViewBooks_Click(object sender, RoutedEventArgs e)
        {

            List<Book> existingBooks = LoadBooks();
            dgBooks.ItemsSource = existingBooks;
            Button clickedButton = (Button)sender;
            HideOtherButtons(clickedButton);
            dgBooks.Visibility = Visibility.Visible;

        }
        //the delete books button event
        public void btnDeleteBooks_Click(object sender, RoutedEventArgs e)
        {
            ModeType.Mode = "Delete";
            List<Book> existingBooks = LoadBooks();
            Button clickedButton = (Button)sender;
            HideOtherButtons(clickedButton);
            grdFind.Visibility = Visibility.Visible;

        }
        //the check out button event
        public void btnCheckOutBook_Click(object sender, RoutedEventArgs e)
        {
            ModeType.Mode = "Checkout";
            Button clickedButton = (Button)sender;
            HideOtherButtons(clickedButton);
            grdFind.Visibility = Visibility.Visible;
        }
        //the return button event
        public void btnReturnBook_Click(object sender, RoutedEventArgs e)
        {
            ModeType.Mode = "Return";
            Button clickedButton = (Button)sender;
            HideOtherButtons(clickedButton);
            grdFind.Visibility = Visibility.Visible;
        }

        //This function hides the main menu buttons
        public void HideOtherButtons(Button clickedButton)
        {


            lblCategory.Content = clickedButton.Content;
            lblCategory.Visibility = Visibility.Visible;
            grdButtons.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;


        }

        //the button event for going back to the main page
        public void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdButtons.Visibility = Visibility.Visible;
            grdAdd.Visibility = Visibility.Collapsed;
            btnSubmit.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Collapsed;
            dgBooks.Visibility = Visibility.Collapsed;
            lblCategory.Visibility = Visibility.Collapsed;
            lblCategory.Content = "";
            grdFind.Visibility = Visibility.Collapsed;
            clearText();
        }

        //the event for the submit button
        public void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

            //Checking the modes to complete different tasks depending on the button selected
            string sCurrentMode = ModeType.Mode;
            switch (sCurrentMode)
            {
                case "Add":

                    SubmitAdd();

                break;

                case "Update":

                    SubmitUpdate();

                break;

                case "Delete":

                    SubmitDelete();

                break;

                case "Return":
         
                    SubmitReturn();
                
                break;

                case "Checkout":

                    SubmitCheckOut();

                break;

            }

        }




        public bool IsYearValid(string input)
        {
            // Use a regular expression to check if the input is a valid year (four digits).
            return Regex.IsMatch(input, @"^\d{4}$");
        }

        //this function searches through the list to see if the inputed data matches an existing book
        public void findBook()
        {
            giFoundBook = -1;
            List<Book> existingBooks = LoadBooks();
            string searchID = txtFind1.Text.ToLower();
            string searchTitle = txtFind2.Text.ToLower();
            string searchAuthor = txtFind3.Text.ToLower();

            var searchResults = new List<Book>();

            if (giSearchMethod == 1)
            {
                searchResults = existingBooks.Where(existingBooks =>
                existingBooks.ID.ToString().Contains(searchID)
            ).ToList();

            }
            if (giSearchMethod == 2)
            {
                searchResults = existingBooks.Where(existingBook =>
                existingBook.Title.ToLower().Contains(searchTitle) &&
                existingBook.Author.ToLower().Contains(searchAuthor)
                ).ToList();



            }

            if (searchResults.Count > 0)
            {

                // Update the foundBookID with the ID of the first book in the search results.
                giFoundBook = searchResults[0].ID;
                string sCurrentMode = ModeType.Mode;
                MessageBox.Show("Book found with ID: " + searchResults[0].ID + "\nTitle: " + searchResults[0].Title + "\nAuthor: " + searchResults[0].Author);
                if (sCurrentMode == "Update")
                {
                    UpdateControls();
                }
                btnSubmit.Visibility = Visibility.Visible;

            }
            else
            {
                MessageBox.Show("There was no book matching your search");
            }

            if (int.TryParse(txtFind1.Text, out int result))
            {

            }
            else
            {
                MessageBox.Show("Invalid input for ID. Please enter a valid ID."); // Display an error message because the input is not a valid integer.
            }
        }

        //this function cleares the texboxes
        public void clearText()
        {
            txtAdd1.Text = "";
            txtAdd2.Text = "";
            txtAdd3.Text = "";
            txtFind1.Text = "";
            txtFind1.Text = "";
            txtFind1.Text = "";

        }

        //This event stores what type of search method the user is using
        public void cmbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSearch.SelectedIndex == 0)
            {
                giSearchMethod = 1;
                txtFind1.Visibility = Visibility.Visible;
                lblFind1.Visibility = Visibility.Visible;

                txtFind2.Text = "";
                txtFind3.Text = "";

                txtFind2.Visibility = Visibility.Collapsed;
                txtFind3.Visibility = Visibility.Collapsed;
                lblFind2.Visibility = Visibility.Collapsed;
                lblFind3.Visibility = Visibility.Collapsed;

            }
            else if (cmbSearch.SelectedIndex == 1)
            {
                giSearchMethod = 2;
                txtFind2.Visibility = Visibility.Visible;
                txtFind3.Visibility = Visibility.Visible;
                lblFind2.Visibility = Visibility.Visible;
                lblFind3.Visibility = Visibility.Visible;

                txtFind1.Visibility = Visibility.Collapsed;
                txtFind1.Text = "";
                lblFind1.Visibility = Visibility.Collapsed;

            }
        }

        //this function holds the logic that wi
        public void SubmitAdd()
        {
            //Sanitizing inputs
            if (IsYearValid(txtAdd3.Text) == true && (!string.IsNullOrEmpty(txtAdd1.Text) && !string.IsNullOrEmpty(txtAdd2.Text)))
            {
                Book book = new Book();
                List<Book> existingBooks = LoadBooks();
                // Find the last book's ID using LINQ.
                int lastBookID = existingBooks.Any() ? existingBooks.Max(b => b.ID) : 0;
                // Generate a new unique ID for a new book.
                int newBookID  = lastBookID + 1;
                string sTitle = txtAdd1.Text;
                string sAuthor = txtAdd2.Text;
                string sYear = txtAdd3.Text;

                book.ID = newBookID;
                book.Title = sTitle;
                book.Author = sAuthor;
                book.publicationYear = int.Parse(sYear);
                book.isCheckedOut = false;
                existingBooks.Add(book);
                try
                {
                    string updateJson = JsonSerializer.Serialize(existingBooks);
                    File.WriteAllText("bookList.json", updateJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                MessageBox.Show(book.Title + " Added Successfully!");
                clearText();

            }
            else
            {
                MessageBox.Show("Invalid input. Either you have a blank title, author or year format is incorrect. Please fix");
            }
        }

        //this function holds the logic to update a book
        public void SubmitUpdate()
        {
            //Sanitizing inputs
            if (IsYearValid(txtAdd3.Text) == true && (!string.IsNullOrEmpty(txtAdd1.Text) && !string.IsNullOrEmpty(txtAdd2.Text)))
            {
                List<Book> existingBooks = LoadBooks();
                Book bookToUpdate = existingBooks.FirstOrDefault(book => book.ID == giFoundBook);
                if (bookToUpdate != null)
                {
                    // Update the book's properties.
                    bookToUpdate.Title = txtAdd1.Text; // Update with the new title.
                    bookToUpdate.Author = txtAdd2.Text; // Update with the new author.
                    bookToUpdate.publicationYear = int.Parse(txtAdd3.Text); // Update with the new year.

                    // Update the JSON data.
                    string updatedJsonData = JsonSerializer.Serialize(existingBooks);
                    File.WriteAllText("bookList.json", updatedJsonData);

                    // Notify the user that the book has been updated.
                    MessageBox.Show("Book updated successfully.");
                }
                else
                {
                    MessageBox.Show("Book not found or could not be updated.");
                }
            }
            else
            {
                MessageBox.Show("Invalid input. Either you have a blank title, author or your year format is incorrect. Please fix");
            }
            grdFind.Visibility = Visibility.Collapsed;
            clearText();
        }

        //this functions displays the controls for updating
        public void UpdateControls()
        {
            grdAdd.Visibility = Visibility.Visible;
            grdFind.Visibility = Visibility.Collapsed;
        }

        //this function contains the logic for deleting a book
        public void SubmitDelete()
        {
            // Check if giFoundBook is a valid book ID.
            if (giFoundBook != -1)
            {
                // Ask the user for confirmation.
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this book?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // User confirmed the deletion.
                    // Implement the book deletion logic here.

                    // For example, you can remove the book from the existingBooks list.
                    Book bookToDelete = existingBooks.FirstOrDefault(book => book.ID == giFoundBook);
                    if (bookToDelete != null)
                    {
                        existingBooks.Remove(bookToDelete);

                        // Update the JSON data.
                        string updatedJsonData = JsonSerializer.Serialize(existingBooks);
                        string relativePath = "bookList.json";
                        string bookListPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                        File.WriteAllText(bookListPath, updatedJsonData);

                        // Notify the user that the book has been deleted.
                        MessageBox.Show("Book deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Book not found or could not be deleted.");
                    }
                }
            }
            else
            {
                MessageBox.Show("No book selected for deletion.");
            }
        }

        //this function contains the logic to check out a book
        public void SubmitCheckOut()
        {
            // Check if giFoundBook is a valid book ID.
            if (giFoundBook != -1)
            {
                // Locate the book to check out based on its ID.
                Book bookToCheckOut = existingBooks.FirstOrDefault(book => book.ID == giFoundBook);

                if (bookToCheckOut != null)
                {
                    if (!bookToCheckOut.isCheckedOut)
                    {
                        // Mark the book as checked out.
                        bookToCheckOut.isCheckedOut = true;

                        // Update the JSON data.
                        string updatedJsonData = JsonSerializer.Serialize(existingBooks);
                        string relativePath = "bookList.json";
                        string bookListPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                        File.WriteAllText(bookListPath, updatedJsonData);

                        // Notify the user that the book has been checked out.
                        MessageBox.Show("Book checked out successfully.");
                    }
                    else
                    {
                        MessageBox.Show("This book is already checked out.");
                    }
                }
                else
                {
                    MessageBox.Show("Book not found.");
                }
            }
            else
            {
                MessageBox.Show("No book selected to check out.");
            }
        }


        //this function contains the logic to return a book
        public void SubmitReturn()
        {
            // Check if giFoundBook is a valid book ID.
            if (giFoundBook != -1)
            {
                // Locate the book to return based on its ID.
                Book bookToReturn = existingBooks.FirstOrDefault(book => book.ID == giFoundBook);

                if (bookToReturn != null)
                {
                    if (bookToReturn.isCheckedOut)
                    {
                        // Mark the book as checked in.
                        bookToReturn.isCheckedOut = false;

                        // Update the JSON data.
                        string updatedJsonData = JsonSerializer.Serialize(existingBooks);
                        string relativePath = "bookList.json";
                        string bookListPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                        File.WriteAllText(bookListPath, updatedJsonData);

                        // Notify the user that the book has been returned.
                        MessageBox.Show("Book returned successfully.");
                    }
                    else
                    {
                        MessageBox.Show("This book is already checked in.");
                    }
                }
                else
                {
                    MessageBox.Show("Book not found.");
                }
            }
            else
            {
                MessageBox.Show("No book selected to return.");
            }
        }


        //this is the find button click event which finds a book and stores the ID of the book
        public void btnFind_Click(object sender, RoutedEventArgs e)
        {
            findBook();
        }

        //For the Testing file to see the text boxes
        public string TxtAdd1Text
        {
            get { return txtAdd1.Text; }
            set { txtAdd1.Text = value; }
        }

        public string TxtAdd2Text
        {
            get { return txtAdd2.Text; }
            set { txtAdd2.Text = value; }
        }

        public string TxtAdd3Text
        {
            get { return txtAdd3.Text; }
            set { txtAdd3.Text = value; }
        }

        public string TxtFind1Text
        {
            get { return txtFind1.Text; }
            set { txtFind1.Text = value; }
        }

        public string TxtFind2Text
        {
            get { return txtFind2.Text; }
            set { txtFind2.Text = value; }
        }

        public string TxtFind3Text
        {
            get { return txtFind3.Text; }
            set { txtFind3.Text = value; }
        }
    }
}
