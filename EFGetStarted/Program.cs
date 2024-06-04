using EFGetStarted.Migrations;
using Microsoft.EntityFrameworkCore;

namespace EFGetStarted
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //настройка объекта для доступа к базе данных
            DbContextOptions<BloggingContext> contextOptions = new DbContextOptionsBuilder<BloggingContext>()
                .UseSqlite($"Data Source={BloggingContext.DbFile}")
                .Options;
            Console.WriteLine($"Путь к базе данных: {BloggingContext.DbFile}.");

            //осздание объекта для доступа к базе данных - создание нового файла БД либо подключение к существующему
            using BloggingContext db = new(contextOptions);

            //применение миграций к базе данных
            db.Database.Migrate();
            
            //создание нового блога
            Console.WriteLine("Создание нового блога");
            db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            db.SaveChanges();

            //чтение блога
            Console.WriteLine("Запрос последнего созданного блога");
            Blog blog = db.Blogs
                .OrderByDescending(b => b.BlogId)
                .First();

            //обновление блога и добавление сообщения
            Console.WriteLine("Обновление блога и добавление сообщения");
            blog.Url = "https://devblogs.microsoft.com/dotnet";
            blog.Posts.Add(
                new Post
                {
                    Title = "Hello World",
                    Content = "I wrote an app using EF Core!"
                });
            db.SaveChanges();

            ////уничтожение блога
            //Console.WriteLine("Уничтожение блога");
            //db.Remove(blog);
            //db.SaveChanges();

            Console.ReadLine();
        }
    }
}
