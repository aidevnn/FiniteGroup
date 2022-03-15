# FiniteGroup
Group theory. A simple C# code to show quickly generated group. 

```
var s4 = new Sigma(4);
s4.DetailGroup(s4.Tau(2), s4.Tau(3, 4));
```

Will output the permutations (with its name and its order) and the generated group table.

```
[ 1  2  3  4](@)
[ 1  2  3  4](1)

[ 1  2  3  4](a)
[ 1  2  4  3](2)

[ 1  2  3  4](b)
[ 2  1  3  4](2)

[ 1  2  3  4](c)
[ 2  1  4  3](2)

 *|@ a b c
--|--------
 @|@ a b c
 a|a @ c b
 b|b c @ a
 c|c b a @
```

And

```
s4.DetailGroup(s4.Tau(2), s4.Tau(3));
```

Will output

```
[ 1  2  3  4](@)
[ 1  2  3  4](1)

[ 1  2  3  4](a)
[ 1  3  2  4](2)

[ 1  2  3  4](b)
[ 2  1  3  4](2)

[ 1  2  3  4](c)
[ 3  2  1  4](2)

[ 1  2  3  4](d)
[ 2  3  1  4](3)

[ 1  2  3  4](e)
[ 3  1  2  4](3)

 *|@ a b c d e
--|------------
 @|@ a b c d e
 a|a @ e d c b
 b|b d @ e a c
 c|c e d @ b a
 d|d b c a e @
 e|e c a b @ d

```

## Now try a disjuncted cycle and permutation

```
var s5 = new Sigma(5);
s5.DetailGroup(s5.PCycle(3), s5.Tau(4, 5));
```

and you will obtain a commutative group

```
[ 1  2  3  4  5](@)
[ 1  2  3  4  5](1)

[ 1  2  3  4  5](a)
[ 1  2  3  5  4](2)

[ 1  2  3  4  5](b)
[ 2  3  1  4  5](3)

[ 1  2  3  4  5](c)
[ 3  1  2  4  5](3)

[ 1  2  3  4  5](d)
[ 2  3  1  5  4](6)

[ 1  2  3  4  5](e)
[ 3  1  2  5  4](6)

 *|@ a b c d e
--|------------
 @|@ a b c d e
 a|a @ d e b c
 b|b d c @ e a
 c|c e @ b a d
 d|d b e a c @
 e|e c a d @ b

```