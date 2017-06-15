using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace CardCatalog.Objects
{
  public class Copy
  {
    public string Title { get; set; }

    public Copy(Book book)
    {

    }
    //TODO: Copy needs, at minimum, a GetAll so librarians can view the whole catalog at once. Probably also a view that shows due dates. The Copy constructor is probably a complex creature that builds itself out of the book, the listed authors, and information about checkout status. Should this class mostly exist as something that Book inherits from? It would save the param on AddCopy and Checkout/Checkin
    //NOTE: I want to store things like string Title and List<string> authors on each copy, but doing so will make them remarkably intolerant of any changes made to those things by later movements in the backend.
    public void Checkout()
    {
      // NOTE: How do we make sure that when this runs it's running on a copy that is currently in the library? Will the addition of an isCheckedOut bool be necessary?
    }
    public void Checkin()
    {
      //NOTE: Don't forget to write this!
    }
    public void AddCopy(Book book)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO copies (book_id) VALUES (@BookId);", conn);

      SqlParameter bookIdParameter = new SqlParameter("@BookId", book.Id);
      cmd.Parameters.Add(bookIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
