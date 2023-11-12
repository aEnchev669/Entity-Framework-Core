namespace SoftUni;

using System.Globalization;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Data;
using Models;

public class StartUp
{
    static void Main(string[] args)
    {

        SoftUniContext dbContext = new SoftUniContext();

        var result = GetLatestProjects(dbContext);

        Console.WriteLine(result);
    }

    // Problem 03
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToArray();
        foreach (var e in employees)
        {
            sb
                .AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }
    // Problem 04

    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .OrderBy(e => e.FirstName)
            .Select(e => new
            {
                e.FirstName,
                e.Salary
            })
            .ToArray();

        foreach (var e in employees)
        {
            if (e.Salary > 50000)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

        }
        return sb.ToString().TrimEnd();
    }
    // Problem 05

    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                departmentName = e.Department.Name,
                e.Salary
            })
            .ToArray();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} from {e.departmentName} - ${e.Salary:f2}");
        }
        return sb.ToString().TrimEnd();
    }
    // Problem 06

    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };

        Employee? employee = context.Employees
            .FirstOrDefault(e => e.LastName == "Nakov");

        employee!.Address = newAddress;
        context.SaveChanges();

        var employees = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e => e.Address!.AddressText)
            .ToArray();

        return String.Join("\n", employees);
    }
    // Problem 07

    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            //  .Where(e => e.EmployeesProjects
            // .Any(ep => ep.Project!.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
            .Take(10)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                ManagerFName = e.Manager!.FirstName,
                ManagerLName = e.Manager!.LastName,
                Projects = e.EmployeesProjects
                            .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                            .Select(ep => new
                            {
                                ProjectName = ep.Project.Name,
                                StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                                EndDate = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                            })
                            .ToArray()
            })
            .ToArray();


        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFName} {e.ManagerLName}");

            foreach (var p in e.Projects)
            {
                sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
            }
        }
        return sb.ToString().TrimEnd();
    }
    // Problem 08
    public static string GetAddressesByTown(SoftUniContext context)
    {

        StringBuilder sb = new StringBuilder();

        var addresses = context.Addresses
            .Select(a => new
            {
                a.AddressText,
                townName = a.Town!.Name,
                employeesCount = a.Employees.Count
            })
            .OrderByDescending(a => a.employeesCount)
            .ThenBy(a => a.townName)
            .ThenBy(a => a.AddressText)
            .Take(10)
            .ToArray();

        foreach (var a in addresses)
        {
            sb.AppendLine($"{a.AddressText}, {a.townName} - {a.employeesCount} employees");
        }

        return sb.ToString().TrimEnd();
    }
    // Problem 09
    public static string GetEmployee147(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employee = context.Employees
            .Where(e => e.EmployeeId == 147)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                ProjectsNames = e.EmployeesProjects
                            .Select(ep => new
                            {
                                ProjectName = ep.Project!.Name
                            })
                            .OrderBy(ep => ep.ProjectName)
                            .ToArray()

            })
            .ToArray();


        foreach (var e in employee)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");

            foreach (var pn in e.ProjectsNames)
            {
                sb.AppendLine($"{pn.ProjectName}");
            }
        }

        return sb.ToString().TrimEnd();
    }
    // Problem 10
    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var departments = context.Departments
            .Where(d => d.Employees.Count > 5)
            .Select(d => new
            {
                d.Name,
                ManagerFName = d.Manager.FirstName,
                ManagerLName = d.Manager.LastName,
                EmployeesCount = d.Employees.Count,
                Employees = d.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .ToArray()
            })
            .OrderBy(d => d.EmployeesCount)
            .ThenBy(d => d.Name)
            .ToArray();

        foreach (var d in departments)
        {
            sb.AppendLine($"{d.Name} - {d.ManagerFName} {d.ManagerLName}");

            foreach (var e in d.Employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
            }
        }

        return sb.ToString().TrimEnd();
    }
    // Problem 11
    public static string GetLatestProjects(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var projects = context.Projects
            .OrderByDescending(p => p.StartDate)
            .Take(10)
            .Select(p => new
            {
                p.Name,
                p.Description,
                StartDate = p.StartDate,
                Date = p.StartDate
                        .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
            })
            .OrderBy(p => p.Name)
            .ToArray();

        

        foreach (var p in projects)
        {
            sb.AppendLine(p.Name);
            sb.AppendLine(p.Description);
            sb.AppendLine(p.Date);
        }

        return sb.ToString().TrimEnd();
    }


}
