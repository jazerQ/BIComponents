using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class BiContext : DbContext, IAppDbContext
{
    public BiContext()
    {
    }

    public BiContext(DbContextOptions<BiContext> options)
        : base(options)
    {
        Database.Migrate(); // автоматически применяем миграции
    }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<TypeOfModel> Types { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Article).HasName("models_pkey");

            entity.ToTable("models");

            entity.Property(e => e.Article)
                .ValueGeneratedNever()
                .HasColumnName("article");
            entity.Property(e => e.ActionRadius)
                .HasMaxLength(255)
                .HasColumnName("action_radius");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.PartNumber)
                .HasMaxLength(255)
                .HasColumnName("part_number");
            entity.Property(e => e.Peculiarities)
                .HasMaxLength(255)
                .HasColumnName("peculiarities");
            entity.Property(e => e.Scale)
                .HasMaxLength(255)
                .HasColumnName("scale");
            entity.Property(e => e.Transport)
                .HasMaxLength(255)
                .HasColumnName("transport");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.TypeOfControl)
                .HasMaxLength(255)
                .HasColumnName("type_of_control");

            entity.HasOne(d => d.ArticleNavigation).WithOne(p => p.Model)
                .HasPrincipalKey<Product>(p => p.Article)
                .HasForeignKey<Model>(d => d.Article)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("models_article_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Models)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("models_type_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.HasIndex(e => e.Article, "products_article_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Article)
                .IsRequired()
                .HasColumnName("article");
            entity.Property(e => e.Countofcomments).HasColumnName("countofcomments");
            entity.Property(e => e.Countofquestions).HasColumnName("countofquestions");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasColumnName("country");
            entity.Property(e => e.Defaultprice).HasColumnName("defaultprice");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Pricewithcard).HasColumnName("pricewithcard");
            entity.Property(e => e.Productrating).HasColumnName("productrating");
        });

        modelBuilder.Entity<TypeOfModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("types_pkey");

            entity.ToTable("types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type1)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
