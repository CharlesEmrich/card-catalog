using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace CardCatalog.Objects
{
  public class Book
  {
    public int Id  { get; set; }
    public string Title { get; set; }

    public Book(string title, int id = 0)
    {
      Id = id;
      Title = title;
    }
    public override bool Equals(System.Object otherBook)
    {
        if (!(otherBook is Book))
        {
          return false;
        }
        else
        {
          Book newBook = (Book) otherBook;
          bool idEquality = (this.Id == newBook.Id);
          bool titleEquality = (this.Title == newBook.Title);
          return (idEquality && titleEquality);
        }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (title) OUTPUT INSERTED.id VALUES (@BookTitle);", conn);

      SqlParameter titleParameter = new SqlParameter("@BookTitle", this.Title);
      cmd.Parameters.Add(titleParameter);
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
    public static List<Book> GetAll()
    {
      List<Book> allCities = new List<Book>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        Book newBook = new Book(bookTitle, bookId);
        allCities.Add(newBook);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCities;
    }
    public void Update(string newTitle)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE books SET title = @NewTitle OUTPUT INSERTED.title WHERE id = @BookId;", conn);
      SqlParameter newTitleParameter = new SqlParameter("@NewTitle", newTitle);
      cmd.Parameters.Add(newTitleParameter);
      SqlParameter flightIdParameter = new SqlParameter("@BookId", this.Id);
      cmd.Parameters.Add(flightIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Title = rdr.GetString(0);
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
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM books;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
