// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 3

using NoConcat;

var deque = Deque<char>.Empty
    .PushLeft('U').PushRight('V').PushLeft('Q').PushLeft('R')
    .PushLeft('S').PushRight('W').PushRight('X').PushRight('Y');
deque = deque.PushLeft('Q');
deque = deque.PushRight('Z');
deque = deque.PushLeft('P').PushLeft('O').PushLeft('N')
    .PushLeft('M').PushLeft('L').PushLeft('K').PushLeft('J')
    .PushLeft('I').PushLeft('H').PushLeft('G').PushLeft('F');
deque = deque.PushLeft('E');
