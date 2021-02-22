using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CasaDoCodigo.Core.Repository;
using CasaDoCodigo.Core.Service;
using CasaDoCodigo.EF;
using CasaDoCodigo.Model;
using CasaDoCodigoWeb.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CasaDoCodigoWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            var connectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<CasaDoCodigoDbContext>(options =>
                options
                    .EnableSensitiveDataLogging()
                    //.UseLazyLoadingProxies()
                    .UseSqlServer(connectionString)
            );

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });

            services.AddTransient<ICartService, InSessionCartService>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedDataBase(serviceProvider);
        }
        
        private void SeedDataBase(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetService<CasaDoCodigoDbContext>();
            using (dbContext)
            {
                var alreadySeeded = dbContext.Authors.Any();

                if (alreadySeeded)
                    return;

                SeedDataBaseImpl(dbContext);
            }
        }
        
        private void SeedDataBaseImpl(CasaDoCodigoDbContext context)
        {
            var data = File.ReadAllText("consolidation.json");
            var booksData = JsonConvert.DeserializeObject<BookData[]>(data);

            var categories = GetCategories(booksData).ToList();
            var authors = GetAuthors(booksData).ToList();
            var books = GetBooks(booksData, categories, authors);

            context.Categories.AddRange(categories);
            context.Authors.AddRange(authors);
            context.Books.AddRange(books);

            context.SaveChanges();
        }

        private static IEnumerable<Book> GetBooks(IEnumerable<BookData> booksData, List<Category> categories, List<Author> authors)
        {
            var booksQuery =
                from book in booksData
                let bookAuthors = book.Author.Split(",")
                let category = string.IsNullOrEmpty(book.Subcategory) ? book.Category : book.Subcategory
                select (new Book
                {
                    Category = categories.First(c => c.Name.Equals(category, StringComparison.OrdinalIgnoreCase)),
                    CoverUri = book.Img,
                    Title = book.Title,
                    SubTitle = book.Sub,
                    Summary = book.Content,
                    DisplayName = book.NomeExibicao,
                    Price = 5,
                    PublishDate = new DateTime(2010, 1, 1),
                    UpdateDate = new DateTime(2010, 1, 1)
                }, authors.Where(a => bookAuthors.Contains(a.Name)).ToList());

            foreach (var item in booksQuery)
            {
                item.Item1.BookAuthors = item.Item2.Select(a => new BookAuthorJoin { Author = a, Book = item.Item1 }).ToList();
                yield return item.Item1;
            }
        }

        private static IEnumerable<Author> GetAuthors(BookData[] books)
        {
            var authorsQuery =
                from book in books
                where !string.IsNullOrEmpty(book.Author)
                let names = book.Author.Split(",")
                from name in names
                select name.Trim();

            var authors =
                authorsQuery
                    .GroupBy(a => a)
                    .Select(g => g.First())
                    .Select(a => new Author { Name = a });

            return authors;
        }

        private static IEnumerable<Category> GetCategories(BookData[] books)
        {
            var categoriesQuery =
                            from book in books
                            let c = book.Category.ToLower()
                            let category = Char.ToUpper(c[0]) + c.Substring(1)
                            select category;

            var categories =
                categoriesQuery
                    .GroupBy(c => c)
                    .Select(g => new Category { Name = g.First() })
                    .ToList();

            var subCategoriesQuery =
                from book in books
                where !string.IsNullOrEmpty(book.Subcategory)
                let subC = book.Subcategory.ToLower()
                let subCategory = Char.ToUpper(subC[0]) + subC.Substring(1)
                let category = categories.First(cat => string.Equals(cat.Name, book.Category, StringComparison.OrdinalIgnoreCase))
                select (Name: subCategory, Parent: category);

            var subCategories =
                subCategoriesQuery
                    .GroupBy(b => b.Name)
                    .Select(g =>  g.First())
                    .Select(c => new Category { Name = c.Name, Parent = c.Parent });

            return categories.Concat(subCategories);
        }
    }

    class BookData
    {
        public string Author { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public string Img { get; set; }
        public string Nome { get; set; }
        public string NomeExibicao { get; set; }
        public string Sub { get; set; }
        public string Subcategory { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
