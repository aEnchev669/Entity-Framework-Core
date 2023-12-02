namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Extensions;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var clientDtos = context.Clients
                .Where(c => c.Invoices.Any(i => i.IssueDate > date))
                .Select(c => new ExportClientsInvoicesDto()
                {
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    InvoicesCount = c.Invoices.Count,
                    Invoices = c.Invoices
                            .OrderBy(i => i.IssueDate)
                            .ThenByDescending(i => i.DueDate)
                            .Select(i => new ExportInvoiceDto()
                            {
                                InvoiceNumber = i.Number,
                                DueDate = i.DueDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                                Currency = i.CurrencyType.ToString(),
                                InvoiceAmount = decimal.Parse(i.Amount.ToString("0.##"))
                            })
                            .ToArray()
                })
                .OrderByDescending(c => c.InvoicesCount)
                .ThenBy(c => c.ClientName)
                .ToArray();

            return clientDtos.SerializeToXml<ExportClientsInvoicesDto[]>("Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products
                .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
                //.Include(p => p.ProductsClients)
                //.ThenInclude(pc => pc.Client)
                .ToArray()
                .Select(p => new ExportProductDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.CategoryType,
                    Clients = p.ProductsClients
                          .Where(pc => pc.Client.Name.Length >= nameLength)
                          .ToArray()
                          .Select(pc => new ExportClientDto()
                          {
                              Name = pc.Client.Name,
                              NumberVat = pc.Client.NumberVat,
                          })
                          .OrderBy(p => p.Name)
                          .ToArray()
                })
                .OrderByDescending(p => p.Clients.Count())
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            return products.SerializeToJson<ExportProductDto[]>();

        }
    }
}