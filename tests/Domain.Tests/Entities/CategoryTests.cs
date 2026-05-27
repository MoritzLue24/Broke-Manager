using Domain.Common;
using Domain.Entities;
using Domain.Events.Categories;
using Domain.ValueObjects;

namespace Domain.Tests.Entities;

public class CategoryTest
{
    [Fact]
    public void Create_ShouldSucceed_WhenDataIsValid()
    {
        var result = Category.Create(Guid.NewGuid(), "Drogen", false);
        Assert.Equal("Drogen", result.Value.Name);
        Assert.Contains(new CategoryCreatedEvent(result.Value.Id), result.Value.DomainEvents);
    }

    [Fact]
    public void Create_ShouldReturnInvalidGuidError_WhenUserIdEmpty()
    {
        var result = Category.Create(new Guid(), "name", false);
        Assert.Equal(new InvalidGuidError(), result.FirstError);
    }

    [Fact]
    public void Create_ShouldReturnEmptyCategoryNameError_WhenNameIsEmpty()
    {
        var result = Category.Create(Guid.NewGuid(), "", false);
        Assert.Equal(new EmptyCategoryNameError(), result.FirstError);
    }

    [Fact]
    public void ChangeName_ShouldChangeName_WhenNameNotWhitespace()
    {
        var category = Category.Create(Guid.NewGuid(), "asd", false).Value;
        var newName = "New name";

        var result = category.ChangeName(newName);

        Assert.True(result.Success);
        Assert.Equal(newName, category.Name);
    }

    [Fact]
    public void ChangeName_ShouldReturnEmptyCategoryNameError_WhenNameWhitespace()
    {
        var category = Category.Create(Guid.NewGuid(), "asd", false).Value;
        var newName = "";

        var result = category.ChangeName(newName);

        Assert.Equal(new EmptyCategoryNameError(), result.FirstError);
    }

    [Fact]
    public void AddRuleShouldFail_WhenCategoryIsDefault()
    {
        var category = Category.Create(Guid.NewGuid(), "asd", true).Value;
        var rule = MatchingRule.Create("Milch").Value;

        var result = category.AddRule(rule);

        Assert.Equal(new CategoryIsDefaultError(), result.FirstError);
    }

    [Fact]
    public void AddRuleShouldFail_WhenRuleIsDuplicate()
    {
        var category = Category.Create(Guid.NewGuid(), "asd", false).Value;
        var rule = MatchingRule.Create("Milch").Value;
        category.AddRule(rule);

        var result = category.AddRule(rule);

        Assert.Equal(new DuplicateKeywordError(), result.FirstError);
    }

    [Fact]
    public void RemoveRule_ShouldFail_WhenRuleNotFound()
    {
        var category = Category.Create(Guid.NewGuid(), "asd", false).Value;
        var rule = MatchingRule.Create("Sex-Puppe").Value;

        var result = category.RemoveRule(rule);

        Assert.Equal(new KeywordNotFoundError(), result.FirstError);
    }

    [Fact]
    public void AddRule_ShouldSucceed()
    {
        var category = Category.Create(Guid.NewGuid(), "asd", false).Value;
        var rule = MatchingRule.Create("Durex").Value;

        var result = category.AddRule(rule);

        Assert.True(result.Success);
        Assert.Contains(rule, category.MatchingRules);
    }

    [Fact]
    public void RemoveRule_ShouldSucceed_WhenRuleExists()
    {
        var category = Category.Create(Guid.NewGuid(), "asd", false).Value;
        var rule = MatchingRule.Create("durex").Value;
        category.AddRule(rule);

        var result = category.RemoveRule(rule);

        Assert.True(result.Success);
        Assert.DoesNotContain(rule, category.MatchingRules);
    }

    [Fact]
    public void Delete_ShouldAddCategoryDeletedEvent_WhenCategoryNotDefault()
    {
        var category = Category.Create(Guid.NewGuid(), "asd", false).Value;

        var result = category.Delete();

        Assert.True(result.Success);
        Assert.Contains(new CategoryDeletedEvent(), category.DomainEvents);
    }

    [Fact]
    public void Delete_ShouldReturnCategoryIsDefaultError_WhenCategoryDefault()
    {
        var category = Category.Create(Guid.NewGuid(), "asd", true).Value;

        var result = category.Delete();

        Assert.Equal(new CategoryIsDefaultError(), result.FirstError);
    }
}

