using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Enums;
using Domain.Entiteis;

namespace Domain.Tests.Entities;

public class CategoryTest
{

    private static Category CreateCategory(bool isDefault = false)
    {
        return Category.Create(Guid.NewGuid(), "Essen", isDefault).Value;
    }

    private static Keyword CreateKeyword(string value = "Dildo")
    {
        return Keyword.Create(value).Value;
    }

    [Fact]
    public void Create_ShouldSucceed_WhenDataIsValid()
    {
        
        var result = Category.Create(Guid.NewGuid(), "Drogen", false);

        Assert.True(result.Success);
        Assert.Equal("Drogen", result.Value.Name);
    }

    [Fact]
    public void Create_ShouldFail_WhenNameIsEmpty()
    {
        
        var result = Category.Create(Guid.NewGuid(), "", false);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.CategoryNameEmpty, result.Error);
    }

    [Fact]
    public void AddKeyWordShoulFail_WhenKeyWordIsDuplicate()
    {
        var category = CreateCategory();
        var keyword = CreateKeyword("Milch");
        category.AddKeyword(keyword);

        var result = category.AddKeyword(keyword);
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.NotUniqueKeywordWithinOneCategory, result.Error);
    }

    [Fact]
    public void DeleteKeyword_ShouldFail_WhenKeyWordNotFoun()
    {
        var category = CreateCategory();
        var keyword = CreateKeyword("Sex-Puppe");
        
        var result = category.DeleteKeyword(keyword);
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.KeywordNotFounInCategory, result.Error);
    }

}

