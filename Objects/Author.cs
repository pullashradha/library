using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Author
  {
    private int _id;
    private string _name;
    public Author(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string NewName)
    {
      _name = NewName;
    }
    public override bool Equals(System.Object otherAuthor)
    {
      if (otherAuthor is Author)
      {
        Author testAuthor = (Author) otherAuthor;
        bool idEquality = (this.GetId() == testAuthor.GetId());
        bool nameEquality = (this.GetName() == testAuthor.GetName());
        return (idEquality && nameEquality);
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
      SqlCommand cmd = new SqlCommand ("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES (@AuthorName);", conn);
      SqlParameter nameParameter = new SqlParameter ();
      nameParameter.ParameterName = "@AuthorName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
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
    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("SELECT * FROM authors;", conn);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author newAuthor = new Author (authorName, authorId);
        allAuthors.Add(newAuthor);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allAuthors;
    }
    public void AddBook(Book newBook)
    {
      SqlConnection conn = DB.Connection;
      conn.Open();
      SqlCommand cmd = new SqlCommand ("INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);", conn);
      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId();

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = newBook.GetId();

      cmd.Parameters.Add(authorIdParameter);
      cmd.Parameters.Add(bookIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public static Author Find(int searchId)
    {
      Author foundAuthor = new Author(""); //Program needs some value inside a Author object
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = @AuthorId;", conn);
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@AuthorId";
      idParameter.Value = searchId;
      cmd.Parameters.Add(idParameter);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author newAuthor = new Author(authorName, authorId);
        foundAuthor = newAuthor;
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundAuthor;
    }
    public void Update ()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("UPDATE authors SET name = @AuthorName WHERE id = @SearchId;", conn);
      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@AuthorName";
      newNameParameter.Value = this.GetName();
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@SearchId";
      idParameter.Value = this.GetId();
      cmd.Parameters.Add(newNameParameter);
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
      SqlCommand cmd = new SqlCommand ("DELETE FROM authors WHERE id = @AuthorId;", conn);
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@AuthorId";
      idParameter.Value = this.GetId();
      cmd.Parameters.Add(idParameter);
      cmd.ExecuteNonQuery();
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand ("DELETE FROM authors;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
