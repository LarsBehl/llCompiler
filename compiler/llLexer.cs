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
		DOUBLE_LITERAL=1, INTEGER_LITERAL=2, RETURN=3, WORD=4, MULT=5, ADD=6, 
		MINUS=7, DIV=8, DOT=9, BRAC_L=10, BRAC_R=11, ASSIGN=12, CURL_L=13, CURL_R=14, 
		SEMCOL=15, WHITESPACE=16;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"DOUBLE_LITERAL", "INTEGER_LITERAL", "RETURN", "WORD", "MULT", "ADD", 
		"MINUS", "DIV", "DOT", "BRAC_L", "BRAC_R", "ASSIGN", "CURL_L", "CURL_R", 
		"SEMCOL", "WHITESPACE"
	};


	public llLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public llLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, "'*'", "'+'", "'-'", "'/'", "'.'", "'('", 
		"')'", "'='", "'{'", "'}'", "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "DOUBLE_LITERAL", "INTEGER_LITERAL", "RETURN", "WORD", "MULT", "ADD", 
		"MINUS", "DIV", "DOT", "BRAC_L", "BRAC_R", "ASSIGN", "CURL_L", "CURL_R", 
		"SEMCOL", "WHITESPACE"
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
		'\x5964', '\x2', '\x12', 'Y', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', 
		'\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x3', '\x2', '\x6', '\x2', '%', '\n', '\x2', '\r', 
		'\x2', '\xE', '\x2', '&', '\x3', '\x2', '\x3', '\x2', '\x6', '\x2', '+', 
		'\n', '\x2', '\r', '\x2', '\xE', '\x2', ',', '\x3', '\x3', '\x6', '\x3', 
		'\x30', '\n', '\x3', '\r', '\x3', '\xE', '\x3', '\x31', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x5', '\x6', '\x5', '<', '\n', '\x5', '\r', '\x5', 
		'\xE', '\x5', '=', '\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', '\a', 
		'\x3', '\b', '\x3', '\b', '\x3', '\t', '\x3', '\t', '\x3', '\n', '\x3', 
		'\n', '\x3', '\v', '\x3', '\v', '\x3', '\f', '\x3', '\f', '\x3', '\r', 
		'\x3', '\r', '\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x3', '\xF', '\x3', 
		'\x10', '\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x11', '\x3', 
		'\x11', '\x2', '\x2', '\x12', '\x3', '\x3', '\x5', '\x4', '\a', '\x5', 
		'\t', '\x6', '\v', '\a', '\r', '\b', '\xF', '\t', '\x11', '\n', '\x13', 
		'\v', '\x15', '\f', '\x17', '\r', '\x19', '\xE', '\x1B', '\xF', '\x1D', 
		'\x10', '\x1F', '\x11', '!', '\x12', '\x3', '\x2', '\x5', '\x3', '\x2', 
		'\x32', ';', '\x4', '\x2', '\x43', '\\', '\x63', '|', '\x5', '\x2', '\v', 
		'\f', '\xF', '\xF', '\"', '\"', '\x2', '\\', '\x2', '\x3', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x5', '\x3', '\x2', '\x2', '\x2', '\x2', '\a', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\t', '\x3', '\x2', '\x2', '\x2', '\x2', '\v', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\r', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\xF', '\x3', '\x2', '\x2', '\x2', '\x2', '\x11', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x13', '\x3', '\x2', '\x2', '\x2', '\x2', '\x15', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x17', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x19', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1B', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x1D', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1F', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '!', '\x3', '\x2', '\x2', '\x2', '\x3', '$', 
		'\x3', '\x2', '\x2', '\x2', '\x5', '/', '\x3', '\x2', '\x2', '\x2', '\a', 
		'\x33', '\x3', '\x2', '\x2', '\x2', '\t', ';', '\x3', '\x2', '\x2', '\x2', 
		'\v', '?', '\x3', '\x2', '\x2', '\x2', '\r', '\x41', '\x3', '\x2', '\x2', 
		'\x2', '\xF', '\x43', '\x3', '\x2', '\x2', '\x2', '\x11', '\x45', '\x3', 
		'\x2', '\x2', '\x2', '\x13', 'G', '\x3', '\x2', '\x2', '\x2', '\x15', 
		'I', '\x3', '\x2', '\x2', '\x2', '\x17', 'K', '\x3', '\x2', '\x2', '\x2', 
		'\x19', 'M', '\x3', '\x2', '\x2', '\x2', '\x1B', 'O', '\x3', '\x2', '\x2', 
		'\x2', '\x1D', 'Q', '\x3', '\x2', '\x2', '\x2', '\x1F', 'S', '\x3', '\x2', 
		'\x2', '\x2', '!', 'U', '\x3', '\x2', '\x2', '\x2', '#', '%', '\t', '\x2', 
		'\x2', '\x2', '$', '#', '\x3', '\x2', '\x2', '\x2', '%', '&', '\x3', '\x2', 
		'\x2', '\x2', '&', '$', '\x3', '\x2', '\x2', '\x2', '&', '\'', '\x3', 
		'\x2', '\x2', '\x2', '\'', '(', '\x3', '\x2', '\x2', '\x2', '(', '*', 
		'\x5', '\x13', '\n', '\x2', ')', '+', '\t', '\x2', '\x2', '\x2', '*', 
		')', '\x3', '\x2', '\x2', '\x2', '+', ',', '\x3', '\x2', '\x2', '\x2', 
		',', '*', '\x3', '\x2', '\x2', '\x2', ',', '-', '\x3', '\x2', '\x2', '\x2', 
		'-', '\x4', '\x3', '\x2', '\x2', '\x2', '.', '\x30', '\t', '\x2', '\x2', 
		'\x2', '/', '.', '\x3', '\x2', '\x2', '\x2', '\x30', '\x31', '\x3', '\x2', 
		'\x2', '\x2', '\x31', '/', '\x3', '\x2', '\x2', '\x2', '\x31', '\x32', 
		'\x3', '\x2', '\x2', '\x2', '\x32', '\x6', '\x3', '\x2', '\x2', '\x2', 
		'\x33', '\x34', '\a', 't', '\x2', '\x2', '\x34', '\x35', '\a', 'g', '\x2', 
		'\x2', '\x35', '\x36', '\a', 'v', '\x2', '\x2', '\x36', '\x37', '\a', 
		'w', '\x2', '\x2', '\x37', '\x38', '\a', 't', '\x2', '\x2', '\x38', '\x39', 
		'\a', 'p', '\x2', '\x2', '\x39', '\b', '\x3', '\x2', '\x2', '\x2', ':', 
		'<', '\t', '\x3', '\x2', '\x2', ';', ':', '\x3', '\x2', '\x2', '\x2', 
		'<', '=', '\x3', '\x2', '\x2', '\x2', '=', ';', '\x3', '\x2', '\x2', '\x2', 
		'=', '>', '\x3', '\x2', '\x2', '\x2', '>', '\n', '\x3', '\x2', '\x2', 
		'\x2', '?', '@', '\a', ',', '\x2', '\x2', '@', '\f', '\x3', '\x2', '\x2', 
		'\x2', '\x41', '\x42', '\a', '-', '\x2', '\x2', '\x42', '\xE', '\x3', 
		'\x2', '\x2', '\x2', '\x43', '\x44', '\a', '/', '\x2', '\x2', '\x44', 
		'\x10', '\x3', '\x2', '\x2', '\x2', '\x45', '\x46', '\a', '\x31', '\x2', 
		'\x2', '\x46', '\x12', '\x3', '\x2', '\x2', '\x2', 'G', 'H', '\a', '\x30', 
		'\x2', '\x2', 'H', '\x14', '\x3', '\x2', '\x2', '\x2', 'I', 'J', '\a', 
		'*', '\x2', '\x2', 'J', '\x16', '\x3', '\x2', '\x2', '\x2', 'K', 'L', 
		'\a', '+', '\x2', '\x2', 'L', '\x18', '\x3', '\x2', '\x2', '\x2', 'M', 
		'N', '\a', '?', '\x2', '\x2', 'N', '\x1A', '\x3', '\x2', '\x2', '\x2', 
		'O', 'P', '\a', '}', '\x2', '\x2', 'P', '\x1C', '\x3', '\x2', '\x2', '\x2', 
		'Q', 'R', '\a', '\x7F', '\x2', '\x2', 'R', '\x1E', '\x3', '\x2', '\x2', 
		'\x2', 'S', 'T', '\a', '=', '\x2', '\x2', 'T', ' ', '\x3', '\x2', '\x2', 
		'\x2', 'U', 'V', '\t', '\x4', '\x2', '\x2', 'V', 'W', '\x3', '\x2', '\x2', 
		'\x2', 'W', 'X', '\b', '\x11', '\x2', '\x2', 'X', '\"', '\x3', '\x2', 
		'\x2', '\x2', '\a', '\x2', '&', ',', '\x31', '=', '\x3', '\b', '\x2', 
		'\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace ll
