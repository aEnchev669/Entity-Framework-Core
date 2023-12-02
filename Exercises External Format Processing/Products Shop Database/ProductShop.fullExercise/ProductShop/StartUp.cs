using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            //string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");

            string result = GetUsersWithProducts(context);
            Console.WriteLine(result);

        }

        // Problem 01
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportUserDto[] userDtos =
                JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);


            ICollection<User> validUsers = new HashSet<User>();
            foreach (ImportUserDto userDto in userDtos)
            {
                User user = mapper.Map<User>(userDto);

                validUsers.Add(user);
            }


            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }

        //Problem 02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportProductDto[] productDtos =
                JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);
            Product[] products = mapper.Map<Product[]>(productDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //Problem 03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();


            ImportCategoryDto[] categoryDtos =
                JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);

            ICollection<Category> validCategories = new HashSet<Category>();
            foreach (ImportCategoryDto categoryDto in categoryDtos)
            {
                if (String.IsNullOrEmpty(categoryDto.Name))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryDto);
                validCategories.Add(category);
            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        //problem 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryProductDto[] cpDtos =
                JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);

            ICollection<CategoryProduct> validEntries = new HashSet<CategoryProduct>();
            foreach (ImportCategoryProductDto cpDto in cpDtos)
            {

                //if (!context.Categories.Any(c => c.Id == cpDto.CategoryId) ||
                //    !context.Products.Any(p => p.Id == cpDto.ProductId))
                //{
                //    continue;
                //}

                CategoryProduct categoryProduct =
                    mapper.Map<CategoryProduct>(cpDto);
                validEntries.Add(categoryProduct);
            }

            context.CategoriesProducts.AddRange(validEntries);
            context.SaveChanges();

            return $"Successfully imported {validEntries.Count}";
        }

        //problem 05 With anonymous object
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName,
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        //problem 05 With Dto
        //Remove "WithDto" from the method name
        public static string GetProductsInRangeWithDto(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            ExportProductInRangeDto[] productsDto = context
                 .Products
                 .Where(p => p.Price >= 500 && p.Price <= 1000)
                 .OrderBy(p => p.Price)
                 .AsNoTracking()
                 .ProjectTo<ExportProductInRangeDto>(mapper.ConfigurationProvider)
                 .ToArray();

            return JsonConvert.SerializeObject(productsDto, Formatting.Indented);

        }

        //Problem 06 With anonymous object
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(sp => sp.LastName)
                .ThenBy(sp => sp.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                            .Where(sp => sp.Buyer != null)
                            .Select(sp => new
                            {
                                name = sp.Name,
                                price = sp.Price,
                                buyerFirstName = sp.Buyer.FirstName,
                                buyerLastName = sp.Buyer.LastName,
                            })
                            .ToArray(),
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        //public static string GetSoldProducts(ProductShopContext context)
        //{
        //    IContractResolver contractResolver = ConfigureCamelCaseNaming();

        //    var usersWithSoldProducts = context.Users
        //        .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
        //        .OrderBy(u => u.LastName)
        //        .ThenBy(u => u.FirstName)
        //        .Select(u => new
        //        {
        //            u.FirstName,
        //            u.LastName,
        //            SoldProducts = u.ProductsSold
        //                .Where(p => p.Buyer != null)
        //                .Select(p => new
        //                {
        //                    p.Name,
        //                    p.Price,
        //                    BuyerFirstName = p.Buyer.FirstName,
        //                    BuyerLastName = p.Buyer.LastName
        //                })
        //                .ToArray()
        //        })
        //        .AsNoTracking()
        //        .ToArray();

        //    return JsonConvert.SerializeObject(usersWithSoldProducts,
        //        Formatting.Indented);
        //}

        //Problem 07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count())
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count,
                    averagePrice = (c.CategoriesProducts.Any() ?
                           c.CategoriesProducts.Average(cp => cp.Product.Price) : 0).ToString("f2"),
                    totalRevenue = (c.CategoriesProducts.Any() ?
                           c.CategoriesProducts.Sum(cp => cp.Product.Price) : 0).ToString("f2"),
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }

        //Problem 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count())
                .Select(u => new
                {
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(u => u.Buyer != null),
                        products = u.ProductsSold
                                     .Where(p => p.Buyer != null)
                                     .Select(p => new
                                     {
                                         name = p.Name,
                                         price = p.Price,
                                     })
                                     .ToArray()
                    }

                })
                .AsNoTracking()
                .ToArray();

            var userWrappedDto = new
            {
                usersCount = users.Length,
                users = users,
            };

            return JsonConvert.SerializeObject(userWrappedDto, Formatting.Indented);


        }

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }
        private static IContractResolver ConfigureCamelCaseNaming()
        {
            return new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(false, true)
            };
        }
    }
}