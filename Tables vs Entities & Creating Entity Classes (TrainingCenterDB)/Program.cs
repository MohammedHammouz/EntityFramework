using ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace Tables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // Look for files in current folder
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load appsettings.json
    .Build();
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("Connection string 'DefaultConnection' was not found.");
                return;
            }
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .LogTo(Console.WriteLine,LogLevel.Information)
                .EnableSensitiveDataLogging()
    .UseSqlServer(connectionString) // Tell EF Core to use SQL Server with this connection string
    .Options;
            using var context = new AppDBContext(options);
            Console.WriteLine("================================================");

            Console.WriteLine(context.Database.CanConnect()
                ? "Connected! You are ready to retrieve Data :-)"
                : "Failed to connect.");

            Console.WriteLine("================================================");
            PrintAndRetrieveData(context);
            GetActiveStudents(context);
        }
        public static void PrintAndRetrieveData(AppDBContext context)
        {
        //    var query = context.Students
        //        .OrderBy(s => s.StudentID);
        //    PrintGeneratedSql("Students", query.ToQueryString());
        //    Console.WriteLine("Preview SQL using ToQueryString():");
        //    Console.WriteLine("----------------------------------");
        //    Console.WriteLine(query.ToQueryString());
        //    Console.WriteLine();
        //    var students = query.ToList();

        //    Console.WriteLine(
        //$"Rows Returned: {students.Count}");
        //    Console.WriteLine();
        //    Console.WriteLine(new string('=', 70));
        //    Console.WriteLine();
        //    foreach (var student in students)
        //    {
        //        Console.WriteLine(
        //            $"Id: {student.StudentID}, " +
        //            $"Name: {student.FirstName} {student.LastName}, " +
        //            $"Email: {student.Email}, " +
        //            $"Status: {student.Status}, " +
        //            $"Phone: {student.PhoneNumber ?? "N/A"}");
        //    }
        //    var query1 = context.Enrollments.Where(e => e.Status == "Completed");
        //    var enrollments = query1.ToList();
        //    Console.WriteLine(query1.ToQueryString());
        //    var query2 = context.Instructors
        //        .Where(i => i.Salary > 5000)
        //        .Select(i => i.InstructorID);
        //    var isntructors = query2
        //        .ToList();
        //    Console.WriteLine(query2.ToQueryString());
            var query3 = context.Instructors.Where(i => i.IsActive==true);
            var instructors = query3.ToList();
            Console.WriteLine(query3.ToQueryString());
        }
        public static void GetActiveStudents(AppDBContext context)
        {
            var query = context.Students
                .Where(s => s.Status == "Active")
                .OrderBy(s=>s.StudentID);
            var students = query.ToList();
            PrintGeneratedSql("Students", query.ToQueryString());
            // Print results
            Console.WriteLine("\nActive Students:");
            Console.WriteLine("----------------");

            foreach (var student in students)
            {
                Console.WriteLine($"{student.StudentID} - {student.FirstName} {student.LastName}");
            }

            Console.WriteLine();
            Console.WriteLine($"Total Active Students: {students.Count}");

        }
        public static void PrintGeneratedSql(string tableName,string sqlQuery)
        {
            Console.WriteLine($"Generated SQL Query for {tableName}:");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(sqlQuery);
            Console.WriteLine();
        }
    }
}
