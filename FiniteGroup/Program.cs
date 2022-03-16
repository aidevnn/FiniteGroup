using System;

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

        public static void Main(string[] args)
        {
            //var s2 = new Sn(2);
            //Sn.DetailGroup(s2.Tau(1));
            //Sn.DetailGroup(s2.Tau(2));

            //var s3 = new Sn(3);
            //Sn.DetailGroup(s3.Tau(1));
            //Sn.DetailGroup(s3.Tau(2));
            //Sn.DetailGroup(s3.Tau(3));
            //Sn.DetailGroup(s3.PCycle(3));
            //Sn.DetailGroup(s3.Tau(2), s3.Tau(3));

            //var s4 = new Sn(4);
            //var s7 = new Sn(7);
            //var s8 = new Sn(8);

            //Sn.DetailGroup(s7.PCycle(3));
            //Sn.DetailGroup(s7.RCycle(4, 4));
            //Sn.DetailGroup(s7.PCycle(3), s7.RCycle(4, 4));
            //Sn.DetailGroup(s8.PCycle(3), s8.RCycle(4, 5));
            //Sn.DetailGroup(s4.Tau(2), s4.Tau(3), s4.Tau(4));

            //SamplesZn();

            //Sn.DetailGroup(s2);
            //Sn.DetailGroup(s3);
            //Sn.DetailGroup(s4);
            //Sn.DetailSn(4);
            //Sn.DetailSn(5);

            var s4 = new Sn(4);
            Sn.DetailGroup(s4.Tau(2), s4.Tau(3, 4));

            var z6 = new Zn(6);
            Zn.DetailGroup(z6.Elt(3));
            Zn.DetailGroup(z6.Elt(4));
            Zn.DetailGroup(z6);
        }
    }
}
