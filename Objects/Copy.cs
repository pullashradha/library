using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Copy
  {
    private int _id;
    private DateTime? _checkoutDate;
    private string _condition;
    private int _bookId;
    private DateTime? _dueDate;

    public Copy (DateTime? CheckoutDate, string Condition, int BookId, DateTime? DueDate, int Id = 0)
    {
      _id = Id;
      _checkoutDate = CheckoutDate;
      _condition = Condition;
      _bookId = BookId;
      _dueDate = DueDate;
    }
    public int GetId()
    {
      return _id;
    }
    public DateTime? GetCheckoutDate()
    {
      return _checkoutDate;
    }
    public void SetCheckoutDate (DateTime? newCheckoutDate)
    {
      _checkoutDate = newCheckoutDate;
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
    public DateTime? GetDueDate()
    {
      return _dueDate;
    }
    public void SetDueDate (DateTime? newDueDate)
    {
      _dueDate = newDueDate;
    }
    public override bool Equals (System.Object otherCopy)
    {
      if (otherCopy is Copy)
      {
        Copy testCopy = (Copy) otherCopy;
        bool idEquality = (this.GetId() == testCopy.GetId());
        bool checkoutDateEquality = (this.GetCheckoutDate() == testCopy.GetCheckoutDate());
        bool conditionEquality = (this.GetCondition() == testCopy.GetCondition());
        bool bookIdEquality = (this.GetBookId() == testCopy.GetBookId());
        bool dueDateEquality = (this.GetDueDate() == testCopy.GetDueDate());

        return (idEquality && checkoutDateEquality && conditionEquality && bookIdEquality && dueDateEquality);
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

      SqlCommand cmd = new SqlCommand ("INSERT INTO copies (checkout_date, condition, book_id, due_date) OUTPUT INSERTED.id VALUES (@CheckoutDate, @CopyCondition, @BookId, @DueDate);", conn);

      SqlParameter checkoutDateParameter = new SqlParameter ();
      checkoutDateParameter.ParameterName = ("@CheckoutDate");
      checkoutDateParameter.Value = this.GetCheckoutDate();

      SqlParameter conditionParameter = new SqlParameter ();
      conditionParameter.ParameterName = "@CopyCondition";
      conditionParameter.Value = this.GetCondition();

      SqlParameter bookIdParameter = new SqlParameter ();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetBookId();

      SqlParameter dueDateParameter = new SqlParameter ();
      dueDateParameter.ParameterName = "@DueDate";
      dueDateParameter.Value = this.GetDueDate();

      cmd.Parameters.Add(checkoutDateParameter);
      cmd.Parameters.Add(conditionParameter);
      cmd.Parameters.Add(bookIdParameter);
      cmd.Parameters.Add(dueDateParameter);

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
        DateTime? copyCheckoutDate = rdr.GetDateTime(1);
        string copyCondition = rdr.GetString(2);
        int bookId = rdr.GetInt32(3);
        DateTime? copyDueDate = rdr.GetDateTime(4);
        Copy newCopy = new Copy (copyCheckoutDate, copyCondition, bookId, copyDueDate, copyId);

        allCopies.Add(newCopy);
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
      Copy foundCopy = new Copy("", 0); //Program needs some value inside a Copy object
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("SELECT * FROM copies WHERE id = @CopyId;", conn);

      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@CopyId";
      idParameter.Value = searchId;

      cmd.Parameters.Add(idParameter);

      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        DateTime? copyCheckoutDate = rdr.GetDateTime(1);
        string copyCondition = rdr.GetString(2);
        int bookId = rdr.GetInt32(3);
        DateTime? copyDueDate = rdr.GetDateTime(4);
        Copy newCopy = new Copy (copyCheckoutDate, copyCondition, bookId, copyDueDate, copyId);

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

      SqlCommand cmd = new SqlCommand ("UPDATE copies SET checkout_date = @CheckoutDate WHERE id = @SearchId; UPDATE copies SET condition = @CopyCondition WHERE id = @SearchId; UPDATE copies SET book_id = @BookId WHERE id = @SearchId; UPDATE copies SET due_date = @DueDate WHERE id = @SearchId;", conn);

      SqlParameter newCheckoutDateParameter = new SqlParameter();
      newCheckoutDateParameter.ParameterName = "@CheckoutDate";
      newCheckoutDateParameter.Value = this.GetCheckoutDate();

      SqlParameter newConditionParameter = new SqlParameter();
      newConditionParameter.ParameterName = "@CopyCondition";
      newConditionParameter.Value = this.GetCondition();

      SqlParameter newBookIdParameter = new SqlParameter();
      newBookIdParameter.ParameterName = "@BookId";
      newBookIdParameter.Value = this.GetBookId();

      SqlParameter newDueDateParameter = new SqlParameter();
      newDueDateParameter.ParameterName = "@DueDate";
      newDueDateParameter.Value = this.GetDueDate();

      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@SearchId";
      idParameter.Value = this.GetId();

      cmd.Parameters.Add(newCheckoutDateParameter);
      cmd.Parameters.Add(newConditionParameter);
      cmd.Parameters.Add(newBookIdParameter);
      cmd.Parameters.Add(newDueDateParameter);
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
