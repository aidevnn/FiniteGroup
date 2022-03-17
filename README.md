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
Z/2Z isnt isomorphic to Z/4Z

```
ZxZ.DetailGroup(2, 2);
|G| = 4 in Z/2Z x Z/2Z
@ = (0 ,0)<1>
a = (1 ,0)<2>
b = (0 ,1)<2>
c = (1 ,1)<2>

 *|@ a b c
--|--------
 @|@ a b c
 a|a @ c b
 b|b c @ a
 c|c b a @
```

And
```
ZxZ.DetailGroup(4);

|G| = 4 in Z/4Z
@ = (0)<1>
a = (2)<2>
b = (1)<4>
c = (3)<4>

 *|@ a b c
--|--------
 @|@ a b c
 a|a @ c b
 b|b c a @
 c|c b @ a

```

Comparing Z/2Z x Z/2Z x Z/2Z with Z/2Z x Z/4Z
```
ZxZ.DetailGroup(2, 2, 2);


|G| = 8 in Z/2Z x Z/2Z x Z/2Z
@ = (0 ,0 ,0)<1>
a = (1 ,0 ,0)<2>
b = (0 ,1 ,0)<2>
c = (1 ,1 ,0)<2>
d = (0 ,0 ,1)<2>
e = (1 ,0 ,1)<2>
f = (0 ,1 ,1)<2>
g = (1 ,1 ,1)<2>

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
ZxZ.DetailGroup(2, 4);

|G| = 8 in Z/2Z x Z/4Z
@ = (0 ,0)<1>
a = (1 ,0)<2>
b = (0 ,2)<2>
c = (1 ,2)<2>
d = (0 ,1)<4>
e = (1 ,1)<4>
f = (0 ,3)<4>
g = (1 ,3)<4>

 *|@ a b c d e f g
--|----------------
 @|@ a b c d e f g
 a|a @ c b e d g f
 b|b c @ a f g d e
 c|c b a @ g f e d
 d|d e f g b c @ a
 e|e d g f c b a @
 f|f g d e @ a b c
 g|g f e d a @ c b

```

Comparing Z/12Z with Z/2Z x Z/6Z and Z/3Z x Z/4Z
```
ZxZ.DisplayGroup(2, 6);
ZxZ.DisplayGroup(3, 4);
ZxZ.DisplayGroup(12);
            
|G| = 12 in Z/2Z x Z/6Z
@ = (0 ,0)<1>
a = (1 ,0)<2>
b = (0 ,3)<2>
c = (1 ,3)<2>
d = (0 ,2)<3>
e = (0 ,4)<3>
f = (0 ,1)<6>
g = (1 ,1)<6>
h = (1 ,2)<6>
i = (1 ,4)<6>
j = (0 ,5)<6>
k = (1 ,5)<6>


|G| = 12 in Z/3Z x Z/4Z
@ = (0 ,0)<1>
a = (0 ,2)<2>
b = (1 ,0)<3>
c = (2 ,0)<3>
d = (0 ,1)<4>
e = (0 ,3)<4>
f = (1 ,2)<6>
g = (2 ,2)<6>
h = (1 ,1)<12>
i = (2 ,1)<12>
j = (1 ,3)<12>
k = (2 ,3)<12>


|G| = 12 in Z/12Z
@ = (0)<1>
a = (6)<2>
b = (4)<3>
c = (8)<3>
d = (3)<4>
e = (9)<4>
f = (2)<6>
g = (10)<6>
h = (1)<12>
i = (5)<12>
j = (7)<12>
k = (11)<12>


```