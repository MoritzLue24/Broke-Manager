

using System.ComponentModel.DataAnnotations;
using Api.DTOs;
using Api.DTOs.Categories;
using Api.DTOs.Keywords;
using Api.Models;
using Api.Services.Categories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Services
{
    public class CategoryServiceTests
    {
        // GET_ALL_BY_USER_ASYNC :)
        [Fact]
        public async Task GetAllByUserAsync_ShouldReturnFilledList_WhenOwnedCategoriesExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };
            Keyword keyword = new Keyword { Id = 1, Value = "Rewe", CategoryId = 1 };

            db.Users.Add(user);
            db.Categories.Add(category);
            db.Keywords.Add(keyword);
            await db.SaveChangesAsync();

            // Service
            var service = new CategoryService(db);

            // Execute
            var result = await service.GetAllByUserAsync(user.Id);

            // Assert
            Assert.Single(result);
            Assert.Equal("Essen", result[0].Name);
            Assert.Single(result[0].Keywords);
            Assert.Equal("Rewe", result[0].Keywords[0].Value);
            Assert.Equal(IntervalDto.Once, result[0].Interval);
            Assert.False(result[0].IsDefault);
        }

        // GET_BY_ID_ASYNC :)
        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_WhenOwnedCategoryExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };
            Keyword keyword = new Keyword { Id = 1, Value = "Rewe", CategoryId = 1 };

            db.Users.Add(user);
            db.Categories.Add(category);
            db.Keywords.Add(keyword);
            await db.SaveChangesAsync();

            // Service
            var service = new CategoryService(db);

            // Execute
            var result = await service.GetByIdAsync(user.Id, category.Id);

            // Assert
            Assert.Equal("Essen", result.Name);
            Assert.Single(result.Keywords);
            Assert.Equal("Rewe", result.Keywords[0].Value);
            Assert.Equal(IntervalDto.Once, result.Interval);
            Assert.False(result.IsDefault);
        }

        // CREATE_ASYNC :)
        [Fact]
        public async Task CreateAsync_ShouldCreateCategoryAndReturnDto_WhenCreateDtoCorrect()
        {
            // Db setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var createDto = new CategoryCreateDto
            {
                Name = "Essen",
                Keywords = { new KeywordCreateDto { Value = "Rewe" }},
                Interval = IntervalDto.Once
            };
            Validator.ValidateObject(
                createDto,
                new ValidationContext(createDto),
                validateAllProperties: true
            );

            // Execute
            var service = new CategoryService(db);
            var result = await service.CreateAsync(user.Id, createDto);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Essen", result.Name);
            Assert.Single(result.Keywords);
            Assert.Equal(0, result.Keywords[0].Id);
            Assert.Equal("Rewe", result.Keywords[0].Value);
            Assert.Equal(IntervalDto.Once, result.Interval);
            Assert.False(result.IsDefault);
        }

        // UPDATE_ASYNC :)
        [Fact]
        public async Task UpdateAsync_ShouldUpdate_WhenOwnedCategoryExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };
            Keyword keyword = new Keyword { Id = 1, Value = "Rewe", CategoryId = 1 };

            db.Users.Add(user);
            db.Categories.Add(category);
            db.Keywords.Add(keyword);
            await db.SaveChangesAsync();

            var updateDto = new CategoryUpdateDto
            {
                Name = "Lebensmittel",
                Interval = IntervalDto.Weekly
            };

            // Execute
            var service = new CategoryService(db);
            await service.UpdateAsync(user.Id, category.Id, updateDto);

            // Assert
            Assert.Equal("Lebensmittel", category.Name);
            Assert.Equal(Interval.Weekly, category.Interval);
            Assert.False(category.IsDefault);
        }
    
        // DELETE_ALL_BY_USER_ASYNC
        [Fact]
        public async Task DeleteAllByUserAsync_ShouldDeleteAll_WhenOwnedCategoriesExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            Category category1 = new Category { Id = 1, Name = "Essen", UserId = user.Id };
            Category category2 = new Category { Id = 2, Name = "Miete", Interval = Interval.Monthly, UserId = user.Id };
            Keyword keyword = new Keyword { Id = 1, Value = "Rewe", CategoryId = 1 };

            db.Users.Add(user);
            db.Categories.Add(category1);
            db.Categories.Add(category2);
            db.Keywords.Add(keyword);
            await db.SaveChangesAsync();

            // Execute
            var service = new CategoryService(db);
            await service.DeleteAllByUserAsync(user.Id);

            // Assert
            Assert.Empty(user.Categories);
            Assert.Empty(await db.Keywords.ToListAsync());
        }

        // DELETE_BY_ID_ASYNC :)
        [Fact]
        public async Task DeleteByIdAsync_ShouldDelete_WhenOwnedCategoryExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };
            Keyword keyword = new Keyword { Id = 1, Value = "Rewe", CategoryId = 1 };

            db.Users.Add(user);
            db.Categories.Add(category);
            db.Keywords.Add(keyword);
            await db.SaveChangesAsync();

            // Execute
            var service = new CategoryService(db);
            await service.DeleteByIdAsync(user.Id, category.Id);

            // Assert
            Assert.Empty(user.Categories);
            Assert.Empty(await db.Keywords.ToListAsync());
        }
    }
}