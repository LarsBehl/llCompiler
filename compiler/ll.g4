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
    | boolExpression #boolAtomExpression
    | WORD #variableExpression
    | expressionSequenz #exprSequ;

statement
    : left=WORD ASSIGN right=expression ';' #assignStatement
    | left=WORD COLON type=typeDefinition ASSIGN right=expression ';' #initializationStatement;

numericExpression
    : sign=('-'|'+')? DOUBLE_LITERAL #doubleAtomExpression
    | sign=('-'|'+')? INTEGER_LITERAL #integerAtomExpression;

boolExpression
    : BOOL_TRUE
    | BOOL_FALSE;

expressionSequenz
    : '{' compositUnit* returnExpression '}';

typeDefinition
    : INT_TYPE
    | DOUBLE_TYPE
    | BOOL_TYPE;

returnExpression
    : RETURN expression ';';

DOUBLE_LITERAL: [0-9]+ DOT [0-9]+;
INTEGER_LITERAL: [0-9]+;
RETURN: 'r' 'e' 't' 'u' 'r' 'n';
INT_TYPE: 'i' 'n' 't';
DOUBLE_TYPE: 'd' 'o' 'u' 'b' 'l' 'e';
BOOL_TYPE: 'b' 'o' 'o' 'l';
BOOL_TRUE: 't' 'r' 'u' 'e';
BOOL_FALSE: 'f' 'a' 'l' 's' 'e';
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
COLON: ':';

WHITESPACE  : [ \t\n\r] -> skip;