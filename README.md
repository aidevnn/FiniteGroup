# FiniteGroup
Group theory. A simple C# code to show quickly generated group. 

```
var s4 = new Sn(4);
Sn.DetailSn(s4.Tau(2), s4.Tau(3, 4));
```

Will output the permutations (with its name, its order and its signature) and the generated group table.

```
|G| = 4 in S4
@ = ( 1  2  3  4)[1+]
a = ( 1  2  4  3)[2-]
b = ( 2  1  3  4)[2-]
c = ( 2  1  4  3)[2+]

|G| = 4 in S4
 *|@ a b c
--|--------
 @|@ a b c
 a|a @ c b
 b|b c @ a
 c|c b a @

```

And

```
Sn.DetailSn(s4.Tau(2), s4.Tau(3));
```

Will output

```
|G| = 6 in S4
@ = ( 1  2  3  4)[1+]
a = ( 1  3  2  4)[2-]
b = ( 2  1  3  4)[2-]
c = ( 3  2  1  4)[2-]
d = ( 2  3  1  4)[3+]
e = ( 3  1  2  4)[3+]

|G| = 6 in S4
 *|@ a b c d e
--|------------
 @|@ a b c d e
 a|a @ d e b c
 b|b e @ d c a
 c|c d e @ a b
 d|d c a b e @
 e|e b c a @ d

```

## Now try disjunct 3-cycle and transposition.

```
var s5 = new Sn(5);
Sn.DetailSn(s5.PCycle(3), s5.Tau(4, 5));
```

and you will obtain a commutative group

```
|G| = 6 in S5
@ = ( 1  2  3  4  5)[1+]
a = ( 1  2  3  5  4)[2-]
b = ( 2  3  1  4  5)[3+]
c = ( 3  1  2  4  5)[3+]
d = ( 2  3  1  5  4)[6-]
e = ( 3  1  2  5  4)[6-]

|G| = 6 in S5
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
Zn.DetailZn(z6.Elt(3));
Zn.DetailZn(z6.Elt(4));
Zn.DetailZn(z6);
            
|G| = 2 in Z/6Z
@ = ( 0)[1]
a = ( 3)[2]

|G| = 2 in Z/6Z
 *|@ a
--|----
 @|@ a
 a|a @


|G| = 3 in Z/6Z
@ = ( 0)[1]
a = ( 2)[3]
b = ( 4)[3]

|G| = 3 in Z/6Z
 *|@ a b
--|------
 @|@ a b
 a|a b @
 b|b @ a


|G| = 6 in Z/6Z
@ = ( 0)[1]
a = ( 3)[2]
b = ( 2)[3]
c = ( 4)[3]
d = ( 1)[6]
e = ( 5)[6]

|G| = 6 in Z/6Z
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
Z/2Z isnt isomorphic to Z/4Z

```
Zn.DetailZn(2, 2);

|G| = 4 in Z/2Z x Z/2Z
@ = ( 0,  0)[1]
a = ( 0,  1)[2]
b = ( 1,  0)[2]
c = ( 1,  1)[2]

|G| = 4 in Z/2Z x Z/2Z
 *|@ a b c
--|--------
 @|@ a b c
 a|a @ c b
 b|b c @ a
 c|c b a @

```

And
```
Zn.DetailZn(4);

|G| = 4 in Z/4Z
@ = ( 0)[1]
a = ( 2)[2]
b = ( 1)[4]
c = ( 3)[4]

|G| = 4 in Z/4Z
 *|@ a b c
--|--------
 @|@ a b c
 a|a @ c b
 b|b c a @
 c|c b @ a
 
```

Comparing Z/2Z x Z/2Z x Z/2Z with Z/2Z x Z/4Z
```
Zn.DetailZn(2, 2, 2);


|G| = 8 in Z/2Z x Z/2Z x Z/2Z
@ = ( 0,  0,  0)[1]
a = ( 0,  0,  1)[2]
b = ( 0,  1,  0)[2]
c = ( 0,  1,  1)[2]
d = ( 1,  0,  0)[2]
e = ( 1,  0,  1)[2]
f = ( 1,  1,  0)[2]
g = ( 1,  1,  1)[2]

|G| = 8 in Z/2Z x Z/2Z x Z/2Z
 *|@ a b c d e f g
--|----------------
 @|@ a b c d e f g
 a|a @ c b e d g f
 b|b c @ a f g d e
 c|c b a @ g f e d
 d|d e f g @ a b c
 e|e d g f a @ c b
 f|f g d e b c @ a
 g|g f e d c b a @

```

```
Zn.DetailZn(2, 4);

|G| = 8 in Z/2Z x Z/4Z
@ = ( 0,  0)[1]
a = ( 0,  2)[2]
b = ( 1,  0)[2]
c = ( 1,  2)[2]
d = ( 0,  1)[4]
e = ( 0,  3)[4]
f = ( 1,  1)[4]
g = ( 1,  3)[4]

|G| = 8 in Z/2Z x Z/4Z
 *|@ a b c d e f g
--|----------------
 @|@ a b c d e f g
 a|a @ c b e d g f
 b|b c @ a f g d e
 c|c b a @ g f e d
 d|d e f g a @ c b
 e|e d g f @ a b c
 f|f g d e c b a @
 g|g f e d b c @ a

```

Comparing Z/12Z with Z/2Z x Z/6Z and Z/3Z x Z/4Z
```
Zn.DisplayZn(2, 6);
Zn.DisplayZn(3, 4);
Zn.DisplayZn(12);
            
|G| = 12 in Z/2Z x Z/6Z
@ = ( 0,  0)[1]
a = ( 0,  3)[2]
b = ( 1,  0)[2]
c = ( 1,  3)[2]
d = ( 0,  2)[3]
e = ( 0,  4)[3]
f = ( 0,  1)[6]
g = ( 0,  5)[6]
h = ( 1,  1)[6]
i = ( 1,  2)[6]
j = ( 1,  4)[6]
k = ( 1,  5)[6]


|G| = 12 in Z/3Z x Z/4Z
@ = ( 0,  0)[1]
a = ( 0,  2)[2]
b = ( 1,  0)[3]
c = ( 2,  0)[3]
d = ( 0,  1)[4]
e = ( 0,  3)[4]
f = ( 1,  2)[6]
g = ( 2,  2)[6]
h = ( 1,  1)[12]
i = ( 1,  3)[12]
j = ( 2,  1)[12]
k = ( 2,  3)[12]


|G| = 12 in Z/12Z
@ = ( 0)[1]
a = ( 6)[2]
b = ( 4)[3]
c = ( 8)[3]
d = ( 3)[4]
e = ( 9)[4]
f = ( 2)[6]
g = (10)[6]
h = ( 1)[12]
i = ( 5)[12]
j = ( 7)[12]
k = (11)[12]
```

# Dihedral Dn generations

```
Sn.Dihedral(4);

(e0 * e1) * (e0 * e1) = id
e0 = ( 2  1  3  4)[2-]
e1 = ( 4  3  1  2)[4-]
e0 * e1
   = ( 4  3  2  1)[2+]

|G| = 8 in S4
@ = ( 1  2  3  4)[1+]
a = ( 1  2  4  3)[2-]
b = ( 2  1  3  4)[2-]
c = ( 2  1  4  3)[2+]
d = ( 3  4  1  2)[2+]
e = ( 4  3  2  1)[2+]
f = ( 3  4  2  1)[4-]
g = ( 4  3  1  2)[4-]

|G| = 8 in S4
 *|@ a b c d e f g
--|----------------
 @|@ a b c d e f g
 a|a @ c b f g d e
 b|b c @ a g f e d
 c|c b a @ e d g f
 d|d g f e @ c b a
 e|e f g d c @ a b
 f|f e d g a b c @
 g|g d e f b a @ c

```
And D6 
```
Sn.Dihedral(6);

(e0 * e1) * (e0 * e1) = id
e0 = ( 2  1  3  4  5  6)[2-]
e1 = ( 5  1  4  3  2  6)[6-]
e0 * e1
   = ( 5  2  4  3  1  6)[2+]

|G| = 12 in S6
@ = ( 1  2  3  4  5  6)[1+]
a = ( 1  2  4  3  5  6)[2-]
b = ( 1  5  3  4  2  6)[2-]
c = ( 1  5  4  3  2  6)[2+]
d = ( 2  1  3  4  5  6)[2-]
e = ( 2  1  4  3  5  6)[2+]
f = ( 5  2  3  4  1  6)[2-]
g = ( 5  2  4  3  1  6)[2+]
h = ( 2  5  3  4  1  6)[3+]
i = ( 5  1  3  4  2  6)[3+]
j = ( 2  5  4  3  1  6)[6-]
k = ( 5  1  4  3  2  6)[6-]

|G| = 12 in S6
 *|@ a b c d e f g h i j k
--|------------------------
 @|@ a b c d e f g h i j k
 a|a @ c b e d g f j k h i
 b|b c @ a h j i k d f e g
 c|c b a @ j h k i e g d f
 d|d e i k @ a h j f b g c
 e|e d k i a @ j h g c f b
 f|f g h j i k @ a b d c e
 g|g f j h k i a @ c e b d
 h|h j f g b c d e i @ k a
 i|i k d e f g b c @ h a j
 j|j h g f c b e d k a i @
 k|k i e d g f c b a j @ h

```