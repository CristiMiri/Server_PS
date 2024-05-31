using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Repositories
{
    internal class Repository
    {
        private NpgsqlConnection connection;
        private static readonly object padlock = new object();
        private static Repository instance = null;

        // Private constructor to prevent direct instantiation
        private Repository()
        {
            string connString = "Server=localhost;Port=5432;Database=proiectps;User Id=postgres;Password=SQL;";
            connection = new NpgsqlConnection(connString);
        }

        // Public property to provide access to the single instance
        public static Repository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Repository();
                        }
                    }
                }
                return instance;
            }
        }

        public NpgsqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        // Open the connection to the database
        public void OpenConnection()
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }

        // Close the connection to the database
        public void CloseConnection()
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }

        // Execute nonquery to modify the database (insert, update, delete)
        // and return status of the operation
        public bool ExecuteNonQuery(string query)
        {
            try
            {
                OpenConnection();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                return true; // Success if no exception
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false; // Return false if an exception is thrown
            }
            finally
            {
                CloseConnection(); // Ensure connection is always closed
            }
        }

        // Execute query to get data from database
        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                OpenConnection();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                CloseConnection();
            }
            return dt;
        }
    }
}
