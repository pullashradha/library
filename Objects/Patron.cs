using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Patron
  {
    private int _id;
    private string _firstName;
    private string _lastName;
    private string _phoneNumber;

    public Patron (string FirstName, string LastName, string PhoneNumber, int Id = 0)
    {
      _id = Id;
      _firstName = FirstName;
      _lastName = LastName;
      _phoneNumber = PhoneNumber;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetFirstName()
    {
      return _firstName;
    }
    public void SetFirstName (string NewFirstName)
    {
      _firstName = NewFirstName;
    }
    public string GetLastName()
    {
      return _lastName;
    }
    public void SetLastName (string NewLastName)
    {
      _lastName = NewLastName;
    }
    public string GetPhoneNumber()
    {
      return _phoneNumber;
    }
    public void SetPhoneNumber (string NewPhoneNumber)
    {
      _phoneNumber = NewPhoneNumber;
    }
    public override bool Equals (System.Object otherPatron)
    {
      if (otherPatron is Patron)
      {
        Patron testPatron = (Patron) otherPatron;
        bool idEquality = (this.GetId() == testPatron.GetId());
        bool firstNameEquality = (this.GetFirstName() == testPatron.GetFirstName());
        bool lastNameEquality = (this.GetLastName() == testPatron.GetLastName());
        bool phoneNumberEquality = (this.GetPhoneNumber() == testPatron.GetPhoneNumber());

        return (idEquality && firstNameEquality && lastNameEquality && phoneNumberEquality);
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

      SqlCommand cmd = new SqlCommand ("INSERT INTO patrons (first_name, last_name, phone_number) OUTPUT INSERTED.id VALUES (@PatronFirstName, @PatronLastName, @PatronPhoneNumber);", conn);

      SqlParameter firstNameParameter = new SqlParameter ();
      firstNameParameter.ParameterName = "@PatronFirstName";
      firstNameParameter.Value = this.GetFirstName();

      SqlParameter lastNameParameter = new SqlParameter ();
      lastNameParameter.ParameterName = "@PatronLastName";
      lastNameParameter.Value = this.GetLastName();

      SqlParameter phoneNumberParameter = new SqlParameter ();
      phoneNumberParameter.ParameterName = "@PatronPhoneNumber";
      phoneNumberParameter.Value = this.GetPhoneNumber();

      cmd.Parameters.Add(firstNameParameter);
      cmd.Parameters.Add(lastNameParameter);
      cmd.Parameters.Add(phoneNumberParameter);

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
    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("SELECT * FROM patrons;", conn);

      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronFirstName = rdr.GetString(1);
        string patronLastName = rdr.GetString(2);
        string phoneNumber = rdr.GetString(3);
        Patron newPatron = new Patron (patronFirstName, patronLastName, phoneNumber, patronId);

        allPatrons.Add(newPatron);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allPatrons;
    }
    public void AddCopy (Copy newCopy)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand ("INSERT INTO checkouts (copy_id, patron_id) VALUES (@CopyId, @PatronId);", conn);

      SqlParameter copyIdParameter = new SqlParameter();
      copyIdParameter.ParameterName = "@CopyId";
      copyIdParameter.Value = newCopy.GetId();

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetId();

      cmd.Parameters.Add(copyIdParameter);
      cmd.Parameters.Add(patronIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Copy> GetCopies()
    {
      List<Copy> allCopies = new List<Copy> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("SELECT copies.* FROM copies JOIN checkouts ON (copies.id = checkouts.copy_id) JOIN patrons ON (patrons.id = checkouts.patron_id) WHERE patrons.id = @PatronId;", conn);

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetId();

      cmd.Parameters.Add(patronIdParameter);

      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        DateTime copyCheckoutDate = rdr.GetDateTime(1);
        string copyCondition = rdr.GetString(2);
        int patronId = rdr.GetInt32(3);
        DateTime copyDueDate = rdr.GetDateTime(4);
        Copy newCopy = new Copy (copyCondition, patronId, copyCheckoutDate, copyDueDate, copyId);
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
    public static Patron Find (int searchId)
    {
      Patron foundPatron = new Patron ("", "", ""); //Program needs some value inside a Patron object
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("SELECT * FROM patrons WHERE id = @PatronId;", conn);

      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@PatronId";
      idParameter.Value = searchId;

      cmd.Parameters.Add(idParameter);

      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronFirstName = rdr.GetString(1);
        string patronLastName = rdr.GetString(2);
        string patronPhoneNumber = rdr.GetString(3);
        Patron newPatron = new Patron(patronFirstName, patronLastName, patronPhoneNumber, patronId);

        foundPatron = newPatron;
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundPatron;
    }
    public void Update()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("UPDATE patrons SET first_name = @PatronFirstName WHERE id = @SearchId; UPDATE patrons SET last_name = @PatronLastName WHERE id = @SearchId; UPDATE patrons SET phone_number = @PatronPhoneNumber WHERE id = @SearchId;", conn);

      SqlParameter newFirstNameParameter = new SqlParameter ();
      newFirstNameParameter.ParameterName = "@PatronFirstName";
      newFirstNameParameter.Value = this.GetFirstName();

      SqlParameter newLastNameParameter = new SqlParameter ();
      newLastNameParameter.ParameterName = "@PatronLastName";
      newLastNameParameter.Value = this.GetLastName();

      SqlParameter newPhoneNumberParameter = new SqlParameter ();
      newPhoneNumberParameter.ParameterName = "@PatronPhoneNumber";
      newPhoneNumberParameter.Value = this.GetPhoneNumber();

      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@SearchId";
      idParameter.Value = this.GetId();

      cmd.Parameters.Add(newFirstNameParameter);
      cmd.Parameters.Add(newLastNameParameter);
      cmd.Parameters.Add(newPhoneNumberParameter);
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

      SqlCommand cmd = new SqlCommand ("DELETE FROM patrons WHERE id = @PatronId;", conn);

      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@PatronId";
      idParameter.Value = this.GetId();

      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand ("DELETE FROM patrons;", conn);

      cmd.ExecuteNonQuery();
    }
  }
}
