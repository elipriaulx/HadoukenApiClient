using System;
using System.Net;
using HadoukenApiClient;

namespace TestClient
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            var connection = new HadoukenConnection(new IPAddress(new byte[]{10, 40, 1, 10}), 7070, "admin", "admin");
            var client = new HadoukenClient(connection);

            try
            {
                var systemInfo = client.GetSystemInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }

        }

        #endregion
    }
}
