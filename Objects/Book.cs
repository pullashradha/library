using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Book
  {
    private int _id;
    private string _title;
    public Book(string Title, int Id = 0)
    {
      _id = Id;
      _title = Title;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetTitle()
    {
      return _title;
    }
    public void SetTitle(string NewTitle)
    {
      _title = NewTitle;
    }
    public override bool Equals(System.Object otherBook)
    {
      if (otherBook is Book)
      {
        Book testBook = (Book) otherBook;
        bool idEquality = (this.GetId() == testBook.GetId());
        bool titleEquality = (this.GetTitle() == testBook.GetTitle());
        return (idEquality && titleEquality);
      }
      else
      {
        return false;
      }
    }
    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("SELECT * FROM books;", conn);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        Book newBook = new Book (bookTitle, bookId);
        allBooks.Add(newBook);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allBooks;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("INSERT INTO books (title) OUTPUT INSERTED.id VALUES (@BookTitle);", conn);
      SqlParameter titleParameter = new SqlParameter ();
      titleParameter.ParameterName = "@BookTitle";
      titleParameter.Value = this.GetTitle();
      cmd.Parameters.Add(titleParameter);
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
    public static Book Find(string findTitle)
    {
      Book foundBook = new Book(""); //Program needs some value inside a Book object
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE title = @BookTitle;", conn);
      SqlParameter titleParameter = new SqlParameter();
      titleParameter.ParameterName = "@BookTitle";
      titleParameter.Value = findTitle;
      cmd.Parameters.Add(titleParameter);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        Book newBook = new Book(bookTitle, bookId);
        foundBook = newBook;
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBook;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand ("DELETE FROM books;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
