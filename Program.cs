using System.Data.Common;
using System.Data.Odbc;

#if PC
    using System.Data.OleDb;
#endif
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DataProviderFactory;

namespace ADONETExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** Fun With Data Provider Factories *****");
            var (provider, connectionString) = GetProviderFromConfiguration();
            DbProviderFactory? factory = GetDbProviderFactory(provider);
            using (DbConnection connection = factory.CreateConnection())
            {
                Console.WriteLine($"Your connection object is: {connection.GetType().Name}");
                connection.ConnectionString = connectionString;
                connection.Open();

                //Make command object
                DbCommand command = factory.CreateCommand();
                Console.WriteLine($"Your command object is a: {command.GetType().Name}");
                command.Connection = connection;
                command.CommandText = "Select i.Id, m.Name From Inventory i inner join Makes m on m.Id = i.MakeId Order By i.Id ASC";

                //print out data with data reader.
                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    Console.WriteLine($"Your data reader object is a: {dataReader.GetType().Name}");
                    Console.WriteLine("\n***** Current Inventory ****");
                    while (dataReader.Read())
                    {
                        
                        Console.WriteLine($"-> Car #{dataReader["Id"]} is a #{dataReader["Name"]}");
                    }
                }
            }

            Console.ReadLine();
        }


        private static (DataProviderEnum provider, string connectionString) GetProviderFromConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var providerName = config["ProviderName"];
            if (Enum.TryParse<DataProviderEnum>(providerName, out DataProviderEnum provider))
            {
                return (provider, config[$"{providerName}:ConnectionString"]);
            }
            throw new Exception("Invalid data provider value supplied.");
        }
        private static DbProviderFactory? GetDbProviderFactory(DataProviderEnum provider) => provider switch
        {
            DataProviderEnum.SqlServer => SqlClientFactory.Instance,
            DataProviderEnum.Odbc => OdbcFactory.Instance,
            #if PC
                    DataProviderEnum.OleDb => OleDbFactory.Instance,
            #endif
            _ => null
        };
    }
}
