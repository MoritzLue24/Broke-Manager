using Api.Data;
using Microsoft.EntityFrameworkCore;


namespace Tests
{
    public class DbContextHelper
    {
        public static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
    }
}