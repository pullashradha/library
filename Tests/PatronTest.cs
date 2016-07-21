using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace Library
{
  public class PatronTest : IDisposable
  {
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_Database_EmptyAtFirst()
    {
      int result = Patron.GetAll().Count;

      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_EntriesMatch()
    {
      Patron testPatron1 = new Patron ("Jane", "Doe", "555-555-5555");
      Patron testPatron2 = new Patron ("Jane", "Doe", "555-555-5555");

      Assert.Equal(testPatron1, testPatron2);
    }
    [Fact]
    public void Test_GetAll_RetrieveAllPatrons()
    {
      Patron testPatron1 = new Patron ("Jane", "Doe", "555-555-5555");
      Patron testPatron2 = new Patron ("John", "Watson", "444-444-4444");
      testPatron1.Save();
      testPatron2.Save();

      List<Patron> testList = new List<Patron> {testPatron1, testPatron2};
      List<Patron> result = Patron.GetAll();

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_SavesPatronsToDatabase()
    {
      Patron newPatron1 = new Patron ("Jane", "Doe", "555-555-5555");
      Patron newPatron2 = new Patron ("John", "Watson", "444-444-4444");
      newPatron1.Save();
      newPatron2.Save();

      int resultCount = Patron.GetAll().Count;

      Assert.Equal(2, resultCount);
    }
    // [Fact]
    // public void Test_AddAuthor_AddAnAuthorToAPatron()
    // {
    //   Patron newPatron = new Patron ("Jane", "Doe", "555-555-5555");
    //   newPatron.Save();
    //   Author newAuthor = new Author ("Brian Jacques");
    //   newAuthor.Save();
    //   newPatron.AddAuthor(newAuthor);
    //
    //   List<Author> testAuthor = new List<Author> {newAuthor};
    //   List<Author> allAuthors = newPatron.GetAuthors();
    //
    //   Assert.Equal(testAuthor, allAuthors);
    // }
    [Fact]
    public void Test_Find_FindPatronInDatabase()
    {
      Patron testPatron1 = new Patron ("Jane", "Doe", "555-555-5555");
      testPatron1.Save();

      Patron result = Patron.Find(testPatron1.GetId());

      Assert.Equal(testPatron1, result);
    }
    [Fact]
    public void Test_Update_UpdatePatronInDatabase()
    {
      Patron newPatron = new Patron ("Jane", "Doe", "555-555-5555");
      newPatron.Save();
      newPatron.SetPhoneNumber ("111-111-1111");
      newPatron.Update();

      Patron updatedPatron = Patron.Find(newPatron.GetId());

      Assert.Equal(newPatron.GetPhoneNumber(), updatedPatron.GetPhoneNumber());
    }
    [Fact]
    public void Test_DeleteOne_DeletesOnePatron()
    {

      Patron newPatron1 = new Patron ("Jane", "Doe", "555-555-5555");
      Patron newPatron2 = new Patron ("John", "Watson", "444-444-4444");
      newPatron1.Save();
      newPatron2.Save();
      List<Patron> newList = Patron.GetAll();

      newPatron1.DeleteOne();

      List<Patron> resultList = Patron.GetAll();
      List<Patron> testList = new List<Patron> {newPatron2};

      Assert.Equal(testList, resultList);
    }
    public void Dispose()
    {
      Patron.DeleteAll();
    }
  }
}
