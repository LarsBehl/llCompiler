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
		PRINT=13, NEW=14, DESTROY=15, WORD=16, MULT=17, PLUS=18, MINUS=19, DIV=20, 
		DOT=21, PAR_L=22, PAR_R=23, ASSIGN=24, CURL_L=25, CURL_R=26, BRAC_L=27, 
		BRAC_R=28, SEMCOL=29, EQUAL=30, ADD_ASSIGN=31, SUB_ASSIGN=32, MULT_ASSIGN=33, 
		DIV_ASSIGN=34, LESS=35, GREATER=36, COLON=37, COMMA=38, NOT=39, AND=40, 
		OR=41, NOT_EQUAL=42, WHITESPACE=43;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"DOUBLE_LITERAL", "INTEGER_LITERAL", "RETURN", "INT_TYPE", "DOUBLE_TYPE", 
		"BOOL_TYPE", "VOID_TYPE", "BOOL_TRUE", "BOOL_FALSE", "IF", "ELSE", "WHILE", 
		"PRINT", "NEW", "DESTROY", "WORD", "MULT", "PLUS", "MINUS", "DIV", "DOT", 
		"PAR_L", "PAR_R", "ASSIGN", "CURL_L", "CURL_R", "BRAC_L", "BRAC_R", "SEMCOL", 
		"EQUAL", "ADD_ASSIGN", "SUB_ASSIGN", "MULT_ASSIGN", "DIV_ASSIGN", "LESS", 
		"GREATER", "COLON", "COMMA", "NOT", "AND", "OR", "NOT_EQUAL", "WHITESPACE"
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
		null, null, null, null, null, "'*'", "'+'", "'-'", "'/'", "'.'", "'('", 
		"')'", "'='", "'{'", "'}'", "'['", "']'", "';'", null, null, null, null, 
		null, "'<'", "'>'", "':'", "','", "'!'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "DOUBLE_LITERAL", "INTEGER_LITERAL", "RETURN", "INT_TYPE", "DOUBLE_TYPE", 
		"BOOL_TYPE", "VOID_TYPE", "BOOL_TRUE", "BOOL_FALSE", "IF", "ELSE", "WHILE", 
		"PRINT", "NEW", "DESTROY", "WORD", "MULT", "PLUS", "MINUS", "DIV", "DOT", 
		"PAR_L", "PAR_R", "ASSIGN", "CURL_L", "CURL_R", "BRAC_L", "BRAC_R", "SEMCOL", 
		"EQUAL", "ADD_ASSIGN", "SUB_ASSIGN", "MULT_ASSIGN", "DIV_ASSIGN", "LESS", 
		"GREATER", "COLON", "COMMA", "NOT", "AND", "OR", "NOT_EQUAL", "WHITESPACE"
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
		'\x5964', '\x2', '-', '\xF9', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
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
		',', '\x3', '\x2', '\x6', '\x2', '[', '\n', '\x2', '\r', '\x2', '\xE', 
		'\x2', '\\', '\x3', '\x2', '\x3', '\x2', '\x6', '\x2', '\x61', '\n', '\x2', 
		'\r', '\x2', '\xE', '\x2', '\x62', '\x3', '\x3', '\x6', '\x3', '\x66', 
		'\n', '\x3', '\r', '\x3', '\xE', '\x3', 'g', '\x3', '\x4', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', 
		'\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', 
		'\a', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', 
		'\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', 
		'\n', '\x3', '\n', '\x3', '\n', '\x3', '\n', '\x3', '\n', '\x3', '\n', 
		'\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\f', '\x3', '\f', '\x3', 
		'\f', '\x3', '\f', '\x3', '\f', '\x3', '\r', '\x3', '\r', '\x3', '\r', 
		'\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\xE', '\x3', '\xE', '\x3', 
		'\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x3', 
		'\xF', '\x3', '\xF', '\x3', '\xF', '\x3', '\x10', '\x3', '\x10', '\x3', 
		'\x10', '\x3', '\x10', '\x3', '\x10', '\x3', '\x10', '\x3', '\x10', '\x3', 
		'\x10', '\x3', '\x11', '\x5', '\x11', '\xB2', '\n', '\x11', '\x3', '\x11', 
		'\a', '\x11', '\xB5', '\n', '\x11', '\f', '\x11', '\xE', '\x11', '\xB8', 
		'\v', '\x11', '\x3', '\x12', '\x3', '\x12', '\x3', '\x13', '\x3', '\x13', 
		'\x3', '\x14', '\x3', '\x14', '\x3', '\x15', '\x3', '\x15', '\x3', '\x16', 
		'\x3', '\x16', '\x3', '\x17', '\x3', '\x17', '\x3', '\x18', '\x3', '\x18', 
		'\x3', '\x19', '\x3', '\x19', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1B', 
		'\x3', '\x1B', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1D', '\x3', '\x1D', 
		'\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', 
		'\x3', ' ', '\x3', ' ', '\x3', ' ', '\x3', '!', '\x3', '!', '\x3', '!', 
		'\x3', '\"', '\x3', '\"', '\x3', '\"', '\x3', '#', '\x3', '#', '\x3', 
		'#', '\x3', '$', '\x3', '$', '\x3', '%', '\x3', '%', '\x3', '&', '\x3', 
		'&', '\x3', '\'', '\x3', '\'', '\x3', '(', '\x3', '(', '\x3', ')', '\x3', 
		')', '\x3', ')', '\x3', '*', '\x3', '*', '\x3', '*', '\x3', '+', '\x3', 
		'+', '\x3', '+', '\x3', ',', '\x3', ',', '\x3', ',', '\x3', ',', '\x2', 
		'\x2', '-', '\x3', '\x3', '\x5', '\x4', '\a', '\x5', '\t', '\x6', '\v', 
		'\a', '\r', '\b', '\xF', '\t', '\x11', '\n', '\x13', '\v', '\x15', '\f', 
		'\x17', '\r', '\x19', '\xE', '\x1B', '\xF', '\x1D', '\x10', '\x1F', '\x11', 
		'!', '\x12', '#', '\x13', '%', '\x14', '\'', '\x15', ')', '\x16', '+', 
		'\x17', '-', '\x18', '/', '\x19', '\x31', '\x1A', '\x33', '\x1B', '\x35', 
		'\x1C', '\x37', '\x1D', '\x39', '\x1E', ';', '\x1F', '=', ' ', '?', '!', 
		'\x41', '\"', '\x43', '#', '\x45', '$', 'G', '%', 'I', '&', 'K', '\'', 
		'M', '(', 'O', ')', 'Q', '*', 'S', '+', 'U', ',', 'W', '-', '\x3', '\x2', 
		'\x6', '\x3', '\x2', '\x32', ';', '\x5', '\x2', '\x43', '\\', '\x61', 
		'\x61', '\x63', '|', '\x6', '\x2', '\x32', ';', '\x43', '\\', '\x61', 
		'\x61', '\x63', '|', '\x5', '\x2', '\v', '\f', '\xF', '\xF', '\"', '\"', 
		'\x2', '\xFC', '\x2', '\x3', '\x3', '\x2', '\x2', '\x2', '\x2', '\x5', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\a', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\t', '\x3', '\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\r', '\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x2', '\x13', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x15', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x17', '\x3', '\x2', '\x2', '\x2', '\x2', '\x19', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1D', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'!', '\x3', '\x2', '\x2', '\x2', '\x2', '#', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '%', '\x3', '\x2', '\x2', '\x2', '\x2', '\'', '\x3', '\x2', '\x2', 
		'\x2', '\x2', ')', '\x3', '\x2', '\x2', '\x2', '\x2', '+', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '-', '\x3', '\x2', '\x2', '\x2', '\x2', '/', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x31', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x33', '\x3', '\x2', '\x2', '\x2', '\x2', '\x35', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x37', '\x3', '\x2', '\x2', '\x2', '\x2', '\x39', '\x3', 
		'\x2', '\x2', '\x2', '\x2', ';', '\x3', '\x2', '\x2', '\x2', '\x2', '=', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '?', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x41', '\x3', '\x2', '\x2', '\x2', '\x2', '\x43', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x45', '\x3', '\x2', '\x2', '\x2', '\x2', 'G', '\x3', '\x2', 
		'\x2', '\x2', '\x2', 'I', '\x3', '\x2', '\x2', '\x2', '\x2', 'K', '\x3', 
		'\x2', '\x2', '\x2', '\x2', 'M', '\x3', '\x2', '\x2', '\x2', '\x2', 'O', 
		'\x3', '\x2', '\x2', '\x2', '\x2', 'Q', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'S', '\x3', '\x2', '\x2', '\x2', '\x2', 'U', '\x3', '\x2', '\x2', '\x2', 
		'\x2', 'W', '\x3', '\x2', '\x2', '\x2', '\x3', 'Z', '\x3', '\x2', '\x2', 
		'\x2', '\x5', '\x65', '\x3', '\x2', '\x2', '\x2', '\a', 'i', '\x3', '\x2', 
		'\x2', '\x2', '\t', 'p', '\x3', '\x2', '\x2', '\x2', '\v', 't', '\x3', 
		'\x2', '\x2', '\x2', '\r', '{', '\x3', '\x2', '\x2', '\x2', '\xF', '\x80', 
		'\x3', '\x2', '\x2', '\x2', '\x11', '\x85', '\x3', '\x2', '\x2', '\x2', 
		'\x13', '\x8A', '\x3', '\x2', '\x2', '\x2', '\x15', '\x90', '\x3', '\x2', 
		'\x2', '\x2', '\x17', '\x93', '\x3', '\x2', '\x2', '\x2', '\x19', '\x98', 
		'\x3', '\x2', '\x2', '\x2', '\x1B', '\x9E', '\x3', '\x2', '\x2', '\x2', 
		'\x1D', '\xA4', '\x3', '\x2', '\x2', '\x2', '\x1F', '\xA8', '\x3', '\x2', 
		'\x2', '\x2', '!', '\xB1', '\x3', '\x2', '\x2', '\x2', '#', '\xB9', '\x3', 
		'\x2', '\x2', '\x2', '%', '\xBB', '\x3', '\x2', '\x2', '\x2', '\'', '\xBD', 
		'\x3', '\x2', '\x2', '\x2', ')', '\xBF', '\x3', '\x2', '\x2', '\x2', '+', 
		'\xC1', '\x3', '\x2', '\x2', '\x2', '-', '\xC3', '\x3', '\x2', '\x2', 
		'\x2', '/', '\xC5', '\x3', '\x2', '\x2', '\x2', '\x31', '\xC7', '\x3', 
		'\x2', '\x2', '\x2', '\x33', '\xC9', '\x3', '\x2', '\x2', '\x2', '\x35', 
		'\xCB', '\x3', '\x2', '\x2', '\x2', '\x37', '\xCD', '\x3', '\x2', '\x2', 
		'\x2', '\x39', '\xCF', '\x3', '\x2', '\x2', '\x2', ';', '\xD1', '\x3', 
		'\x2', '\x2', '\x2', '=', '\xD3', '\x3', '\x2', '\x2', '\x2', '?', '\xD6', 
		'\x3', '\x2', '\x2', '\x2', '\x41', '\xD9', '\x3', '\x2', '\x2', '\x2', 
		'\x43', '\xDC', '\x3', '\x2', '\x2', '\x2', '\x45', '\xDF', '\x3', '\x2', 
		'\x2', '\x2', 'G', '\xE2', '\x3', '\x2', '\x2', '\x2', 'I', '\xE4', '\x3', 
		'\x2', '\x2', '\x2', 'K', '\xE6', '\x3', '\x2', '\x2', '\x2', 'M', '\xE8', 
		'\x3', '\x2', '\x2', '\x2', 'O', '\xEA', '\x3', '\x2', '\x2', '\x2', 'Q', 
		'\xEC', '\x3', '\x2', '\x2', '\x2', 'S', '\xEF', '\x3', '\x2', '\x2', 
		'\x2', 'U', '\xF2', '\x3', '\x2', '\x2', '\x2', 'W', '\xF5', '\x3', '\x2', 
		'\x2', '\x2', 'Y', '[', '\t', '\x2', '\x2', '\x2', 'Z', 'Y', '\x3', '\x2', 
		'\x2', '\x2', '[', '\\', '\x3', '\x2', '\x2', '\x2', '\\', 'Z', '\x3', 
		'\x2', '\x2', '\x2', '\\', ']', '\x3', '\x2', '\x2', '\x2', ']', '^', 
		'\x3', '\x2', '\x2', '\x2', '^', '`', '\x5', '+', '\x16', '\x2', '_', 
		'\x61', '\t', '\x2', '\x2', '\x2', '`', '_', '\x3', '\x2', '\x2', '\x2', 
		'\x61', '\x62', '\x3', '\x2', '\x2', '\x2', '\x62', '`', '\x3', '\x2', 
		'\x2', '\x2', '\x62', '\x63', '\x3', '\x2', '\x2', '\x2', '\x63', '\x4', 
		'\x3', '\x2', '\x2', '\x2', '\x64', '\x66', '\t', '\x2', '\x2', '\x2', 
		'\x65', '\x64', '\x3', '\x2', '\x2', '\x2', '\x66', 'g', '\x3', '\x2', 
		'\x2', '\x2', 'g', '\x65', '\x3', '\x2', '\x2', '\x2', 'g', 'h', '\x3', 
		'\x2', '\x2', '\x2', 'h', '\x6', '\x3', '\x2', '\x2', '\x2', 'i', 'j', 
		'\a', 't', '\x2', '\x2', 'j', 'k', '\a', 'g', '\x2', '\x2', 'k', 'l', 
		'\a', 'v', '\x2', '\x2', 'l', 'm', '\a', 'w', '\x2', '\x2', 'm', 'n', 
		'\a', 't', '\x2', '\x2', 'n', 'o', '\a', 'p', '\x2', '\x2', 'o', '\b', 
		'\x3', '\x2', '\x2', '\x2', 'p', 'q', '\a', 'k', '\x2', '\x2', 'q', 'r', 
		'\a', 'p', '\x2', '\x2', 'r', 's', '\a', 'v', '\x2', '\x2', 's', '\n', 
		'\x3', '\x2', '\x2', '\x2', 't', 'u', '\a', '\x66', '\x2', '\x2', 'u', 
		'v', '\a', 'q', '\x2', '\x2', 'v', 'w', '\a', 'w', '\x2', '\x2', 'w', 
		'x', '\a', '\x64', '\x2', '\x2', 'x', 'y', '\a', 'n', '\x2', '\x2', 'y', 
		'z', '\a', 'g', '\x2', '\x2', 'z', '\f', '\x3', '\x2', '\x2', '\x2', '{', 
		'|', '\a', '\x64', '\x2', '\x2', '|', '}', '\a', 'q', '\x2', '\x2', '}', 
		'~', '\a', 'q', '\x2', '\x2', '~', '\x7F', '\a', 'n', '\x2', '\x2', '\x7F', 
		'\xE', '\x3', '\x2', '\x2', '\x2', '\x80', '\x81', '\a', 'x', '\x2', '\x2', 
		'\x81', '\x82', '\a', 'q', '\x2', '\x2', '\x82', '\x83', '\a', 'k', '\x2', 
		'\x2', '\x83', '\x84', '\a', '\x66', '\x2', '\x2', '\x84', '\x10', '\x3', 
		'\x2', '\x2', '\x2', '\x85', '\x86', '\a', 'v', '\x2', '\x2', '\x86', 
		'\x87', '\a', 't', '\x2', '\x2', '\x87', '\x88', '\a', 'w', '\x2', '\x2', 
		'\x88', '\x89', '\a', 'g', '\x2', '\x2', '\x89', '\x12', '\x3', '\x2', 
		'\x2', '\x2', '\x8A', '\x8B', '\a', 'h', '\x2', '\x2', '\x8B', '\x8C', 
		'\a', '\x63', '\x2', '\x2', '\x8C', '\x8D', '\a', 'n', '\x2', '\x2', '\x8D', 
		'\x8E', '\a', 'u', '\x2', '\x2', '\x8E', '\x8F', '\a', 'g', '\x2', '\x2', 
		'\x8F', '\x14', '\x3', '\x2', '\x2', '\x2', '\x90', '\x91', '\a', 'k', 
		'\x2', '\x2', '\x91', '\x92', '\a', 'h', '\x2', '\x2', '\x92', '\x16', 
		'\x3', '\x2', '\x2', '\x2', '\x93', '\x94', '\a', 'g', '\x2', '\x2', '\x94', 
		'\x95', '\a', 'n', '\x2', '\x2', '\x95', '\x96', '\a', 'u', '\x2', '\x2', 
		'\x96', '\x97', '\a', 'g', '\x2', '\x2', '\x97', '\x18', '\x3', '\x2', 
		'\x2', '\x2', '\x98', '\x99', '\a', 'y', '\x2', '\x2', '\x99', '\x9A', 
		'\a', 'j', '\x2', '\x2', '\x9A', '\x9B', '\a', 'k', '\x2', '\x2', '\x9B', 
		'\x9C', '\a', 'n', '\x2', '\x2', '\x9C', '\x9D', '\a', 'g', '\x2', '\x2', 
		'\x9D', '\x1A', '\x3', '\x2', '\x2', '\x2', '\x9E', '\x9F', '\a', 'r', 
		'\x2', '\x2', '\x9F', '\xA0', '\a', 't', '\x2', '\x2', '\xA0', '\xA1', 
		'\a', 'k', '\x2', '\x2', '\xA1', '\xA2', '\a', 'p', '\x2', '\x2', '\xA2', 
		'\xA3', '\a', 'v', '\x2', '\x2', '\xA3', '\x1C', '\x3', '\x2', '\x2', 
		'\x2', '\xA4', '\xA5', '\a', 'p', '\x2', '\x2', '\xA5', '\xA6', '\a', 
		'g', '\x2', '\x2', '\xA6', '\xA7', '\a', 'y', '\x2', '\x2', '\xA7', '\x1E', 
		'\x3', '\x2', '\x2', '\x2', '\xA8', '\xA9', '\a', '\x66', '\x2', '\x2', 
		'\xA9', '\xAA', '\a', 'g', '\x2', '\x2', '\xAA', '\xAB', '\a', 'u', '\x2', 
		'\x2', '\xAB', '\xAC', '\a', 'v', '\x2', '\x2', '\xAC', '\xAD', '\a', 
		't', '\x2', '\x2', '\xAD', '\xAE', '\a', 'q', '\x2', '\x2', '\xAE', '\xAF', 
		'\a', '{', '\x2', '\x2', '\xAF', ' ', '\x3', '\x2', '\x2', '\x2', '\xB0', 
		'\xB2', '\t', '\x3', '\x2', '\x2', '\xB1', '\xB0', '\x3', '\x2', '\x2', 
		'\x2', '\xB2', '\xB6', '\x3', '\x2', '\x2', '\x2', '\xB3', '\xB5', '\t', 
		'\x4', '\x2', '\x2', '\xB4', '\xB3', '\x3', '\x2', '\x2', '\x2', '\xB5', 
		'\xB8', '\x3', '\x2', '\x2', '\x2', '\xB6', '\xB4', '\x3', '\x2', '\x2', 
		'\x2', '\xB6', '\xB7', '\x3', '\x2', '\x2', '\x2', '\xB7', '\"', '\x3', 
		'\x2', '\x2', '\x2', '\xB8', '\xB6', '\x3', '\x2', '\x2', '\x2', '\xB9', 
		'\xBA', '\a', ',', '\x2', '\x2', '\xBA', '$', '\x3', '\x2', '\x2', '\x2', 
		'\xBB', '\xBC', '\a', '-', '\x2', '\x2', '\xBC', '&', '\x3', '\x2', '\x2', 
		'\x2', '\xBD', '\xBE', '\a', '/', '\x2', '\x2', '\xBE', '(', '\x3', '\x2', 
		'\x2', '\x2', '\xBF', '\xC0', '\a', '\x31', '\x2', '\x2', '\xC0', '*', 
		'\x3', '\x2', '\x2', '\x2', '\xC1', '\xC2', '\a', '\x30', '\x2', '\x2', 
		'\xC2', ',', '\x3', '\x2', '\x2', '\x2', '\xC3', '\xC4', '\a', '*', '\x2', 
		'\x2', '\xC4', '.', '\x3', '\x2', '\x2', '\x2', '\xC5', '\xC6', '\a', 
		'+', '\x2', '\x2', '\xC6', '\x30', '\x3', '\x2', '\x2', '\x2', '\xC7', 
		'\xC8', '\a', '?', '\x2', '\x2', '\xC8', '\x32', '\x3', '\x2', '\x2', 
		'\x2', '\xC9', '\xCA', '\a', '}', '\x2', '\x2', '\xCA', '\x34', '\x3', 
		'\x2', '\x2', '\x2', '\xCB', '\xCC', '\a', '\x7F', '\x2', '\x2', '\xCC', 
		'\x36', '\x3', '\x2', '\x2', '\x2', '\xCD', '\xCE', '\a', ']', '\x2', 
		'\x2', '\xCE', '\x38', '\x3', '\x2', '\x2', '\x2', '\xCF', '\xD0', '\a', 
		'_', '\x2', '\x2', '\xD0', ':', '\x3', '\x2', '\x2', '\x2', '\xD1', '\xD2', 
		'\a', '=', '\x2', '\x2', '\xD2', '<', '\x3', '\x2', '\x2', '\x2', '\xD3', 
		'\xD4', '\a', '?', '\x2', '\x2', '\xD4', '\xD5', '\a', '?', '\x2', '\x2', 
		'\xD5', '>', '\x3', '\x2', '\x2', '\x2', '\xD6', '\xD7', '\a', '-', '\x2', 
		'\x2', '\xD7', '\xD8', '\a', '?', '\x2', '\x2', '\xD8', '@', '\x3', '\x2', 
		'\x2', '\x2', '\xD9', '\xDA', '\a', '/', '\x2', '\x2', '\xDA', '\xDB', 
		'\a', '?', '\x2', '\x2', '\xDB', '\x42', '\x3', '\x2', '\x2', '\x2', '\xDC', 
		'\xDD', '\a', ',', '\x2', '\x2', '\xDD', '\xDE', '\a', '?', '\x2', '\x2', 
		'\xDE', '\x44', '\x3', '\x2', '\x2', '\x2', '\xDF', '\xE0', '\a', '\x31', 
		'\x2', '\x2', '\xE0', '\xE1', '\a', '?', '\x2', '\x2', '\xE1', '\x46', 
		'\x3', '\x2', '\x2', '\x2', '\xE2', '\xE3', '\a', '>', '\x2', '\x2', '\xE3', 
		'H', '\x3', '\x2', '\x2', '\x2', '\xE4', '\xE5', '\a', '@', '\x2', '\x2', 
		'\xE5', 'J', '\x3', '\x2', '\x2', '\x2', '\xE6', '\xE7', '\a', '<', '\x2', 
		'\x2', '\xE7', 'L', '\x3', '\x2', '\x2', '\x2', '\xE8', '\xE9', '\a', 
		'.', '\x2', '\x2', '\xE9', 'N', '\x3', '\x2', '\x2', '\x2', '\xEA', '\xEB', 
		'\a', '#', '\x2', '\x2', '\xEB', 'P', '\x3', '\x2', '\x2', '\x2', '\xEC', 
		'\xED', '\a', '(', '\x2', '\x2', '\xED', '\xEE', '\a', '(', '\x2', '\x2', 
		'\xEE', 'R', '\x3', '\x2', '\x2', '\x2', '\xEF', '\xF0', '\a', '~', '\x2', 
		'\x2', '\xF0', '\xF1', '\a', '~', '\x2', '\x2', '\xF1', 'T', '\x3', '\x2', 
		'\x2', '\x2', '\xF2', '\xF3', '\a', '#', '\x2', '\x2', '\xF3', '\xF4', 
		'\a', '?', '\x2', '\x2', '\xF4', 'V', '\x3', '\x2', '\x2', '\x2', '\xF5', 
		'\xF6', '\t', '\x5', '\x2', '\x2', '\xF6', '\xF7', '\x3', '\x2', '\x2', 
		'\x2', '\xF7', '\xF8', '\b', ',', '\x2', '\x2', '\xF8', 'X', '\x3', '\x2', 
		'\x2', '\x2', '\t', '\x2', '\\', '\x62', 'g', '\xB1', '\xB4', '\xB6', 
		'\x3', '\b', '\x2', '\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace ll
