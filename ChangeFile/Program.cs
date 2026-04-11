using System;

namespace ChangeFile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = new ChangeFileApplication(new ValidatedConsolePrompts());
            app.Run();
        }
    }
}
