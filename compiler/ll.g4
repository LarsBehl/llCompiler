grammar ll;


compileUnit: program EOF;

program
    : functionDefinition+
    | compositUnit;

compositUnit
    : statement
    | expression;

line
    : statement
    | expression SEMCOL;

expression
    : PAR_L expression PAR_R #parenthes
    | left=expression op=(MULT|DIV) right=expression #binOpMultDiv
    | left=expression op=(PLUS|MINUS) right=expression #binOpAddSub
    | left=expression op=EQUAL right=expression #equalityOpertor
    | left=expression op=LESS ASSIGN? right=expression #lessOperator
    | left=expression op=GREATER ASSIGN? right=expression #greaterOperator
    | left=expression op=AND right=expression #andOperator
    | left=expression op=OR right=expression #orOperator
    | left=expression op=NOT_EQUAL right=expression #notEqualOperator
    | unaryExpression #unaryExpr;

statement
    : left=WORD ASSIGN (expression|refTypeCreation) SEMCOL #assignStatement
    | left=arrayIndexing ASSIGN (expression) SEMCOL #assignArrayField
    | left=WORD ADD_ASSIGN right=expression SEMCOL #addAssignStatement
    | left=WORD SUB_ASSIGN right=expression SEMCOL #subAssignStatement
    | left=WORD MULT_ASSIGN right=expression SEMCOL #multAssignStatement
    | left=WORD DIV_ASSIGN right=expression SEMCOL #divAssignStatement
    | left=WORD COLON type=typeDefinition SEMCOL #instantiationStatement
    | left=WORD COLON type=typeDefinition ASSIGN (expression|refTypeCreation) SEMCOL #initializationStatement
    | refTypeDestruction #destructionStatement
    | RETURN (expression|refTypeCreation)? SEMCOL #returnStatement
    | IF PAR_L cond=compositUnit PAR_R blockStatement (ELSE blockStatement)? #ifStatement
    | WHILE PAR_L cond=compositUnit PAR_R blockStatement #whileStatement
    | PRINT PAR_L expression PAR_R SEMCOL #printStatement;

unaryExpression
    : numericExpression
    | boolExpression
    | functionCall
    | variableExpression
    | incrementPostExpression
    | decrementPostExpression
    | decrementPreExpression
    | incrementPreExpression
    | notExpression
    | arrayIndexing;

functionCall
    : name=WORD PAR_L (expression (COMMA expression)*)? PAR_R;

functionDefinition
    : name=WORD PAR_L (WORD COLON typeDefinition (COMMA WORD COLON typeDefinition)*)? PAR_R COLON typeDefinition body=blockStatement;

variableExpression
    : WORD;

numericExpression
    : sign=(MINUS|PLUS)? DOUBLE_LITERAL #doubleAtomExpression
    | sign=(MINUS|PLUS)? INTEGER_LITERAL #integerAtomExpression;

boolExpression
    : BOOL_TRUE
    | BOOL_FALSE;

blockStatement
    : CURL_L line* CURL_R;

typeDefinition
    : INT_TYPE
    | DOUBLE_TYPE
    | BOOL_TYPE
    | VOID_TYPE
    | arrayTypes;

incrementPostExpression
    : variableExpression PLUS PLUS;

decrementPostExpression
    : variableExpression MINUS MINUS;

incrementPreExpression
    : PLUS PLUS variableExpression;

decrementPreExpression
    : MINUS MINUS variableExpression;

notExpression
    : NOT expression;

arrayTypes
    : INT_TYPE BRAC_L BRAC_R #intArrayType
    | DOUBLE_TYPE BRAC_L BRAC_R #doubleArrayType
    | BOOL_TYPE BRAC_L BRAC_R #boolArrayType;

arrayCreation
    : INT_TYPE BRAC_L expression BRAC_R #intArrayCreation
    | DOUBLE_TYPE BRAC_L expression BRAC_R #doubleArrayCreation
    | BOOL_TYPE BRAC_L expression BRAC_R #boolArrayCreation;

refTypeCreation
    : NEW arrayCreation;

arrayIndexing
    : variableExpression BRAC_L expression BRAC_R;

refTypeDestruction
    : DESTROY variableExpression;

DOUBLE_LITERAL: [0-9]+ DOT [0-9]+;
INTEGER_LITERAL: [0-9]+;
RETURN: 'r' 'e' 't' 'u' 'r' 'n';
INT_TYPE: 'i' 'n' 't';
DOUBLE_TYPE: 'd' 'o' 'u' 'b' 'l' 'e';
BOOL_TYPE: 'b' 'o' 'o' 'l';
VOID_TYPE: 'v' 'o' 'i' 'd';
BOOL_TRUE: 't' 'r' 'u' 'e';
BOOL_FALSE: 'f' 'a' 'l' 's' 'e';
IF: 'i' 'f';
ELSE: 'e' 'l' 's' 'e';
WHILE: 'w' 'h' 'i' 'l' 'e';
PRINT: 'p' 'r' 'i' 'n' 't';
NEW: 'n' 'e' 'w';
DESTROY: 'd' 'e' 's' 't' 'r' 'o' 'y';
WORD: ([a-zA-Z] | '_') ([a-zA-Z0-9] | '_')*;
MULT: '*';
PLUS: '+';
MINUS: '-';
DIV: '/';
DOT: '.';
PAR_L: '(';
PAR_R: ')';
ASSIGN: '=';
CURL_L: '{';
CURL_R: '}';
BRAC_L: '[';
BRAC_R: ']';
SEMCOL: ';';
EQUAL: '=' '=';
ADD_ASSIGN: '+' '=';
SUB_ASSIGN: '-' '=';
MULT_ASSIGN: '*' '=';
DIV_ASSIGN: '/' '=';
LESS: '<';
GREATER: '>';
COLON: ':';
COMMA: ',';
NOT: '!';
AND: '&' '&';
OR: '|' '|';
NOT_EQUAL: '!' '=';

WHITESPACE  : [ \t\n\r] -> skip;