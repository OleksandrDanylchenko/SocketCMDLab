using System;

namespace SocketServer
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                while (true)
                {
                    ServerSocket.ReceiveExecuteMessage();
                    ServerSocket.SendMessage();

                    if (ServerSocket.IsAllCommandsExecuted)
                    {
                        ServerSocket.CloseSocket();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception: {ex.Message}");
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
