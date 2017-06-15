using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using CardCatalog.Objects;

namespace CardCatalog
{
  [Collection("CardCatalogTests")]
  public class AuthorTest : IDisposable
  {
    public AuthorTest()
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
    public void Author_Equal_ReturnsTrueForIdenticalObjects()
    {
      //Arrange, Act
      Author firstAuthor = new Author("Yukio Mishima");
      Author secondAuthor = new Author("Yukio Mishima");
      //Assert
      Assert.Equal(firstAuthor, secondAuthor);
    }
    [Fact]
    public void Author_AuthorsEmptyAtFirst()
    {
      //Arrange, Act
      int result = Author.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Author_Save_SavesAuthorToDatabase()
    {
      //Arrange
      Author testAuthor = new Author("Edogawa Ranpo");
      testAuthor.Save();
      //Act
      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor};
      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Find_FindsAuthorInDatabase()
    {
      //Arrange
      Author testAuthor = new Author("Mark Twain");
      testAuthor.Save();
      //Act
      Author foundAuthor = Author.Find(testAuthor.Id);
      //Assert
      Assert.Equal(testAuthor, foundAuthor);
    }
    [Fact]
    public void Author_Update_UpdatesAuthorNameInDatabase()
    {
      //Arrange
      Author testAuthor = new Author("Roxane Gay");
      testAuthor.Save();
      string newName = "Lindy West";
      //Act
      testAuthor.Update(newName);
      string result = testAuthor.Name;
      //Assert
      Assert.Equal(newName, result);
    }
    [Fact]
    public void Author_AddBook_AddsBookAssociationToAuthorsBooks()
    {
      //Arrange
      Book testBook1 = new Book("Phantom Tollbooth");
      testBook1.Save();
      Book testBook2 = new Book("The Purity Myth");
      testBook2.Save();
      Author testAuthor = new Author("Roxane Gay");
      testAuthor.Save();
      //Act
      testAuthor.AddBook(testBook1);
      testAuthor.AddBook(testBook2);
      List<Book> actual = testAuthor.GetBooks();
      List<Book> expected = new List<Book> {testBook1, testBook2};
      //Assert
      Assert.Equal(expected, actual);
    }
    [Fact]
    public void Test_Delete_ReturnsTrueIfListsAreTheSame()
    {
      //Arrange
      Author firstTestAuthor = new Author("G.R.R. Martin");
      firstTestAuthor.Save();
      Author secondTestAuthor = new Author("C.S. Lewis");
      secondTestAuthor.Save();
      Author thirdTestAuthor = new Author("Margaret Atwood");
      thirdTestAuthor.Save();
      List<Author> expectedList = new List<Author>{firstTestAuthor, secondTestAuthor};
      //Act
      thirdTestAuthor.Delete();
      List<Author> resultList = Author.GetAll();
      //Assert
      Assert.Equal(resultList, expectedList);
    }

  }
}
