using System;

namespace Life.Sandbox
{
    class Program
    {
        static void Main()
        {
            Console.SetWindowSize(100, 50);
            Console.SetBufferSize(100, 50);
            new TestApp().Run();
        }
    }
}
