//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ll.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace ll {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public partial class llLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		DOUBLE_LITERAL=1, INTEGER_LITERAL=2, RETURN=3, INT_TYPE=4, DOUBLE_TYPE=5, 
		BOOL_TYPE=6, VOID_TYPE=7, BOOL_TRUE=8, BOOL_FALSE=9, IF=10, ELSE=11, WHILE=12, 
		PRINT=13, NEW=14, DESTROY=15, NULL=16, WORD=17, MULT=18, PLUS=19, MINUS=20, 
		DIV=21, DOT=22, PAR_L=23, PAR_R=24, ASSIGN=25, CURL_L=26, CURL_R=27, BRAC_L=28, 
		BRAC_R=29, SEMCOL=30, EQUAL=31, ADD_ASSIGN=32, SUB_ASSIGN=33, MULT_ASSIGN=34, 
		DIV_ASSIGN=35, LESS=36, GREATER=37, COLON=38, COMMA=39, NOT=40, AND=41, 
		OR=42, NOT_EQUAL=43, WHITESPACE=44;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"DOUBLE_LITERAL", "INTEGER_LITERAL", "RETURN", "INT_TYPE", "DOUBLE_TYPE", 
		"BOOL_TYPE", "VOID_TYPE", "BOOL_TRUE", "BOOL_FALSE", "IF", "ELSE", "WHILE", 
		"PRINT", "NEW", "DESTROY", "NULL", "WORD", "MULT", "PLUS", "MINUS", "DIV", 
		"DOT", "PAR_L", "PAR_R", "ASSIGN", "CURL_L", "CURL_R", "BRAC_L", "BRAC_R", 
		"SEMCOL", "EQUAL", "ADD_ASSIGN", "SUB_ASSIGN", "MULT_ASSIGN", "DIV_ASSIGN", 
		"LESS", "GREATER", "COLON", "COMMA", "NOT", "AND", "OR", "NOT_EQUAL", 
		"WHITESPACE"
	};


	public llLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public llLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, "'*'", "'+'", "'-'", "'/'", "'.'", 
		"'('", "')'", "'='", "'{'", "'}'", "'['", "']'", "';'", null, null, null, 
		null, null, "'<'", "'>'", "':'", "','", "'!'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "DOUBLE_LITERAL", "INTEGER_LITERAL", "RETURN", "INT_TYPE", "DOUBLE_TYPE", 
		"BOOL_TYPE", "VOID_TYPE", "BOOL_TRUE", "BOOL_FALSE", "IF", "ELSE", "WHILE", 
		"PRINT", "NEW", "DESTROY", "NULL", "WORD", "MULT", "PLUS", "MINUS", "DIV", 
		"DOT", "PAR_L", "PAR_R", "ASSIGN", "CURL_L", "CURL_R", "BRAC_L", "BRAC_R", 
		"SEMCOL", "EQUAL", "ADD_ASSIGN", "SUB_ASSIGN", "MULT_ASSIGN", "DIV_ASSIGN", 
		"LESS", "GREATER", "COLON", "COMMA", "NOT", "AND", "OR", "NOT_EQUAL", 
		"WHITESPACE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "ll.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static llLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '.', '\x100', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', 
		'\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x4', '\x12', '\t', '\x12', '\x4', '\x13', '\t', 
		'\x13', '\x4', '\x14', '\t', '\x14', '\x4', '\x15', '\t', '\x15', '\x4', 
		'\x16', '\t', '\x16', '\x4', '\x17', '\t', '\x17', '\x4', '\x18', '\t', 
		'\x18', '\x4', '\x19', '\t', '\x19', '\x4', '\x1A', '\t', '\x1A', '\x4', 
		'\x1B', '\t', '\x1B', '\x4', '\x1C', '\t', '\x1C', '\x4', '\x1D', '\t', 
		'\x1D', '\x4', '\x1E', '\t', '\x1E', '\x4', '\x1F', '\t', '\x1F', '\x4', 
		' ', '\t', ' ', '\x4', '!', '\t', '!', '\x4', '\"', '\t', '\"', '\x4', 
		'#', '\t', '#', '\x4', '$', '\t', '$', '\x4', '%', '\t', '%', '\x4', '&', 
		'\t', '&', '\x4', '\'', '\t', '\'', '\x4', '(', '\t', '(', '\x4', ')', 
		'\t', ')', '\x4', '*', '\t', '*', '\x4', '+', '\t', '+', '\x4', ',', '\t', 
		',', '\x4', '-', '\t', '-', '\x3', '\x2', '\x6', '\x2', ']', '\n', '\x2', 
		'\r', '\x2', '\xE', '\x2', '^', '\x3', '\x2', '\x3', '\x2', '\x6', '\x2', 
		'\x63', '\n', '\x2', '\r', '\x2', '\xE', '\x2', '\x64', '\x3', '\x3', 
		'\x6', '\x3', 'h', '\n', '\x3', '\r', '\x3', '\xE', '\x3', 'i', '\x3', 
		'\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', 
		'\x4', '\x3', '\x4', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', 
		'\x5', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', 
		'\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', '\a', '\x3', '\a', 
		'\x3', '\a', '\x3', '\a', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', 
		'\b', '\x3', '\b', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', 
		'\x3', '\t', '\x3', '\n', '\x3', '\n', '\x3', '\n', '\x3', '\n', '\x3', 
		'\n', '\x3', '\n', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\f', 
		'\x3', '\f', '\x3', '\f', '\x3', '\f', '\x3', '\f', '\x3', '\r', '\x3', 
		'\r', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\xE', 
		'\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', 
		'\x3', '\xF', '\x3', '\xF', '\x3', '\xF', '\x3', '\xF', '\x3', '\x10', 
		'\x3', '\x10', '\x3', '\x10', '\x3', '\x10', '\x3', '\x10', '\x3', '\x10', 
		'\x3', '\x10', '\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x11', 
		'\x3', '\x11', '\x3', '\x11', '\x3', '\x12', '\x5', '\x12', '\xB9', '\n', 
		'\x12', '\x3', '\x12', '\a', '\x12', '\xBC', '\n', '\x12', '\f', '\x12', 
		'\xE', '\x12', '\xBF', '\v', '\x12', '\x3', '\x13', '\x3', '\x13', '\x3', 
		'\x14', '\x3', '\x14', '\x3', '\x15', '\x3', '\x15', '\x3', '\x16', '\x3', 
		'\x16', '\x3', '\x17', '\x3', '\x17', '\x3', '\x18', '\x3', '\x18', '\x3', 
		'\x19', '\x3', '\x19', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1B', '\x3', 
		'\x1B', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1D', '\x3', '\x1D', '\x3', 
		'\x1E', '\x3', '\x1E', '\x3', '\x1F', '\x3', '\x1F', '\x3', ' ', '\x3', 
		' ', '\x3', ' ', '\x3', '!', '\x3', '!', '\x3', '!', '\x3', '\"', '\x3', 
		'\"', '\x3', '\"', '\x3', '#', '\x3', '#', '\x3', '#', '\x3', '$', '\x3', 
		'$', '\x3', '$', '\x3', '%', '\x3', '%', '\x3', '&', '\x3', '&', '\x3', 
		'\'', '\x3', '\'', '\x3', '(', '\x3', '(', '\x3', ')', '\x3', ')', '\x3', 
		'*', '\x3', '*', '\x3', '*', '\x3', '+', '\x3', '+', '\x3', '+', '\x3', 
		',', '\x3', ',', '\x3', ',', '\x3', '-', '\x3', '-', '\x3', '-', '\x3', 
		'-', '\x2', '\x2', '.', '\x3', '\x3', '\x5', '\x4', '\a', '\x5', '\t', 
		'\x6', '\v', '\a', '\r', '\b', '\xF', '\t', '\x11', '\n', '\x13', '\v', 
		'\x15', '\f', '\x17', '\r', '\x19', '\xE', '\x1B', '\xF', '\x1D', '\x10', 
		'\x1F', '\x11', '!', '\x12', '#', '\x13', '%', '\x14', '\'', '\x15', ')', 
		'\x16', '+', '\x17', '-', '\x18', '/', '\x19', '\x31', '\x1A', '\x33', 
		'\x1B', '\x35', '\x1C', '\x37', '\x1D', '\x39', '\x1E', ';', '\x1F', '=', 
		' ', '?', '!', '\x41', '\"', '\x43', '#', '\x45', '$', 'G', '%', 'I', 
		'&', 'K', '\'', 'M', '(', 'O', ')', 'Q', '*', 'S', '+', 'U', ',', 'W', 
		'-', 'Y', '.', '\x3', '\x2', '\x6', '\x3', '\x2', '\x32', ';', '\x5', 
		'\x2', '\x43', '\\', '\x61', '\x61', '\x63', '|', '\x6', '\x2', '\x32', 
		';', '\x43', '\\', '\x61', '\x61', '\x63', '|', '\x5', '\x2', '\v', '\f', 
		'\xF', '\xF', '\"', '\"', '\x2', '\x103', '\x2', '\x3', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x5', '\x3', '\x2', '\x2', '\x2', '\x2', '\a', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\t', '\x3', '\x2', '\x2', '\x2', '\x2', '\v', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\r', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\xF', '\x3', '\x2', '\x2', '\x2', '\x2', '\x11', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x13', '\x3', '\x2', '\x2', '\x2', '\x2', '\x15', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x17', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x19', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1B', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x1D', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1F', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '!', '\x3', '\x2', '\x2', '\x2', '\x2', '#', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '%', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\'', '\x3', '\x2', '\x2', '\x2', '\x2', ')', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '+', '\x3', '\x2', '\x2', '\x2', '\x2', '-', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '/', '\x3', '\x2', '\x2', '\x2', '\x2', '\x31', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x33', '\x3', '\x2', '\x2', '\x2', '\x2', '\x35', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x37', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x39', '\x3', '\x2', '\x2', '\x2', '\x2', ';', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '=', '\x3', '\x2', '\x2', '\x2', '\x2', '?', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x41', '\x3', '\x2', '\x2', '\x2', '\x2', '\x43', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x45', '\x3', '\x2', '\x2', '\x2', 
		'\x2', 'G', '\x3', '\x2', '\x2', '\x2', '\x2', 'I', '\x3', '\x2', '\x2', 
		'\x2', '\x2', 'K', '\x3', '\x2', '\x2', '\x2', '\x2', 'M', '\x3', '\x2', 
		'\x2', '\x2', '\x2', 'O', '\x3', '\x2', '\x2', '\x2', '\x2', 'Q', '\x3', 
		'\x2', '\x2', '\x2', '\x2', 'S', '\x3', '\x2', '\x2', '\x2', '\x2', 'U', 
		'\x3', '\x2', '\x2', '\x2', '\x2', 'W', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'Y', '\x3', '\x2', '\x2', '\x2', '\x3', '\\', '\x3', '\x2', '\x2', '\x2', 
		'\x5', 'g', '\x3', '\x2', '\x2', '\x2', '\a', 'k', '\x3', '\x2', '\x2', 
		'\x2', '\t', 'r', '\x3', '\x2', '\x2', '\x2', '\v', 'v', '\x3', '\x2', 
		'\x2', '\x2', '\r', '}', '\x3', '\x2', '\x2', '\x2', '\xF', '\x82', '\x3', 
		'\x2', '\x2', '\x2', '\x11', '\x87', '\x3', '\x2', '\x2', '\x2', '\x13', 
		'\x8C', '\x3', '\x2', '\x2', '\x2', '\x15', '\x92', '\x3', '\x2', '\x2', 
		'\x2', '\x17', '\x95', '\x3', '\x2', '\x2', '\x2', '\x19', '\x9A', '\x3', 
		'\x2', '\x2', '\x2', '\x1B', '\xA0', '\x3', '\x2', '\x2', '\x2', '\x1D', 
		'\xA6', '\x3', '\x2', '\x2', '\x2', '\x1F', '\xAA', '\x3', '\x2', '\x2', 
		'\x2', '!', '\xB2', '\x3', '\x2', '\x2', '\x2', '#', '\xB8', '\x3', '\x2', 
		'\x2', '\x2', '%', '\xC0', '\x3', '\x2', '\x2', '\x2', '\'', '\xC2', '\x3', 
		'\x2', '\x2', '\x2', ')', '\xC4', '\x3', '\x2', '\x2', '\x2', '+', '\xC6', 
		'\x3', '\x2', '\x2', '\x2', '-', '\xC8', '\x3', '\x2', '\x2', '\x2', '/', 
		'\xCA', '\x3', '\x2', '\x2', '\x2', '\x31', '\xCC', '\x3', '\x2', '\x2', 
		'\x2', '\x33', '\xCE', '\x3', '\x2', '\x2', '\x2', '\x35', '\xD0', '\x3', 
		'\x2', '\x2', '\x2', '\x37', '\xD2', '\x3', '\x2', '\x2', '\x2', '\x39', 
		'\xD4', '\x3', '\x2', '\x2', '\x2', ';', '\xD6', '\x3', '\x2', '\x2', 
		'\x2', '=', '\xD8', '\x3', '\x2', '\x2', '\x2', '?', '\xDA', '\x3', '\x2', 
		'\x2', '\x2', '\x41', '\xDD', '\x3', '\x2', '\x2', '\x2', '\x43', '\xE0', 
		'\x3', '\x2', '\x2', '\x2', '\x45', '\xE3', '\x3', '\x2', '\x2', '\x2', 
		'G', '\xE6', '\x3', '\x2', '\x2', '\x2', 'I', '\xE9', '\x3', '\x2', '\x2', 
		'\x2', 'K', '\xEB', '\x3', '\x2', '\x2', '\x2', 'M', '\xED', '\x3', '\x2', 
		'\x2', '\x2', 'O', '\xEF', '\x3', '\x2', '\x2', '\x2', 'Q', '\xF1', '\x3', 
		'\x2', '\x2', '\x2', 'S', '\xF3', '\x3', '\x2', '\x2', '\x2', 'U', '\xF6', 
		'\x3', '\x2', '\x2', '\x2', 'W', '\xF9', '\x3', '\x2', '\x2', '\x2', 'Y', 
		'\xFC', '\x3', '\x2', '\x2', '\x2', '[', ']', '\t', '\x2', '\x2', '\x2', 
		'\\', '[', '\x3', '\x2', '\x2', '\x2', ']', '^', '\x3', '\x2', '\x2', 
		'\x2', '^', '\\', '\x3', '\x2', '\x2', '\x2', '^', '_', '\x3', '\x2', 
		'\x2', '\x2', '_', '`', '\x3', '\x2', '\x2', '\x2', '`', '\x62', '\x5', 
		'-', '\x17', '\x2', '\x61', '\x63', '\t', '\x2', '\x2', '\x2', '\x62', 
		'\x61', '\x3', '\x2', '\x2', '\x2', '\x63', '\x64', '\x3', '\x2', '\x2', 
		'\x2', '\x64', '\x62', '\x3', '\x2', '\x2', '\x2', '\x64', '\x65', '\x3', 
		'\x2', '\x2', '\x2', '\x65', '\x4', '\x3', '\x2', '\x2', '\x2', '\x66', 
		'h', '\t', '\x2', '\x2', '\x2', 'g', '\x66', '\x3', '\x2', '\x2', '\x2', 
		'h', 'i', '\x3', '\x2', '\x2', '\x2', 'i', 'g', '\x3', '\x2', '\x2', '\x2', 
		'i', 'j', '\x3', '\x2', '\x2', '\x2', 'j', '\x6', '\x3', '\x2', '\x2', 
		'\x2', 'k', 'l', '\a', 't', '\x2', '\x2', 'l', 'm', '\a', 'g', '\x2', 
		'\x2', 'm', 'n', '\a', 'v', '\x2', '\x2', 'n', 'o', '\a', 'w', '\x2', 
		'\x2', 'o', 'p', '\a', 't', '\x2', '\x2', 'p', 'q', '\a', 'p', '\x2', 
		'\x2', 'q', '\b', '\x3', '\x2', '\x2', '\x2', 'r', 's', '\a', 'k', '\x2', 
		'\x2', 's', 't', '\a', 'p', '\x2', '\x2', 't', 'u', '\a', 'v', '\x2', 
		'\x2', 'u', '\n', '\x3', '\x2', '\x2', '\x2', 'v', 'w', '\a', '\x66', 
		'\x2', '\x2', 'w', 'x', '\a', 'q', '\x2', '\x2', 'x', 'y', '\a', 'w', 
		'\x2', '\x2', 'y', 'z', '\a', '\x64', '\x2', '\x2', 'z', '{', '\a', 'n', 
		'\x2', '\x2', '{', '|', '\a', 'g', '\x2', '\x2', '|', '\f', '\x3', '\x2', 
		'\x2', '\x2', '}', '~', '\a', '\x64', '\x2', '\x2', '~', '\x7F', '\a', 
		'q', '\x2', '\x2', '\x7F', '\x80', '\a', 'q', '\x2', '\x2', '\x80', '\x81', 
		'\a', 'n', '\x2', '\x2', '\x81', '\xE', '\x3', '\x2', '\x2', '\x2', '\x82', 
		'\x83', '\a', 'x', '\x2', '\x2', '\x83', '\x84', '\a', 'q', '\x2', '\x2', 
		'\x84', '\x85', '\a', 'k', '\x2', '\x2', '\x85', '\x86', '\a', '\x66', 
		'\x2', '\x2', '\x86', '\x10', '\x3', '\x2', '\x2', '\x2', '\x87', '\x88', 
		'\a', 'v', '\x2', '\x2', '\x88', '\x89', '\a', 't', '\x2', '\x2', '\x89', 
		'\x8A', '\a', 'w', '\x2', '\x2', '\x8A', '\x8B', '\a', 'g', '\x2', '\x2', 
		'\x8B', '\x12', '\x3', '\x2', '\x2', '\x2', '\x8C', '\x8D', '\a', 'h', 
		'\x2', '\x2', '\x8D', '\x8E', '\a', '\x63', '\x2', '\x2', '\x8E', '\x8F', 
		'\a', 'n', '\x2', '\x2', '\x8F', '\x90', '\a', 'u', '\x2', '\x2', '\x90', 
		'\x91', '\a', 'g', '\x2', '\x2', '\x91', '\x14', '\x3', '\x2', '\x2', 
		'\x2', '\x92', '\x93', '\a', 'k', '\x2', '\x2', '\x93', '\x94', '\a', 
		'h', '\x2', '\x2', '\x94', '\x16', '\x3', '\x2', '\x2', '\x2', '\x95', 
		'\x96', '\a', 'g', '\x2', '\x2', '\x96', '\x97', '\a', 'n', '\x2', '\x2', 
		'\x97', '\x98', '\a', 'u', '\x2', '\x2', '\x98', '\x99', '\a', 'g', '\x2', 
		'\x2', '\x99', '\x18', '\x3', '\x2', '\x2', '\x2', '\x9A', '\x9B', '\a', 
		'y', '\x2', '\x2', '\x9B', '\x9C', '\a', 'j', '\x2', '\x2', '\x9C', '\x9D', 
		'\a', 'k', '\x2', '\x2', '\x9D', '\x9E', '\a', 'n', '\x2', '\x2', '\x9E', 
		'\x9F', '\a', 'g', '\x2', '\x2', '\x9F', '\x1A', '\x3', '\x2', '\x2', 
		'\x2', '\xA0', '\xA1', '\a', 'r', '\x2', '\x2', '\xA1', '\xA2', '\a', 
		't', '\x2', '\x2', '\xA2', '\xA3', '\a', 'k', '\x2', '\x2', '\xA3', '\xA4', 
		'\a', 'p', '\x2', '\x2', '\xA4', '\xA5', '\a', 'v', '\x2', '\x2', '\xA5', 
		'\x1C', '\x3', '\x2', '\x2', '\x2', '\xA6', '\xA7', '\a', 'p', '\x2', 
		'\x2', '\xA7', '\xA8', '\a', 'g', '\x2', '\x2', '\xA8', '\xA9', '\a', 
		'y', '\x2', '\x2', '\xA9', '\x1E', '\x3', '\x2', '\x2', '\x2', '\xAA', 
		'\xAB', '\a', '\x66', '\x2', '\x2', '\xAB', '\xAC', '\a', 'g', '\x2', 
		'\x2', '\xAC', '\xAD', '\a', 'u', '\x2', '\x2', '\xAD', '\xAE', '\a', 
		'v', '\x2', '\x2', '\xAE', '\xAF', '\a', 't', '\x2', '\x2', '\xAF', '\xB0', 
		'\a', 'q', '\x2', '\x2', '\xB0', '\xB1', '\a', '{', '\x2', '\x2', '\xB1', 
		' ', '\x3', '\x2', '\x2', '\x2', '\xB2', '\xB3', '\a', 'p', '\x2', '\x2', 
		'\xB3', '\xB4', '\a', 'w', '\x2', '\x2', '\xB4', '\xB5', '\a', 'n', '\x2', 
		'\x2', '\xB5', '\xB6', '\a', 'n', '\x2', '\x2', '\xB6', '\"', '\x3', '\x2', 
		'\x2', '\x2', '\xB7', '\xB9', '\t', '\x3', '\x2', '\x2', '\xB8', '\xB7', 
		'\x3', '\x2', '\x2', '\x2', '\xB9', '\xBD', '\x3', '\x2', '\x2', '\x2', 
		'\xBA', '\xBC', '\t', '\x4', '\x2', '\x2', '\xBB', '\xBA', '\x3', '\x2', 
		'\x2', '\x2', '\xBC', '\xBF', '\x3', '\x2', '\x2', '\x2', '\xBD', '\xBB', 
		'\x3', '\x2', '\x2', '\x2', '\xBD', '\xBE', '\x3', '\x2', '\x2', '\x2', 
		'\xBE', '$', '\x3', '\x2', '\x2', '\x2', '\xBF', '\xBD', '\x3', '\x2', 
		'\x2', '\x2', '\xC0', '\xC1', '\a', ',', '\x2', '\x2', '\xC1', '&', '\x3', 
		'\x2', '\x2', '\x2', '\xC2', '\xC3', '\a', '-', '\x2', '\x2', '\xC3', 
		'(', '\x3', '\x2', '\x2', '\x2', '\xC4', '\xC5', '\a', '/', '\x2', '\x2', 
		'\xC5', '*', '\x3', '\x2', '\x2', '\x2', '\xC6', '\xC7', '\a', '\x31', 
		'\x2', '\x2', '\xC7', ',', '\x3', '\x2', '\x2', '\x2', '\xC8', '\xC9', 
		'\a', '\x30', '\x2', '\x2', '\xC9', '.', '\x3', '\x2', '\x2', '\x2', '\xCA', 
		'\xCB', '\a', '*', '\x2', '\x2', '\xCB', '\x30', '\x3', '\x2', '\x2', 
		'\x2', '\xCC', '\xCD', '\a', '+', '\x2', '\x2', '\xCD', '\x32', '\x3', 
		'\x2', '\x2', '\x2', '\xCE', '\xCF', '\a', '?', '\x2', '\x2', '\xCF', 
		'\x34', '\x3', '\x2', '\x2', '\x2', '\xD0', '\xD1', '\a', '}', '\x2', 
		'\x2', '\xD1', '\x36', '\x3', '\x2', '\x2', '\x2', '\xD2', '\xD3', '\a', 
		'\x7F', '\x2', '\x2', '\xD3', '\x38', '\x3', '\x2', '\x2', '\x2', '\xD4', 
		'\xD5', '\a', ']', '\x2', '\x2', '\xD5', ':', '\x3', '\x2', '\x2', '\x2', 
		'\xD6', '\xD7', '\a', '_', '\x2', '\x2', '\xD7', '<', '\x3', '\x2', '\x2', 
		'\x2', '\xD8', '\xD9', '\a', '=', '\x2', '\x2', '\xD9', '>', '\x3', '\x2', 
		'\x2', '\x2', '\xDA', '\xDB', '\a', '?', '\x2', '\x2', '\xDB', '\xDC', 
		'\a', '?', '\x2', '\x2', '\xDC', '@', '\x3', '\x2', '\x2', '\x2', '\xDD', 
		'\xDE', '\a', '-', '\x2', '\x2', '\xDE', '\xDF', '\a', '?', '\x2', '\x2', 
		'\xDF', '\x42', '\x3', '\x2', '\x2', '\x2', '\xE0', '\xE1', '\a', '/', 
		'\x2', '\x2', '\xE1', '\xE2', '\a', '?', '\x2', '\x2', '\xE2', '\x44', 
		'\x3', '\x2', '\x2', '\x2', '\xE3', '\xE4', '\a', ',', '\x2', '\x2', '\xE4', 
		'\xE5', '\a', '?', '\x2', '\x2', '\xE5', '\x46', '\x3', '\x2', '\x2', 
		'\x2', '\xE6', '\xE7', '\a', '\x31', '\x2', '\x2', '\xE7', '\xE8', '\a', 
		'?', '\x2', '\x2', '\xE8', 'H', '\x3', '\x2', '\x2', '\x2', '\xE9', '\xEA', 
		'\a', '>', '\x2', '\x2', '\xEA', 'J', '\x3', '\x2', '\x2', '\x2', '\xEB', 
		'\xEC', '\a', '@', '\x2', '\x2', '\xEC', 'L', '\x3', '\x2', '\x2', '\x2', 
		'\xED', '\xEE', '\a', '<', '\x2', '\x2', '\xEE', 'N', '\x3', '\x2', '\x2', 
		'\x2', '\xEF', '\xF0', '\a', '.', '\x2', '\x2', '\xF0', 'P', '\x3', '\x2', 
		'\x2', '\x2', '\xF1', '\xF2', '\a', '#', '\x2', '\x2', '\xF2', 'R', '\x3', 
		'\x2', '\x2', '\x2', '\xF3', '\xF4', '\a', '(', '\x2', '\x2', '\xF4', 
		'\xF5', '\a', '(', '\x2', '\x2', '\xF5', 'T', '\x3', '\x2', '\x2', '\x2', 
		'\xF6', '\xF7', '\a', '~', '\x2', '\x2', '\xF7', '\xF8', '\a', '~', '\x2', 
		'\x2', '\xF8', 'V', '\x3', '\x2', '\x2', '\x2', '\xF9', '\xFA', '\a', 
		'#', '\x2', '\x2', '\xFA', '\xFB', '\a', '?', '\x2', '\x2', '\xFB', 'X', 
		'\x3', '\x2', '\x2', '\x2', '\xFC', '\xFD', '\t', '\x5', '\x2', '\x2', 
		'\xFD', '\xFE', '\x3', '\x2', '\x2', '\x2', '\xFE', '\xFF', '\b', '-', 
		'\x2', '\x2', '\xFF', 'Z', '\x3', '\x2', '\x2', '\x2', '\t', '\x2', '^', 
		'\x64', 'i', '\xB8', '\xBB', '\xBD', '\x3', '\b', '\x2', '\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace ll
