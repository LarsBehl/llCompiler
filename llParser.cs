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
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public partial class llParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		DOUBLE_LITERAL=1, INTEGER_LITERAL=2, WORD=3, MULT=4, ADD=5, MINUS=6, DIV=7, 
		DOT=8, BRAC_L=9, BRAC_R=10, ASSIGN=11, CURL_L=12, CURL_R=13, SECOL=14, 
		WHITESPACE=15;
	public const int
		RULE_compileUnit = 0, RULE_expression = 1, RULE_numericExpression = 2;
	public static readonly string[] ruleNames = {
		"compileUnit", "expression", "numericExpression"
	};

	private static readonly string[] _LiteralNames = {
		null, null, null, null, "'*'", "'+'", "'-'", "'/'", "'.'", "'('", "')'", 
		"'='", "'{'", "'}'", "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "DOUBLE_LITERAL", "INTEGER_LITERAL", "WORD", "MULT", "ADD", "MINUS", 
		"DIV", "DOT", "BRAC_L", "BRAC_R", "ASSIGN", "CURL_L", "CURL_R", "SECOL", 
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

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static llParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public llParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public llParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class CompileUnitContext : ParserRuleContext {
		public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		public ITerminalNode Eof() { return GetToken(llParser.Eof, 0); }
		public CompileUnitContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_compileUnit; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitCompileUnit(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public CompileUnitContext compileUnit() {
		CompileUnitContext _localctx = new CompileUnitContext(Context, State);
		EnterRule(_localctx, 0, RULE_compileUnit);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 6; expression(0);
			State = 7; Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ExpressionContext : ParserRuleContext {
		public ExpressionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_expression; } }
	 
		public ExpressionContext() { }
		public virtual void CopyFrom(ExpressionContext context) {
			base.CopyFrom(context);
		}
	}
	public partial class BinOpAddSubContext : ExpressionContext {
		public ExpressionContext left;
		public IToken op;
		public ExpressionContext right;
		public ExpressionContext[] expression() {
			return GetRuleContexts<ExpressionContext>();
		}
		public ExpressionContext expression(int i) {
			return GetRuleContext<ExpressionContext>(i);
		}
		public ITerminalNode ADD() { return GetToken(llParser.ADD, 0); }
		public ITerminalNode MINUS() { return GetToken(llParser.MINUS, 0); }
		public BinOpAddSubContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBinOpAddSub(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class BinOpMultDivContext : ExpressionContext {
		public ExpressionContext left;
		public IToken op;
		public ExpressionContext right;
		public ExpressionContext[] expression() {
			return GetRuleContexts<ExpressionContext>();
		}
		public ExpressionContext expression(int i) {
			return GetRuleContext<ExpressionContext>(i);
		}
		public ITerminalNode MULT() { return GetToken(llParser.MULT, 0); }
		public ITerminalNode DIV() { return GetToken(llParser.DIV, 0); }
		public BinOpMultDivContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBinOpMultDiv(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class NumericAtomExpressionContext : ExpressionContext {
		public NumericExpressionContext numericExpression() {
			return GetRuleContext<NumericExpressionContext>(0);
		}
		public NumericAtomExpressionContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNumericAtomExpression(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class AssignExpressionContext : ExpressionContext {
		public IToken left;
		public ExpressionContext right;
		public ITerminalNode ASSIGN() { return GetToken(llParser.ASSIGN, 0); }
		public ITerminalNode WORD() { return GetToken(llParser.WORD, 0); }
		public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		public AssignExpressionContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitAssignExpression(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ParenthesContext : ExpressionContext {
		public ITerminalNode BRAC_L() { return GetToken(llParser.BRAC_L, 0); }
		public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		public ITerminalNode BRAC_R() { return GetToken(llParser.BRAC_R, 0); }
		public ParenthesContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitParenthes(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class VariableExpressionContext : ExpressionContext {
		public ITerminalNode WORD() { return GetToken(llParser.WORD, 0); }
		public VariableExpressionContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVariableExpression(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ExpressionContext expression() {
		return expression(0);
	}

	private ExpressionContext expression(int _p) {
		ParserRuleContext _parentctx = Context;
		int _parentState = State;
		ExpressionContext _localctx = new ExpressionContext(Context, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 2;
		EnterRecursionRule(_localctx, 2, RULE_expression, _p);
		int _la;
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 19;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,0,Context) ) {
			case 1:
				{
				_localctx = new ParenthesContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;

				State = 10; Match(BRAC_L);
				State = 11; expression(0);
				State = 12; Match(BRAC_R);
				}
				break;
			case 2:
				{
				_localctx = new AssignExpressionContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 14; ((AssignExpressionContext)_localctx).left = Match(WORD);
				State = 15; Match(ASSIGN);
				State = 16; ((AssignExpressionContext)_localctx).right = expression(3);
				}
				break;
			case 3:
				{
				_localctx = new NumericAtomExpressionContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 17; numericExpression();
				}
				break;
			case 4:
				{
				_localctx = new VariableExpressionContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 18; Match(WORD);
				}
				break;
			}
			Context.Stop = TokenStream.LT(-1);
			State = 29;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,2,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					State = 27;
					ErrorHandler.Sync(this);
					switch ( Interpreter.AdaptivePredict(TokenStream,1,Context) ) {
					case 1:
						{
						_localctx = new BinOpMultDivContext(new ExpressionContext(_parentctx, _parentState));
						((BinOpMultDivContext)_localctx).left = _prevctx;
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 21;
						if (!(Precpred(Context, 5))) throw new FailedPredicateException(this, "Precpred(Context, 5)");
						State = 22;
						((BinOpMultDivContext)_localctx).op = TokenStream.LT(1);
						_la = TokenStream.LA(1);
						if ( !(_la==MULT || _la==DIV) ) {
							((BinOpMultDivContext)_localctx).op = ErrorHandler.RecoverInline(this);
						}
						else {
							ErrorHandler.ReportMatch(this);
						    Consume();
						}
						State = 23; ((BinOpMultDivContext)_localctx).right = expression(6);
						}
						break;
					case 2:
						{
						_localctx = new BinOpAddSubContext(new ExpressionContext(_parentctx, _parentState));
						((BinOpAddSubContext)_localctx).left = _prevctx;
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 24;
						if (!(Precpred(Context, 4))) throw new FailedPredicateException(this, "Precpred(Context, 4)");
						State = 25;
						((BinOpAddSubContext)_localctx).op = TokenStream.LT(1);
						_la = TokenStream.LA(1);
						if ( !(_la==ADD || _la==MINUS) ) {
							((BinOpAddSubContext)_localctx).op = ErrorHandler.RecoverInline(this);
						}
						else {
							ErrorHandler.ReportMatch(this);
						    Consume();
						}
						State = 26; ((BinOpAddSubContext)_localctx).right = expression(5);
						}
						break;
					}
					} 
				}
				State = 31;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,2,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			UnrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public partial class NumericExpressionContext : ParserRuleContext {
		public NumericExpressionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_numericExpression; } }
	 
		public NumericExpressionContext() { }
		public virtual void CopyFrom(NumericExpressionContext context) {
			base.CopyFrom(context);
		}
	}
	public partial class IntegerAtomExpressionContext : NumericExpressionContext {
		public IToken sign;
		public ITerminalNode INTEGER_LITERAL() { return GetToken(llParser.INTEGER_LITERAL, 0); }
		public ITerminalNode MINUS() { return GetToken(llParser.MINUS, 0); }
		public IntegerAtomExpressionContext(NumericExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitIntegerAtomExpression(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class DoubleAtomExpressionContext : NumericExpressionContext {
		public IToken sign;
		public ITerminalNode DOUBLE_LITERAL() { return GetToken(llParser.DOUBLE_LITERAL, 0); }
		public ITerminalNode MINUS() { return GetToken(llParser.MINUS, 0); }
		public DoubleAtomExpressionContext(NumericExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitDoubleAtomExpression(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public NumericExpressionContext numericExpression() {
		NumericExpressionContext _localctx = new NumericExpressionContext(Context, State);
		EnterRule(_localctx, 4, RULE_numericExpression);
		int _la;
		try {
			State = 40;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,5,Context) ) {
			case 1:
				_localctx = new DoubleAtomExpressionContext(_localctx);
				EnterOuterAlt(_localctx, 1);
				{
				State = 33;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==MINUS) {
					{
					State = 32; ((DoubleAtomExpressionContext)_localctx).sign = Match(MINUS);
					}
				}

				State = 35; Match(DOUBLE_LITERAL);
				}
				break;
			case 2:
				_localctx = new IntegerAtomExpressionContext(_localctx);
				EnterOuterAlt(_localctx, 2);
				{
				State = 37;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==MINUS) {
					{
					State = 36; ((IntegerAtomExpressionContext)_localctx).sign = Match(MINUS);
					}
				}

				State = 39; Match(INTEGER_LITERAL);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public override bool Sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 1: return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private bool expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0: return Precpred(Context, 5);
		case 1: return Precpred(Context, 4);
		}
		return true;
	}

	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x3', '\x11', '-', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', 
		'\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x3', '\x2', '\x3', '\x2', '\x3', 
		'\x2', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x5', '\x3', '\x16', '\n', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\a', '\x3', '\x1E', 
		'\n', '\x3', '\f', '\x3', '\xE', '\x3', '!', '\v', '\x3', '\x3', '\x4', 
		'\x5', '\x4', '$', '\n', '\x4', '\x3', '\x4', '\x3', '\x4', '\x5', '\x4', 
		'(', '\n', '\x4', '\x3', '\x4', '\x5', '\x4', '+', '\n', '\x4', '\x3', 
		'\x4', '\x2', '\x3', '\x4', '\x5', '\x2', '\x4', '\x6', '\x2', '\x4', 
		'\x4', '\x2', '\x6', '\x6', '\t', '\t', '\x3', '\x2', '\a', '\b', '\x2', 
		'\x31', '\x2', '\b', '\x3', '\x2', '\x2', '\x2', '\x4', '\x15', '\x3', 
		'\x2', '\x2', '\x2', '\x6', '*', '\x3', '\x2', '\x2', '\x2', '\b', '\t', 
		'\x5', '\x4', '\x3', '\x2', '\t', '\n', '\a', '\x2', '\x2', '\x3', '\n', 
		'\x3', '\x3', '\x2', '\x2', '\x2', '\v', '\f', '\b', '\x3', '\x1', '\x2', 
		'\f', '\r', '\a', '\v', '\x2', '\x2', '\r', '\xE', '\x5', '\x4', '\x3', 
		'\x2', '\xE', '\xF', '\a', '\f', '\x2', '\x2', '\xF', '\x16', '\x3', '\x2', 
		'\x2', '\x2', '\x10', '\x11', '\a', '\x5', '\x2', '\x2', '\x11', '\x12', 
		'\a', '\r', '\x2', '\x2', '\x12', '\x16', '\x5', '\x4', '\x3', '\x5', 
		'\x13', '\x16', '\x5', '\x6', '\x4', '\x2', '\x14', '\x16', '\a', '\x5', 
		'\x2', '\x2', '\x15', '\v', '\x3', '\x2', '\x2', '\x2', '\x15', '\x10', 
		'\x3', '\x2', '\x2', '\x2', '\x15', '\x13', '\x3', '\x2', '\x2', '\x2', 
		'\x15', '\x14', '\x3', '\x2', '\x2', '\x2', '\x16', '\x1F', '\x3', '\x2', 
		'\x2', '\x2', '\x17', '\x18', '\f', '\a', '\x2', '\x2', '\x18', '\x19', 
		'\t', '\x2', '\x2', '\x2', '\x19', '\x1E', '\x5', '\x4', '\x3', '\b', 
		'\x1A', '\x1B', '\f', '\x6', '\x2', '\x2', '\x1B', '\x1C', '\t', '\x3', 
		'\x2', '\x2', '\x1C', '\x1E', '\x5', '\x4', '\x3', '\a', '\x1D', '\x17', 
		'\x3', '\x2', '\x2', '\x2', '\x1D', '\x1A', '\x3', '\x2', '\x2', '\x2', 
		'\x1E', '!', '\x3', '\x2', '\x2', '\x2', '\x1F', '\x1D', '\x3', '\x2', 
		'\x2', '\x2', '\x1F', ' ', '\x3', '\x2', '\x2', '\x2', ' ', '\x5', '\x3', 
		'\x2', '\x2', '\x2', '!', '\x1F', '\x3', '\x2', '\x2', '\x2', '\"', '$', 
		'\a', '\b', '\x2', '\x2', '#', '\"', '\x3', '\x2', '\x2', '\x2', '#', 
		'$', '\x3', '\x2', '\x2', '\x2', '$', '%', '\x3', '\x2', '\x2', '\x2', 
		'%', '+', '\a', '\x3', '\x2', '\x2', '&', '(', '\a', '\b', '\x2', '\x2', 
		'\'', '&', '\x3', '\x2', '\x2', '\x2', '\'', '(', '\x3', '\x2', '\x2', 
		'\x2', '(', ')', '\x3', '\x2', '\x2', '\x2', ')', '+', '\a', '\x4', '\x2', 
		'\x2', '*', '#', '\x3', '\x2', '\x2', '\x2', '*', '\'', '\x3', '\x2', 
		'\x2', '\x2', '+', '\a', '\x3', '\x2', '\x2', '\x2', '\b', '\x15', '\x1D', 
		'\x1F', '#', '\'', '*',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace ll
