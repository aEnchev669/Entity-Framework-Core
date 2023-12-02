using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ExportClientsInvoicesDto
    {
        public string ClientName { get; set; }

        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; }
        public string VatNumber { get; set; }
        [XmlArray("Invoices")]
        public virtual ExportInvoiceDto[] Invoices { get; set; } 

    }
}
