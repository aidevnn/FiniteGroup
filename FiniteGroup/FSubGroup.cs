using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class FSubGroup<T> where T: GElt
    {
        static List<string> GenLetters(int n)
        {
            if (n > 50)
                return Enumerable.Range(1, n).Select(a => $"E{a,2:0000}").ToList();

            return "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(n).Select(c => $"{c}").ToList();
        }

        List<T> Elements;
        FGroup<T> Group;
        public FSubGroup(FGroup<T> group)
        {
            Group = group;
            Elements = new List<T>() { Group.Identity };
        }

        public FSubGroup(FSubGroup<T> other, T element)
        {
            Group = other.Group;
            Elements = new List<T>(other.Elements);
            if (Group.HashCode == element.FSet.HashCode)
                Generate(element);
        }

        public FSubGroup(FSubGroup<T> a, FSubGroup<T> b)
        {
            Group = a.Group;
            Elements = new List<T>(a.Elements);

            if (a.Group.HashCode == b.Group.HashCode)
                b.Elements.ForEach(Generate);
        }

        void Generate(T element)
        {
            var hs = new HashSet<T>(Elements, new ObjEquality<T>());
            element.Generated.ForEach(hash => hs.Add(Group.GetElement<T>(hash)));
            int sz = 0;

            do
            {
                sz = hs.Count;
                var lt = hs.ToHashSet();
                foreach (var e0 in lt)
                {
                    foreach (var e1 in lt)
                    {
                        if (Group.TableOpContains(e0.HashCode, e1.HashCode))
                        {
                            var hash = Group.TableOp(e0.HashCode, e1.HashCode);
                            hs.Add(Group.GetElement<T>(hash));
                        }
                        else
                            hs.Add(Group.Op(e0, e1));
                    }
                }
            } while (hs.Count != sz);

            Elements = new List<T>(hs);
            Elements.Sort();
        }

        public List<T> AllElements => Elements.ToList();

        public bool SubGroupOf(FSubGroup<T> gr)
        {
            if (Group.HashCode != gr.Group.HashCode)
                return false;

            return Elements.All(e0 => gr.Elements.Any(e1 => e1.HashCode == e0.HashCode));
        }

        public void DisplayElements()
        {
            Console.WriteLine(Group.Fmt, Elements.Count);

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
            Console.WriteLine(Group.Fmt, Elements.Count);

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
                var l0 = Elements.Select(e1 => ec[Group.Op(e1, e0)]).ToList();
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

        public FSubGroup<T> GenerateSubGroup(params T[] ts)
        {
            if (ts.Length == 0)
                return this;

            return (new FSubGroup<T>(this, ts[0])).GenerateSubGroup(ts.Skip(1).ToArray());
        }

        public FSubGroup<T> Compose(FSubGroup<T> fSub) => new FSubGroup<T>(this, fSub);
    }
}
