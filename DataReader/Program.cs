using Microsoft.Data.SqlClient;
using System.Data.Common;
namespace ADONETExample;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("*** Fun With DataReaders ****\n");

        using (SqlConnection connection = new SqlConnection())
        {
            ShowConnectionInfo(connection);
            string conString = "Server=GRAMM\\TESTDB;Database=AutoLot;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=30;";

            connection.ConnectionString = "Server=GRAMM\\TESTDB;Database=AutoLot;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=30;";


            connection.Open();

            ShowConnectionInfo(connection);

            string sql = @"SELECT i.Id, m.Name AS Make, i.Color, i.PetName
                            FROM Inventory i
                            INNER JOIN Makes m on m.Id = i.MakeId ORDER BY i.Id ASC";
            SqlCommand command = new SqlCommand(sql, connection);


            using (SqlDataReader reader = command.ExecuteReader())
            {

                while (reader.Read())
                {

                    Console.WriteLine($"{connection.Database    }");
                    Console.WriteLine($"------------\nID: {reader["Id"]}\nMake: {reader["Make"]} \nColor: {reader["Color"]} \nPetName: {reader["PetName"]}");
                    Console.WriteLine("------------");
                }
            }

            
            connection.Close();
            ShowConnectionInfo(connection);

        }
    }

    static void ShowConnectionInfo (DbConnection connection)
    {
        Console.WriteLine($"DB Location: {connection.DataSource}");
        Console.WriteLine($"DB Name: {connection.Database}");
        Console.WriteLine($"TimeOut: {connection.ConnectionTimeout}");
        Console.WriteLine($"Connection state: {connection.State}");
    }

}
