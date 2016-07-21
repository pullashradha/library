using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace Library
{
  public class CopyTest : IDisposable
  {
    public CopyTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_Database_EmptyAtFirst()
    {
      int result = Copy.GetAll().Count;

      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_EntriesMatch()
    {
     Copy testCopy1 = new Copy (new DateTime(2016, 7, 25), "New", 1, new DateTime(2016, 8, 25));
     Copy testCopy2 = new Copy (new DateTime(2016, 7, 25), "New", 1, new DateTime(2016, 8, 25));

      Assert.Equal(testCopy1, testCopy2);
    }
    [Fact]
    public void Test_GetAll_RetrieveAllCopies()
    {
     Copy testCopy1 = new Copy (new DateTime(2016, 7, 25), "New", 1, new DateTime(2016, 8, 25));
     Copy testCopy2 = new Copy (new DateTime(2016, 7, 25), "Worn & Torn", 2, new DateTime(2016, 8, 25));
     testCopy1.Save();
     testCopy2.Save();

      List<Copy> testList = new List<Copy> {testCopy1, testCopy2};
      List<Copy> result = Copy.GetAll();

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_SavesCopiesToDatabase()
    {
     Copy newCopy1 = new Copy (new DateTime(2016, 7, 25), "New", 1, new DateTime(2016, 8, 25));
     Copy newCopy2 = new Copy (new DateTime(2016, 7, 25), "Worn & Torn", 2, new DateTime (2016, 8, 25));
     newCopy1.Save();
     newCopy2.Save();

     int resultCount = Copy.GetAll().Count;

    Assert.Equal(2, resultCount);
    }
    [Fact]
    public void Test_Find_FindCopyInDatabase()
    {
     Copy testCopy = new Copy (new DateTime(2016, 7, 25), "New", 1, new DateTime(2016, 8, 25));
     testCopy.Save();

     Copy result = Copy.Find(testCopy.GetId());

     Assert.Equal(testCopy, result);
    }
    [Fact]
    public void Test_Update_UpdateCopyInDatabase()
    {
     Copy newCopy = new Copy (new DateTime(2016, 7, 25), "New", 1, new DateTime(2016, 8, 25));
     newCopy.Save();
     newCopy.SetCondition("Worn & Torn");
     newCopy.Update();

     Copy updatedCopy = Copy.Find(newCopy.GetId());

     Assert.Equal(newCopy.GetCondition(), updatedCopy.GetCondition());
    }
    [Fact]
    public void Test_DeleteOne_DeletesOneCopy()
    {
     Copy newCopy1 = new Copy (new DateTime(2016, 7, 25), "New", 1, new DateTime(2016, 8, 25));
     Copy newCopy2 = new Copy (new DateTime(2016, 7, 25), "Worn & Torn", 2, new DateTime(2016, 8, 25));
     newCopy1.Save();
     newCopy2.Save();
     List<Copy> newList = Copy.GetAll();

     newCopy1.DeleteOne();

     List<Copy> resultList = Copy.GetAll();
     List<Copy> testList = new List<Copy> {newCopy2};

      Assert.Equal(testList, resultList);
    }
    public void Dispose()
    {
     Copy.DeleteAll();
    }
  }
}
