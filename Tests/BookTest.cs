using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace Library
{
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_Database_EmptyAtFirst()
    {
      int result = Book.GetAll().Count;

      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_EntriesMatch()
    {
      Book testBook1 = new Book ("Redwall");
      Book testBook2 = new Book ("Redwall");

      Assert.Equal(testBook1, testBook2);
    }
    [Fact]
    public void Test_GetAll_RetrievesAllBooks()
    {
      Book testBook1 = new Book ("Redwall");
      Book testBook2 = new Book ("Memnoch the Devil");
      testBook1.Save();
      testBook2.Save();

      List<Book> testList = new List<Book> {testBook1, testBook2};
      List<Book> result = Book.GetAll();

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_SavesBooksToDatabase()
    {
      Book newBook1 = new Book ("Redwall");
      Book newBook2 = new Book ("Memnoch the Devil");
      newBook1.Save();
      newBook2.Save();

      int resultCount = Book.GetAll().Count;

      Assert.Equal(2, resultCount);
    }
    [Fact]
    public void Test_AddAuthor_AddsAuthorToBook()
    {
      Book newBook = new Book ("Redwall");
      newBook.Save();
      Author newAuthor = new Author ("Brian Jacques");
      newAuthor.Save();
      newBook.AddAuthor(newAuthor);

      List<Author> testAuthor = new List<Author> {newAuthor};
      List<Author> allAuthors = newBook.GetAuthors();

      Assert.Equal(testAuthor, allAuthors);
    }
    [Fact]
    public void Test_GetCopies_ReturnsAllCopiesForBook()
    {
      Book testBook = new Book ("Redwall");
      testBook.Save();
      Copy newCopy1 = new Copy ("New", testBook.GetId(), new DateTime(2016, 7, 25), new DateTime(2016, 8, 25));
      Copy newCopy2 = new Copy ("Worn & Torn", testBook.GetId(), new DateTime(2016, 7, 25), new DateTime (2016, 8, 25));
      newCopy1.Save();
      newCopy2.Save();

      List<Copy> testCopyList = new List<Copy> {newCopy1, newCopy2};
      List<Copy> resultCopyList = testBook.GetCopies();

      Assert.Equal(testCopyList, resultCopyList);
    }
    [Fact]
    public void Test_Find_FindsBookInDatabase()
    {
      Book testBook = new Book ("Redwall");
      testBook.Save();

      Book result = Book.Find(testBook.GetId());

      Assert.Equal(testBook, result);
    }
    [Fact]
    public void Test_FindByAuthor_FindsBookByAuthor()
    {
      Book testBook = new Book ("Redwall");
      testBook.Save();

      Book resultBook = Book.FindByAuthor(testBook.GetAuthors());

      Assert.Equal(testBook, resultBook);
    }
    [Fact]
    public void Test_FindByTitle_FindsBookByTitle()
    {
      Book testBook = new Book ("Redwall");
      testBook.Save();

      Book resultBook = Book.FindByTitle(testBook.GetTitle());

      Assert.Equal(testBook, resultBook);
    }
    [Fact]
    public void Test_Update_UpdatesBookInDatabase()
    {
      Book newBook = new Book ("Redwall");
      newBook.Save();
      newBook.SetTitle("Redwall: A Journey");
      newBook.Update();

      Book updatedBook = Book.Find(newBook.GetId());

      Assert.Equal(newBook.GetTitle(), updatedBook.GetTitle());
    }
    [Fact]
    public void Test_DeleteOne_DeletesOneBook()
    {

      Book newBook1 = new Book ("Redwall");
      Book newBook2 = new Book ("Memnoch the Devil");
      newBook1.Save();
      newBook2.Save();
      List<Book> newList = Book.GetAll();

      newBook1.DeleteOne();

      List<Book> resultList = Book.GetAll();
      List<Book> testList = new List<Book> {newBook2};

      Assert.Equal(testList, resultList);
    }
    public void Dispose()
    {
      Copy.DeleteAll();
      Author.DeleteAll();
      Book.DeleteAll();
    }
  }
}
