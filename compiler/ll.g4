grammar ll;


compileUnit: compositUnit EOF;

compositUnit
    : statement
    | expression;

expression
    : PAR_L expression PAR_R #parenthes
    | left=expression op=(MULT|DIV) right=expression #binOpMultDiv
    | left=expression op=(ADD|MINUS) right=expression #binOpAddSub
    | left=expression op=EQUAL right=expression #equalityOpertor
    | left=expression op=LESS right=expression #lessOperator
    | left=expression op=GREATER right=expression #greaterOperator
    | unaryExpression #unaryExpr
    | expressionSequenz #exprSequ;

statement
    : left=WORD ASSIGN right=expression SEMCOL #assignStatement
    | left=WORD COLON type=typeDefinition ASSIGN right=expression SEMCOL #initializationStatement;

unaryExpression
    : numericExpression
    | boolExpression
    | variableExpression;

variableExpression
    : WORD;

numericExpression
    : sign=(MINUS|ADD)? DOUBLE_LITERAL #doubleAtomExpression
    | sign=(MINUS|ADD)? INTEGER_LITERAL #integerAtomExpression;

boolExpression
    : BOOL_TRUE
    | BOOL_FALSE;

expressionSequenz
    : CURL_L compositUnit* returnStatement CURL_R;

typeDefinition
    : INT_TYPE
    | DOUBLE_TYPE
    | BOOL_TYPE;

returnStatement
    : RETURN expression SEMCOL;

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
PAR_L: '(';
PAR_R: ')';
ASSIGN: '=';
CURL_L: '{';
CURL_R: '}';
SEMCOL: ';';
EQUAL: '=' '=';
LESS: '<';
GREATER: '>';
COLON: ':';

WHITESPACE  : [ \t\n\r] -> skip;