using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace Library
{
  public class AuthorTest : IDisposable
  {
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_Database_EmptyAtFirst()
    {
      int result = Author.GetAll().Count;

      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_EntriesMatch()
    {
      Author testAuthor1 = new Author("Brian Jacques");
      Author testAuthor2 = new Author("Brian Jacques");

      Assert.Equal(testAuthor1, testAuthor2);
    }
    [Fact]
    public void Test_GetAll_RetrieveAllAuthors()
    {
      Author testAuthor1 = new Author("Brian Jacques");
      Author testAuthor2 = new Author("Anne Rice");
      testAuthor1.Save();
      testAuthor2.Save();

      List<Author> testList = new List<Author>{testAuthor1, testAuthor2};
      List<Author> result = Author.GetAll();

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_SavesAuthorsToDatabase()
    {
      Author newAuthor1 = new Author("Brian Jacques");
      Author newAuthor2 = new Author("Anne Rice");
      newAuthor1.Save();
      newAuthor2.Save();

      int resultCount = Author.GetAll().Count;

      Assert.Equal(2, resultCount);
    }
    [Fact]
    public void Test_AddBook_AddABookToAnAuthor()
    {
      Author newAuthor = new Author("Brian Jacques");
      newAuthor.Save();
      Book newBook = new Book("Redwall");
      newBook.Save();
      newAuthor.AddBook(newBook);

      List<Book> testBook = new List<Book> {newBook};
      List<Book> allBooks = newAuthor.GetBooks();

      Assert.Equal(testBook, allBooks);
    }
    [Fact]
    public void Test_Find_FindAuthorInDatabase()
    {
      Author testAuthor1 = new Author("Brian Jacques");
      testAuthor1.Save();

      Author result = Author.Find(testAuthor1.GetId());

      Assert.Equal(testAuthor1, result);
    }
    [Fact]
    public void Test_Update_UpdateAuthorInDatabase()
    {
      Author newAuthor = new Author("Brian Jacques");
      newAuthor.Save();
      newAuthor.SetName("Brian Jacques!!");
      newAuthor.Update();

      Author updatedAuthor = Author.Find(newAuthor.GetId());

      Assert.Equal(newAuthor.GetName(), updatedAuthor.GetName());
    }
    [Fact]
    public void Test_DeleteOne_DeletesOneAuthor()
    {

      Author newAuthor1 = new Author("Brian Jacques");
      Author newAuthor2 = new Author("Anne Rice");
      newAuthor1.Save();
      newAuthor2.Save();
      List<Author> newList = Author.GetAll();

      newAuthor1.DeleteOne();

      List<Author> resultList = Author.GetAll();
      List<Author> testList = new List<Author> {newAuthor2};

      Assert.Equal(testList, resultList);
    }
    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
    }
  }
}
