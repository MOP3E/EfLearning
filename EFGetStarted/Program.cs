using Microsoft.EntityFrameworkCore;

namespace EFGetStarted
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dbFile = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "blogging.db");

            DbContextOptions<BloggingContext> contextOptions = new DbContextOptionsBuilder<BloggingContext>()
                .UseSqlite($"Data Source={dbFile}")
                .Options;

            //объект для доступа к базе данных
            using BloggingContext db = new(contextOptions);

            //ВАЖНО: в этом примере ожидается, что база данных уже создана и существует
            Console.WriteLine($"Путь к базе данных: {dbFile}.");

            //создание содержимого базы данных
            db.Database.ExecuteSqlRaw(
                "CREATE TABLE IF NOT EXISTS \"__EFMigrationsHistory\" (\r\n    \"MigrationId\" TEXT NOT NULL CONSTRAINT \"PK___EFMigrationsHistory\" PRIMARY KEY,\r\n    \"ProductVersion\" TEXT NOT NULL\r\n);\r\n\r\nBEGIN TRANSACTION;\r\n\r\nCREATE TABLE \"Blogs\" (\r\n    \"BlogId\" INTEGER NOT NULL CONSTRAINT \"PK_Blogs\" PRIMARY KEY AUTOINCREMENT,\r\n    \"Url\" TEXT NULL\r\n);\r\n\r\nCREATE TABLE \"Posts\" (\r\n    \"PostId\" INTEGER NOT NULL CONSTRAINT \"PK_Posts\" PRIMARY KEY AUTOINCREMENT,\r\n    \"Title\" TEXT NULL,\r\n    \"Content\" TEXT NULL,\r\n    \"BlogId\" INTEGER NOT NULL,\r\n    CONSTRAINT \"FK_Posts_Blogs_BlogId\" FOREIGN KEY (\"BlogId\") REFERENCES \"Blogs\" (\"BlogId\") ON DELETE CASCADE\r\n);\r\n\r\nCREATE INDEX \"IX_Posts_BlogId\" ON \"Posts\" (\"BlogId\");\r\n\r\nINSERT INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\")\r\nVALUES ('20240603105431_InitialCreate', '8.0.6');\r\n\r\nCOMMIT;\r\n\r\n"
            );

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

        }
    }
}
