grammar ll;


compileUnit: expression EOF;
expression
    : '(' expression ')' #parenthes
    | left=expression op=('*'|'/') right=expression #binOpMultDiv
    | left=expression op=('+'|'-') right=expression #binOpAddSub
    | left=WORD '=' right=expression #assignExpression
    | numericExpression #numericAtomExpression
    | WORD #variableExpression
    | expressionSequenz #exprSequ;

numericExpression
    : sign='-'? DOUBLE_LITERAL #doubleAtomExpression
    | sign='-'? INTEGER_LITERAL #integerAtomExpression;

expressionSequenz
    : '{' expression* '}';

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
ASSIGN: '=';
CURL_L: '{';
CURL_R: '}';

WHITESPACE  : [ \t\n\r] -> skip;