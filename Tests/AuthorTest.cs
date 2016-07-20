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
    public void Dispose()
    {
      // Author.DeleteAll();
    }
  }
}
