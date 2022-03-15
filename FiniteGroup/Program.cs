using System;

namespace FiniteGroup
{
    class MainClass
    {
        public static void Main(string[] args)
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

            //Sn.DetailGroup(s8.RCycle(3, 6), s8.Tau(2));
        }
    }
}
