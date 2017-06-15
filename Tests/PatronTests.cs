using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using CardCatalog.Objects;

namespace CardCatalog
{
  [Collection("CardCatalogTests")]
  public class PatronTest : IDisposable
  {
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=card_catalog_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Patron.DeleteAll();
    }
    [Fact]
    public void Patron_Equal_ReturnsTrueForIdenticalObjects()
    {
      //Arrange, Act
      Patron firstPatron = new Patron("Jimothy Twilliams");
      Patron secondPatron = new Patron("Jimothy Twilliams");
      //Assert
      Assert.Equal(firstPatron, secondPatron);
    }
    [Fact]
    public void Patron_PatronsEmptyAtFirst()
    {
      //Arrange, Act
      int result = Patron.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Patron_Save_SavesPatronToDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("Tedward Sklemp");
      testPatron.Save();
      //Act
      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron};
      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Find_FindsPatronInDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("Harry Potter");
      testPatron.Save();
      //Act
      Patron foundPatron = Patron.Find(testPatron.Id);
      //Assert
      Assert.Equal(testPatron, foundPatron);
    }
    [Fact]
    public void Patron_Update_UpdatesPatronNameInDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("Hermione Granger");
      testPatron.Save();
      string newName = "Ron Weasley";
      //Act
      testPatron.Update(newName);
      string result = testPatron.Name;
      //Assert
      Assert.Equal(newName, result);
    }
  }
}
