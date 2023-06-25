using System;
using System.Data;
using MySql.Data.MySqlClient;

public class Program
{
    static string connectionString = "Server=localhost;Port=3306;Database=EmployeeManagement;Uid=root;Pwd=password;";

    static void Main(string[] args)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();


                if (connection.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connection successful!");


                    string testQuery = "SELECT 1";
                    using (var command = new MySqlCommand(testQuery, connection))
                    {
                        int result = Convert.ToInt32(command.ExecuteScalar());
                        if (result == 1)
                        {
                            Console.WriteLine("Test query executed successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Test query failed!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while connecting to the database: " + ex.Message);
            }
        }

        while (true)
        {
            Console.WriteLine("Employee Management System");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View Employees");
            Console.WriteLine("3. Update Employee");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Add Manager");
            Console.WriteLine("6. View Managers");
            Console.WriteLine("7. Exit");
            Console.WriteLine("Enter your choice:");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddEmployee();
                    break;
                case "2":
                    ViewEmployees();
                    break;
                case "3":
                    UpdateEmployee();
                    break;
                case "4":
                    DeleteEmployee();
                    break;
                case "5":
                    AddManager();
                    break;
                case "6":
                    ViewManagers();
                    break;
                case "7":
                    Console.WriteLine("Exiting the application...");
                    return;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void AddEmployee()
    {
        Console.WriteLine("Enter employee details:");
        Console.Write("First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();
        Console.Write("Birth Date (YYYY-MM-DD): ");
        string birthDate = Console.ReadLine();

        DateTime parsedBirthDate;
        if (!DateTime.TryParse(birthDate, out parsedBirthDate))
        {
            Console.WriteLine("Invalid date format! Please try again.");
            return;
        }

        Console.Write("Employee ID: ");
        string employeeId = Console.ReadLine();

        Console.Write("Employee Write-ups: ");
        string employeeWriteups = Console.ReadLine();

        Console.Write("Hours Worked: ");
        int hoursWorked = int.Parse(Console.ReadLine());

        Console.Write("Time Off: ");
        int timeOff = int.Parse(Console.ReadLine());

        Console.Write("Sales Made: ");
        int salesMade = int.Parse(Console.ReadLine());

        Console.Write("Starting Date (YYYY-MM-DD): ");
        string startingDate = Console.ReadLine();

        DateTime parsedStartingDate;
        if (!DateTime.TryParse(startingDate, out parsedStartingDate))
        {
            Console.WriteLine("Invalid date format! Please try again.");
            return;
        }

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string insertQuery = "INSERT INTO Employees (FirstName, LastName, BirthDate, EmployeeID, EmployeeWriteups, HoursWorked, Timeoff, SalesMade, StartingDate) " +
                "VALUES (@FirstName, @LastName, @BirthDate, @EmployeeID, @EmployeeWriteups, @HoursWorked, @Timeoff, @SalesMade, @StartingDate)";

            using (var command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@BirthDate", parsedBirthDate);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EmployeeWriteups", employeeWriteups);
                command.Parameters.AddWithValue("@HoursWorked", hoursWorked);
                command.Parameters.AddWithValue("@Timeoff", timeOff);
                command.Parameters.AddWithValue("@SalesMade", salesMade);
                command.Parameters.AddWithValue("@StartingDate", parsedStartingDate);

                command.ExecuteNonQuery();
                Console.WriteLine("Employee added successfully!");
            }
        }
    }

    static void ViewEmployees()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM Employees";
            using (var command = new MySqlCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("Employee List:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["FirstName"]} {reader["LastName"]}, " +
                            $"Birth Date: {((DateTime)reader["BirthDate"]).ToShortDateString()}, " +
                            $"Employee ID: {reader["EmployeeID"]}, " +
                            $"Employee Write-ups: {reader["EmployeeWriteups"]}, " +
                            $"Hours Worked: {reader["HoursWorked"]}, " +
                            $"Time Off: {reader["Timeoff"]}, " +
                            $"Sales Made: {reader["SalesMade"]}, " +
                            $"Starting Date: {((DateTime)reader["StartingDate"]).ToShortDateString()}");
                    }
                }
            }
        }
    }

    static void UpdateEmployee()
    {
        Console.Write("Enter the ID of the employee to update: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter the new last name: ");
        string lastName = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string updateQuery = "UPDATE Employees SET LastName = @LastName WHERE Id = @Id";
            using (var command = new MySqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Id", id);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee updated successfully!");
                }
                else
                {
                    Console.WriteLine("Employee not found!");
                }
            }
        }
    }

    static void DeleteEmployee()
    {
        Console.Write("Enter the ID of the employee to delete: ");
        int id = int.Parse(Console.ReadLine());

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string deleteQuery = "DELETE FROM Employees WHERE Id = @Id";
            using (var command = new MySqlCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Employee not found!");
                }
            }
        }
    }
    static void AddManager()
    {
        Console.WriteLine("Enter manager details:");
        Console.Write("First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();
        Console.Write("Starting Date (YYYY-MM-DD): ");
        string startingDate = Console.ReadLine();
        Console.Write("Management Department: ");
        string managementDepartment = Console.ReadLine();
        Console.Write("Salary: ");
        decimal salary = decimal.Parse(Console.ReadLine());

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();


            string createTableQuery = "CREATE TABLE IF NOT EXISTS management (Id INT AUTO_INCREMENT PRIMARY KEY, FirstName VARCHAR(50), LastName VARCHAR(50), StartingDate DATE, ManagementDepartment VARCHAR(50), Salary DECIMAL(10, 2))";
            using (var command = new MySqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }


            string insertQuery = "INSERT INTO management (FirstName, LastName, StartingDate, ManagementDepartment, Salary) " +
                "VALUES (@FirstName, @LastName, @StartingDate, @ManagementDepartment, @Salary)";

            using (var command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@StartingDate", DateTime.Parse(startingDate));
                command.Parameters.AddWithValue("@ManagementDepartment", managementDepartment);
                command.Parameters.AddWithValue("@Salary", salary);

                command.ExecuteNonQuery();
                Console.WriteLine("Manager added successfully!");
            }
        }
    }


    static void ViewManagers()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM Managers";
            using (var command = new MySqlCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("Manager List:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["FirstName"]} {reader["LastName"]}, " +
                            $"Starting Date: {((DateTime)reader["StartingDate"]).ToShortDateString()}, " +
                            $"Management Department: {reader["ManagementDepartment"]}, " +
                            $"Salary: {reader["Salary"]}, " +
                            $"Department: {reader["Department"]}");
                    }
                }
            }
        }
    }
}
