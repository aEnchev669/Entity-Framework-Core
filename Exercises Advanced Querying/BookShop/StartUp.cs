namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Globalization;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            // DbInitializer.ResetDatabase(db);

            string givenYear = Console.ReadLine();
            string result = GetAuthorNamesEndingIn(db, givenYear);

            Console.WriteLine(result);
        }
        //Problem 02
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            bool tryToParse = Enum.TryParse(typeof(AgeRestriction), command, true, out object? ageObj);
            AgeRestriction ageRestriction;
            if (tryToParse)
            {
                ageRestriction = (AgeRestriction)ageObj;

                var books = context.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();

                return string.Join("\n", books);
            }

            return null;
        }
        //Problem 03
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join("\n", books);
        }

        //Problem 04
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 05
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate!.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join("\n", books);
        }
        //Problem 06
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            var books = context.Books
                .Where(b => b.BookCategories
                     .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join("\n", books);
        }
        //Problem 07
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            DateTime givenDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < givenDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    Price = b.Price.ToString("f2")
                })
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();

        }
        //Problem 08
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors  = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                   FullName = a.FirstName + ' ' + a.LastName
                   
                })
                .OrderBy(a => a.FullName)
                .ToArray();

            foreach (var a in authors)
            {
                sb.AppendLine(a.FullName);
            }
           
            return sb.ToString().TrimEnd();
        }
    }

}


