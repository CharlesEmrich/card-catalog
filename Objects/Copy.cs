using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace CardCatalog.Objects
{
  public class Copy
  {
    public int Id { get; set; }
    public int BookId { get; set; }
    public DateTime DueDate { get; set; }

    public static DateTime DefaultDate()
    {
      return new DateTime(9999, 12, 31);
    }

    public Copy(int bookId, DateTime dueDate, int id = 0)
    {
      Id = id;
      BookId = bookId;
      DueDate = dueDate;
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
          bool dueDateEquality = (this.DueDate == newCopy.DueDate);
          return (idEquality && dueDateEquality);
        }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO copies (book_id, due_date) OUTPUT INSERTED.id VALUES (@BookId, @DueDate);", conn);

      SqlParameter idParameter = new SqlParameter("@BookId", this.BookId);
      cmd.Parameters.Add(idParameter);
      SqlParameter dueDateParameter = new SqlParameter("@DueDate", this.DueDate);
      cmd.Parameters.Add(dueDateParameter);
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
        DateTime dueDate = Convert.ToDateTime(rdr.GetString(2));
        Copy newCopy = new Copy(bookId, dueDate, copyId);
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
    // NOTE: Go into checkouts, match copy's id with a row in checkouts, and if there is a match, then grab the due date associated with that copyId. For ease, maybe also grab the patron.
    // public static List<Copy> Search(string searchString)
    // {
    // }

    public void Checkout(int patronId)
    {
      if (this.DueDate != null)
      {
        SqlConnection conn = DB.Connection();
        conn.Open();

        DateTime newDueDate = DateTime.Today;
        newDueDate = newDueDate.AddDays(7).AddHours(17);
        this.DueDate = newDueDate;

        SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (patron_id, copy_id) VALUES (@PatronId, @CopyId);", conn);

        SqlParameter patronIdParameter = new SqlParameter("@PatronId", patronId);
        cmd.Parameters.Add(patronIdParameter);
        SqlParameter copyIdParameter = new SqlParameter("@CopyId", this.Id);
        cmd.Parameters.Add(copyIdParameter);

        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }
      // NOTE: How do we make sure that when this runs it's running on a copy that is currently in the library? Will the addition of an isCheckedOut bool be necessary?
    }

    public void Checkin()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      this.DueDate = Copy.DefaultDate();

      SqlCommand cmd = new SqlCommand("DELETE FROM checkouts WHERE copy_id = @CopyId;", conn);

      SqlParameter copyIdParameter = new SqlParameter("@CopyId", this.Id);
      cmd.Parameters.Add(copyIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public bool IsCheckedOut()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT copy_id FROM checkouts WHERE copy_id = @CopyId;", conn); //use try/catch and checked/unchecked? ExecuteScalar?

      SqlParameter copyIdParameter = new SqlParameter("@CopyId", this.Id);
      cmd.Parameters.Add(copyIdParameter);

      object result = cmd.ExecuteScalar();
      if (conn != null)
      {
        conn.Close();
      }

      if (result != null)
      {
        return true;
      }
      else
      {
        return false;
      }
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
