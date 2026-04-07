using System.ComponentModel.DataAnnotations;
using Api.DTOs.Keywords;
using Api.Exceptions;
using Api.Models;
using Api.Services.Keywords;


namespace Tests.Services
{
    public class KeywordServiceTests
    {
        // CREATE_ASYNC :)
        [Fact]
        public async Task CreateAsync_ShouldReturnKeyword_WhenOwnedCategoryExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };

            await db.Users.AddAsync(user);
            await db.Categories.AddAsync(category);
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

        // CREATE_ASYNC :(
        [Fact]
        public async Task CreateAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") };
            await db.Users.AddAsync(user);
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
        public async Task CreateAsync_ShouldThrowNotFoundException_WhenCategoryNotOwned()
        {
            // Setup
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") };
            User owner = new User { Id = 2, Email = "hans@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Doggo!456") };
            Category category = new Category { Id = 1, Name = "Essen", UserId = owner.Id };

            // Setup db & service
            var dbContext = DbContextHelper.CreateContext();
            await dbContext.Users.AddAsync(user);
            await dbContext.Users.AddAsync(owner);
            await dbContext.Categories.AddAsync(category);
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
        public async Task CreateAsync_ShouldThrowAlreadyExistsException_WhenKeywordAlreadyExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") };
            Category food = new Category { Id = 1, Name = "Essen", UserId = user.Id };
            Category subscriptions = new Category { Id = 2, Name = "Abos", UserId = user.Id };
            Keyword existsKeyword = new Keyword { Id = 1, Value = "Aldi", CategoryId = 2 };
            
            await db.Users.AddAsync(user);
            await db.Categories.AddAsync(food);
            await db.Categories.AddAsync(subscriptions);
            await db.Keywords.AddAsync(existsKeyword);
            await db.SaveChangesAsync();

            // Service & validate
            var service = new KeywordService(db);
            var createDto = new KeywordCreateDto { Value = "Aldi" };
            Validator.ValidateObject(
                createDto,
                new ValidationContext(createDto),
                validateAllProperties: true
            );

            // Execute & Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(async () =>
            {
                await service.CreateAsync(user.Id, 1, createDto);
            });
        }

        // UPDATE_ASYNC :)
        [Fact]
        public async Task UpdateAsync_ShouldUpdateKeyword_WhenOwnedKeywordExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };
            Keyword keyword = new Keyword { Id = 1, Value = "Edeka", CategoryId = 1 };

            await db.Users.AddAsync(user);
            await db.Categories.AddAsync(category);
            await db.Keywords.AddAsync(keyword);
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

        // UPDATE_ASYNC :(
        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenKeywordDoesNotExist()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") };

            await db.Users.AddAsync(user);
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

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenKeywordDoesNotExistsOwnedCategoryExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") };
            Category category = new Category { Id = 1, Name = "Essen", UserId = user.Id };

            await db.Users.AddAsync(user);
            await db.Categories.AddAsync(category);
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
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenCategoryNotOwned()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") };
            Category category = new Category { Id = 1, Name = "Essen", UserId = 2 };
            Keyword keyword = new Keyword { Id = 1, Value = "Edeka", CategoryId = 1 };

            await db.Users.AddAsync(user);
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
    
        [Fact]
        public async Task UpdateAsync_ShouldThrowAlreadyExistsException_WhenKeywordAlreadyExists()
        {
            // Db Setup
            var db = DbContextHelper.CreateContext();
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") };
            Category food = new Category { Id = 1, Name = "Essen", UserId = user.Id };
            Category subscriptions = new Category { Id = 2, Name = "Abos", UserId = user.Id };
            Keyword existsKeyword = new Keyword { Id = 1, Value = "Aldi", CategoryId = 2 };
            Keyword keyword = new Keyword { Id = 2, Value = "Rewe", CategoryId = 1 };
            
            await db.Users.AddAsync(user);
            await db.Categories.AddAsync(food);
            await db.Categories.AddAsync(subscriptions);
            await db.Keywords.AddAsync(existsKeyword);
            await db.Keywords.AddAsync(keyword);
            await db.SaveChangesAsync();

            // Service & validate
            var service = new KeywordService(db);
            var updateDto = new KeywordUpdateDto { Value = "Aldi" };
            Validator.ValidateObject(
                updateDto,
                new ValidationContext(updateDto),
                validateAllProperties: true
            );

            // Execute & Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(async () =>
            {
                await service.UpdateAsync(user.Id, food.Id, keyword.Id, updateDto);
            });
        }
    }
}