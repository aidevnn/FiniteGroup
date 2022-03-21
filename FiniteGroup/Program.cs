using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    class MainClass
    {
        static void SamplesSn()
        {
            Console.WriteLine("##    S4   ##");
            var s4 = new Sn(4);
            Sn.Details(s4.Tau(2), s4.Tau(3, 4));
            Sn.From((1, 2), (3, 4)).Details();
            Console.WriteLine("##");

            Sn.Details(s4.Tau(2), s4.Tau(3));
            Sn.From((1, 2), (1, 3)).Details();

            Console.WriteLine("##    S5   ##");
            var s5 = new Sn(5);
            Sn.Details(s5.PCycle(3), s5.Tau(4, 5));
            Sn.From(Sn.PC(3), (4, 5)).Details();

            Console.WriteLine("## Dihedral ##");
            Sn.Dihedral(4);
            Sn.Dihedral(6);
        }

        static void SamplesZn()
        {
            var z6 = new Zn(6);
            Zn.DetailZn(z6.Elt(3));
            Zn.DetailZn(z6.Elt(4));
            Zn.Details(z6);

            Zn.Details(2, 2);
            Zn.Details(4);

            Zn.Details(2, 2, 2);
            Zn.Details(2, 4);

            Zn.Display(2, 6);
            Zn.Display(3, 4);
            Zn.Display(12);
        }

        public static void Main(string[] args)
        {
            SamplesSn();
        }
    }
}
