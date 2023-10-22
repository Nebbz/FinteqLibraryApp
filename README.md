# Finteq Library Management System

The Finteq Library Management System is a C# application that simplifies the management of books in a library. This guide will walk you through the various features and provide step-by-step instructions on how to use the application effectively.

## Table of Contents

1. [Getting Started](#getting-started)
   - [Prerequisites](#prerequisites)
   - [Installation](#installation)
   
2. [Application Features](#application-features)
   - [Add a New Book](#1-add-a-new-book)
   - [Update Book Information](#2-update-book-information)
   - [Delete a Book](#3-delete-a-book)
   - [Check Out a Book](#4-check-out-a-book)
   - [Return a Book](#5-return-a-book)
   - [View Available Books](#6-view-available-books)
   
3. [Usage Guide](#usage-guide)
   - [Adding a New Book](#adding-a-new-book)
   - [Updating Book Information](#updating-book-information)
   - [Deleting a Book](#deleting-a-book)
   - [Checking Out a Book](#checking-out-a-book)
   - [Returning a Book](#returning-a-book)
   - [Viewing Available Books](#viewing-available-books)
   
4. [Running Unit Tests](#running-unit-tests)

## Getting Started

### Prerequisites

Before using the Finteq Library Management System, ensure you have the following installed on your machine:

- [Visual Studio](https://visualstudio.microsoft.com/) or any other C# development environment.
- [.NET SDK](https://dotnet.microsoft.com/download/dotnet) for running C# applications.

### Installation

1. Clone the repository to your local machine.

   ```bash
   git clone https://github.com/yourusername/Finteq-Library-Management.git
   ```

2. Open the project in Visual Studio or your preferred C# development environment.

3. Build and run the application.

## Application Features

### 1. Add a New Book

- Allows you to add a new book to the library with details such as title, author, and publication year.

### 2. Update Book Information

- Enables you to update existing book details, including the title, author, and publication year.

### 3. Delete a Book

- Allows you to remove a book from the library permanently. Use with caution.

### 4. Check Out a Book

- Lets users check out a book for borrowing. Marks the book as checked out.

### 5. Return a Book

- Allows users to return a checked-out book to the library. Marks the book as available.

### 6. View Available Books

- Displays a list of all available books in the library.

## Usage Guide

### Adding a New Book

1. Click the "Add Book" button on the main menu.
2. Enter the book's title, author, and publication year in the respective text boxes.
3. Click the "Submit" button to add the book to the library.
4. A success message will appear if the book is added successfully.

### Updating Book Information

1. Click the "Update Book" button on the main menu.
2. Enter the book's ID to search for the book you want to update.
3. Enter the new title, author, or publication year as needed.
4. Click the "Submit" button to save the changes.
5. A success message will appear if the book is updated successfully.

### Deleting a Book

1. Click the "Delete Book" button on the main menu.
2. Enter the book's ID to search for the book you want to delete.
3. Confirm the deletion in the warning prompt.
4. A success message will appear if the book is deleted successfully.

### Checking Out a Book

1. Click the "Check Out Book" button on the main menu.
2. Enter the book's ID to search for the book you want to check out.
3. Confirm the checkout in the warning prompt.
4. A success message will appear if the book is checked out successfully.

### Returning a Book

1. Click the "Return Book" button on the main menu.
2. Enter the book's ID to search for the book you want to return.
3. Confirm the return in the warning prompt.
4. A success message will appear if the book is returned successfully.

### Viewing Available Books

1. Click the "View Books" button on the main menu.
2. A list of all available books in the library will be displayed in a data grid.

## Running Unit Tests

This project includes test files for different aspects of the application. To run unit tests:

1. Open the solution in Visual Studio.
2. In the "Test Explorer" window, you will see a list of available tests.
3. Right-click on a test and select "Run" to execute the test.
4. Review the test results in the "Test Explorer" window.

Now you have a comprehensive guide on how to use the Finteq Library Management System. Enjoy managing your library efficiently!
