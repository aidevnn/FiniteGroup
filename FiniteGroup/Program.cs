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
            var s5 = new Sn(5);
            var s7 = new Sn(7);
            var s8 = new Sn(8);

            //Sn.DetailGroup(s4.Tau(2), s4.Tau(3, 4));
            //Sn.DetailGroup(s4.Tau(2), s4.Tau(3));

            //Sn.DetailGroup(s5.PCycle(3), s5.Tau(4, 5));
            //Sn.DetailGroup(s5.PCycle(3), s5.Tau(2), s5.Tau(4, 5));
            //Sn.DetailGroup(s5.Tau(2), s5.Tau(3), s5.Tau(4, 5));

            //Sn.DetailGroup(s7.PCycle(3), s7.Tau(4, 5), s7.Tau(6, 7));
            //Sn.DetailGroup(s7.PCycle(3), s7.RCycle(4, 4));

            Sn.DetailGroup(s7.PCycle(3));
            Sn.DetailGroup(s7.RCycle(4, 4));
            Sn.DetailGroup(s7.PCycle(3), s7.RCycle(4, 4));
            Sn.DetailGroup(s8.PCycle(3), s8.RCycle(4, 5));

            //Sn.DetailGroup(s8.RCycle(3, 6), s8.Tau(2));
        }

        static void SamplesZn()
        {
            var z2 = new Zn(2);
            var z3 = new Zn(3);
            var z4 = new Zn(4);
            var z5 = new Zn(5);

            Zn.DetailGroup(z2.Elt(1));

            Zn.DetailGroup(z3.Elt(1));
            Zn.DetailGroup(z5.Elt(2));

            Zn.DetailGroup(z4.Elt(2));
            Zn.DetailGroup(z4);

            var z6 = new Zn(6);
            Zn.DetailGroup(z6.Elt(3));
            Zn.DetailGroup(z6.Elt(4));
            Zn.DetailGroup(z6);
            Zn.DetailGroup(new Zn(12));
        }

        static void SamplesZxZ()
        {
            var z2xz3 = new ZxZ(2, 3);
            z2xz3.Elt(1, 0).Display();
            z2xz3.Elt(1, 2).Display();
            z2xz3.Canonic(0).Display();
            z2xz3.Canonic(1).Display();
            ZxZ.DetailGroup(z2xz3.Canonic(0));
            ZxZ.DetailGroup(z2xz3.Canonic(1));
            ZxZ.DetailGroup(z2xz3.Canonic(0), z2xz3.Canonic(1));

            ZxZ.DetailGroup(2, 2, 2);
            ZxZ.DetailGroup(2, 4);
            ZxZ.DetailGroup(4, 2);

            ZxZ.DetailGroup(2, 2);
            ZxZ.DetailGroup(4);
            ZxZ.DetailGroup(2, 2, 2);
            ZxZ.DetailGroup(2, 4);

            ZxZ.DisplayGroup(2, 6);
            ZxZ.DisplayGroup(3, 4);
            ZxZ.DisplayGroup(12);
        }

        public static void Main(string[] args)
        {

            ZxZ.DisplayGroup(12);

        }
    }
}
