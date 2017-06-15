using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace CardCatalog.Objects
{
  public class Copy
  {
    public int Id { get; set; }
    public int BookId { get; set; }

    public Copy(int bookId, int id = 0)
    {
      Id = id;
      BookId = bookId;
    }
    //TODO: Copy needs, at minimum, a GetAll so librarians can view the whole catalog at once. Probably also a view that shows due dates. The Copy constructor is probably a complex creature that builds itself out of the book, the listed authors, and information about checkout status. Should this class mostly exist as something that Copy inherits from? It would save the param on AddCopy and Checkout/Checkin
    //NOTE: I want to store things like string Title and List<string> authors on each copy, but doing so will make them remarkably intolerant of any changes made to those things by later movements in the backend.
    public override bool Equals(System.Object otherCopy)
    {
        if (!(otherCopy is Copy))
        {
          return false;
        }
        else
        {
          Copy newCopy = (Copy) otherCopy;
          bool idEquality = (this.Id == newCopy.Id);
          return (idEquality);
        }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO copies (book_id) OUTPUT INSERTED.id VALUES (@BookId);", conn);

      SqlParameter idParameter = new SqlParameter("@BookId", this.BookId);
      cmd.Parameters.Add(idParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Id = rdr.GetInt32(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static List<Copy> GetAll()
    {
      List<Copy> allCopies = new List<Copy>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        int bookId = rdr.GetInt32(1);
        Copy newCopy = new Copy(bookId, copyId);
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
    
    // public static List<Copy> Search(string searchString)
    // {
    // }

    public void Checkout(int patronId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      DateTime dueDate = DateTime.Today;
      dueDate = dueDate.AddDays(7.00).AddHours(17.00);

      SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (patron_id, copy_id, due_date) VALUES (@PatronId, @CopyId, @DueDate);", conn);

      SqlParameter patronIdParameter = new SqlParameter("@PatronId", patronId);
      cmd.Parameters.Add(patronIdParameter);
      SqlParameter copyIdParameter = new SqlParameter("@CopyId", this.Id);
      cmd.Parameters.Add(copyIdParameter);
      SqlParameter dueDateIdParameter = new SqlParameter("@DueDate", dueDate);
      cmd.Parameters.Add(dueDateIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
      // NOTE: How do we make sure that when this runs it's running on a copy that is currently in the library? Will the addition of an isCheckedOut bool be necessary?
    }

    public void Checkin()
    {
      //NOTE: Don't forget to write this!
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM copies;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
