namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Extensions;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            ImportClientDto[] clientDtos = xmlString
                .DeserializeFromXml<ImportClientDto[]>("Clients");

            List<Client> validClients = new List<Client>();
            foreach (ImportClientDto clientDto in clientDtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                var client = new Client
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,

                };


                foreach (ImportAddressDto addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    client.Addresses.Add(new Address()
                    {
                        StreetName = addressDto.StreetName,
                        StreetNumber = addressDto.StreetNumber,
                        PostCode = addressDto.PostCode,
                        City = addressDto.City,
                        Country = addressDto.Country,
                    });
                }

                validClients.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClients, client.Name));
            }

            context.Clients.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            ImportInvoiceDto[] invoiceDtos = jsonString.DeserializeFromJson<ImportInvoiceDto[]>();

            List<Invoice> validInvoices = new List<Invoice>();

            StringBuilder sb = new StringBuilder();

            foreach (var invoiceDto in invoiceDtos)
            {
                if (!IsValid(invoiceDto) || invoiceDto.IssueDate > invoiceDto.DueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var invoice = new Invoice
                {
                    Number = invoiceDto.Number,
                    IssueDate = invoiceDto.IssueDate,
                    DueDate = invoiceDto.DueDate,
                    ClientId = invoiceDto.ClientId,
                    Amount = invoiceDto.Amount,
                    CurrencyType = invoiceDto.CurrencyType,
                };

                validInvoices.Add(invoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
            }

            context.Invoices.AddRange(validInvoices);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            ImportProductDto[] productDtos = jsonString.DeserializeFromJson<ImportProductDto[]>();

            List<Product> validProducts = new List<Product>();

            StringBuilder sb = new StringBuilder();

            int[] clientIds = context.Clients.Select(c => c.Id).ToArray();

            foreach (var productDto in productDtos)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product productToAdd = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = productDto.CategoryType
                };

                foreach (var client in productDto.Clients.Distinct())
                {
                    if (!clientIds.Contains(client))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    productToAdd.ProductsClients.Add(new ProductClient()
                    {
                        ClientId = client
                    });

                }
                validProducts.Add(productToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, productToAdd.Name, productToAdd.ProductsClients.Count));
            }
            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
