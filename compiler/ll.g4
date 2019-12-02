grammar ll;


compileUnit: program EOF;

program
    : functionDefinition+
    | compositUnit;

compositUnit
    : statement
    | expression;

expression
    : PAR_L expression PAR_R #parenthes
    | left=expression op=(MULT|DIV) right=expression #binOpMultDiv
    | left=expression op=(ADD|MINUS) right=expression #binOpAddSub
    | left=expression op=EQUAL right=expression #equalityOpertor
    | left=expression op=LESS ASSIGN? right=expression #lessOperator
    | left=expression op=GREATER ASSIGN? right=expression #greaterOperator
    | unaryExpression #unaryExpr
    | blockStatement #blockSta;

statement
    : left=WORD ASSIGN right=expression SEMCOL #assignStatement
    | left=WORD COLON type=typeDefinition SEMCOL #instantiationStatement
    | left=WORD COLON type=typeDefinition ASSIGN right=expression SEMCOL #initializationStatement
    | RETURN expression SEMCOL #returnStatement
    | functionDefinition #funcDefinitionStatement
    | IF PAR_L cond=compositUnit PAR_R blockStatement (ELSE blockStatement)? #ifStatement;

unaryExpression
    : numericExpression
    | boolExpression
    | functionCall
    | variableExpression;

functionCall
    : name=WORD PAR_L (expression (COMMA expression)*)? PAR_R;

functionDefinition
    : name=WORD PAR_L (WORD COLON typeDefinition (COMMA WORD COLON typeDefinition)*)? PAR_R COLON typeDefinition body=blockStatement;

variableExpression
    : WORD;

numericExpression
    : sign=(MINUS|ADD)? DOUBLE_LITERAL #doubleAtomExpression
    | sign=(MINUS|ADD)? INTEGER_LITERAL #integerAtomExpression;

boolExpression
    : BOOL_TRUE
    | BOOL_FALSE;

blockStatement
    : CURL_L compositUnit* CURL_R;

typeDefinition
    : INT_TYPE
    | DOUBLE_TYPE
    | BOOL_TYPE;

DOUBLE_LITERAL: [0-9]+ DOT [0-9]+;
INTEGER_LITERAL: [0-9]+;
RETURN: 'r' 'e' 't' 'u' 'r' 'n';
INT_TYPE: 'i' 'n' 't';
DOUBLE_TYPE: 'd' 'o' 'u' 'b' 'l' 'e';
BOOL_TYPE: 'b' 'o' 'o' 'l';
BOOL_TRUE: 't' 'r' 'u' 'e';
BOOL_FALSE: 'f' 'a' 'l' 's' 'e';
IF: 'i' 'f';
ELSE: 'e' 'l' 's' 'e';
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
COMMA: ',';

WHITESPACE  : [ \t\n\r] -> skip;