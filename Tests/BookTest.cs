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
      Book testBook1 = new Book("Redwall");
      Book testBook2 = new Book("Redwall");

      Assert.Equal(testBook1, testBook2);
    }
    [Fact]
    public void Test_GetAll_RetrieveAllBooks()
    {
      Book testBook1 = new Book("Redwall");
      Book testBook2 = new Book("Memnoch the Devil");
      testBook1.Save();
      testBook2.Save();

      List<Book> testList = new List<Book>{testBook1, testBook2};
      List<Book> result = Book.GetAll();

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_SavesBooksToDatabase()
    {
      Book newBook1 = new Book("Redwall");
      Book newBook2 = new Book("Memnoch the Devil");
      newBook1.Save();
      newBook2.Save();

      int resultCount = Book.GetAll().Count;

      Assert.Equal(2, resultCount);
    }
    [Fact]
    public void Test_AddAuthor_AddAnAuthorToABook()
    {
      Book newBook = new Book("Redwall");
      newBook.Save();
      Author newAuthor = new Author("Brian Jacques");
      newAuthor.Save();
      newBook.AddAuthor(newAuthor);

      List<Author> testAuthor = new List<Author> {newAuthor};
      List<Author> allAuthors = newBook.GetAuthors();

      Assert.Equal(testAuthor, allAuthors);
    }
    [Fact]
    public void Test_Find_FindBookInDatabase()
    {
      Book testBook1 = new Book("Redwall");
      testBook1.Save();

      Book result = Book.Find(testBook1.GetId());

      Assert.Equal(testBook1, result);
    }
    [Fact]
    public void Test_Update_UpdateBookInDatabase()
    {
      Book newBook = new Book("Redwall");
      newBook.Save();
      newBook.SetTitle("Redwall!!");
      newBook.Update();

      Book updatedBook = Book.Find(newBook.GetId());

      Assert.Equal(newBook.GetTitle(), updatedBook.GetTitle());
    }
    [Fact]
    public void Test_DeleteOne_DeletesOneBook()
    {

      Book newBook1 = new Book("Redwall");
      Book newBook2 = new Book("Memnoch the Devil");
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
      Author.DeleteAll();
      Book.DeleteAll();
    }
  }
}
