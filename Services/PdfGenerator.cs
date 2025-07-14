using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.IO;

// Alias the two conflicting types to keep the code crystal clear:
using QContainer = QuestPDF.Infrastructure.IContainer;
using QColors = QuestPDF.Helpers.Colors;
using QuestPDF.Helpers;

namespace BillingMAUIDesktopApp.Services
{
    /// <summary>
    /// Generates a simple invoice PDF and saves it to local storage.
    /// </summary>
    public class PdfGenerator
    {
        /// <summary>
        /// Creates the PDF and returns the saved file path.
        /// </summary>
        public string GenerateInvoice()
        {
            // Save under the app's data folder so no permissions issues on Windows
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "invoice.pdf");

            Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    // === PAGE SETUP ===
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(QColors.White);                // Use PDF white
                    page.DefaultTextStyle(x => x.FontSize(12));   // Base font size

                    // === HEADER ===
                    page.Header()
                        .Text("Invoice – ACME Corporation")
                        .FontSize(20)
                        .SemiBold()
                        .FontColor(QColors.Blue.Medium);

                    // === CONTENT ===
                    // Wrap our method group in a lambda so the compiler
                    // picks the overload that takes Action<IContainer>
                    page.Content().Element(c => ComposeContent(c));

                    // === FOOTER ===
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Thank you for your business! ").FontSize(10);
                            x.Span("Generated on: " + DateTime.Now.ToShortDateString())
                             .FontSize(10);
                        });
                });
            })
            .GeneratePdf(filePath); // Actually write the file

            return filePath;
        }

        /// <summary>
        /// Draws the billing details and a sample table of services.
        /// </summary>
        void ComposeContent(QContainer container)
        {
            container.PaddingVertical(10)
                     .Column(column =>
                     {
                         column.Spacing(10); // Space out each section

                         // === BILL TO BLOCK ===
                         // Verbatim string (@) for multi-line text
                         column.Item()
                               .Text(@"Bill To: John Doe
123 Main Street
Cityville, Country")
                               .FontSize(12);

                         // === SERVICES TABLE ===
                         column.Item().Table(table =>
                         {
                             // Define three equally-sized columns
                             table.ColumnsDefinition(cols =>
                             {
                                 cols.RelativeColumn();
                                 cols.RelativeColumn();
                                 cols.RelativeColumn();
                             });

                             // Header row
                             table.Header(header =>
                             {
                                 header.Cell().Element(c => CellStyle(c)).Text("Date");
                                 header.Cell().Element(c => CellStyle(c)).Text("Description");
                                 header.Cell().Element(c => CellStyle(c))
                                               .AlignRight()
                                               .Text("Amount");
                             });

                             // Four example rows
                             for (int i = 0; i < 4; i++)
                             {
                                 table.Cell().Element(c => CellStyle(c))
                                              .Text(DateTime.Now.AddDays(-i)
                                                               .ToShortDateString());

                                 table.Cell().Element(c => CellStyle(c))
                                              .Text($"Service Item {i + 1}");

                                 table.Cell().Element(c => CellStyle(c))
                                              .AlignRight()
                                              .Text("$100");
                             }
                         });

                         // === TOTALS ===
                         column.Item().AlignRight().Text("Subtotal: $400");
                         column.Item().AlignRight().Text("VAT (0%): $0");
                         column.Item().AlignRight()
                               .Text("Total: $400")
                               .Bold(); // Bold only applies after Text(...)
                     });
        }

        /// <summary>
        /// Styles each table cell with padding and a bottom border.
        /// </summary>
        QContainer CellStyle(QContainer c) =>
            c.Padding(5)
             .BorderBottom(1)
             .BorderColor(QColors.Grey.Lighten2);
    }
}
