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
      Copy firstCopy = new Copy(newBook.Id, Copy.DefaultDate());
      Copy secondCopy = new Copy(newBook.Id, Copy.DefaultDate());
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
      Copy newCopy = new Copy(1, Copy.DefaultDate());
      newCopy.Save();
      //Act
      List<Copy> result = Copy.GetAll();
      List<Copy> testList = new List<Copy>{newCopy};
      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Copy_Checkout_UpdatesDueDateInFront()
    {
      //Arrange
      Book newBook = new Book("Confessions of a Mask");
      newBook.Save();
      Copy firstCopy = new Copy(newBook.Id, Copy.DefaultDate());
      //Act
      firstCopy.Checkout(1);
      DateTime expected = DateTime.Today;
      expected = expected.AddDays(7).AddHours(17);
      //Assert
      Assert.Equal(expected, firstCopy.DueDate);
    }
    [Fact]
    public void Copy_Checkin_UpdatesDueDateInFront()
    {
      //Arrange
      Book newBook = new Book("Confessions of a Mask");
      newBook.Save();
      Copy firstCopy = new Copy(newBook.Id, Copy.DefaultDate());
      firstCopy.Checkout(1);
      //Act
      firstCopy.Checkin();

      DateTime expected = Copy.DefaultDate();
      //Assert
      Assert.Equal(expected, firstCopy.DueDate);
    }
    [Fact]
    public void Copy_CheckIn_UpdatesDueDateInBack()
    {
      //Arrange
      Book newBook = new Book("Confessions of a Mask");
      newBook.Save();
      Copy firstCopy = new Copy(newBook.Id, Copy.DefaultDate());
      firstCopy.Checkout(1);
      //Act
      firstCopy.Checkin();

      bool actual = firstCopy.IsCheckedOut();
      bool expected = false;
      //Assert
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void Copy_Checkout_UpdatesDueDateInBack()
    {
      //Arrange
      Book newBook = new Book("Confessions of a Mask");
      newBook.Save();
      Copy firstCopy = new Copy(newBook.Id, Copy.DefaultDate());
      firstCopy.Save();
      firstCopy.Checkout(1);
      //Act
      bool actual = firstCopy.IsCheckedOut();
      bool expected = true;

      //Assert
      Assert.Equal(expected, actual);
    }

  }
}
