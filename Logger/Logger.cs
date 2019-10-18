using System;
using System.IO;

namespace Logging
{
    public class Logger
    {
        private DateTime LastActionTime = DateTime.Now;
        static uint commandNum = 0;
        static bool IsFirstOpen = true;

        private readonly string FilePath = string.Empty;

        public Logger(string filePath)
        {
            FilePath = filePath;
        }

        public void LogCommand(bool isSending, bool isClient, string command)
        {
            LastActionTime = DateTime.Now;
            ++commandNum;

            if (IsFirstOpen) // cleaning all data from text file if it's first command
            {
                using (StreamWriter writer = new StreamWriter(FilePath)) { }
                IsFirstOpen = false;
            }

            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"{commandNum} command:");
                writer.WriteLine(createOutput(isSending, isClient, command));
                writer.WriteLine(LastActionTime.ToString("F"));
                writer.WriteLine();
            }
        }

        private string createOutput(bool isSending, bool isClient, string command)
        {
            string result = string.Empty;

            if (isSending && isClient)
            {
                result = $"Client is sending: \"{command}\"";
            }
            else if (!isSending && isClient)
            {
                result = $"Client is receiving: \"{command}\"";
            }
            else if (isSending && !isClient)
            {
                result = $"Server is sending: \"{command}\"";
            }
            else if (!isSending && !isClient)
            {
                result = $"Server is receiving: \"{command}\"";
            }

            return result;
        }
    }
}