using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Enums;

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
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Error; });
    }
    
    [Fact]
    public void Create_ShouldSetDefault()
    {
        var category = CreateCategory(true);
        Assert.True(category.IsDefault);
    }
    
    
    [Fact]
    public void Create_ShouldFail_WhenNameIsEmpty()
    {
        
        var result = Category.Create(Guid.NewGuid(), "", false);
        
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.CategoryNameEmpty, result.Error);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }

    [Fact]
    public void AddKeyWordShouldFail_WhenKeyWordIsDuplicate()
    {
        var category = CreateCategory();
        var keyword = CreateKeyword("Milch");
        category.AddKeyword(keyword);

        var result = category.AddKeyword(keyword);
        
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.KeywordAlreadyExists, result.Error);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }

    [Fact]
    public void DeleteKeyword_ShouldFail_WhenKeyWordNotFound()
    {
        var category = CreateCategory();
        var keyword = CreateKeyword("Sex-Puppe");
        
        var result = category.RemoveKeyword(keyword);
        
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.KeywordNotFound, result.Error);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }

    [Fact]
    public void AddKeyWord_ShoudlSucceed()
    {
        var category = CreateCategory();
        var keyword = CreateKeyword("durex");

        var result = category.AddKeyword(keyword );

        Assert.True(result.Success);
        Assert.Contains(keyword,category.Keywords);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Error; });
    }

    [Fact]
    public void RemoveKeyword_ShouldSucceed()
    {
        var category = CreateCategory();
        var keyword = CreateKeyword("durex");
        category.AddKeyword(keyword );
        
        var result = category.RemoveKeyword(keyword);
        Assert.True(result.Success);
        Assert.DoesNotContain(keyword,category.Keywords);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Error; });
    }

}

