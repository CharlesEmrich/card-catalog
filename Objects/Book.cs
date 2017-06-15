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
      List<Book> allBooks = new List<Book>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        Book newBook = new Book(bookTitle, bookId);
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

    public static Book Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = @BookId;", conn);
      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = id.ToString();
      cmd.Parameters.Add(bookIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundBookId = 0;
      string foundBookName = null;
      while(rdr.Read())
      {
        foundBookId = rdr.GetInt32(0);
        foundBookName = rdr.GetString(1);
      }
      Book foundBook = new Book(foundBookName, foundBookId);

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

    public void AddAuthor(Author author)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);", conn);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = author.Id;
      cmd.Parameters.Add(authorIdParameter);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.Id;
      cmd.Parameters.Add(bookIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
      conn.Close();
      }
    }

    public List<Author> GetAuthors()
    {
      SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("SELECT authors.* FROM books JOIN authors_books ON (books.id = authors_books.book_id) JOIN authors ON (authors_books.author_id = authors.id)  WHERE books.id = @BookId;", conn);

     SqlParameter bookIdParameter = new SqlParameter();
     bookIdParameter.ParameterName = "@BookId";
     bookIdParameter.Value = this.Id;

     cmd.Parameters.Add(bookIdParameter);
     SqlDataReader rdr = cmd.ExecuteReader();

     List<Author> authors = new List<Author>{};

     while(rdr.Read())
     {
       int authorId = rdr.GetInt32(0);
       string authorName = rdr.GetString(1);

       Author newAuthor = new Author(authorName, authorId);
       authors.Add(newAuthor);
     }

     if (rdr != null)
     {
       rdr.Close();
     }
     if (conn != null)
     {
       conn.Close();
     }
     return authors;
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
