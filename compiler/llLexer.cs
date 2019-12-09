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
		BOOL_TYPE=6, BOOL_TRUE=7, BOOL_FALSE=8, IF=9, ELSE=10, WHILE=11, WORD=12, 
		MULT=13, ADD=14, MINUS=15, DIV=16, DOT=17, PAR_L=18, PAR_R=19, ASSIGN=20, 
		CURL_L=21, CURL_R=22, SEMCOL=23, EQUAL=24, LESS=25, GREATER=26, COLON=27, 
		COMMA=28, WHITESPACE=29;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"DOUBLE_LITERAL", "INTEGER_LITERAL", "RETURN", "INT_TYPE", "DOUBLE_TYPE", 
		"BOOL_TYPE", "BOOL_TRUE", "BOOL_FALSE", "IF", "ELSE", "WHILE", "WORD", 
		"MULT", "ADD", "MINUS", "DIV", "DOT", "PAR_L", "PAR_R", "ASSIGN", "CURL_L", 
		"CURL_R", "SEMCOL", "EQUAL", "LESS", "GREATER", "COLON", "COMMA", "WHITESPACE"
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
		null, "'*'", "'+'", "'-'", "'/'", "'.'", "'('", "')'", "'='", "'{'", "'}'", 
		"';'", null, "'<'", "'>'", "':'", "','"
	};
	private static readonly string[] _SymbolicNames = {
		null, "DOUBLE_LITERAL", "INTEGER_LITERAL", "RETURN", "INT_TYPE", "DOUBLE_TYPE", 
		"BOOL_TYPE", "BOOL_TRUE", "BOOL_FALSE", "IF", "ELSE", "WHILE", "WORD", 
		"MULT", "ADD", "MINUS", "DIV", "DOT", "PAR_L", "PAR_R", "ASSIGN", "CURL_L", 
		"CURL_R", "SEMCOL", "EQUAL", "LESS", "GREATER", "COLON", "COMMA", "WHITESPACE"
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
		'\x5964', '\x2', '\x1F', '\xA7', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
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
		'\x1D', '\x4', '\x1E', '\t', '\x1E', '\x3', '\x2', '\x6', '\x2', '?', 
		'\n', '\x2', '\r', '\x2', '\xE', '\x2', '@', '\x3', '\x2', '\x3', '\x2', 
		'\x6', '\x2', '\x45', '\n', '\x2', '\r', '\x2', '\xE', '\x2', '\x46', 
		'\x3', '\x3', '\x6', '\x3', 'J', '\n', '\x3', '\r', '\x3', '\xE', '\x3', 
		'K', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', 
		'\x3', '\x5', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', '\a', '\x3', 
		'\a', '\x3', '\a', '\x3', '\a', '\x3', '\b', '\x3', '\b', '\x3', '\b', 
		'\x3', '\b', '\x3', '\b', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', 
		'\t', '\x3', '\t', '\x3', '\t', '\x3', '\n', '\x3', '\n', '\x3', '\n', 
		'\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', 
		'\f', '\x3', '\f', '\x3', '\f', '\x3', '\f', '\x3', '\f', '\x3', '\f', 
		'\x3', '\r', '\x6', '\r', '\x7F', '\n', '\r', '\r', '\r', '\xE', '\r', 
		'\x80', '\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x3', '\xF', '\x3', 
		'\x10', '\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x12', '\x3', 
		'\x12', '\x3', '\x13', '\x3', '\x13', '\x3', '\x14', '\x3', '\x14', '\x3', 
		'\x15', '\x3', '\x15', '\x3', '\x16', '\x3', '\x16', '\x3', '\x17', '\x3', 
		'\x17', '\x3', '\x18', '\x3', '\x18', '\x3', '\x19', '\x3', '\x19', '\x3', 
		'\x19', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1B', '\x3', '\x1B', '\x3', 
		'\x1C', '\x3', '\x1C', '\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1E', '\x3', 
		'\x1E', '\x3', '\x1E', '\x3', '\x1E', '\x2', '\x2', '\x1F', '\x3', '\x3', 
		'\x5', '\x4', '\a', '\x5', '\t', '\x6', '\v', '\a', '\r', '\b', '\xF', 
		'\t', '\x11', '\n', '\x13', '\v', '\x15', '\f', '\x17', '\r', '\x19', 
		'\xE', '\x1B', '\xF', '\x1D', '\x10', '\x1F', '\x11', '!', '\x12', '#', 
		'\x13', '%', '\x14', '\'', '\x15', ')', '\x16', '+', '\x17', '-', '\x18', 
		'/', '\x19', '\x31', '\x1A', '\x33', '\x1B', '\x35', '\x1C', '\x37', '\x1D', 
		'\x39', '\x1E', ';', '\x1F', '\x3', '\x2', '\x5', '\x3', '\x2', '\x32', 
		';', '\x4', '\x2', '\x43', '\\', '\x63', '|', '\x5', '\x2', '\v', '\f', 
		'\xF', '\xF', '\"', '\"', '\x2', '\xAA', '\x2', '\x3', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x5', '\x3', '\x2', '\x2', '\x2', '\x2', '\a', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\t', '\x3', '\x2', '\x2', '\x2', '\x2', '\v', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\r', '\x3', '\x2', '\x2', '\x2', '\x2', '\xF', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x11', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x13', '\x3', '\x2', '\x2', '\x2', '\x2', '\x15', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x17', '\x3', '\x2', '\x2', '\x2', '\x2', '\x19', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x1D', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '!', '\x3', '\x2', '\x2', '\x2', '\x2', '#', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '%', '\x3', '\x2', '\x2', '\x2', '\x2', '\'', 
		'\x3', '\x2', '\x2', '\x2', '\x2', ')', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'+', '\x3', '\x2', '\x2', '\x2', '\x2', '-', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '/', '\x3', '\x2', '\x2', '\x2', '\x2', '\x31', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x33', '\x3', '\x2', '\x2', '\x2', '\x2', '\x35', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x37', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x39', '\x3', '\x2', '\x2', '\x2', '\x2', ';', '\x3', '\x2', '\x2', '\x2', 
		'\x3', '>', '\x3', '\x2', '\x2', '\x2', '\x5', 'I', '\x3', '\x2', '\x2', 
		'\x2', '\a', 'M', '\x3', '\x2', '\x2', '\x2', '\t', 'T', '\x3', '\x2', 
		'\x2', '\x2', '\v', 'X', '\x3', '\x2', '\x2', '\x2', '\r', '_', '\x3', 
		'\x2', '\x2', '\x2', '\xF', '\x64', '\x3', '\x2', '\x2', '\x2', '\x11', 
		'i', '\x3', '\x2', '\x2', '\x2', '\x13', 'o', '\x3', '\x2', '\x2', '\x2', 
		'\x15', 'r', '\x3', '\x2', '\x2', '\x2', '\x17', 'w', '\x3', '\x2', '\x2', 
		'\x2', '\x19', '~', '\x3', '\x2', '\x2', '\x2', '\x1B', '\x82', '\x3', 
		'\x2', '\x2', '\x2', '\x1D', '\x84', '\x3', '\x2', '\x2', '\x2', '\x1F', 
		'\x86', '\x3', '\x2', '\x2', '\x2', '!', '\x88', '\x3', '\x2', '\x2', 
		'\x2', '#', '\x8A', '\x3', '\x2', '\x2', '\x2', '%', '\x8C', '\x3', '\x2', 
		'\x2', '\x2', '\'', '\x8E', '\x3', '\x2', '\x2', '\x2', ')', '\x90', '\x3', 
		'\x2', '\x2', '\x2', '+', '\x92', '\x3', '\x2', '\x2', '\x2', '-', '\x94', 
		'\x3', '\x2', '\x2', '\x2', '/', '\x96', '\x3', '\x2', '\x2', '\x2', '\x31', 
		'\x98', '\x3', '\x2', '\x2', '\x2', '\x33', '\x9B', '\x3', '\x2', '\x2', 
		'\x2', '\x35', '\x9D', '\x3', '\x2', '\x2', '\x2', '\x37', '\x9F', '\x3', 
		'\x2', '\x2', '\x2', '\x39', '\xA1', '\x3', '\x2', '\x2', '\x2', ';', 
		'\xA3', '\x3', '\x2', '\x2', '\x2', '=', '?', '\t', '\x2', '\x2', '\x2', 
		'>', '=', '\x3', '\x2', '\x2', '\x2', '?', '@', '\x3', '\x2', '\x2', '\x2', 
		'@', '>', '\x3', '\x2', '\x2', '\x2', '@', '\x41', '\x3', '\x2', '\x2', 
		'\x2', '\x41', '\x42', '\x3', '\x2', '\x2', '\x2', '\x42', '\x44', '\x5', 
		'#', '\x12', '\x2', '\x43', '\x45', '\t', '\x2', '\x2', '\x2', '\x44', 
		'\x43', '\x3', '\x2', '\x2', '\x2', '\x45', '\x46', '\x3', '\x2', '\x2', 
		'\x2', '\x46', '\x44', '\x3', '\x2', '\x2', '\x2', '\x46', 'G', '\x3', 
		'\x2', '\x2', '\x2', 'G', '\x4', '\x3', '\x2', '\x2', '\x2', 'H', 'J', 
		'\t', '\x2', '\x2', '\x2', 'I', 'H', '\x3', '\x2', '\x2', '\x2', 'J', 
		'K', '\x3', '\x2', '\x2', '\x2', 'K', 'I', '\x3', '\x2', '\x2', '\x2', 
		'K', 'L', '\x3', '\x2', '\x2', '\x2', 'L', '\x6', '\x3', '\x2', '\x2', 
		'\x2', 'M', 'N', '\a', 't', '\x2', '\x2', 'N', 'O', '\a', 'g', '\x2', 
		'\x2', 'O', 'P', '\a', 'v', '\x2', '\x2', 'P', 'Q', '\a', 'w', '\x2', 
		'\x2', 'Q', 'R', '\a', 't', '\x2', '\x2', 'R', 'S', '\a', 'p', '\x2', 
		'\x2', 'S', '\b', '\x3', '\x2', '\x2', '\x2', 'T', 'U', '\a', 'k', '\x2', 
		'\x2', 'U', 'V', '\a', 'p', '\x2', '\x2', 'V', 'W', '\a', 'v', '\x2', 
		'\x2', 'W', '\n', '\x3', '\x2', '\x2', '\x2', 'X', 'Y', '\a', '\x66', 
		'\x2', '\x2', 'Y', 'Z', '\a', 'q', '\x2', '\x2', 'Z', '[', '\a', 'w', 
		'\x2', '\x2', '[', '\\', '\a', '\x64', '\x2', '\x2', '\\', ']', '\a', 
		'n', '\x2', '\x2', ']', '^', '\a', 'g', '\x2', '\x2', '^', '\f', '\x3', 
		'\x2', '\x2', '\x2', '_', '`', '\a', '\x64', '\x2', '\x2', '`', '\x61', 
		'\a', 'q', '\x2', '\x2', '\x61', '\x62', '\a', 'q', '\x2', '\x2', '\x62', 
		'\x63', '\a', 'n', '\x2', '\x2', '\x63', '\xE', '\x3', '\x2', '\x2', '\x2', 
		'\x64', '\x65', '\a', 'v', '\x2', '\x2', '\x65', '\x66', '\a', 't', '\x2', 
		'\x2', '\x66', 'g', '\a', 'w', '\x2', '\x2', 'g', 'h', '\a', 'g', '\x2', 
		'\x2', 'h', '\x10', '\x3', '\x2', '\x2', '\x2', 'i', 'j', '\a', 'h', '\x2', 
		'\x2', 'j', 'k', '\a', '\x63', '\x2', '\x2', 'k', 'l', '\a', 'n', '\x2', 
		'\x2', 'l', 'm', '\a', 'u', '\x2', '\x2', 'm', 'n', '\a', 'g', '\x2', 
		'\x2', 'n', '\x12', '\x3', '\x2', '\x2', '\x2', 'o', 'p', '\a', 'k', '\x2', 
		'\x2', 'p', 'q', '\a', 'h', '\x2', '\x2', 'q', '\x14', '\x3', '\x2', '\x2', 
		'\x2', 'r', 's', '\a', 'g', '\x2', '\x2', 's', 't', '\a', 'n', '\x2', 
		'\x2', 't', 'u', '\a', 'u', '\x2', '\x2', 'u', 'v', '\a', 'g', '\x2', 
		'\x2', 'v', '\x16', '\x3', '\x2', '\x2', '\x2', 'w', 'x', '\a', 'y', '\x2', 
		'\x2', 'x', 'y', '\a', 'j', '\x2', '\x2', 'y', 'z', '\a', 'k', '\x2', 
		'\x2', 'z', '{', '\a', 'n', '\x2', '\x2', '{', '|', '\a', 'g', '\x2', 
		'\x2', '|', '\x18', '\x3', '\x2', '\x2', '\x2', '}', '\x7F', '\t', '\x3', 
		'\x2', '\x2', '~', '}', '\x3', '\x2', '\x2', '\x2', '\x7F', '\x80', '\x3', 
		'\x2', '\x2', '\x2', '\x80', '~', '\x3', '\x2', '\x2', '\x2', '\x80', 
		'\x81', '\x3', '\x2', '\x2', '\x2', '\x81', '\x1A', '\x3', '\x2', '\x2', 
		'\x2', '\x82', '\x83', '\a', ',', '\x2', '\x2', '\x83', '\x1C', '\x3', 
		'\x2', '\x2', '\x2', '\x84', '\x85', '\a', '-', '\x2', '\x2', '\x85', 
		'\x1E', '\x3', '\x2', '\x2', '\x2', '\x86', '\x87', '\a', '/', '\x2', 
		'\x2', '\x87', ' ', '\x3', '\x2', '\x2', '\x2', '\x88', '\x89', '\a', 
		'\x31', '\x2', '\x2', '\x89', '\"', '\x3', '\x2', '\x2', '\x2', '\x8A', 
		'\x8B', '\a', '\x30', '\x2', '\x2', '\x8B', '$', '\x3', '\x2', '\x2', 
		'\x2', '\x8C', '\x8D', '\a', '*', '\x2', '\x2', '\x8D', '&', '\x3', '\x2', 
		'\x2', '\x2', '\x8E', '\x8F', '\a', '+', '\x2', '\x2', '\x8F', '(', '\x3', 
		'\x2', '\x2', '\x2', '\x90', '\x91', '\a', '?', '\x2', '\x2', '\x91', 
		'*', '\x3', '\x2', '\x2', '\x2', '\x92', '\x93', '\a', '}', '\x2', '\x2', 
		'\x93', ',', '\x3', '\x2', '\x2', '\x2', '\x94', '\x95', '\a', '\x7F', 
		'\x2', '\x2', '\x95', '.', '\x3', '\x2', '\x2', '\x2', '\x96', '\x97', 
		'\a', '=', '\x2', '\x2', '\x97', '\x30', '\x3', '\x2', '\x2', '\x2', '\x98', 
		'\x99', '\a', '?', '\x2', '\x2', '\x99', '\x9A', '\a', '?', '\x2', '\x2', 
		'\x9A', '\x32', '\x3', '\x2', '\x2', '\x2', '\x9B', '\x9C', '\a', '>', 
		'\x2', '\x2', '\x9C', '\x34', '\x3', '\x2', '\x2', '\x2', '\x9D', '\x9E', 
		'\a', '@', '\x2', '\x2', '\x9E', '\x36', '\x3', '\x2', '\x2', '\x2', '\x9F', 
		'\xA0', '\a', '<', '\x2', '\x2', '\xA0', '\x38', '\x3', '\x2', '\x2', 
		'\x2', '\xA1', '\xA2', '\a', '.', '\x2', '\x2', '\xA2', ':', '\x3', '\x2', 
		'\x2', '\x2', '\xA3', '\xA4', '\t', '\x4', '\x2', '\x2', '\xA4', '\xA5', 
		'\x3', '\x2', '\x2', '\x2', '\xA5', '\xA6', '\b', '\x1E', '\x2', '\x2', 
		'\xA6', '<', '\x3', '\x2', '\x2', '\x2', '\a', '\x2', '@', '\x46', 'K', 
		'\x80', '\x3', '\b', '\x2', '\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace ll
