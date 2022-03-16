# FiniteGroup
Group theory. A simple C# code to show quickly generated group. 

```
var s4 = new Sn(4);
Sn.DetailGroup(s4.Tau(2), s4.Tau(3, 4));
```

Will output the permutations (with its name, its order and its signature) and the generated group table.

```
|G| = 4 in S4
@ = [ 1  2  3  4]<1+>
a = [ 1  2  4  3]<2->
b = [ 2  1  3  4]<2->
c = [ 2  1  4  3]<2+>

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
|G| = 6 in S4
@ = [ 1  2  3  4]<1+>
a = [ 1  3  2  4]<2->
b = [ 2  1  3  4]<2->
c = [ 3  2  1  4]<2->
d = [ 2  3  1  4]<3+>
e = [ 3  1  2  4]<3+>

 *|@ a b c d e
--|------------
 @|@ a b c d e
 a|a @ e d c b
 b|b d @ e a c
 c|c e d @ b a
 d|d b c a e @
 e|e c a b @ d

```

## Now try disjunct 3-cycle and transposition.

```
var s5 = new Sn(5);
Sn.DetailGroup(s5.PCycle(3), s5.Tau(4, 5));
```

and you will obtain a commutative group

```
|G| = 6 in S5
@ = [ 1  2  3  4  5]<1+>
a = [ 1  2  3  5  4]<2->
b = [ 2  3  1  4  5]<3+>
c = [ 3  1  2  4  5]<3+>
d = [ 2  3  1  5  4]<6->
e = [ 3  1  2  5  4]<6->

 *|@ a b c d e
--|------------
 @|@ a b c d e
 a|a @ d e b c
 b|b d c @ e a
 c|c e @ b a d
 d|d b e a c @
 e|e c a d @ b

```

# Z/nZ Groups are easiest to study


```
var z6 = new Zn(6);
Zn.DetailGroup(z6.Elt(3));
Zn.DetailGroup(z6.Elt(4));
Zn.DetailGroup(z6);
            
|G| = 2 in Z/6Z
@ = (0)<1>
a = (3)<2>

 *|@ a
--|----
 @|@ a
 a|a @

 
|G| = 3 in Z/6Z
@ = (0)<1>
a = (2)<3>
b = (4)<3>

 *|@ a b
--|------
 @|@ a b
 a|a b @
 b|b @ a


 |G| = 6 in Z/6Z
@ = (0)<1>
a = (3)<2>
b = (2)<3>
c = (4)<3>
d = (1)<6>
e = (5)<6>

 *|@ a b c d e
--|------------
 @|@ a b c d e
 a|a @ e d c b
 b|b e c @ a d
 c|c d @ b e a
 d|d c a e b @
 e|e b d a @ c

```

# Direct Product of Z/nZ

(In Progress)