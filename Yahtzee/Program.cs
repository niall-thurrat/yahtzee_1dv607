using System;

namespace Yahtzee
{
    class Program
    {
        static void Main(string[] args)
        {
            model.Game g = new model.Game();
            view.UI v = new view.UI();
            controller.Application ctrl = new controller.Application(v, g);

            ctrl.Play();
        }
    }
}
