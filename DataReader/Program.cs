using Microsoft.Data.SqlClient;
namespace ADONETExample;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("*** Fun With DataReaders ****\n");

        using (SqlConnection connection = new SqlConnection())
        {
            connection.ConnectionString = "Server=GRAMM\\TESTDB;Database=AutoLot;Trusted_Connection=True;TrustServerCertificate=True;";

            connection.Open();

            string sql = @"SELECT i.Id, m.Name AS Make, i.Color, i.PetName
                            FROM Inventory i
                            INNER JOIN Makes m on m.Id = i.MakeId ORDER BY i.Id ASC";
            SqlCommand command = new SqlCommand(sql, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"------------\nMake: {reader["Make"]} \nColor: {reader["Color"]} \nPetName: {reader["PetName"]}");
                    Console.WriteLine("------------");
                }
            }
        }
    }
}
