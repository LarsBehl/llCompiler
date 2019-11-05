grammar ll;


compileUnit: compositUnit EOF;

compositUnit
    : statement
    | expression;

expression
    : '(' expression ')' #parenthes
    | left=expression op=('*'|'/') right=expression #binOpMultDiv
    | left=expression op=('+'|'-') right=expression #binOpAddSub
    | left=expression op=EQUAL right=expression #equalityOpertor
    | left=expression op=LESS right=expression #lessOperator
    | left=expression op=GREATER right=expression #greaterOperator
    | numericExpression #numericAtomExpression
    | WORD #variableExpression
    | expressionSequenz #exprSequ;

statement
    : left=WORD ASSIGN right=expression ';' #assignStatement;

numericExpression
    : sign=('-'|'+')? DOUBLE_LITERAL #doubleAtomExpression
    | sign=('-'|'+')? INTEGER_LITERAL #integerAtomExpression;

expressionSequenz
    : '{' compositUnit* returnExpression '}';

returnExpression
    : RETURN expression ';';

DOUBLE_LITERAL: [0-9]+ DOT [0-9]+;
INTEGER_LITERAL: [0-9]+;
RETURN: 'r' 'e' 't' 'u' 'r' 'n';
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
SEMCOL: ';';
EQUAL: '=' '=';
LESS: '<';
GREATER: '>';

WHITESPACE  : [ \t\n\r] -> skip;