using Logging;
using Message;
using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
    internal class ServerSocket
    {
        protected static readonly Socket serverSocket = ConfigureClientSocket();
        private static Socket handler; // represents connected client's socket
        protected static FormatMessage LastMessage;

        private static readonly Logger logger = new Logger(@"D:\Studying\2_Course\AOC\SocketLab\serverLog.txt");


        public static bool IsAllCommandsExecuted = false;
        protected static string ErrorCMDReply = string.Empty; // can be generated in CmdExecuter

        private static Socket ConfigureClientSocket()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            int portNum = 1025 + 3;
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, portNum);

            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // setting socket to local endPoint and listening for input sockets
            listener.Bind(ipEndPoint);
            listener.Listen(100);

            return listener;
        }

        public static void ReceiveMessage()
        {
            // waiting for input connection
            handler = serverSocket.Accept();

            // client has just connected

            // input data buffer
            byte[] receivedBytes = new byte[256];

            // receiving server's answer
            handler.Receive(receivedBytes);

            FormatMessage receivedMessage = FormatMessage.Desserialize(receivedBytes);
            LastMessage = receivedMessage;

            // analysing received messages
            DataAnalyse.Parse();

            logger.LogCommand(false, false, LastMessage.Command);
        }

        public static void SendMessage()
        {
            FormatMessage replyMsg;
            if (!string.IsNullOrWhiteSpace(ErrorCMDReply))
            {
                replyMsg = new FormatMessage($"Command: \"{ErrorCMDReply}\" made an error!");
                ErrorCMDReply = string.Empty;
            }
            else if (IsAllCommandsExecuted)
            {
                replyMsg = new FormatMessage("All commands executed!");
            }
            else
            {
                replyMsg = new FormatMessage("Server in work");
            }

            byte[] msg = replyMsg.Serialize();

            handler.Send(msg);

            logger.LogCommand(true, false, replyMsg.Command);
        }

        public static void CloseSocket()
        {
            serverSocket.Close();
        }
    }
}
