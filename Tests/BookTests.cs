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
      Author.DeleteAll();
      Patron.DeleteAll();
    }

    [Fact]
    public void Book_Equal_ReturnsTrueForIdenticalObjects()
    {
      //Arrange, Act
      Book firstBook = new Book("Confessions of a Mask");
      Book secondBook = new Book("Confessions of a Mask");
      //Assert
      Assert.Equal(firstBook, secondBook);
    }
    [Fact]
    public void Book_CitiesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Book.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Book_Save_SavesBookToDatabase()
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
    public void Test_Find_FindsBookInDatabase()
    {
      //Arrange
      Book testBook = new Book("Shrill");
      testBook.Save();
      //Act
      Book foundBook = Book.Find(testBook.Id);
      //Assert
      Assert.Equal(testBook, foundBook);
    }
    [Fact]
    public void Book_Update_UpdatesBookTitleInDatabase()
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
    [Fact]
    public void Book_AddAuthor_AddsAuthorAssociationToBooksAuthors()
    {
      //Arrange
      Author testAuthor1 = new Author("Edogawa Ranpo");
      testAuthor1.Save();
      Author testAuthor2 = new Author("Suehiro Maruo");
      testAuthor2.Save();
      Book testBook = new Book("The Strange Tale of Panorama Island");
      testBook.Save();
      //Act
      testBook.AddAuthor(testAuthor1);
      testBook.AddAuthor(testAuthor2);
      List<Author> actual = testBook.GetAuthors();
      List<Author> expected = new List<Author> {testAuthor1, testAuthor2};
      //Assert
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void Book_Search_ReturnsBooksWithStringInTitle()
    {
      //Arrange
      Author testAuthor1 = new Author("Edogawa Ranpo");
      testAuthor1.Save();
      Author testAuthor2 = new Author("Suehiro Maruo");
      testAuthor2.Save();
      Book testBook = new Book("The Strange Tale of Panorama Island");
      testBook.Save();
      testBook.AddAuthor(testAuthor1);
      testBook.AddAuthor(testAuthor2);

      //Act
      List<Book> actual = Book.Search("Strange");
      List<Book> expected = new List<Book> {testBook};

      //Assert
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void Book_Search_ReturnsBooksWithStringInAuthors()
    {
      //Arrange
      Author testAuthor1 = new Author("Edogawa Ranpo");
      testAuthor1.Save();
      Author testAuthor2 = new Author("Suehiro Maruo");
      testAuthor2.Save();
      Book testBook = new Book("The Strange Tale of Panorama Island");
      testBook.Save();
      testBook.AddAuthor(testAuthor1);
      testBook.AddAuthor(testAuthor2);

      //Act
      List<Book> actual = Book.Search("Maruo");
      List<Book> expected = new List<Book> {testBook};

      //Assert
      Assert.Equal(expected, actual);
    }
  }
}
