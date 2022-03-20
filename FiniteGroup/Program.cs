using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    class MainClass
    {
        static void SamplesSn()
        {
            var s4 = new Sn(4);
            Sn.DetailSn(s4.Tau(2), s4.Tau(3, 4));
            Sn.DetailSn(s4.Tau(2), s4.Tau(3));

            var s5 = new Sn(5);
            Sn.DetailSn(s5.PCycle(3), s5.Tau(4, 5));

            Sn.Dihedral(4);
            Sn.Dihedral(6);
        }

        static void SamplesZn()
        {
            var z6 = new Zn(6);
            Zn.DetailZn(z6.Elt(3));
            Zn.DetailZn(z6.Elt(4));
            Zn.DetailZn(z6);

            Zn.DetailZn(2, 2);
            Zn.DetailZn(4);

            Zn.DetailZn(2, 2, 2);
            Zn.DetailZn(2, 4);

            Zn.DisplayZn(2, 6);
            Zn.DisplayZn(3, 4);
            Zn.DisplayZn(12);
        }

        public static void Main(string[] args)
        {
            Sn.Dihedral(8);
        }
    }
}
