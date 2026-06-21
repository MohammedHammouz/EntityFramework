using ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Data;
using ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlTypes;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
            //PrintAndRetrieveData(context);
            //GetActiveStudents(context);
            //Example_First(context);
            //Example_First(context);
            //Example_Single(context);
            //Example_SingleOrDefault(context);
            //GetStudentByIdUsingFirstOrDefault(context);
            //GetStudentByIdUsingFind(context);
            //GetFilteredStudents(context);
            //GetFilteredStudents(context);
            //CheckData(context);
            //ShowStudentsPerStatusReport(context);
            ShowStudentsWithEnrollmentsAndCourses(context);
        }
        public static void PrintAndRetrieveData(AppDBContext context)
        {
        
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
        public static void Example_First(AppDBContext context)
        {
            Console.WriteLine("\nExample 1 - First()");
            Console.WriteLine("-------------------");
            // Build query first(no execution yet)
            var query = context.Students
                .Where(s => s.Status == "Active")
                .OrderBy(s => s.StudentID);
            // Preview query shape
            PreviewSQLUsingToQueryString(query.ToQueryString());

            // Execute query
            // Runtime logging will show the actual executed SQL.
            var student = query.First();
            Console.WriteLine("\nFirst Active Student:");
            Console.WriteLine($"{student.StudentID} - {student.FirstName} {student.LastName}");
            
        }
            /// <summary>
            /// FirstOrDefault() returns the first matching row,
            /// or null if no row exists.
            /// </summary>
            public static void Example_FirstOrDefault(AppDBContext context)
            {
            Console.WriteLine("\nExample 2 - FirstOrDefault()");
            Console.WriteLine("----------------------------");

            // Build query first (no execution yet)
            var query = context.Students
                .Where(s => s.Email == "notfound@student.com");
            // Execute query
            // Runtime logging will show the actual executed SQL.
            var student = query.FirstOrDefault();

            if (student == null)
            {
                Console.WriteLine("\nNo student found.");
            }
            else
            {
                Console.WriteLine("\nStudent Found:");
                Console.WriteLine($"{student.StudentID} - {student.FirstName} {student.LastName}");
            }
        }
       public static void Example_Single(AppDBContext context)
        {
            Console.WriteLine("\nExample 3 - Single()");
            Console.WriteLine("--------------------");

            // Build query first (no execution yet)
            var query = context.Courses
                .Where(c => c.Code == "SQL-101");
            // Preview query shape
            PreviewSQLUsingToQueryString(query.ToQueryString());

            // Execute query
            // Runtime logging will show the actual executed SQL.
            var course = query.Single();
            Console.WriteLine("\nCourse Found:");
            Console.WriteLine($"{course.CourseId} - {course.Code} - {course.Title}");
        }
        /// <summary>
        /// SingleOrDefault() expects zero or one matching row.
        /// Returns null if none exists, but throws if duplicates exist.
        /// </summary>
        public static void Example_SingleOrDefault(AppDBContext context)
        {
            Console.WriteLine("\nExample 4 - SingleOrDefault()");
            Console.WriteLine("-----------------------------");

            // Build query first (no execution yet)
            var query = context.Courses
                .Where(c => c.Code == "UNKNOWN-999");

            // Preview query shape
            PreviewSQLUsingToQueryString(query.ToQueryString());

            // Execute query
            // Runtime logging will show the actual executed SQL.
            var course = query.SingleOrDefault();

            if (course == null)
            {
                Console.WriteLine("\nNo course found.");
            }
            else
            {
                Console.WriteLine("\nCourse Found:");
                Console.WriteLine($"{course.CourseId} - {course.Code} - {course.Title}");
            }
        }
        public static void PrintGeneratedSql(string tableName,string sqlQuery)
        {
            Console.WriteLine($"Generated SQL Query for {tableName}:");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(sqlQuery);
            Console.WriteLine();
        }
        static void PreviewSQLUsingToQueryString(string SQLString)
        {
            Console.WriteLine("\nPreview SQL using ToQueryString():");
            Console.WriteLine("----------------------------------");
            Console.WriteLine(SQLString);
            Console.WriteLine();
        }
        /// <summary>
        /// Retrieves student by Primary Key using Find().
        /// Best method for direct PK lookup.
        /// May return tracked entity without executing SQL again.
        /// </summary>
        static void GetStudentByIdUsingFind(AppDBContext context)
        {
            Console.WriteLine("Using Find()");
            Console.WriteLine("------------");

            // Find() does not support ToQueryString().
            // Runtime logging will show actual SQL only if query is sent to database.
            var student = context.Students.Find(1);

        }
        /// <summary>
        /// Retrieves student by Primary Key using FirstOrDefault().
        /// Useful when filtering with conditions.
        /// </summary>
        static void GetStudentByIdUsingFirstOrDefault(AppDBContext context) {
            Console.WriteLine("Using FirstOrDefault()");
            Console.WriteLine("----------------------");

            // Build query first
            var Query = context.Students
                .Where(s => s.StudentID == 1);
            // Preview SQL query shape
            PreviewSQLUsingToQueryString(Query.ToQueryString());
            // Execute query
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL.
            var student = Query.FirstOrDefault();
            PrintStudent(student);
        }
        /// <summary>
        /// Retrieves only student names using projection.
        /// </summary>
        static void GetStudentName(AppDBContext context)
        {
            Console.WriteLine("Projection Example Using Select()");
            Console.WriteLine("---------------------------------");
            Console.WriteLine();

            // Build query first (no execution yet)
            var query = context.Students.Select(e => new
            {
                e.FirstName,
                e.LastName
            });
            // Preview SQL before execution
            PreviewSQLUsingToQueryString(query.ToQueryString());

            // Execute query
            var students = query.ToList();

            // Print results
            Console.WriteLine("\n\nStudent Names:");
            Console.WriteLine("--------------");

            foreach (var student in students)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName}");
            }

            Console.WriteLine();
            Console.WriteLine($"\nTotal Students: {students.Count}");
            Console.WriteLine();
        }
        static void GetFilteredStudents(AppDBContext context)
        {
            Console.WriteLine("Filtered Projection With Sorting");
            Console.WriteLine("--------------------------------");
            Console.WriteLine();

            // Build query first
            var query = context.Students
                .Where(s => s.Status == "Active")
                .Select(s => new
                {
                    s.StudentID,
                    FullName = s.FirstName + " " + s.LastName
                })
                .OrderByDescending(s=>s.FullName)
                .ThenBy(s=>s.StudentID);

            // Preview SQL before execution
            PreviewSQLUsingToQueryString(query.ToQueryString());

            // Execute query
            var students = query.ToList();

            // Print results
            Console.WriteLine("\n\nFiltered Students:");
            Console.WriteLine("------------------");

            foreach (var student in students)
            {
                Console.WriteLine($"{student.StudentID} - {student.FullName}");
            }

            Console.WriteLine();
            Console.WriteLine($"Total Students: {students.Count}");
            Console.WriteLine();
        }
        static void PrintStudent(dynamic? student)
        {
            if (student == null)
            {
                Console.WriteLine("Student not found.");
            }
            else
            {
                Console.WriteLine("\n\nStudent Found:");
                Console.WriteLine(
                    $"{student.StudentID} - {student.FirstName} {student.LastName}");
            }
        }
        /// <summary>
        /// Demonstrates Any() and All().
        /// </summary>
        static void CheckData(AppDBContext context)
        {
            Console.WriteLine("Any() and All() Example");
            Console.WriteLine("-----------------------");
            Console.WriteLine();

            // --------------------------------------------------
            // Any() Example
            // --------------------------------------------------

            // Build query first
            var activeStudentsQuery = context.Students.Where(s => s.Status == "Active");
            // Preview SQL query shape
            PreviewSQLUsingToQueryString(activeStudentsQuery.ToQueryString());

            // Execute query
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL for Any().
            bool hasActiveStudents = activeStudentsQuery.Any();



            Console.WriteLine($"Has Active Students: {hasActiveStudents}");
            Console.WriteLine();

            // --------------------------------------------------
            // All() Example
            // --------------------------------------------------

            // Build query first
            var coursesQuery =
                context.Courses;

            // Preview SQL query shape
            PreviewSQLUsingToQueryString(coursesQuery.ToQueryString());

            // Execute query
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL for All().

            bool allCoursesValid = coursesQuery.All(c => c.Price > 0);

            Console.WriteLine($"All Courses Price > 0: {allCoursesValid}");
            Console.WriteLine();
            
        }
        /// <summary>
        /// Compares bad vs good COUNT approach.
        /// </summary>
        static void CompareCount(AppDBContext context)
        {
            Console.WriteLine("COUNT EXAMPLE");
            Console.WriteLine();

            Console.WriteLine("BAD WAY:");
            Console.WriteLine();

            // Build query first
            var badQuery = context.Students;
            // Preview SQL query shape
            PreviewSQLUsingToQueryString(badQuery.ToQueryString());

            // Execute query and load all rows into memory
            var students = badQuery.ToList();
            // Count happens in memory after data is already loaded
            int badCount = students.Count(s => s.Status == "Active");

            Console.WriteLine($"Bad Count (calculated in memory): {badCount}");
            Console.WriteLine();

            Console.WriteLine("GOOD WAY:");
            Console.WriteLine();

            var goodQuery = context.Students
                .Where(s => s.Status == "Active");

            // Preview SQL query shape
            PreviewSQLUsingToQueryString(goodQuery.ToQueryString());

            // Execute COUNT in the database
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL for Count().

            int goodCount = goodQuery.Count();

            Console.WriteLine($"Good Count (calculated in database): {goodCount}");
            Console.WriteLine();

            
            }

        /// <summary>
        /// Compares bad vs good AVERAGE approach.
        /// </summary>

        static void CompareAverage(AppDBContext context)
        {
            Console.WriteLine("AVERAGE EXAMPLE");
            Console.WriteLine();

            Console.WriteLine("BAD WAY:");
            Console.WriteLine();

            //Build query First

            var badQuery = context.Enrollments;

            // Preview SQL query shape
            PreviewSQLUsingToQueryString(badQuery.ToQueryString());
            // Execute query and load all rows into memory
            var enrollments = badQuery.ToList();
            // Execute query and load all rows into memory
            decimal badAverage = enrollments.Average(e => e.ProgressPercent);

            Console.WriteLine($"Bad Average (calculated in memory): {badAverage}");
            Console.WriteLine();

            Console.WriteLine("GOOD WAY:");
            Console.WriteLine();

            var goodQuery = context.Enrollments
                .Select(e => e.ProgressPercent);

            // Preview SQL query shape
            PreviewSQLUsingToQueryString(goodQuery.ToQueryString());

            // Execute AVERAGE in the database
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL for Average().
            decimal goodAverage = goodQuery.Average();
            Console.WriteLine($"Good Average (calculated in database): {goodAverage}");
            Console.WriteLine();
        }

        /// <summary>
        /// Compares bad vs good SUM approach.
        /// </summary>
        static void CompareSum(AppDBContext context)
        {
            Console.WriteLine("SUM EXAMPLE");
            Console.WriteLine();

            Console.WriteLine("BAD WAY:");
            Console.WriteLine();

            var badQuery = context.Courses;
            // Preview SQL query shape
            PreviewSQLUsingToQueryString(badQuery.ToQueryString());

            // Execute query and load all rows into memory

            var courses = badQuery.ToList();
            // Sum happens in memory after data is already loaded
            int badSum = courses.Sum(c => c.DurationHours);
            Console.WriteLine($"Bad Sum (calculated in memory): {badSum}");
            Console.WriteLine();

            Console.WriteLine("GOOD WAY:");
            Console.WriteLine();

            // Build query first

            var goodQuery = context.Courses.Select(c => c.DurationHours);
            // Preview SQL query shape
            PreviewSQLUsingToQueryString(goodQuery.ToQueryString());

            // Execute SUM in the database
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL for Sum().

            int goodSum = goodQuery.Sum();

            Console.WriteLine($"Good Sum (calculated in database): {goodSum}");
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates Min() and Max() using TrainingCenterDB.
        /// </summary>
        static void ShowMinMax(AppDBContext context)
        {
            Console.WriteLine("Min() and Max() Example");
            Console.WriteLine("-----------------------");
            Console.WriteLine();

            // --------------------------------------------------
            // Lowest Course Price
            // --------------------------------------------------

            // Build query first

            var coursePricesQuery = context.Courses
                .Select(c => c.Price);

            // Preview SQL query shape
            PreviewSQLUsingToQueryString(coursePricesQuery.ToQueryString());

            // Execute query
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL for Min().

            decimal lowestPrice = coursePricesQuery.Min();

            // Execute query
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL for Max().

            decimal highestPrice = coursePricesQuery.Max();

            // --------------------------------------------------
            // Earliest Registration Date
            // --------------------------------------------------

            // Build query first
            var registrationDatesQuery = context.Students
                .Select(s => s.RegisteredAt);
            // Preview SQL query shape
            PreviewSQLUsingToQueryString(registrationDatesQuery.ToQueryString());

            // Execute query
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL for Min().

            DateTime earliestRegistration = registrationDatesQuery.Min();

            // Print readable output
            Console.WriteLine($"Lowest Course Price     : {lowestPrice}");
            Console.WriteLine($"Highest Course Price    : {highestPrice}");
            Console.WriteLine($"Earliest Registration   : {earliestRegistration:d}");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows unique student statuses using Distinct().
        /// </summary>
        static void ShowDistinctStudentStatuses(AppDBContext context)
        {
            Console.WriteLine("Unique Student Statuses");
            Console.WriteLine("-----------------------");

            // Build query first
            var query = context.Students
                .Select(s => s.Status)
                .Distinct();

            // Preview SQL before execution
            PreviewSQLUsingToQueryString(query.ToQueryString());



            // Execute query
            var statuses = query.ToList();

            Console.WriteLine();
            // Print readable output
            foreach (var status in statuses)
            {
                Console.WriteLine(status);
            }
        }
        /// <summary>
        /// Shows number of students grouped by status.
        /// </summary>
        static void ShowStudentsPerStatusReport(AppDBContext context)
        {
            Console.WriteLine("Students Per Status");
            Console.WriteLine("-------------------");

            // Build query first

            var query = context.Students
                .GroupBy(s => s.Status)
                .Where(x => x.Count() > 6)
                .Select(g => new
                {
                    Status = g.Key,
                    TotalStudents = g.Count()
                })
                
                .OrderBy(x => x.Status);
            // Preview SQL before execution
            PreviewSQLUsingToQueryString(query.ToQueryString());

            // Execute query
            // ToQueryString previews query shape,
            // runtime logging shows actual executed SQL for Count().
            var report = query.ToList();

            Console.WriteLine();
            // Print readable output
            foreach (var row in report)
            {
                Console.WriteLine($"{row.Status} : {row.TotalStudents}");
            }

        }

        /// <summary>
        /// Demonstrates the bad N+1 approach by loading students first,
        /// then running one additional count query per student.
        /// </summary>
        static void ShowBadNPlusOneApproach(AppDBContext context) {
            Console.WriteLine("BAD APPROACH - N+1 Problem");
            Console.WriteLine("--------------------------");
            Console.WriteLine();

            // Build query first
            var studentsQuery = context.Students;
            // Preview SQL before execution
            PreviewSQLUsingToQueryString(studentsQuery.ToQueryString());

            // Execute first query: load all students
            var students = studentsQuery.ToList();

            Console.WriteLine();

            foreach(var student in students)
            {
                // Build query first for each student
                var enrollmentsQuery = context.Enrollments
                    .Where(e => e.StudentId == student.StudentID);
                // Preview SQL query shape
                PreviewSQLUsingToQueryString(enrollmentsQuery.ToQueryString());

                // Execute Count() for each student
                // ToQueryString previews query shape,
                // runtime logging shows actual executed SQL for Count().

                int countEnrollment = enrollmentsQuery.Count();
            }


        }

        /// <summary>
        /// Demonstrates the good approach by loading students with their enrollments using Include().
        /// </summary>
        static void ShowGoodIncludeApproach(AppDBContext context)
        {
            Console.WriteLine("GOOD APPROACH - Include()");
            Console.WriteLine("-------------------------");
            Console.WriteLine();


            //Build Query first
            var query = context.Students
                .Include(s => s.Enrolments)
                ;

            // Execute query
            var studentsWithEnrollments = query.ToList();
            
            
            Console.WriteLine();
            
            foreach (var student in studentsWithEnrollments)
            {
                Console.WriteLine(
                    $"{student.FirstName} {student.LastName} - Enrollments: {student.Enrolments.Count} ");
            }
            Console.WriteLine();
            Console.WriteLine("Result: Related enrollments are loaded with the students.");
            Console.WriteLine();
        }
        /// <summary>
        /// Loads students with their enrollments and related courses.
        /// </summary>
        static void ShowStudentsWithEnrollmentsAndCourses(AppDBContext context)
        {
            //var Query = context.Students
            //    .Include(s => s.Enrolments)
            //    .ThenInclude(s => s.Course)
            //    .OrderBy(s => s.StudentID);
            var Query = context.Students
                .Select(s => new
                {
                    s.FirstName,
                    Course = s
                    .Enrolments
                    .Select(e => new { e.Course.Title,e.Course.Price})
                });
            PreviewSQLUsingToQueryString(Query.ToQueryString());
            // Execute query
            var students = Query.ToList();

            Console.WriteLine("\nStudents With Enrollments and Courses:");
            Console.WriteLine("--------------------------------------");

            foreach (var student in students)
            {
                var courses = student.Course.FirstOrDefault();
                Console.WriteLine($"{student.FirstName} ");
                
                //foreach (var enrollment in student.Course)
                //{
                //    Console.WriteLine(
                //        $"   Course: {enrollment.Course.Title}, " +
                //        $"Status: {enrollment.Status}, " +
                //        $"Progress: {enrollment.ProgressPercent}%");
                //}

                Console.WriteLine();
            }
        }
    }
}
