using System;

namespace Yahtzee
{
    class Program
    {
        static void Main(string[] args)
        {
            view.UI v = new view.UI();
            controller.Application ctrl = new controller.Application(v);

            ctrl.Run();
        }
    }
}
