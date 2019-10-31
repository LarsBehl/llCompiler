grammar ll;


compileUnit: expression EOF;
expression
    : '(' expression ')' #parenthes
    | left=WORD '=' right=expression #assignExpression
    | WORD #variableExpression
    | left=expression op=('*'|'/') right=expression #infixExpression
    | left=expression op=('+'|'-') right=expression #infixExpression
    | numericExpression #numericAtomExpression;

numericExpression
    : sign='-'? DOUBLE_LITERAL #doubleAtomExpression
    | sign='-'? INTEGER_LITERAL #integerAtomExpression;

DOUBLE_LITERAL: [0-9]+ DOT [0-9]+;
INTEGER_LITERAL: [0-9]+;
WORD: [a-zA-Z]+;
MULT: '*';
ADD: '+';
MINUS: '-';
DIV: '/';
DOT: '.';

WHITESPACE  : [ \t\n\r] -> skip;