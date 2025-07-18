﻿using System.Text;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;

namespace TravelEase.Infrastructure.Persistence.Services.PDFServices
{
    public class InvoiceHtmlGenerator : IInvoiceHtmlGenerator
    {
        public string GenerateHtml(Invoice invoice, string userName)
        {
            var sb = new StringBuilder();

            sb.Append(@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Hotel Invoice</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        th, td {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }
        th {
            background-color: #f2f2f2;
        }
        .total {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <div class=""invoice"">
        <div class=""invoice-header"">
            <h1>Hotel Invoice</h1>
        </div>
        <div class=""invoice-details"">
            <p>Booking Number: <strong>#")
                .Append(invoice.Id).Append(@"</strong></p>
            <p>Booking Date: <strong>")
                .Append(invoice.BookingDate.ToString("yyyy/MM/dd")).Append(@"</strong></p>
            <p>Hotel Name: <strong>")
                .Append(invoice.HotelName).Append(@"</strong></p>
            <p>Guest Name: <strong>")
                .Append(userName).Append(@"</strong></p>
        </div>
        <table class=""invoice-table"">
            <thead>
                <tr>
                    <th>Description</th>
                    <th>Quantity</th>
                    <th>Unit Price</th>
                    <th>Total</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Room Charge</td>
                    <td>1</td>
                    <td>$").Append(invoice.Price).Append(@"</td>
                    <td>$").Append(invoice.Price).Append(@"</td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan=""3"" style=""text-align: right;"">Total:</td>
                    <td>$").Append(invoice.Price).Append(@"</td>
                </tr>
            </tfoot>
        </table>
    </div>
</body>
</html>");

            return sb.ToString();
        }
    }
}