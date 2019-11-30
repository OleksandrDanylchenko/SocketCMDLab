using System;
using System.Collections.Generic;

namespace SocketServer
{
    internal class DataAnalyse : ServerSocket
    {
        public static List<string> cmdCommands = new List<string>();

        public static void Parse()
        {
            byte headerLen = LastMessage.HeaderLength;
            string command = LastMessage.Command;

            if (headerLen != command.Length)
            {
                throw new ArgumentException("Header length doesn't match command length!");
            }
            else if (command.Equals("Who", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine("Laboratory Work №3");
                Console.WriteLine("Alexander Danilchenko");
            }
            else if (command.Equals("End", StringComparison.CurrentCultureIgnoreCase))
            {
                CmdExecuter.ExecuteAllCommands(); // execution after successfull receiving of all
            }
            else
            {
                CmdExecuter.AccumulateCommand(command);
            }
        }
    }
}
