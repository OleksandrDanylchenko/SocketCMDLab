using Logging;
using Message;
using System;
using System.Net;
using System.Net.Sockets;

namespace SocketClient
{
    internal static class ClientSocket
    {
        private static Socket clientSocket;

        private static readonly Logger logger = new Logger(@"D:\Studying\2_Course\AOC\SocketLab\clientLog.txt");

        private static Socket ConfigureClientSocket()
        {
            // setting remote endPoint for socket
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            int port = 1025 + 3;
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // connecting socket with remote endPoint
            sender.Connect(ipEndPoint);

            return sender;
        }

        public static void SendMessage(FormatMessage formatMsg)
        {
            clientSocket = ConfigureClientSocket(); // configuring clientSocket before every message sending

            Console.WriteLine("\n\tSocket connecting with {0} ", clientSocket.RemoteEndPoint.ToString());
            byte[] byteMessage = formatMsg.Serialize();

            // sending data through socket
            clientSocket.Send(byteMessage);

            Console.WriteLine($"Client sent \"{formatMsg.Command}\"");
            logger.LogCommand(true, true, formatMsg.Command);
        }

        public static void ReceiveMessage()
        {
            // input data buffer
            byte[] receivedBytes = new byte[256];

            // receiving server's answer
            clientSocket.Receive(receivedBytes);
            FormatMessage receivedMessage = FormatMessage.Desserialize(receivedBytes);

            string answer = receivedMessage.Command;
            
            Console.WriteLine($"Server answer:\t{answer}");
            logger.LogCommand(false, true, answer);
        }

        public static void CloseSocket()
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}