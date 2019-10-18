using Message;
using System;
using System.Collections.Generic;
using System.IO;

namespace SocketClient
{
    internal class FileParser
    {
        private readonly string FilePath = string.Empty;
        public List<FormatMessage> MessagesLs { get; private set; } = new List<FormatMessage>();

        public FileParser(string[] args)
        {
            if (args.Length == 1)
            {
                FilePath = args[0];

                if (!FilePath.Contains(".txt"))
                {
                    throw new ArgumentException("Given path doesn't contain input txt file!");
                }

                ParseAndCreateCommandsLs();
            }
            else
            {
                throw new ArgumentException("Command Line argument formatting error!");
            }
        }

        private void ParseAndCreateCommandsLs()
        {
            using (StreamReader sr = File.OpenText(FilePath))
            {
                while (!sr.EndOfStream)
                {
                    string command = sr.ReadLine();
                    FormatMessage newMess = new FormatMessage(command);
                    MessagesLs.Add(newMess);
                }
            }
        }
    }
}
