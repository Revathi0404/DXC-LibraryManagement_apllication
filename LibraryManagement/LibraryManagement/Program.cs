using Microsoft.VisualBasic;
using Spectre.Console;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Runtime.Intrinsics.Arm;

namespace LibraryManagement
{
    class Program
    {
        static SqlConnection con;
        
        static void Main(string[] args)
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3;Initial Catalog=LibraryManagement;Integrated Security=true");
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Book");

                bool isLoggedIn = Login();

                if (isLoggedIn)
                {
                    AnsiConsole.Markup("Login successful.[green] Welcome to the Library Management System[/]!");

                    bool isRunning = true;
                    while (isRunning)
                    {
                        int choice = DisplayMenu();

                        try
                        {
                            switch (choice)
                            {
                                case 1:
                                    AddBookDetails();
                                    break;
                                case 2:
                                    EditBookDetails();
                                    break;
                                case 3:
                                    DeleteBookDetails();
                                    break;
                                case 4:
                                    AddStudentDetails();
                                    break;
                                case 5:
                                    EditStudentDetails();
                                    break;
                                case 6:
                                    SearchStudentByRollNumber();
                                    break;
                                case 7:
                                    DeleteStudentDetails();
                                    break;
                                case 8:
                                    IssueBookToStudent();
                                    break;
                                case 9:
                                    ReturnBookFromStudent();
                                    break;
                                case 10:
                                    SearchBookByAuthorOrPublication();
                                    break;
                                case 11:
                                    DisplayStudentsWithBooks();
                                    break;
                                case 12:
                                    Logout();
                                    isRunning = false;
                                    break;
                                default:
                                    AnsiConsole.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
                }
                else
                {
                    AnsiConsole.WriteLine("Login failed. Exiting the Library Management System.");
                }

                con.Close();
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
            }
        }

       
        static bool Login()

        {
            AnsiConsole.Markup("[blue]Please login to continue[/]:");
            string username = Console.ReadLine();
            string password = Console.ReadLine();
            string connectionString = ("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"); 
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                con.Open();

                string query = $"Select * from Login WHERE user_id = '{username}' AND password = '{password}'";

                SqlCommand command = new SqlCommand(query, con);

                SqlDataReader reader = command.ExecuteReader();

                bool isValidUser = reader.HasRows;
                reader.Close();
                return isValidUser;

            }

        }

        static int DisplayMenu()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("=== Library Management System ===");
            AnsiConsole.WriteLine("1. Add Book Details");
            AnsiConsole.WriteLine("2. Edit Book Details");
            AnsiConsole.WriteLine("3. Delete Book Details");
            AnsiConsole.WriteLine("4. Add Student Details");
            AnsiConsole.WriteLine("5. Edit Student Details");
            AnsiConsole.WriteLine("6. Search Student Details by Roll Number");
            AnsiConsole.WriteLine("7. Delete Student Details");
            AnsiConsole.WriteLine("8. Issue Book to Student");
            AnsiConsole.WriteLine("9. Return Book from Student");
            AnsiConsole.WriteLine("10. Search Book by Author or Publication");
            AnsiConsole.WriteLine("11. Display Students with Books");
            AnsiConsole.WriteLine("12. Logout");

            return AnsiConsole.Ask<int>("Enter your choice: ");
        }
        static void AddBookDetails()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("=== Add Book Details ===");

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3;Initial Catalog=LibraryManagement;Integrated Security=true"))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO Book (book_name, author_name, publication_name, available_stock) values (@book_name, @author_name, @publication_name, @available_stock)", con);

                    cmd.Parameters.AddWithValue("@book_name", AnsiConsole.Ask<string>("Book Name: "));
                    cmd.Parameters.AddWithValue("@author_name", AnsiConsole.Ask<string>("Athor Name: "));
                    cmd.Parameters.AddWithValue("@publication_name", AnsiConsole.Ask<string>("Publication Name: "));
                    cmd.Parameters.AddWithValue("@available_stock", AnsiConsole.Ask<int>("Available Stock: "));

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        AnsiConsole.WriteLine("Book details added successfully.");
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Failed to add book details.");
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        static void EditBookDetails()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("=== Edit Book Details ===");
            Console.WriteLine("enter id of the book to update");
            int id = Convert.ToInt32(Console.ReadLine());
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3;Initial Catalog=LibraryManagement;Integrated Security=true"))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand($"UPDATE Book SET book_name = @book_name, author_name = @author_name, publication_name = @publication_name, available_stock = @available_stock WHERE book_id = {id}", con);

                    cmd.Parameters.AddWithValue("@book_name", AnsiConsole.Ask<string>("NewBook Name: "));
                    cmd.Parameters.AddWithValue("@author_name", AnsiConsole.Ask<string>("New Author Name: "));
                    cmd.Parameters.AddWithValue("@publication_name", AnsiConsole.Ask<string>("New Publication Name: "));
                    cmd.Parameters.AddWithValue("@available_stock", AnsiConsole.Ask<int>("New Available Stock: "));

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        AnsiConsole.WriteLine("Book details Edited successfully.");
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Failed to Edited book details.");
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void DeleteBookDetails()
        {
            using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"))
            {
                con.Open();

                AnsiConsole.WriteLine();
                AnsiConsole.Markup("[yellow]=== Delete Book Details ===[/]");

                try
                {
                    int bookId = AnsiConsole.Ask<int>("Enter the Book ID to delete: ");

                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM Book WHERE book_id = @book_id", con))
                    {
                        deleteCmd.Parameters.AddWithValue("@book_id", bookId);

                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            AnsiConsole.WriteLine("Book details deleted successfully.");
                        }
                        else
                        {
                            AnsiConsole.WriteLine($"Book with ID {bookId} not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void AddStudentDetails()
        {
            using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"))
            {
                con.Open();

                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine("=== Add Student Details ===");

                try
                {
                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Student (student_name, department, address, roll_number) VALUES (@student_name, @department, @address, @roll_number)", con))
                    {
                        insertCmd.Parameters.AddWithValue("@student_name", AnsiConsole.Ask<string>("Student Name: "));
                        insertCmd.Parameters.AddWithValue("@department", AnsiConsole.Ask<string>("Department: "));
                        insertCmd.Parameters.AddWithValue("@address", AnsiConsole.Ask<string>("Address: "));
                        insertCmd.Parameters.AddWithValue("@roll_number", AnsiConsole.Ask<string>("Roll Number: "));

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            AnsiConsole.WriteLine("Student details added successfully.");
                        }
                        else
                        {
                            AnsiConsole.WriteLine("Failed to add student details.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void EditStudentDetails()
        {
            using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"))
            {
                con.Open();

                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine("=== Edit Student Details ===");

                try
                {
                    int studentId = AnsiConsole.Ask<int>("Enter the Student ID to edit: ");

                    using (SqlCommand updateCmd = new SqlCommand("UPDATE Student SET student_name = @student_name, department = @department, address = @address, roll_number = @roll_number WHERE student_id = @student_id", con))
                    {
                        updateCmd.Parameters.AddWithValue("@student_name", AnsiConsole.Ask<string>("Student Name: "));
                        updateCmd.Parameters.AddWithValue("@department", AnsiConsole.Ask<string>("Department: "));
                        updateCmd.Parameters.AddWithValue("@address", AnsiConsole.Ask<string>("Address: "));
                        updateCmd.Parameters.AddWithValue("@roll_number", AnsiConsole.Ask<string>("Roll Number: "));
                        updateCmd.Parameters.AddWithValue("@student_id", studentId);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            AnsiConsole.WriteLine("Student details updated successfully.");
                        }
                        else
                        {
                            AnsiConsole.WriteLine($"Student with ID {studentId} not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void SearchStudentByRollNumber()
        {
            using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"))
            {
                con.Open();

                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine("=== Search Student Details ===");

                try
                {
                    int rollNumber = AnsiConsole.Ask<int>("Enter the Roll Number to search: ");

                    using (SqlCommand selectCmd = new SqlCommand("SELECT * FROM Student WHERE roll_number = @roll_number", con))
                    {
                        selectCmd.Parameters.AddWithValue("@roll_number", rollNumber);

                        SqlDataReader reader = selectCmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            AnsiConsole.WriteLine("Matching Student Details:");

                            while (reader.Read())
                            {
                                int student_Id = reader.GetInt32(0);
                                int roll_number = reader.GetInt32(1);
                                string student_name = reader.GetString(2);
                                string department = reader.GetString(3);
                                string address = reader.GetString(4);

                                Console.WriteLine($"{student_Id}\t{student_name}\t\t{department}\t{address}\t{roll_number}");
                            }
                        }
                        else
                        {
                            AnsiConsole.WriteLine($"No student found with Roll Number {rollNumber}.");
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void DeleteStudentDetails()
        {
            using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"))
            {
                con.Open();

                AnsiConsole.WriteLine();
                AnsiConsole.Markup("[yellow]=== Delete Student Details ===[/]");

                try
                {
                    int studentId = AnsiConsole.Ask<int>("Enter the student ID to delete: ");

                   
                    using (SqlCommand deleteRelatedCmd = new SqlCommand("DELETE FROM Book_issue WHERE student_id = @student_id", con))
                    {
                        deleteRelatedCmd.Parameters.AddWithValue("@student_id", studentId);

                        int rowsAffected = deleteRelatedCmd.ExecuteNonQuery();
                    }

                   
                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM Student WHERE student_id = @student_id", con))
                    {
                        deleteCmd.Parameters.AddWithValue("@student_id", studentId);

                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            AnsiConsole.WriteLine("Student details deleted successfully.");
                        }
                        else
                        {
                            AnsiConsole.WriteLine($"Student with ID {studentId} not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }


        static void IssueBookToStudent()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("=== Issue Book to Student ===");

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"))
                {
                    con.Open();

                    int bookId = AnsiConsole.Ask<int>("Enter the Book ID: ");
                    int studentId = AnsiConsole.Ask<int>("Enter the Student ID: ");

                    string bookSelectQuery = "SELECT available_stock, book_name FROM Book WHERE book_id = @book_id";
                    string studentSelectQuery = "SELECT student_name FROM Student WHERE student_id = @student_id";
                    string bookIssueInsertQuery = "INSERT INTO Book_issue (book_id, student_id, date_of_issue) VALUES (@book_id, @student_id, @date_of_issue)";
                    string bookUpdateQuery = "UPDATE Book SET available_stock = available_stock - @issueCount WHERE book_id = @book_id";

                    using (SqlCommand bookSelectCommand = new SqlCommand(bookSelectQuery, con))
                    {
                        bookSelectCommand.Parameters.AddWithValue("@book_id", bookId);

                        using (SqlDataReader bookReader = bookSelectCommand.ExecuteReader())
                        {
                            if (bookReader.Read())
                            {
                                int availableQuantity = (int)bookReader["available_stock"];
                                string bookName = bookReader["book_name"].ToString();
                                bookReader.Close();

                                if (availableQuantity > 0)
                                {
                                    using (SqlCommand studentSelectCommand = new SqlCommand(studentSelectQuery, con))
                                    {
                                        studentSelectCommand.Parameters.AddWithValue("@student_id", studentId);

                                        using (SqlDataReader studentReader = studentSelectCommand.ExecuteReader())
                                        {
                                            if (studentReader.Read())
                                            {
                                                string studentName = studentReader["student_name"].ToString();
                                                studentReader.Close();

                                                int issueCount = 1;

                                                if (availableQuantity >= issueCount)
                                                {
                                                    using (SqlCommand bookIssueInsertCommand = new SqlCommand(bookIssueInsertQuery, con))
                                                    {
                                                        bookIssueInsertCommand.Parameters.AddWithValue("@book_id", bookId);
                                                        bookIssueInsertCommand.Parameters.AddWithValue("@student_id", studentId);
                                                        bookIssueInsertCommand.Parameters.AddWithValue("@date_of_issue", DateTime.Now);

                                                        bookIssueInsertCommand.ExecuteNonQuery();

                                                        using (SqlCommand bookUpdateCommand = new SqlCommand(bookUpdateQuery, con))
                                                        {
                                                            bookUpdateCommand.Parameters.AddWithValue("@issueCount", issueCount);
                                                            bookUpdateCommand.Parameters.AddWithValue("@book_id", bookId);

                                                            bookUpdateCommand.ExecuteNonQuery();

                                                            AnsiConsole.WriteLine("Book issued successfully.");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    AnsiConsole.WriteLine("Book is not available in sufficient quantity.");
                                                }
                                            }
                                            else
                                            {
                                                AnsiConsole.WriteLine($"Student with ID {studentId} not found.");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    AnsiConsole.WriteLine("Book is not available in stock.");
                                }
                            }
                            else
                            {
                                bookReader.Close();
                                AnsiConsole.WriteLine($"Book with ID {bookId} not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void ReturnBookFromStudent()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("=== Return Book from Student ===");

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"))
                {
                    con.Open();

                    int bookId = AnsiConsole.Ask<int>("Enter the Book ID: ");
                    int studentId = AnsiConsole.Ask<int>("Enter the Student ID: ");

                    string updateQuery = "UPDATE Book SET available_stock = available_stock + 1 WHERE book_id = @book_id";
                    string deleteQuery = "UPDATE Book_issue SET date_of_return = @date_of_return WHERE student_id = @student_id AND book_id = @book_id";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, con))
                    {
                        updateCommand.Parameters.AddWithValue("@book_id", bookId);
                        int rowsUpdated = updateCommand.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, con))
                            {
                                deleteCommand.Parameters.AddWithValue("@book_id", bookId);
                                deleteCommand.Parameters.AddWithValue("@student_id", studentId);
                                deleteCommand.Parameters.AddWithValue("@date_of_return", DateTime.Now);
                                int rowsDeleted = deleteCommand.ExecuteNonQuery();

                                if (rowsDeleted > 0)
                                {
                                    AnsiConsole.WriteLine("Book returned successfully.");
                                }
                                else
                                {
                                    AnsiConsole.WriteLine("Book is not issued to the specified student.");
                                }
                            }
                        }
                        else
                        {
                            AnsiConsole.WriteLine("Book is not issued to the specified student.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void SearchBookByAuthorOrPublication()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("=== Search Book by Author or Publication ===");

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"))
                {
                    con.Open();

                    string keyword = AnsiConsole.Ask<string>("Enter the Author Name or Publication Name to search: ");

                    string query = "SELECT * FROM Book WHERE author_name LIKE @keyword OR publication_name LIKE @keyword";

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                AnsiConsole.WriteLine("Matching Book Details:");

                                while (reader.Read())
                                {
                                    int bookId = (int)reader["book_id"];
                                    string bookName = (string)reader["book_name"];
                                    string authorName = (string)reader["author_name"];
                                    string publicationName = (string)reader["publication_name"];
                                    int Available_Stock = (int)reader["available_stock"];

                                    AnsiConsole.WriteLine($"Book ID: {bookId}");
                                    AnsiConsole.WriteLine($"Book Name: {bookName}");
                                    AnsiConsole.WriteLine($"Author Name: {authorName}");
                                    AnsiConsole.WriteLine($"Publication Name: {publicationName}");
                                    AnsiConsole.WriteLine($"Available_Stock: {Available_Stock}");
                                    AnsiConsole.WriteLine();
                                }
                            }
                            else
                            {
                                AnsiConsole.WriteLine("No book found with the specified Author Name or Publication Name.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        static void DisplayStudentsWithBooks()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("=== Students with Issued Books ===");

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true"))
                {
                    con.Open();

                    string query = @"SELECT bi.book_id, bi.date_of_issue, bi.date_of_return,
                            s.student_id, s.student_name
                            FROM Book_issue AS bi
                            INNER JOIN Student AS s ON bi.student_id = s.student_id where bi.date_of_return is null";

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    int bookId = (int)reader["book_id"];
                                    DateTime dateOfIssue = (DateTime)reader["date_of_issue"];
                                    DateTime? dateOfReturn = reader["date_of_return"] == DBNull.Value ? null : (DateTime?)reader["date_of_return"];
                                    int studentId = (int)reader["student_id"];
                                    string studentName = (string)reader["student_name"];

                                    AnsiConsole.WriteLine($"Student ID: {studentId}");
                                    AnsiConsole.WriteLine($"Student Name: {studentName}");
                                    AnsiConsole.WriteLine($"Book ID: {bookId}");
                                    AnsiConsole.WriteLine($"Date of Issue: {dateOfIssue}");

                                    if (dateOfReturn.HasValue)
                                    {
                                        AnsiConsole.WriteLine($"Due Date: {dateOfReturn.Value}");
                                    }
                                    else
                                    {
                                        AnsiConsole.WriteLine("Due Date: Not returned yet");
                                    }

                                    AnsiConsole.WriteLine();
                                }
                            }
                            else
                            {
                                AnsiConsole.WriteLine("No students have issued books at the moment.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        static void Logout()
        {
            SqlConnection con = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=LibraryManagement; Integrated Security=true");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("=== Logout ===");

            con.Close();
          

            AnsiConsole.WriteLine("Logged out successfully.");
        }

    }
}
