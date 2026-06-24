// ============================================================
// FILE: ./src/Contracts/Features/Transactions/CreateTransactionRequest.cs
// ============================================================
namespace Contracts.Features.Transactions;

public record CreateTransactionRequest(
    Guid UserId,
    Guid? CategoryId,
    decimal Amount,
    string Type,
    DateOnly Date,
    string Title,
    string Description,
    string CounterParty
);
// ============================================================
// FILE: ./src/Contracts/Features/Transactions/TransactionDetailResponse.cs
// ============================================================
namespace Contracts.Features.Transactions;

public record TransactionDetailResponse(
    Guid Id,
    Guid UserId,
    Guid CategoryId,
    string CategorySource,
    decimal Amount,
    string Type,
    DateOnly Date,
    string Title,
    string Description,
    string CounterParty,
    DateTime CreatedAt
);
// ============================================================
// FILE: ./src/Infrastructure/DependencyInjection.cs
// ============================================================
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Infrastructure.Persistence.Repositories;
using Application.Features.Transactions;

namespace Infrastructure;

public static class DependencyInjection
{
    /// Used for runtime, not design-time
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
            connectionString,
            builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        ));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryReaderRepository, CategoryReaderRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }
}

// ============================================================
// FILE: ./src/Infrastructure/Persistence/Repositories/TransactionRepository.cs
// ============================================================
using Application.Features.Transactions;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Transaction?> GetByIdAsync(Guid id)
        => await _dbContext.Transactions.FindAsync(id);

    public void Add(Transaction transaction)
        => _dbContext.Transactions.Add(transaction);
}
// ============================================================
// FILE: ./src/Infrastructure/Persistence/Repositories/CategoryReaderRepository.cs
// ============================================================
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoryReaderRepository : ICategoryReaderRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryReaderRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Guid?> GetDefaultByUserIdAsync(Guid userId)
    {
        return await _dbContext.Categories
            .Where(c => c.UserId == userId && c.IsDefault)
            .Select(c => (Guid?)c.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId)
    {
        return await _dbContext.Categories
            .Where(c => c.UserId == userId && c.Id == categoryId)
            .AnyAsync();
    }
}
// ============================================================
// FILE: ./src/Infrastructure/Persistence/Configurations/CategoryConfiguration.cs
// ============================================================
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    // Aus irgendeinen grund brauchen wir das hier
    private class KeywordConverter : ValueConverter<Keyword, string>
    {
        public KeywordConverter()
            : base(k => k.Value, v => Keyword.Create(v).Value)
        { }
    }

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        // Id
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        // UserId
        builder.Property(c => c.UserId).HasColumnName("user_id");
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(c => c.UserId)
            .HasDatabaseName("ix_categories_user_id");

        // Name
        builder.Property(c => c.Name).HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();
        builder.HasIndex(c => new { c.UserId, c.Name }) // For unique category name
            .IsUnique()
            .HasDatabaseName("ix_categories_user_id_name");

        // IsDefault
        builder.Property(c => c.IsDefault).HasColumnName("is_default");
        builder.HasIndex(c => c.UserId) // User has only 1 Default Category
            .HasFilter("is_default = true") 
            .IsUnique()                      
            .HasDatabaseName("ix_categories_user_id_unique_default");
        
        // Keywords
        builder.PrimitiveCollection<List<Keyword>>("_keywords")
            .HasColumnName("keywords")
            .IsRequired()
        // Single keyword element
            .ElementType()
            .HasConversion(typeof(KeywordConverter))
            .HasMaxLength(255)
            .IsRequired();

        // CreatedAt
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");
    }
}
// ============================================================
// FILE: ./src/Infrastructure/Persistence/Configurations/TransactionConfiguration.cs
// ============================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        // Id
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");

        // UserId
        builder.Property(t => t.UserId).HasColumnName("user_id");
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => t.UserId)     
            .HasDatabaseName("ix_transactions_user_id");


        // StandingOrderId, autom. nicht IsRequired, weil nullable
        builder.Property(t => t.StandingOrderId).HasColumnName("standing_order_id");
        // builder.HasOne<StandingOrder>()
        //     .WithMany()
        //     .HasForeignKey(t => t.StandingOrderId)
        //     .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(t => t.StandingOrderId) 
            .HasFilter("\"standing_order_id\" IS NOT NULL")
            .HasDatabaseName("ix_transactions_standing_order_id");

        // CategoryId
        builder.Property(t => t.CategoryId).HasColumnName("category_id");
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(t => t.CategoryId) 
            .HasDatabaseName("ix_transactions_category_id");

        // Sources
        builder.Property(t => t.CategorySource).HasColumnName("category_source")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(t => t.StandingOrderSource).HasColumnName("standing_order_source")
            .HasConversion<string>()
            .HasMaxLength(20);  

        // Amount
        builder.Property(t => t.Amount).HasColumnName("amount")
            .HasColumnType("numeric(12,2)");

        builder.ToTable("transactions", t => 
            t.HasCheckConstraint("CK_transactions_amount_positive", "amount > 0")); //Amount nicht 0 und immer positiv

        // Type
        builder.Property(t => t.Type).HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Date
        builder.Property(t => t.Date).HasColumnName("date");
        builder.HasIndex(t => new { t.UserId, t.Date }) 
            .HasDatabaseName("ix_transactions_user_id_date");

        // Title
        builder.Property(t => t.Title).HasColumnName("title")
            .HasMaxLength(255)
            .IsRequired();

        // Description
        builder.Property(t => t.Description).HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        // CounterParty
        builder.Property(t => t.CounterParty).HasColumnName("counter_party")
            .HasMaxLength(255)
            .IsRequired();

        // CreatedAt
        builder.Property(t => t.CreatedAt).HasColumnName("created_at");
    }
}
// ============================================================
// FILE: ./src/Infrastructure/Persistence/Configurations/UserConfiguration.cs
// ============================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        // Id
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        // Email
        builder.OwnsOne(u => u.Email, nav =>
        {
            nav.Property(email => email.Value)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();
            nav.HasIndex(email => email.Value)
                .IsUnique()
                .HasDatabaseName("ix_users_email");
        });

        // PasswordHash
        builder.OwnsOne(u => u.PasswordHash, nav =>
        {
            nav.Property(h => h.Value)
                .HasColumnName("password_hash")
                .HasMaxLength(128)
                .IsRequired();
        });

        // Role
        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // CreatedAt
        builder.Property(u => u.CreatedAt).HasColumnName("created_at");
    }
}
// ============================================================
// FILE: ./src/Infrastructure/Persistence/UnitOfWork.cs
// ============================================================
using Application.Common.Interfaces;

namespace Infrastructure.Persistence;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _dbContext.SaveChangesAsync(ct);
}
// ============================================================
// FILE: ./src/Infrastructure/Persistence/Migrations/20260510222002_Test.Designer.cs
// ============================================================
﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260510222002_Test")]
    partial class Test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean")
                        .HasColumnName("is_default");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string[]>("_keywords")
                        .IsRequired()
                        .HasColumnType("character varying(255)[]")
                        .HasColumnName("keywords");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_categories_user_id");

                    b.HasIndex("UserId", "Name")
                        .IsUnique()
                        .HasDatabaseName("ix_categories_user_id_name");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric(12,2)")
                        .HasColumnName("amount");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<string>("CategorySource")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("category_source");

                    b.Property<string>("CounterParty")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("counter_party");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<Guid?>("StandingOrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("standing_order_id");

                    b.Property<string>("StandingOrderSource")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("standing_order_source");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("type");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_transactions_category_id");

                    b.HasIndex("StandingOrderId")
                        .HasDatabaseName("ix_transactions_standing_order_id")
                        .HasFilter("\"standing_order_id\" IS NOT NULL");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_transactions_user_id");

                    b.HasIndex("UserId", "Date")
                        .HasDatabaseName("ix_transactions_user_id_date");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Transaction", b =>
                {
                    b.HasOne("Domain.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("character varying(255)")
                                .HasColumnName("email");

                            b1.HasKey("UserId");

                            b1.HasIndex("Value")
                                .IsUnique()
                                .HasDatabaseName("ix_users_email");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Domain.ValueObjects.Hash", "PasswordHash", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("character varying(128)")
                                .HasColumnName("password_hash");

                            b1.HasKey("UserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("PasswordHash")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

// ============================================================
// FILE: ./src/Infrastructure/Persistence/Migrations/20260510222002_Test.cs
// ============================================================
﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    keywords = table.Column<string[]>(type: "character varying(255)[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_categories_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    standing_order_id = table.Column<Guid>(type: "uuid", nullable: true),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    standing_order_source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    amount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    counter_party = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_transactions_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_categories_user_id",
                table: "categories",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_user_id_name",
                table: "categories",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transactions_category_id",
                table: "transactions",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_standing_order_id",
                table: "transactions",
                column: "standing_order_id",
                filter: "\"standing_order_id\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_user_id",
                table: "transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_user_id_date",
                table: "transactions",
                columns: new[] { "user_id", "date" });

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

// ============================================================
// FILE: ./src/Infrastructure/Persistence/Migrations/AppDbContextModelSnapshot.cs
// ============================================================
﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean")
                        .HasColumnName("is_default");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string[]>("_keywords")
                        .IsRequired()
                        .HasColumnType("character varying(255)[]")
                        .HasColumnName("keywords");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_categories_user_id");

                    b.HasIndex("UserId", "Name")
                        .IsUnique()
                        .HasDatabaseName("ix_categories_user_id_name");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric(12,2)")
                        .HasColumnName("amount");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<string>("CategorySource")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("category_source");

                    b.Property<string>("CounterParty")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("counter_party");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<Guid?>("StandingOrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("standing_order_id");

                    b.Property<string>("StandingOrderSource")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("standing_order_source");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("type");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_transactions_category_id");

                    b.HasIndex("StandingOrderId")
                        .HasDatabaseName("ix_transactions_standing_order_id")
                        .HasFilter("\"standing_order_id\" IS NOT NULL");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_transactions_user_id");

                    b.HasIndex("UserId", "Date")
                        .HasDatabaseName("ix_transactions_user_id_date");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Transaction", b =>
                {
                    b.HasOne("Domain.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("character varying(255)")
                                .HasColumnName("email");

                            b1.HasKey("UserId");

                            b1.HasIndex("Value")
                                .IsUnique()
                                .HasDatabaseName("ix_users_email");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Domain.ValueObjects.Hash", "PasswordHash", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("character varying(128)")
                                .HasColumnName("password_hash");

                            b1.HasKey("UserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("PasswordHash")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

// ============================================================
// FILE: ./src/Infrastructure/Persistence/AppDbContextFactory.cs
// ============================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// Searches recursivly the folders above for a .env file and applies it
    private static void LoadNearestEnv()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, ".env")))
        {
            Console.WriteLine($"Searching for .env in '{dir}'");
            dir = dir.Parent;
        }

        if (dir == null)
            throw new InvalidOperationException(".env file not found");

        Console.WriteLine($"Found .env in '{dir}'");
        DotNetEnv.Env.Load(Path.Combine(dir.FullName, ".env"));
    }

    /// Used for Design-Time, without DI / Program.cs
    /// For example when running just migrate
    public AppDbContext CreateDbContext(string[] args)
    {
        LoadNearestEnv();

        return new(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"))
                .Options
        );
    }
}

// ============================================================
// FILE: ./src/Infrastructure/Persistence/AppDbContext.cs
// ============================================================
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.ValueObjects;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    // public DbSet<StandingOrder> StandingOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Sonst behandelt EFCore das aus irgendeinen grund als Entity
        modelBuilder.Ignore<Keyword>();
        modelBuilder.Ignore<Email>();
        modelBuilder.Ignore<Hash>();
        // TODO, Besser:
        // var valueObjectTypes = typeof(Keyword).Assembly
        //     .GetTypes()
        //     .Where(t => typeof(IValueObject).IsAssignableFrom(t) && t.IsClass);

        // foreach (var type in valueObjectTypes)
        //     modelBuilder.Ignore(type);
        // Und alle VOs in Domain implementen IValueObject

        // Lädt alle Classes, die in diesen Projekt IEntityTypeConfiguration implementieren
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
// ============================================================
// FILE: ./src/Api/Controllers/TransactionController.cs
// ============================================================
using Application.Features.Transactions.Commands.CreateTransaction;
using Contracts.Features.Transactions;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost("")]
    public async Task<ActionResult<TransactionDetailResponse>> CreateTransaction(
        [FromBody] CreateTransactionRequest createRequest)
    {
        var result = await _mediator.Send(new CreateTransactionCommand(
            createRequest.UserId,
            createRequest.CategoryId,
            createRequest.Amount,
            Enum.Parse<TransactionType>(createRequest.Type),
            createRequest.Date,
            createRequest.Title,
            createRequest.Description,
            createRequest.CounterParty
        ));

        return result.Match<ActionResult<TransactionDetailResponse>>(
            dto => Ok(new TransactionDetailResponse(
                dto.Id,
                dto.UserId,
                dto.CategoryId,
                dto.CategorySource.ToString(),
                dto.Amount,
                dto.Type.ToString(),
                dto.Date,
                dto.Title,
                dto.Description,
                dto.CounterParty,
                dto.CreatedAt
            )),
            error => Problem(error.GetType().FullName)
        ); 
    }
}
// ============================================================
// FILE: ./src/Api/Program.cs
// ============================================================
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddControllers();
    
    builder.Services.AddInfrastructure(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Default connection string not set"));
    builder.Services.AddApplication();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}

// ============================================================
// FILE: ./src/Domain/Entities/Category.cs
// ============================================================
using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Category
{
    public Guid Id { get; private set;}
    public Guid UserId { get; private set;}
    public string Name { get; private set;} = null!;    // für leeren constructor
    public bool IsDefault { get; private set;}
    public DateTime CreatedAt {get; private set;}

    private readonly List<Keyword> _keywords = [];

    public IReadOnlyCollection<Keyword> Keywords
    {
        get 
        {
            return _keywords.AsReadOnly();
        }
    }

    private Category () { }
    
    private Category (Guid userId, string name, bool isDefault)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<Category> Create(Guid userId, string name, bool isDefault)
    {
        if(userId == Guid.Empty)
            return new InvalidGuidError();

        if (string.IsNullOrWhiteSpace(name))
            return new EmptyCategoryNameError();

        return new Category(userId, name, isDefault);
    }

    public Result<Unit> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new EmptyCategoryNameError();

        Name = name;
        return Unit.Value;
    }

    public Result<Unit> AddKeyword(Keyword keyword)
    {
        if(IsDefault)
            return new CategoryIsDefaultError();

        if(_keywords.Any(k => k == keyword))
            return new DuplicateKeywordError();

        _keywords.Add(keyword);
        return Unit.Value;
    }

    public Result<Unit> RemoveKeyword(Keyword keyword)
    {
        if (_keywords.Remove(keyword) == false)
            return new KeywordNotFoundError();

        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        if (IsDefault)
            return new CategoryIsDefaultError();

        return Unit.Value;
    }
}
// ============================================================
// FILE: ./src/Domain/Entities/User.cs
// ============================================================
using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; }
    public Email Email { get; private set; } = null!;   // für leeren constructor
    public Hash PasswordHash { get; private set; } = null!; // auch
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; }

    private User() { } // Für EF Core??

    private User(Email email, Hash passwordHash, Role role)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<User> Create(Email email, Hash passwordHash)
    {
        return new User(email, passwordHash, Role.User);
    }

    public Result<Unit> ChangeEmail(Email email)
    {
        Email = email;
        return Unit.Value;
    }

    public Result<Unit> ChangePasswordHash(Hash passwordHash)
    {
        PasswordHash = passwordHash;
        return Unit.Value;
    }

    public Result<Unit> ChangeRole(Role role)
    {
        Role = role;
        return Unit.Value;
    }
}

// ============================================================
// FILE: ./src/Domain/Entities/StandingOrderPause.cs
// ============================================================

using Domain.Common;

namespace Domain.Entities;

public class StandingOrderPause
{
    public Guid Id { get; }
    public DateOnly From { get; private set; }
    public DateOnly To { get; private set; }

    private StandingOrderPause(DateOnly from, DateOnly to)
    {
        Id = Guid.NewGuid();
        From = from;
        To = to;
    }

    public static Result<StandingOrderPause> Create(DateOnly from, DateOnly? to)
    {
        if (from > (to ?? DateOnly.MaxValue))
            throw new NotImplementedException();

        return new StandingOrderPause(
            from,
            to ?? DateOnly.MaxValue
        );
    }

    public Result<Unit> UpdateFrom(DateOnly from)
    {
        if (from > To)
            throw new NotImplementedException();

        From = from;
        return Unit.Value;
    }

    public Result<Unit> UpdateTo(DateOnly to)
    {
        if (From > to)
            throw new NotImplementedException();

        To = to;
        return Unit.Value;
    }

    public Result<Unit> MakeInfinite()
    {
        To = DateOnly.MaxValue;
        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        return Unit.Value;
    }
}
// ============================================================
// FILE: ./src/Domain/Entities/Transaction.cs
// ============================================================
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid? StandingOrderId { get; private set; }
    public Guid CategoryId { get; private set; }
    public CategorySource CategorySource { get; private set; }
    public StandingOrderSource? StandingOrderSource { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public DateOnly Date {get; private set;}
    public string Title {get; private set;} = null!;    // für leeren constructor
    public string Description { get; private set; } = null!;
    public string CounterParty {get; private set;} = null!;
    public DateTime CreatedAt { get; }

    private Transaction() { }

    private Transaction(
        Guid userId,
        Guid categoryId,
        CategorySource categorySource,
        decimal amount,
        TransactionType type,
        DateOnly date,
        string title,
        string description,
        string counterParty)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        StandingOrderId = null;
        CategoryId = categoryId;
        CategorySource = categorySource;
        StandingOrderSource = null;
        Amount = amount;
        Type = type;
        Date = date;
        Title = title;
        Description = description;
        CounterParty = counterParty;
    }

    public static Result<Transaction> Create(
        Guid userId,
        Guid categoryId,
        CategorySource categorySource,
        decimal amount,
        TransactionType type,
        DateOnly date,
        string title,
        string description, 
        string counterParty)
    {
        if(userId == Guid.Empty || categoryId == Guid.Empty)
            return new InvalidGuidError();

        if(amount <= 0)
            return new InvalidAmountError();

        if (string.IsNullOrWhiteSpace(title))
            return new EmptyTransactionTitleError();

        if (!Enum.IsDefined(typeof(CategorySource), categorySource))
            return new InvalidCategorySourceError();

        return new Transaction(
            userId,
            categoryId,
            categorySource,
            amount,
            type,
            date,
            title,
            description,
            counterParty
        );
    }

    public Result<Unit> ChangeCategory(Guid categoryId, CategorySource source)
    {
        if(categoryId == Guid.Empty)
            return new InvalidGuidError();

        CategoryId = categoryId;
        CategorySource = source;
        return Unit.Value;
    }

    public Result<Unit> ChangeStandingOrder(Guid standingOrderId, StandingOrderSource source)
    {
        if(standingOrderId == Guid.Empty)
            return new InvalidGuidError();

        StandingOrderId = standingOrderId;
        StandingOrderSource = source;
        return Unit.Value;
    }

    public Result<Unit> RemoveStandingOrder()
    {
        StandingOrderId = null;
        StandingOrderSource = null;
        return Unit.Value;
    }

    public Result<Unit> ChangeAmount(decimal amount, TransactionType type)
    {
        if(amount <= 0)
            return new InvalidAmountError();

        Amount = amount;
        Type = type;
        return Unit.Value;
    }

    public Result<Unit> ChangeDate(DateOnly date)
    {
        Date = date;
        return Unit.Value;
    }

    public Result<Unit> ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return new EmptyTransactionTitleError();

        Title = title;
        return Unit.Value;
    }

    public Result<Unit> ChangeDescription(string description)
    {
        Description = description;
        return Unit.Value;
    }

    public Result<Unit> ChangeCounterParty(string counterParty)
    {
        CounterParty = counterParty;
        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        return Unit.Value;
    }
}
// ============================================================
// FILE: ./src/Domain/Entities/StandingOrder.cs
// ============================================================
using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class StandingOrder
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid? CategoryId { get; private set; }
    public string Name { get; private set; }
    private readonly List<Keyword> _keywords = [];
    public IReadOnlyCollection<Keyword> Keywords
    {
        get => _keywords.AsReadOnly();
    }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public RecurrencePattern RecurrencePattern { get; private set; }
    private readonly List<Guid> _pauseHistory = [];
    public IReadOnlyCollection<Guid> PauseHistory
    {
        get => _pauseHistory.AsReadOnly();
    }
    public DateTime CreatedAt { get; }

    private StandingOrder(
        Guid userId,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        RecurrencePattern recurrencePattern)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CategoryId = null;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        RecurrencePattern = recurrencePattern;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<StandingOrder> Create(
        Guid userId,
        string name,
        DateOnly startDate,
        DateOnly? endDate,
        RecurrencePattern recurrencePattern)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new NotImplementedException();

        if (startDate > (endDate ?? DateOnly.MaxValue))
            throw new NotImplementedException();

        return new StandingOrder(
            userId,
            name,
            startDate,
            endDate ?? DateOnly.MaxValue,
            recurrencePattern
        );
    }

    public Result<Unit> ChangeCategory(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            return new InvalidGuidError();

        CategoryId = categoryId;
        return Unit.Value;
    }

    public Result<Unit> RemoveCategory()
    {
        CategoryId = null;
        return Unit.Value;
    }

    public Result<Unit> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new NotImplementedException();

        Name = name;
        return Unit.Value;
    }

    public Result<Unit> AddKeyword(Keyword keyword)
    {
        if(_keywords.Any(k => k == keyword))
            throw new NotImplementedException();

        _keywords.Add(keyword);
        return Unit.Value;
    }

    public Result<Unit> RemoveKeyword(Keyword keyword)
    {
        if (_keywords.Remove(keyword) == false)
            throw new NotImplementedException();

        return Unit.Value;
    }

    public Result<Unit> ChangeStartDate(DateOnly startDate)
    {
        if (startDate > EndDate)
            throw new NotImplementedException();

        StartDate = startDate;
        return Unit.Value;
    }

    public Result<Unit> ChangeEndDate(DateOnly endDate)
    {
        if (StartDate > endDate)
            throw new NotImplementedException();

        EndDate = endDate;
        return Unit.Value;
    }

    public Result<Unit> MakeInfinite()
    {
        EndDate = DateOnly.MaxValue;
        return Unit.Value;
    }

    public Result<Unit> ChangeRecurrencePattern(RecurrencePattern recurrencePattern)
    {
        RecurrencePattern = recurrencePattern;
        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        return Unit.Value;
    }
}
// ============================================================
// FILE: ./src/Domain/Enums/CategorySource.cs
// ============================================================
namespace Domain.Enums;

public enum CategorySource
{
    Unmatched,
    Manual,
    Auto,
    FromStandingOrder
}
// ============================================================
// FILE: ./src/Domain/Enums/Interval.cs
// ============================================================
namespace Domain.Enums;

public enum Interval
{
    Weekly,
    Monthly,
    Quarterly,
    Yearly
}
// ============================================================
// FILE: ./src/Domain/Enums/StandingOrderSource.cs
// ============================================================

namespace Domain.Enums;

public enum StandingOrderSource
{
    Manual,
    Auto
}
// ============================================================
// FILE: ./src/Domain/Enums/Role.cs
// ============================================================
namespace Domain.Enums;

public enum Role
{
    User,
    Admin
}
// ============================================================
// FILE: ./src/Domain/Enums/TransactionType.cs
// ============================================================

namespace Domain.Enums;

public enum TransactionType
{
    Income,
    Expense
}
// ============================================================
// FILE: ./src/Domain/ValueObjects/Keyword.cs
// ============================================================
using Domain.Common;

namespace Domain.ValueObjects;

public sealed record Keyword
{
    public string Value { get; }

    private Keyword(string value)
    {
        Value = value;
    }

    public static Result<Keyword> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new EmptyKeywordError();

        return new Keyword(value);
    }
}
// ============================================================
// FILE: ./src/Domain/ValueObjects/Email.cs
// ============================================================
using System.Net.Mail;
using Domain.Common;

namespace Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }
        

    public static Result<Email> Create(string value)
    {
        MailAddress email;
        try
        {
            email = new MailAddress(value);
        }
        catch (Exception)
        {
            return new InvalidEmailFormatError();
        }
        return new Email(email.Address);
    }
}

// ============================================================
// FILE: ./src/Domain/ValueObjects/Hash.cs
// ============================================================
using Domain.Common;

namespace Domain.ValueObjects;

public sealed record Hash
{
    public string Value { get; }

    private Hash(string value)
    {
        Value = value;
    }

    public static Result<Hash> Create(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            return new InvalidHashFormatError();

        return new Hash(hash);
    }
}
// ============================================================
// FILE: ./src/Domain/ValueObjects/RecurrencePattern.cs
// ============================================================

using Domain.Common;
using Domain.Enums;

namespace Domain.ValueObjects;

public sealed record RecurrencePattern
{
    public Interval Interval { get; }
    public int ExecutionDay { get; }

    private RecurrencePattern(Interval interval, int executionDay)
    {
        Interval = interval;
        ExecutionDay = executionDay;
    }

    public static Result<RecurrencePattern> Create(Interval interval, int executionDay)
    {
        if (executionDay < 1)
            throw new NotImplementedException();

        return new RecurrencePattern(
            interval,
            executionDay
        );
    }

    public Result<DateOnly> GetActualDay(DateOnly referenceDate)
    {
        DateOnly periodStart = Interval switch
        {
            Interval.Weekly => referenceDate.AddDays(1 - (
                referenceDate.DayOfWeek == DayOfWeek.Sunday
                ? 7
                : (int)referenceDate.DayOfWeek
            )),
            Interval.Monthly => new DateOnly(
                referenceDate.Year,
                referenceDate.Month,
                1
            ),
            Interval.Quarterly => new DateOnly(
                referenceDate.Year,
                (int)((referenceDate.Month - 1) / 3) * 3 + 1,
                1
            ),
            Interval.Yearly => new DateOnly(referenceDate.Year, 1, 1),
            _ => throw new NotImplementedException()
        };

        DateOnly periodEnd = Interval switch
        {
            Interval.Weekly => periodStart.AddDays(6),
            Interval.Monthly => periodStart.AddMonths(1).AddDays(-1),
            Interval.Quarterly => periodStart.AddMonths(3).AddDays(-1),
            Interval.Yearly => periodStart.AddYears(1).AddDays(-1),
            _ => throw new NotImplementedException()
        };

        DateOnly executionDate = periodStart.AddDays(ExecutionDay - 1);
        return  executionDate > periodEnd
            ? periodEnd
            : executionDate;
    }
}
// ============================================================
// FILE: ./src/Domain/Common/DomainErrorCode.cs
// ============================================================
namespace Domain.Common;

public enum DomainErrorCode
{
    //Common
    InvalidGuid = 0,

    // Keyword
    KeywordEmpty,
    KeywordAlreadyExists,
    KeywordNotFound,

    //User
    InvalidEmailFormat = 10,
    InvaildHashFormat = 11,
    UserNotFound = 12,

    //Category
    CategoryNameEmpty = 20,
    CategoryNotFound = 21,
    NoKeywordForDefaultCategory = 24,
    CannotDeleteDefaultCategory = 26,

    //Transaction
    InvalidTransactionDate = 30,
    InvalidAmount = 31,
    TransactionTitleEmpty = 32,
    InvalidCategorySource = 34,
    InvalidInterval = 35,

    // Standing Order
    StandingOrderNameEmpty = 40,
    StandingOrderDatesInvalid = 41,

    // Recurrence Pattern
    RecurrencePatternInvalidExecutionDay = 50,

    // Standing Order Pause
    StandingOrderPauseDatesInvalid = 60,
}

// ============================================================
// FILE: ./src/Domain/Common/Errors.cs
// ============================================================
namespace Domain.Common;

public abstract record Error;


public sealed record InvalidGuidError : Error;

public sealed record InvalidEmailFormatError : Error;
public sealed record InvalidHashFormatError : Error;
public sealed record EmptyKeywordError : Error;

public sealed record InvalidAmountError : Error;
public sealed record EmptyTransactionTitleError : Error;
public sealed record InvalidCategorySourceError : Error;

public sealed record EmptyCategoryNameError : Error;
public sealed record CategoryIsDefaultError : Error;
public sealed record DuplicateKeywordError : Error;
public sealed record KeywordNotFoundError : Error;
// ============================================================
// FILE: ./src/Domain/Common/Result.cs
// ============================================================
namespace Domain.Common;

public class Result<V>
{
    private readonly V _value;
    private readonly Error _error;

    public bool Success { get; }

    public V Value => Success
        ? _value
        : throw new InvalidOperationException("No value on failure");

    public Error Error => !Success
        ? _error
        : throw new InvalidOperationException("No error on success");

    private Result(V value)
    {
        Success = true;
        _value = value;
        _error = default!;
    }

    private Result(Error error)
    {
        Success = false;
        _value = default!;
        _error = error;
    }

    public static implicit operator Result<V>(V value) => new(value);
    public static implicit operator Result<V>(Error error) => new(error);

    public Result<U> Cast<U>(Func<V, U>? converter = null)
    {
        if (!Success)
            return new(_error);

        if (converter != null)
            return new(converter(_value));

        if (_value is U valueAsU)
            return new(valueAsU);

        throw new InvalidOperationException(
            $"Cannot convert {typeof(V).Name} to {typeof(U).Name} without converter"
        );
    }

    public TOut Match<TOut>(Func<V, TOut> onSuccess, Func<Error, TOut> onError)
        => Success ? onSuccess(_value) : onError(_error);
}
// ============================================================
// FILE: ./src/Domain/Common/Unit.cs
// ============================================================
namespace Domain.Common;

public readonly struct Unit
{
    public static readonly Unit Value = new();
} 
// ============================================================
// FILE: ./src/Application/Common/Interfaces/ICategoryReaderRepository.cs
// ============================================================
namespace Application.Common.Interfaces;

/// This exists to prevent direct dependency between features:
/// The feature "transactions" need the default repository, so we create this interface
/// We could give the "transactions" feature the full category repository,
/// but this would be too much responsibility over the "category feature"
public interface ICategoryReaderRepository
{
    /// Gets the default category by userid
    Task<Guid?> GetDefaultByUserIdAsync(Guid userId);

    /// Check if a category for the user exists
    Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId);
}
// ============================================================
// FILE: ./src/Application/Common/Interfaces/IUnitOfWork.cs
// ============================================================
namespace Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct);
}
// ============================================================
// FILE: ./src/Application/Common/Errors.cs
// ============================================================
using Domain.Common;

namespace Application.Common;

public sealed record CategoryNotFoundError : Error;
public sealed record DefaultCategoryNotFoundError : Error;

public sealed record TransactionNotFoundError : Error;
// ============================================================
// FILE: ./src/Application/DepdendencyInjection.cs
// ============================================================
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(
            config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly)
        );
        return services;
    }
}
// ============================================================
// FILE: ./src/Application/Features/Categories/CategoryExtension.cs
// ============================================================
using Domain.Entities;

namespace Application.Features.Categories;

public static class CategoryExtension
{
    public static CategoryDto ToDto(this Category category)
        => new(
            category.Id,
            category.UserId,
            category.Name,
            category.IsDefault,
            category.Keywords.Select(k => k.Value).ToList(),
            category.CreatedAt
        );
}
// ============================================================
// FILE: ./src/Application/Features/Categories/CategoryDto.cs
// ============================================================
namespace Application.Features.Categories;

public record CategoryDto(
    Guid Id,
    Guid UserId,
    string Name,
    bool IsDefault,
    List<string> Keywords,
    DateTime CreatedAt
);
// ============================================================
// FILE: ./src/Application/Features/Transactions/Queries/GetTransaction/GetTransactionHandler.cs
// ============================================================
using Application.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransaction;

public class GetTransactionHandler : IRequestHandler<GetTransactionQuery, Result<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepo;

    public GetTransactionHandler(ITransactionRepository transactionRepo)
        => _transactionRepo = transactionRepo;

    public async Task<Result<TransactionDto>> Handle(GetTransactionQuery query, CancellationToken ct)
    {
        var transaction = await _transactionRepo.GetByIdAsync(query.TransactionId);

        if (transaction == null || transaction.UserId != query.UserId)
            return new TransactionNotFoundError();;

        return transaction.ToDto();
    }
}
// ============================================================
// FILE: ./src/Application/Features/Transactions/Queries/GetTransaction/GetTransactionQuery.cs
// ============================================================
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransaction;

public record GetTransactionQuery(
    Guid UserId,
    Guid TransactionId
) : IRequest<Result<TransactionDto>>;
// ============================================================
// FILE: ./src/Application/Features/Transactions/ITransactionRepository.cs
// ============================================================
using Domain.Entities;

namespace Application.Features.Transactions;

/// Transaction specific repository, should not be used across features
public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    void Add(Transaction transaction);
}
// ============================================================
// FILE: ./src/Application/Features/Transactions/Commands/CreateTransaction/CreateTransactionHandler.cs
// ============================================================
using Application.Common;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Result<TransactionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryReaderRepository _categoryReaderRepo;

    public CreateTransactionHandler(
        IUnitOfWork uow,
        ITransactionRepository transactionRepo,
        ICategoryReaderRepository categoryReaderRepo)
    {
        _uow = uow;
        _transactionRepo = transactionRepo;
        _categoryReaderRepo = categoryReaderRepo;
    }

    public async Task<Result<TransactionDto>> Handle(CreateTransactionCommand command, CancellationToken ct)
    {
        Guid categoryId;
        CategorySource categorySource;

        // Category specified -> source = Manual
        if (command.CategoryId.HasValue)
        {
            // Check if the given category exists
            if (!await _categoryReaderRepo.ExistsForUserAsync(command.UserId, command.CategoryId.Value))
                return new CategoryNotFoundError();
            categoryId = command.CategoryId.Value;
            categorySource = CategorySource.Manual;
        }
        // No category specified -> (later auto-categorize) -> source = Unmatched with default category
        else
        {
            // Get default category
            Guid? categoryIdRes = await _categoryReaderRepo.GetDefaultByUserIdAsync(command.UserId);
            if (!categoryIdRes.HasValue)
                return new DefaultCategoryNotFoundError();
            categoryId = categoryIdRes.Value;
            categorySource = CategorySource.Unmatched;
        }

        var domainResult = Transaction.Create(
            command.UserId,
            // TODO: Auto-categorize?
            categoryId,
            categorySource,
            command.Amount,
            command.Type,
            command.Date,
            command.Title,
            command.Description,
            command.CounterParty
        );
        // On failure, map the domain error to an application error
        // For now, the errors are basically the same but we dont want to
        // pass domain errors into the Api layer
        if (!domainResult.Success)
            return domainResult.Cast<TransactionDto>();

        _transactionRepo.Add(domainResult.Value);
        await _uow.SaveChangesAsync(ct);
        return domainResult.Cast(t => t.ToDto());
    }
}
// ============================================================
// FILE: ./src/Application/Features/Transactions/Commands/CreateTransaction/CreateTransactionCommand.cs
// ============================================================
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    Guid UserId,
    Guid? CategoryId,
    decimal Amount,
    TransactionType Type,
    DateOnly Date,
    string Title,
    string Description,
    string CounterParty
) : IRequest<Result<TransactionDto>>;
// ============================================================
// FILE: ./src/Application/Features/Transactions/TransactionExtension.cs
// ============================================================
using Domain.Entities;

namespace Application.Features.Transactions;

public static class TransactionExtension
{
    public static TransactionDto ToDto(this Transaction transaction)
        => new(
            transaction.Id,
            transaction.UserId,
            transaction.CategoryId,
            transaction.CategorySource,
            transaction.Amount,
            transaction.Type,
            transaction.Date,
            transaction.Title,
            transaction.Description,
            transaction.CounterParty,
            transaction.CreatedAt
        );
}
// ============================================================
// FILE: ./src/Application/Features/Transactions/TransactionDto.cs
// ============================================================
using Domain.Enums;

namespace Application.Features.Transactions;

/// Basic response dto. Other dtos like 
/// CreateDto, UpdateDto are now Commands / Queries.
/// Maybe later more Dtos, like TransactionDetailDto
public record TransactionDto(
    Guid Id,
    Guid UserId,
    Guid CategoryId,
    CategorySource CategorySource,
    decimal Amount,
    TransactionType Type,
    DateOnly Date,
    string Title,
    string Description,
    string CounterParty,
    DateTime CreatedAt
);
