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
     Copy testCopy1 = new Copy ("New", 1);
     Copy testCopy2 = new Copy ("New", 1);

      Assert.Equal(testCopy1, testCopy2);
    }
    [Fact]
    public void Test_GetAll_RetrieveAllCopies()
    {
     Copy testCopy1 = new Copy ("New", 1);
     Copy testCopy2 = new Copy ("Worn & Torn", 2);
     testCopy1.Save();
     testCopy2.Save();

      List<Copy> testList = new List<Copy> {testCopy1, testCopy2};
      List<Copy> result = Copy.GetAll();

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_SavesCopiesToDatabase()
    {
     Copy newCopy1 = new Copy ("New", 1);
     Copy newCopy2 = new Copy ("Worn & Torn", 2);
     newCopy1.Save();
     newCopy2.Save();

     int resultCount = Copy.GetAll().Count;

    Assert.Equal(2, resultCount);
    }
    [Fact]
    public void Test_Find_FindCopyInDatabase()
    {
     Copy testCopy1 = new Copy ("New", 1);
     testCopy1.Save();

     Copy result = Copy.Find(testCopy1.GetId());

     Assert.Equal(testCopy1, result);
    }
    [Fact]
    public void Test_Update_UpdateCopyInDatabase()
    {
     Copy newCopy = new Copy ("New", 1);
     newCopy.Save();
     newCopy.SetCondition("New!!");
     newCopy.Update();

     Copy updatedCopy = Copy.Find(newCopy.GetId());

     Assert.Equal(newCopy.GetCondition(), updatedCopy.GetCondition());
    }
    [Fact]
    public void Test_DeleteOne_DeletesOneCopy()
    {

     Copy newCopy1 = new Copy ("New", 1);
     Copy newCopy2 = new Copy ("Worn & Torn", 2);
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
