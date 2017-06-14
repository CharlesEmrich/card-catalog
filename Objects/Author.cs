using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace CardCatalog.Objects
{
  public class Author
  {
    public int Id  { get; set; }
    public string Name { get; set; }

    public Author(string name, int id = 0)
    {
      Id = id;
      Name = name;
    }
    public override bool Equals(System.Object otherAuthor)
    {
        if (!(otherAuthor is Author))
        {
          return false;
        }
        else
        {
          Author newAuthor = (Author) otherAuthor;
          bool idEquality = (this.Id == newAuthor.Id);
          bool nameEquality = (this.Name == newAuthor.Name);
          return (idEquality && nameEquality);
        }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES (@AuthorName);", conn);

      SqlParameter nameParameter = new SqlParameter("@AuthorName", this.Name);
      cmd.Parameters.Add(nameParameter);
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
    public static List<Author> GetAll()
    {
      List<Author> allCities = new List<Author>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author newAuthor = new Author(authorName, authorId);
        allCities.Add(newAuthor);
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

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
