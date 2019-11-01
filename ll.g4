grammar ll;


compileUnit: expression EOF;
expression
    : '(' expression ')' #parenthes
    | funcName=WORD '(' (WORD ',')* ')' body=expressionSequenz #functionDefinition
    | left=WORD '=' right=expression ';' #assignExpression
    | left=expression op=('*'|'/') right=expression #infixExpression
    | left=expression op=('+'|'-') right=expression #infixExpression
    | numericExpression #numericAtomExpression
    | variableExpression #varExpr
    | expressionSequenz #exprSequ;

expressionSequenz
    : '{' expression* '}';

variableExpression
    : WORD ';';

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
BRAC_L: '(';
BRAC_R: ')';
CURL_L: '{';
CURL_R: '}';
SECOL: ';';

WHITESPACE  : [ \t\n\r] -> skip;