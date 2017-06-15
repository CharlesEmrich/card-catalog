using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using CardCatalog.Objects;

namespace CardCatalog
{
  [Collection("CardCatalogTests")]
  public class CopyTest : IDisposable
  {
    public CopyTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=card_catalog_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
      Patron.DeleteAll();
      Copy.DeleteAll();
    }
    [Fact]
    public void Copy_Equal_ReturnsTrueForIdenticalObjects()
    {
      //Arrange, Act
      Book newBook = new Book("Confessions of a Mask");
      newBook.Save();
      Copy firstCopy = new Copy(newBook.Id);
      Copy secondCopy = new Copy(newBook.Id);
      //Assert
      Assert.Equal(firstCopy, secondCopy);
    }
    [Fact]
    public void Copy_CopiesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Copy.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Copy_Save_SavesCopyToDatabase()
    {
      //Arrange
      Copy newCopy = new Copy(1);
      newCopy.Save();
      //Act
      List<Copy> result = Copy.GetAll();
      List<Copy> testList = new List<Copy>{newCopy};
      //Assert
      Assert.Equal(testList, result);
    }


  }
}
