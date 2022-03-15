using System;

namespace FiniteGroup
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var s4 = new Sigma(4);
            //s4.DisplayGroup(s4.PCycle(3));
            //s4.DisplayGroup(s4.Tau(2), s4.Tau(3));

            //s4.TableGroup(s4.Tau(2));
            //s4.TableGroup(s4.PCycle(3));
            //s4.TableGroup(s4.Tau(2), s4.Tau(3, 4));
            //s4.TableGroup(s4.Tau(2), s4.Tau(3));

            //s4.DetailGroup(s4.Tau(2));
            //s4.DetailGroup(s4.PCycle(3));
            s4.DetailGroup(s4.Tau(2), s4.Tau(3, 4));
            s4.DetailGroup(s4.Tau(2), s4.Tau(3));

            var s5 = new Sigma(5);
            s5.DetailGroup(s5.PCycle(3), s5.Tau(4, 5));
        }
    }
}
