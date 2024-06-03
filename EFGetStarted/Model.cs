using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFGetStarted
{
    /// <summary>
    /// ???
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
        public string DbFile { get; }

        public BloggingContext()
        {
            //настройка полного имени файла БД SQLITE
            DbFile = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "blogging.db");
        }

        //public BloggingContext(DbContextOptions<BloggingContext> options) : base(options)
        //{
        //}

        //создать файл БД SQLITE в папке локальных данных программ
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbFile}");
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
