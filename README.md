# FiniteGroup
Group theory

```
var s4 = new Sigma(4);
s4.DetailGroup(s4.Tau(2), s4.Tau(3, 4));
```

Will output the permutations and the generated  group table

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