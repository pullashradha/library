using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Copy
  {
    private int _id;
    private string _condition;
    private int _bookId;
    public Book(string Condition, int BookId, int Id = 0)
    {
      _id = Id;
      _condition = Condition;
      _bookId = BookId;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetCondition()
    {
      return _condition;
    }
    public void SetCondition (string NewCondition)
    {
      _condition = NewCondition;
    }
    public int GetBookId()
    {
      return _bookId;
    }
    public void SetBookId (int newBookId)
    {
      _bookId = newBookId;
    }
    public override bool Equals (System.Object otherCopy)
    {
      if (otherCopy is Copy)
      {
        Copy testCopy = (Copy) otherCopy;
        bool idEquality = (this.GetId() == testCopy.GetId());
        bool conditionEquality = (this.GetCondition() == testCopy.GetCondition());
        bool bookIdEquality = (this.GetBookId() == testCopy.GetBookId());
        return (idEquality && conditionEquality && bookIdEquality);
      }
      else
      {
        return false;
      }
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("INSERT INTO copies (condition, book_id) OUTPUT INSERTED.id VALUES (@CopyCondition, @BookId);", conn);
      SqlParameter conditionParameter = new SqlParameter ();
      conditionParameter.ParameterName = "@CopyCondition";
      conditionParameter.Value = this.GetCondition();
      SqlParameter bookIdParameter = new SqlParameter ();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetBookId();
      cmd.Parameters.Add(conditionParameter);
      cmd.Parameters.Add(bookIdParameter);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static List<Copy> GetAll()
    {
      List<Copy> allCopies = new List<Copy> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("SELECT * FROM copies;", conn);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        string copyCondition = rdr.GetString(1);
        int bookId = rdr.GetInt32(2);
        Copy newCopy = new Copy (copyCondition, bookId, copyId);
        allCopys.Add(newCopy);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCopies;
    }
    public static Copy Find (int searchId)
    {
      Copy foundCopy = new Copy(""); //Program needs some value inside a Copy object
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE id = @CopyId;", conn);
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@CopyId";
      idParameter.Value = searchId;
      cmd.Parameters.Add(idParameter);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        string copyCondition = rdr.GetString(1);
        int bookId = rdr.GetInt32(2);
        Copy newCopy = new Copy(copyCondition, bookId, copyId);
        foundCopy = newCopy;
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCopy;
    }
    public void Update()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("UPDATE copies SET condition = @CopyCondition WHERE id = @SearchId; UPDATE copies SET book_id = @BookId WHERE id = @SearchId;", conn);
      SqlParameter newConditionParameter = new SqlParameter();
      newConditionParameter.ParameterName = "@CopyCondition";
      newConditionParameter.Value = this.GetTitle();
      SqlParameter newBookIdParameter = new SqlParameter();
      newBookIdParameter.ParameterName = "@BookId";
      newBookIdParameter.Value = this.GetBookId();
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@SearchId";
      idParameter.Value = this.GetId();
      cmd.Parameters.Add(newConditionParameter);
      cmd.Parameters.Add(newBookIdParameter);
      cmd.Parameters.Add(idParameter);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public void DeleteOne()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand ("DELETE FROM copies WHERE id = @Copyd;", conn);
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@Copyd";
      idParameter.Value = this.GetId();
      cmd.Parameters.Add(idParameter);
      cmd.ExecuteNonQuery();
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand ("DELETE FROM copies;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
