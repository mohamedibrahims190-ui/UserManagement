using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(
    string? name,
    string? gender,
    string? country,
    string? nationality,
    string? status)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(u => u.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                users = users.Where(u => u.Gender == gender);
            }

            if (!string.IsNullOrEmpty(country))
            {
                users = users.Where(u => u.Country.Contains(country));
            }

            if (!string.IsNullOrEmpty(nationality))
            {
                users = users.Where(u => u.Nationality.Contains(nationality));
            }
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Active")
                {
                    users = users.Where(u => u.IsActive);
                }
                else if (status == "Inactive")
                {
                    users = users.Where(u => !u.IsActive);
                }
            }

            return View(users.ToList());
        }

        public IActionResult DownloadPdf(
    string? name,
    string? gender,
    string? country,
    string? nationality,
    string? status)
        {
            var users = _context.Users.AsQueryable();

            // Name filter
            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(u => u.Name.Contains(name));
            }

            // Gender filter
            if (!string.IsNullOrEmpty(gender))
            {
                users = users.Where(u => u.Gender == gender);
            }

            // Country filter
            if (!string.IsNullOrEmpty(country))
            {
                users = users.Where(u => u.Country.Contains(country));
            }

            // Nationality filter
            if (!string.IsNullOrEmpty(nationality))
            {
                users = users.Where(u => u.Nationality.Contains(nationality));
            }

            // Status filter
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Active")
                {
                    users = users.Where(u => u.IsActive);
                }
                else if (status == "Inactive")
                {
                    users = users.Where(u => !u.IsActive);
                }
            }

            var userList = users.ToList();

            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    // Header
                    page.Header()
                        .Text("User Report")
                        .FontSize(20)
                        .Bold();

                    // Content
                    page.Content()
                        .Table(table =>
                        {
                            // 6 columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Table Header
                            table.Header(header =>
                            {
                                header.Cell()
                                    .Element(CellStyle)
                                    .Text("Name");

                                header.Cell()
                                    .Element(CellStyle)
                                    .Text("Email");

                                header.Cell()
                                    .Element(CellStyle)
                                    .Text("Gender");

                                header.Cell()
                                    .Element(CellStyle)
                                    .Text("Country");

                                header.Cell()
                                    .Element(CellStyle)
                                    .Text("Nationality");

                                header.Cell()
                                    .Element(CellStyle)
                                    .Text("Status");
                            });

                            // Table Data
                            foreach (var user in userList)
                            {
                                table.Cell()
                                    .Element(CellStyle)
                                    .Text(user.Name);

                                table.Cell()
                                    .Element(CellStyle)
                                    .Text(user.Email);

                                table.Cell()
                                    .Element(CellStyle)
                                    .Text(user.Gender);

                                table.Cell()
                                    .Element(CellStyle)
                                    .Text(user.Country);

                                table.Cell()
                                    .Element(CellStyle)
                                    .Text(user.Nationality);

                                table.Cell()
                                    .Element(CellStyle)
                                    .Text(user.IsActive ? "Active" : "Inactive");
                            }
                        });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated on {DateTime.Now:dd-MM-yyyy HH:mm}");
                });
            });

            var pdfBytes = document.GeneratePdf();

            return File(
                pdfBytes,
                "application/pdf",
                "UserReport.pdf");
        }
        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .Padding(5);
        }

        public IActionResult Print(
    string? name,
    string? gender,
    string? country,
    string? nationality,
    string? status)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(u => u.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                users = users.Where(u => u.Gender == gender);
            }

            if (!string.IsNullOrEmpty(country))
            {
                users = users.Where(u => u.Country.Contains(country));
            }

            if (!string.IsNullOrEmpty(nationality))
            {
                users = users.Where(u => u.Nationality.Contains(nationality));
            }
            // Status filter
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Active")
                {
                    users = users.Where(u => u.IsActive);
                }
                else if (status == "Inactive")
                {
                    users = users.Where(u => !u.IsActive);
                }
            }
            return View(users.ToList());
        }

        public IActionResult DownloadExcel(
            string? name,
            string? gender,
            string? country,
            string? nationality,
            string? status)
        {
            var users = _context.Users.AsQueryable();

            // Name filter
            if (!string.IsNullOrWhiteSpace(name))
            {
                users = users.Where(u => u.Name.Contains(name));
            }

            // Gender filter
            if (!string.IsNullOrWhiteSpace(gender))
            {
                users = users.Where(u => u.Gender == gender);
            }

            // Country filter
            if (!string.IsNullOrWhiteSpace(country))
            {
                users = users.Where(u => u.Country.Contains(country));
            }

            // Nationality filter
            if (!string.IsNullOrWhiteSpace(nationality))
            {
                users = users.Where(u => u.Nationality.Contains(nationality));
            }

            // Status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status == "Active")
                {
                    users = users.Where(u => u.IsActive);
                }
                else if (status == "Inactive")
                {
                    users = users.Where(u => !u.IsActive);
                }
            }

            var userList = users.ToList();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Users");

            // Title
            worksheet.Cell(1, 1).Value = "USER REPORT";
            worksheet.Range(1, 1, 1, 6).Merge();

            worksheet.Cell(2, 1).Value =
                $"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}";

            worksheet.Range(2, 1, 2, 6).Merge();

            worksheet.Cell(3, 1).Value =
                $"Total Records: {userList.Count}";

            worksheet.Range(3, 1, 3, 6).Merge();

            // Headers
            worksheet.Cell(5, 1).Value = "Name";
            worksheet.Cell(5, 2).Value = "Email";
            worksheet.Cell(5, 3).Value = "Gender";
            worksheet.Cell(5, 4).Value = "Country";
            worksheet.Cell(5, 5).Value = "Nationality";
            worksheet.Cell(5, 6).Value = "Status";

            // Data
            int row = 6;

            foreach (var user in userList)
            {
                worksheet.Cell(row, 1).Value = user.Name;
                worksheet.Cell(row, 2).Value = user.Email;
                worksheet.Cell(row, 3).Value = user.Gender;
                worksheet.Cell(row, 4).Value = user.Country;
                worksheet.Cell(row, 5).Value = user.Nationality;
                worksheet.Cell(row, 6).Value =
                    user.IsActive ? "Active" : "Inactive";

                row++;
            }

            // Formatting
            var headerRange = worksheet.Range(5, 1, 5, 6);

            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;

            worksheet.Columns().AdjustToContents();

            // Create Excel file
            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            var content = stream.ToArray();

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "UserReport.xlsx");
        }
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }
    }
}