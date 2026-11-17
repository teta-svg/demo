using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace demo.Context;

public partial class DemoDbContext : DbContext
{
    public DemoDbContext()
    {
    }

    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Production> Productions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<StockItem> StockItems { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9CLF3RH\\SQLEXPRESS;Database=DemoDb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_100_CI_AI_KS_SC_UTF8");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.ToTable("Category");

            entity.Property(e => e.IdCategory)
                .ValueGeneratedNever()
                .HasColumnName("ID_Category");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder);

            entity.ToTable("Order");

            entity.Property(e => e.IdOrder)
                .ValueGeneratedNever()
                .HasColumnName("ID_Order");
            entity.Property(e => e.IdSize).HasColumnName("ID_Size");
            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.OrderDate).HasColumnName("Order_Date");
            entity.Property(e => e.OrderNumber).HasColumnName("Order_Number");

            entity.HasOne(d => d.IdSizeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdSize)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Size");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => e.IdOrderProduct);

            entity.ToTable("Order_Product");

            entity.Property(e => e.IdOrderProduct)
                .ValueGeneratedNever()
                .HasColumnName("ID_Order_Product");
            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Product_Order");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Product_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct);

            entity.ToTable("Product");

            entity.Property(e => e.IdProduct)
                .ValueGeneratedNever()
                .HasColumnName("ID_Product");
            entity.Property(e => e.Composition).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IdCategory).HasColumnName("ID_Category");
            entity.Property(e => e.IdProduction).HasColumnName("ID_Production");
            entity.Property(e => e.IdSubCategory).HasColumnName("ID_Sub_Category");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("Product_Name");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.IdProductionNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdProduction)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Production");

            entity.HasOne(d => d.IdSubCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSubCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Sub_Category");
        });

        modelBuilder.Entity<Production>(entity =>
        {
            entity.HasKey(e => e.IdProduction);

            entity.ToTable("Production");

            entity.Property(e => e.IdProduction)
                .ValueGeneratedNever()
                .HasColumnName("ID_Production");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole);

            entity.ToTable("Role");

            entity.Property(e => e.IdRole)
                .ValueGeneratedNever()
                .HasColumnName("ID_Role");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("Role_Name");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.IdSize);

            entity.ToTable("Size");

            entity.Property(e => e.IdSize)
                .ValueGeneratedNever()
                .HasColumnName("ID_Size");
            entity.Property(e => e.Size1)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Size");
        });

        modelBuilder.Entity<StockItem>(entity =>
        {
            entity.HasKey(e => e.IdStockItem);

            entity.ToTable("Stock_Item");

            entity.Property(e => e.IdStockItem)
                .ValueGeneratedNever()
                .HasColumnName("ID_Stock_Item");
            entity.Property(e => e.IdProduct).HasColumnName("ID_Product");
            entity.Property(e => e.IdProduction).HasColumnName("ID_Production");
            entity.Property(e => e.IdSize).HasColumnName("ID_Size");
            entity.Property(e => e.QuantityAvailable).HasColumnName("Quantity_Available");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Item_Product");

            entity.HasOne(d => d.IdProductionNavigation).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.IdProduction)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Item_Production");

            entity.HasOne(d => d.IdSizeNavigation).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.IdSize)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Item_Size");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.IdSubCategory);

            entity.ToTable("Sub_Category");

            entity.Property(e => e.IdSubCategory)
                .ValueGeneratedNever()
                .HasColumnName("ID_Sub_Category");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.ToTable("User");

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("ID_User");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("First_Name");
            entity.Property(e => e.IdRole).HasColumnName("ID_Role");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("Last_Name");
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(255)
                .HasColumnName("Middle_Name");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
