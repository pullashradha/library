using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Book
  {
    private int _id;
    private string _title;

    public Book (string Title, int Id = 0)
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
    public void SetTitle (string NewTitle)
    {
      _title = NewTitle;
    }
    public override bool Equals (System.Object otherBook)
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
    public void AddAuthor (Author newAuthor)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand ("INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);", conn);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = newAuthor.GetId();

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();

      cmd.Parameters.Add(authorIdParameter);
      cmd.Parameters.Add(bookIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Author> GetAuthors()
    {
      List<Author> allAuthors = new List<Author> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("SELECT authors.* FROM books JOIN authors_books ON (books.id = authors_books.book_id) JOIN authors ON (authors.id = authors_books.author_id) WHERE books.id = @BookId;", conn);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();

      cmd.Parameters.Add(bookIdParameter);

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
    public List<Copy> GetCopies()
    {
      List<Copy> allCopies = new List<Copy> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("SELECT * FROM copies WHERE book_id = @BookId;", conn);

      SqlParameter bookIdParameter = new SqlParameter ();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();

      cmd.Parameters.Add(bookIdParameter);

      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        DateTime copyCheckoutDate = rdr.GetDateTime(1);
        string copyCondition = rdr.GetString(2);
        int copyBookId = rdr.GetInt32(3);
        DateTime copyDueDate = rdr.GetDateTime(4);

        Copy newCopy = new Copy (copyCondition, copyBookId, copyCheckoutDate, copyDueDate, copyId);

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
    public static Book Find (int searchId)
    {
      Book foundBook = new Book(""); //Program needs some value inside a Book object
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("SELECT * FROM books WHERE id = @BookId;", conn);

      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@BookId";
      idParameter.Value = searchId;

      cmd.Parameters.Add(idParameter);

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
    public static List<Book> FindByAuthor (string searchAuthor)
    {
      List<Book> resultList = new List<Book> {};
      string bareAuthor = "";
      foreach (char i in searchAuthor)
      {
        if (!char.IsPunctuation(i))
        {
          bareAuthor += i;
        }
      }
      string[] bareCharArray = bareAuthor.Split(' ');
      // Search by This //
      string bareSearchAuthor = "";

      foreach (string character in bareCharArray)
      {
        bareSearchAuthor += character.ToLower();
      }
      List<Book> allBooks = Book.GetAll();
      foreach (Book currentBook in allBooks)
      {
        List<Author> currentBookAuthors = currentBook.GetAuthors();
        List<string> searchNames = new List<string> {};
        foreach (Author currentAuthor in currentBookAuthors)
        {
          string[] bareAuthorString = currentAuthor.GetName().ToLower().Split(' ');
          string bareSearchName = "";
          foreach (string singleAuthor in bareAuthorString)
          {
            bareSearchName += singleAuthor;
          }
          searchNames.Add(bareSearchName);
        }
        for (var i = 0; i < searchNames.Count; i++)
        {
          List<Book> tempList = new List<Book> {};
          if (searchNames[i].Contains(bareSearchAuthor))
          {
            tempList = currentBookAuthors[i].GetBooks();
            foreach (Book currentBookInList in tempList)
            {
              resultList.Add(currentBookInList);
            }
          }
        }
      }
      return resultList;
    }
    public void Update()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("UPDATE books SET title = @BookTitle WHERE id = @SearchId;", conn);

      SqlParameter newTitleParameter = new SqlParameter();
      newTitleParameter.ParameterName = "@BookTitle";
      newTitleParameter.Value = this.GetTitle();

      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@SearchId";
      idParameter.Value = this.GetId();

      cmd.Parameters.Add(newTitleParameter);
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

      SqlCommand cmd = new SqlCommand ("DELETE FROM books WHERE id = @BookId;", conn);

      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@BookId";
      idParameter.Value = this.GetId();

      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
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
