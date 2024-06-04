using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFGetStarted
{
    /// <summary>
    /// Фабрика классов для BloggingContext. Нужна для работы инструментов EF Core.
    /// </summary>
    public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    {
        public BloggingContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<BloggingContext> optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
            optionsBuilder.UseSqlite("Data Source=blogging.db");
            return new BloggingContext(optionsBuilder.Options);
        }
    }

    /// <summary>
    /// Класс для доступа к базе данных (контекст).
    /// </summary>
    public class BloggingContext : DbContext
    {
        /// <summary>
        /// Таблица с блогами.
        /// </summary>
        public DbSet<Blog> Blogs { get; set; }

        /// <summary>
        /// Таблица с сообщениями.
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Файл БД SQLITE.
        /// </summary>
        public static string DbFile => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "blogging.db");

        public BloggingContext() : base()
        {
        }

        public BloggingContext(DbContextOptions<BloggingContext> options) : base(options)
        {
        }
    }

    /// <summary>
    /// Модель таблицы с блогами.
    /// </summary>
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new();
    }

    /// <summary>
    /// Модель таблицы с сообщениями.
    /// </summary>
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
