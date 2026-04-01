using System.ComponentModel.DataAnnotations;
using Api.DTOs.Keywords;
using Api.Exceptions;
using Api.Models;
using Api.Services.Keywords;


namespace Tests.Services
{
    public class KeywordServiceTests
    {
        [Fact]
        public async Task CreateAsync_OwnedCategoryExists_ReturnsKeyword()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };

            db.Users.Add(user);
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            // Service & Validation
            var service = new KeywordService(db);
            var createDto = new KeywordCreateDto { Value = "Edeka" };
            Validator.ValidateObject(
                createDto,
                new ValidationContext(createDto),
                validateAllProperties: true
            );

            // Execute
            var result = await service.CreateAsync(user.Id, category.Id, createDto);

            // Assert
            Assert.Equal("Edeka", result.Value);
            Assert.Equal(category.Id, result.CategoryId);
        }

        [Fact]
        public async Task CreateAsync_CategoryDoesNotExists_ThrowsNotFoundException()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            // Service & validate
            var service = new KeywordService(db);
            var createDto = new KeywordCreateDto { Value = "Edeka" };
            Validator.ValidateObject(
                createDto,
                new ValidationContext(createDto),
                validateAllProperties: true
            );

            // Execute & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await service.CreateAsync(user.Id, 1, createDto);
            });
        }

        [Fact]
        public async Task CreateAsync_NotOwnedCategoryExists_ThrowsNotFoundException()
        {
            // Setup
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            User owner = new User { Id = 2, Email = "hans@gmx.de", Password = "Doggo!456" };
            Category category = new Category { Id = 1, Name = "Essen", UserId = owner.Id };

            // Setup db & service
            var dbContext = DbContextHelper.CreateContext();
            dbContext.Users.Add(user);
            dbContext.Users.Add(owner);
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            var service = new KeywordService(dbContext);

            // Validate
            var createDto = new KeywordCreateDto { Value = "Edeka" };
            Validator.ValidateObject(
                createDto,
                new ValidationContext(createDto),
                validateAllProperties: true
            );

            // Execute & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await service.CreateAsync(user.Id, category.Id, createDto);
            });
        }

        [Fact]
        public async Task UpdateAsync_OwnedCategoryExistsKeywordExists_ReturnsNull()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };
            Keyword keyword = new Keyword { Id = 1, Value = "Edeka", CategoryId = 1 };

            db.Users.Add(user);
            db.Categories.Add(category);
            db.Keywords.Add(keyword);
            await db.SaveChangesAsync();

            // Service & Validation
            var service = new KeywordService(db);
            var updateDto = new KeywordUpdateDto { Value = "Rewe" };
            Validator.ValidateObject(
                updateDto,
                new ValidationContext(updateDto),
                validateAllProperties: true
            );

            // Execute
            await service.UpdateAsync(user.Id, category.Id, keyword.Id, updateDto);

            // Assert
            Assert.Equal("Rewe", keyword.Value);
        }

        [Fact]
        public async Task UpdateAsync_KeywordDoesNotExistsOwnedCategoryExists_ThrowsNotFoundException()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };

            db.Users.Add(user);
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            // Service & Validation
            var service = new KeywordService(db);
            var updateDto = new KeywordUpdateDto { Value = "Rewe" };
            Validator.ValidateObject(
                updateDto,
                new ValidationContext(updateDto),
                validateAllProperties: true
            );

            // Execute & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await service.UpdateAsync(user.Id, category.Id, 1, updateDto);
            });
        }
    
        [Fact]
        public async Task UpdateAsync_KeywordDoesNotExistsCategoryDoesNotExists_ThrowsNotFoundException()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", Password = "MeinPassort123!" };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            // Service & Validation
            var service = new KeywordService(db);
            var updateDto = new KeywordUpdateDto { Value = "Rewe" };
            Validator.ValidateObject(
                updateDto,
                new ValidationContext(updateDto),
                validateAllProperties: true
            );

            // Execute & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await service.UpdateAsync(user.Id, 1, 1, updateDto);
            });
        }
    }
}