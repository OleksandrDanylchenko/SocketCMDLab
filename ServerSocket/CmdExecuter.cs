using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SocketServer
{
    internal class CmdExecuter : ServerSocket
    {
        private static readonly List<string> commandsLs = new List<string>();
        private static string[] availableCommands = { "cd", "dir", "echo", "ipconfig", "Who", "End" };

        public static void AccumulateCommand(string fm)
        {
            if (IsAvailableOperation(fm))
            {
                commandsLs.Add(fm);
            }
            else
            {
                ErrorCMDReply = fm;
            }
        }

        private static string ExecuteSingleCommand(string execCmd)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + execCmd)
                {
                    // The following commands are needed to redirect the standard output.
                    // This means that it will be redirected to the Process.StandardOutput StreamReader.
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    // Do not create the black window.
                    CreateNoWindow = true
                };

                // Now we create a process, assign its ProcessStartInfo and start it
                using (Process proc = new Process())
                {
                    proc.StartInfo = procStartInfo;
                    proc.Start();

                    // Get the output into a string
                    string result = proc.StandardOutput.ReadToEnd();
                    return result;
                }
            }
            catch (Exception)
            {
                ErrorCMDReply = execCmd;
                return "Unhandled command";
            }
        }

        public static void ExecuteAllCommands()
        {
            for (int i = 0; i < commandsLs.Count; ++i)
            {
                Console.WriteLine($"\nCommand \"{commandsLs[i]}\" returned:");
                string result = ExecuteSingleCommand(commandsLs[i]);
                Console.WriteLine(result);
                Console.WriteLine();
            }
            IsAllCommandsExecuted = true;
        }

        private static bool IsAvailableOperation(string command)
        {
            bool contained = availableCommands.Any(command.Contains);
            return contained;
        }
    }
}
