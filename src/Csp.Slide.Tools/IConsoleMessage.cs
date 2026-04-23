using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csp.Slide.Tools
{
    internal interface IConsoleMessage
    {
        void WriteLine(string message);
        void Write(string message);
    }

    public class ConsoleMessage : IConsoleMessage
    {
        public void Write(string message)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            do { Console.Write("\b \b"); } while (Console.CursorLeft > 0);
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
