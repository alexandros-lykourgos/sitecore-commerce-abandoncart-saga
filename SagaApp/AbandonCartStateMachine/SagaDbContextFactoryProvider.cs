using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbandonCartStateMachine
{
    public class SagaDbContextFactoryProvider
    {
        static readonly Lazy<string> _connectionString = new Lazy<string>(GetConnectionString);

        private static string GetConnectionString()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["AbandonCartConnectionString"].ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                {
                    // It worked, we can save this as our connection string
                    return connectionString;
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException(
                "Couldn't connect to any of the LocalDB Databases. You might have a version installed that is not in the list. Please check the list and modify as necessary");
            }



        }


        public static string ConnectionString => _connectionString.Value;
    }
}
