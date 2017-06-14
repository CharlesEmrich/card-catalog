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
      Author.DeleteAll();
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
    public void Author_CitiesEmptyAtFirst()
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
  }
}
