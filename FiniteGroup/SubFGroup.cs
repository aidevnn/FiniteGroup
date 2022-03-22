using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public abstract class SubFGroup<T> where T : GElt, IComparable<T>
    {
        public FGroup<T> FGroup { get; }
        protected readonly List<T> elements;
        public List<T> Elements => elements.ToList();

        protected SubFGroup(FGroup<T> fGroup)
        {
            FGroup = fGroup;
            elements = new List<T>() { FGroup.Identity };
        }

        public bool IsGroup()
        {
            foreach (var e0 in elements)
            {
                var e_0 = FGroup.Invert(e0);
                if (!elements.Contains(e_0))
                    return false;

                foreach (var e1 in elements)
                {
                    var e2 = FGroup.Op(e0, e1);
                    if (!elements.Contains(e2))
                        return false;
                }
            }

            return true;
        }

        public void Amplify()
        {
            var hashList = elements.SelectMany(e => e.Generated).ToHashSet();
            var hs = hashList.Select(FGroup.GetElement<T>).ToHashSet();
            int sz = 0;

            do
            {
                sz = hs.Count;
                var lt = hs.ToList();
                foreach (var e0 in lt)
                {
                    foreach (var e1 in lt)
                    {
                        T e2;
                        if (FGroup.TableOpContains(e0.HashCode, e1.HashCode))
                            e2 = FGroup.GetElement<T>(FGroup.TableOp(e0.HashCode, e1.HashCode));
                        else
                            e2 = FGroup.Op(e0, e1);

                        foreach (var h in e2.Generated)
                            hs.Add(FGroup.GetElement<T>(h));
                    }
                }
            } while (sz != hs.Count);

            elements.Clear();
            elements.AddRange(hs);
        }

        static List<string> GenLetters(int n)
        {
            if (n > 50)
                return Enumerable.Range(1, n).Select(a => $"E{a,2:0000}").ToList();

            return "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(n).Select(c => $"{c}").ToList();
        }

        public void DisplayElements()
        {
            Console.WriteLine(FGroup.Fmt, Elements.Count);

            if (Elements.Count > 1000)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = GenLetters(Elements.Count);
            for (int k = 0; k < Elements.Count; ++k)
                Elements.ElementAt(k).Display(word[k].ToString());

            Console.WriteLine();
        }

        public void Table()
        {
            Console.WriteLine(FGroup.Fmt, Elements.Count);
            if (!IsGroup())
            {
                Console.WriteLine("Not a Group, need to be amplified");
                return;
            }

            if (Elements.Count > 50)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = GenLetters(Elements.Count).Select(w => w[0]).ToList();
            Dictionary<char, T> ce = new Dictionary<char, T>();
            Dictionary<T, char> ec = new Dictionary<T, char>(new ObjEquality<T>());

            for (int k = 0; k < Elements.Count; ++k)
            {
                var c = word[k];
                var e = Elements.ElementAt(k);
                ce[c] = e;
                ec[e] = c;
            }

            string MyFormat(string c, string g, List<char> l) => string.Format("{0,2}|{1}", c, string.Join(g, l));

            var head = MyFormat("*", " ", word);
            var line = MyFormat("--", "", Enumerable.Repeat('-', word.Count * 2).ToList());
            Console.WriteLine(head);
            Console.WriteLine(line);

            foreach (var e0 in Elements)
            {
                var v0 = ec[e0].ToString();
                var l0 = Elements.Select(e1 => ec[FGroup.Op(e1, e0)]).ToList();
                Console.WriteLine(MyFormat(v0, " ", l0));
            }

            Console.WriteLine();
        }

        public void Details()
        {
            DisplayElements();
            Table();
            Console.WriteLine();
        }
    }

    public class Monogenic<T> : SubFGroup<T> where T : GElt
    {
        public T E { get; }
        public Monogenic(FGroup<T> fGroup, T e) : base(fGroup)
        {
            E = e;
            elements.Clear();
            elements.AddRange(E.Generated.Select(FGroup.GetElement<T>));
            elements.Sort();
        }
    }

    public class LeftOp<T> : SubFGroup<T> where T : GElt
    {
        public T E { get; }
        public SubFGroup<T> SubFGroup { get; }
        public LeftOp(T e, SubFGroup<T> subFGroup, bool amplify = false) : base(subFGroup.FGroup)
        {
            SubFGroup = subFGroup;
            E = e;
            elements.AddRange(subFGroup.Elements.Select(e1 => FGroup.Op(e, e1)));

            if (amplify) Amplify();
            elements.Sort();
        }

        public LeftOp(FGroup<T> fGroup, T e, SubFGroup<T> subFGroup, bool amplify = false) : base(fGroup)
        {
            SubFGroup = subFGroup;
            E = e;
            elements.AddRange(subFGroup.Elements.Select(e1 => FGroup.Op(e, e1)));

            if (amplify) Amplify();
            elements.Sort();
        }
    }

    public partial class FGroup<T>
    {
        protected SubFGroup<T> MonoGenic(T t) => new Monogenic<T>(this, t);
        protected SubFGroup<T> LeftCompose(params T[] elts)
        {
            var q = new Queue<T>(elts);
            SubFGroup<T> sg0 = new Monogenic<T>(this, q.Dequeue());

            while (q.Count != 0)
                sg0 = new LeftOp<T>(this, q.Dequeue(), sg0, true);

            return sg0;
        }

        protected SubFGroup<T> LeftCompose(T elt, SubFGroup<T> subFGroup, bool amplify = false) => new LeftOp<T>(this, elt, subFGroup, amplify);
    }
}
