using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using CardCatalog.Objects;

namespace CardCatalog
{
  [Collection("CardCatalogTests")]
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=card_catalog_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Book.DeleteAll();
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForIdenticalObjects()
    {
      //Arrange, Act
      Book firstBook = new Book("Confessions of a Mask");
      Book secondBook = new Book("Confessions of a Mask");

      //Assert
      Assert.Equal(firstBook, secondBook);
    }
    [Fact]
    public void Test_CitiesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Book.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Save_SavesBookToDatabase()
    {
      //Arrange
      Book testBook = new Book("The Strange Tale of Panorama Island");
      testBook.Save();

      //Act
      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
   public void Test_Update_UpdatesBookTitleInDatabase()
   {
     //Arrange
     Book testBook = new Book("The Human Char");
     testBook.Save();
     string newTitle = "The Human Chair";

     //Act
     testBook.Update(newTitle);

     string result = testBook.Title;

     //Assert
     Assert.Equal(newTitle, result);
   }
  }
}
