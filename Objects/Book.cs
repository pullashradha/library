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
  }
}
