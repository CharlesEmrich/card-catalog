using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace CardCatalog.Objects
{
  public class Patron
  {
    public int Id  { get; set; }
    public string Name { get; set; }

    public Patron(string name, int id = 0)
    {
      Id = id;
      Name = name;
    }
    public override bool Equals(System.Object otherPatron)
    {
        if (!(otherPatron is Patron))
        {
          return false;
        }
        else
        {
          Patron newPatron = (Patron) otherPatron;
          bool idEquality = (this.Id == newPatron.Id);
          bool nameEquality = (this.Name == newPatron.Name);
          return (idEquality && nameEquality);
        }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO patrons (name) OUTPUT INSERTED.id VALUES (@PatronName);", conn);

      SqlParameter nameParameter = new SqlParameter("@PatronName", this.Name);
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

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        Patron newPatron = new Patron(patronName, patronId);
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

    // public static Author Find(int id)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = @PatronId;", conn);
    //   SqlParameter patronIdParameter = new SqlParameter();
    //   patronIdParameter.ParameterName = "@PatronId";
    //   patronIdParameter.Value = id.ToString();
    //   cmd.Parameters.Add(patronIdParameter);
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   int foundPatronId = 0;
    //   string foundPatronName = null;
    //   while(rdr.Read())
    //   {
    //     foundPatronId = rdr.GetInt32(0);
    //     foundPatronName = rdr.GetString(1);
    //   }
    //   Patron foundPatron = new Patron(foundPatronName, foundPatronId);
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return foundPatron;
    // }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
