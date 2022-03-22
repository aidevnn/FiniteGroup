using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    class MainClass
    {
        static void SamplesSn()
        {
            Sn.From((1, 2), (3, 4)).Details();
            Sn.From((1, 2), (1, 3)).Details();
            Sn.From((2, 3, 1), (4, 5)).Details();

            Sn.Dihedral(4);
            Sn.From((2, 4), Sn.PC(4)).Details();

            Sn.Dihedral(6);
            Sn.From(((2, 6), (3, 5)), Sn.PC(6)).Details();
        }

        static void SamplesZn()
        {
            Zn.Dim(6).From(3).Details();
            Zn.Dim(6).From(4).Details();
            Zn.Details(6);

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
            //Sn.Dihedral(4);
            //Sn.Dihedral(6);

            Sn.Dihedral(4);
            Sn.From((2, 4), Sn.PC(4)).Details();

            Sn.Dihedral(6);
            Sn.From(((2, 6), (3, 5)), Sn.PC(6)).Details();
        }
    }
}
