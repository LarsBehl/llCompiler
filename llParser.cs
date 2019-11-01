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
		T__0=1, T__1=2, DOUBLE_LITERAL=3, INTEGER_LITERAL=4, WORD=5, MULT=6, ADD=7, 
		MINUS=8, DIV=9, DOT=10, BRAC_L=11, BRAC_R=12, CURL_L=13, CURL_R=14, SECOL=15, 
		WHITESPACE=16;
	public const int
		RULE_compileUnit = 0, RULE_expression = 1, RULE_expressionSequenz = 2, 
		RULE_variableExpression = 3, RULE_numericExpression = 4;
	public static readonly string[] ruleNames = {
		"compileUnit", "expression", "expressionSequenz", "variableExpression", 
		"numericExpression"
	};

	private static readonly string[] _LiteralNames = {
		null, "','", "'='", null, null, null, "'*'", "'+'", "'-'", "'/'", "'.'", 
		"'('", "')'", "'{'", "'}'", "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, "DOUBLE_LITERAL", "INTEGER_LITERAL", "WORD", "MULT", 
		"ADD", "MINUS", "DIV", "DOT", "BRAC_L", "BRAC_R", "CURL_L", "CURL_R", 
		"SECOL", "WHITESPACE"
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
			State = 10; expression(0);
			State = 11; Match(Eof);
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
	public partial class VarExprContext : ExpressionContext {
		public VariableExpressionContext variableExpression() {
			return GetRuleContext<VariableExpressionContext>(0);
		}
		public VarExprContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVarExpr(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ExprSequContext : ExpressionContext {
		public ExpressionSequenzContext expressionSequenz() {
			return GetRuleContext<ExpressionSequenzContext>(0);
		}
		public ExprSequContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExprSequ(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class InfixExpressionContext : ExpressionContext {
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
		public ITerminalNode ADD() { return GetToken(llParser.ADD, 0); }
		public ITerminalNode MINUS() { return GetToken(llParser.MINUS, 0); }
		public InfixExpressionContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitInfixExpression(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class FunctionDefinitionContext : ExpressionContext {
		public IToken funcName;
		public ExpressionSequenzContext body;
		public ITerminalNode BRAC_L() { return GetToken(llParser.BRAC_L, 0); }
		public ITerminalNode BRAC_R() { return GetToken(llParser.BRAC_R, 0); }
		public ITerminalNode[] WORD() { return GetTokens(llParser.WORD); }
		public ITerminalNode WORD(int i) {
			return GetToken(llParser.WORD, i);
		}
		public ExpressionSequenzContext expressionSequenz() {
			return GetRuleContext<ExpressionSequenzContext>(0);
		}
		public FunctionDefinitionContext(ExpressionContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFunctionDefinition(this);
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
		public ITerminalNode SECOL() { return GetToken(llParser.SECOL, 0); }
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
			State = 37;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,1,Context) ) {
			case 1:
				{
				_localctx = new ParenthesContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;

				State = 14; Match(BRAC_L);
				State = 15; expression(0);
				State = 16; Match(BRAC_R);
				}
				break;
			case 2:
				{
				_localctx = new FunctionDefinitionContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 18; ((FunctionDefinitionContext)_localctx).funcName = Match(WORD);
				State = 19; Match(BRAC_L);
				State = 24;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				while (_la==WORD) {
					{
					{
					State = 20; Match(WORD);
					State = 21; Match(T__0);
					}
					}
					State = 26;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
				}
				State = 27; Match(BRAC_R);
				State = 28; ((FunctionDefinitionContext)_localctx).body = expressionSequenz();
				}
				break;
			case 3:
				{
				_localctx = new AssignExpressionContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 29; ((AssignExpressionContext)_localctx).left = Match(WORD);
				State = 30; Match(T__1);
				State = 31; ((AssignExpressionContext)_localctx).right = expression(0);
				State = 32; Match(SECOL);
				}
				break;
			case 4:
				{
				_localctx = new NumericAtomExpressionContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 34; numericExpression();
				}
				break;
			case 5:
				{
				_localctx = new VarExprContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 35; variableExpression();
				}
				break;
			case 6:
				{
				_localctx = new ExprSequContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 36; expressionSequenz();
				}
				break;
			}
			Context.Stop = TokenStream.LT(-1);
			State = 47;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,3,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					State = 45;
					ErrorHandler.Sync(this);
					switch ( Interpreter.AdaptivePredict(TokenStream,2,Context) ) {
					case 1:
						{
						_localctx = new InfixExpressionContext(new ExpressionContext(_parentctx, _parentState));
						((InfixExpressionContext)_localctx).left = _prevctx;
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 39;
						if (!(Precpred(Context, 5))) throw new FailedPredicateException(this, "Precpred(Context, 5)");
						State = 40;
						((InfixExpressionContext)_localctx).op = TokenStream.LT(1);
						_la = TokenStream.LA(1);
						if ( !(_la==MULT || _la==DIV) ) {
							((InfixExpressionContext)_localctx).op = ErrorHandler.RecoverInline(this);
						}
						else {
							ErrorHandler.ReportMatch(this);
						    Consume();
						}
						State = 41; ((InfixExpressionContext)_localctx).right = expression(6);
						}
						break;
					case 2:
						{
						_localctx = new InfixExpressionContext(new ExpressionContext(_parentctx, _parentState));
						((InfixExpressionContext)_localctx).left = _prevctx;
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 42;
						if (!(Precpred(Context, 4))) throw new FailedPredicateException(this, "Precpred(Context, 4)");
						State = 43;
						((InfixExpressionContext)_localctx).op = TokenStream.LT(1);
						_la = TokenStream.LA(1);
						if ( !(_la==ADD || _la==MINUS) ) {
							((InfixExpressionContext)_localctx).op = ErrorHandler.RecoverInline(this);
						}
						else {
							ErrorHandler.ReportMatch(this);
						    Consume();
						}
						State = 44; ((InfixExpressionContext)_localctx).right = expression(5);
						}
						break;
					}
					} 
				}
				State = 49;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,3,Context);
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

	public partial class ExpressionSequenzContext : ParserRuleContext {
		public ITerminalNode CURL_L() { return GetToken(llParser.CURL_L, 0); }
		public ITerminalNode CURL_R() { return GetToken(llParser.CURL_R, 0); }
		public ExpressionContext[] expression() {
			return GetRuleContexts<ExpressionContext>();
		}
		public ExpressionContext expression(int i) {
			return GetRuleContext<ExpressionContext>(i);
		}
		public ExpressionSequenzContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_expressionSequenz; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitExpressionSequenz(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ExpressionSequenzContext expressionSequenz() {
		ExpressionSequenzContext _localctx = new ExpressionSequenzContext(Context, State);
		EnterRule(_localctx, 4, RULE_expressionSequenz);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 50; Match(CURL_L);
			State = 54;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_LITERAL) | (1L << INTEGER_LITERAL) | (1L << WORD) | (1L << MINUS) | (1L << BRAC_L) | (1L << CURL_L))) != 0)) {
				{
				{
				State = 51; expression(0);
				}
				}
				State = 56;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 57; Match(CURL_R);
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

	public partial class VariableExpressionContext : ParserRuleContext {
		public ITerminalNode WORD() { return GetToken(llParser.WORD, 0); }
		public ITerminalNode SECOL() { return GetToken(llParser.SECOL, 0); }
		public VariableExpressionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_variableExpression; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IllVisitor<TResult> typedVisitor = visitor as IllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVariableExpression(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public VariableExpressionContext variableExpression() {
		VariableExpressionContext _localctx = new VariableExpressionContext(Context, State);
		EnterRule(_localctx, 6, RULE_variableExpression);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 59; Match(WORD);
			State = 60; Match(SECOL);
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
		EnterRule(_localctx, 8, RULE_numericExpression);
		int _la;
		try {
			State = 70;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,7,Context) ) {
			case 1:
				_localctx = new DoubleAtomExpressionContext(_localctx);
				EnterOuterAlt(_localctx, 1);
				{
				State = 63;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==MINUS) {
					{
					State = 62; ((DoubleAtomExpressionContext)_localctx).sign = Match(MINUS);
					}
				}

				State = 65; Match(DOUBLE_LITERAL);
				}
				break;
			case 2:
				_localctx = new IntegerAtomExpressionContext(_localctx);
				EnterOuterAlt(_localctx, 2);
				{
				State = 67;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==MINUS) {
					{
					State = 66; ((IntegerAtomExpressionContext)_localctx).sign = Match(MINUS);
					}
				}

				State = 69; Match(INTEGER_LITERAL);
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
		'\x5964', '\x3', '\x12', 'K', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', 
		'\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', '\x5', '\x4', 
		'\x6', '\t', '\x6', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\a', '\x3', '\x19', '\n', '\x3', 
		'\f', '\x3', '\xE', '\x3', '\x1C', '\v', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x5', '\x3', '(', '\n', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\a', '\x3', '\x30', '\n', '\x3', '\f', '\x3', '\xE', '\x3', 
		'\x33', '\v', '\x3', '\x3', '\x4', '\x3', '\x4', '\a', '\x4', '\x37', 
		'\n', '\x4', '\f', '\x4', '\xE', '\x4', ':', '\v', '\x4', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', 
		'\x5', '\x6', '\x42', '\n', '\x6', '\x3', '\x6', '\x3', '\x6', '\x5', 
		'\x6', '\x46', '\n', '\x6', '\x3', '\x6', '\x5', '\x6', 'I', '\n', '\x6', 
		'\x3', '\x6', '\x2', '\x3', '\x4', '\a', '\x2', '\x4', '\x6', '\b', '\n', 
		'\x2', '\x4', '\x4', '\x2', '\b', '\b', '\v', '\v', '\x3', '\x2', '\t', 
		'\n', '\x2', 'Q', '\x2', '\f', '\x3', '\x2', '\x2', '\x2', '\x4', '\'', 
		'\x3', '\x2', '\x2', '\x2', '\x6', '\x34', '\x3', '\x2', '\x2', '\x2', 
		'\b', '=', '\x3', '\x2', '\x2', '\x2', '\n', 'H', '\x3', '\x2', '\x2', 
		'\x2', '\f', '\r', '\x5', '\x4', '\x3', '\x2', '\r', '\xE', '\a', '\x2', 
		'\x2', '\x3', '\xE', '\x3', '\x3', '\x2', '\x2', '\x2', '\xF', '\x10', 
		'\b', '\x3', '\x1', '\x2', '\x10', '\x11', '\a', '\r', '\x2', '\x2', '\x11', 
		'\x12', '\x5', '\x4', '\x3', '\x2', '\x12', '\x13', '\a', '\xE', '\x2', 
		'\x2', '\x13', '(', '\x3', '\x2', '\x2', '\x2', '\x14', '\x15', '\a', 
		'\a', '\x2', '\x2', '\x15', '\x1A', '\a', '\r', '\x2', '\x2', '\x16', 
		'\x17', '\a', '\a', '\x2', '\x2', '\x17', '\x19', '\a', '\x3', '\x2', 
		'\x2', '\x18', '\x16', '\x3', '\x2', '\x2', '\x2', '\x19', '\x1C', '\x3', 
		'\x2', '\x2', '\x2', '\x1A', '\x18', '\x3', '\x2', '\x2', '\x2', '\x1A', 
		'\x1B', '\x3', '\x2', '\x2', '\x2', '\x1B', '\x1D', '\x3', '\x2', '\x2', 
		'\x2', '\x1C', '\x1A', '\x3', '\x2', '\x2', '\x2', '\x1D', '\x1E', '\a', 
		'\xE', '\x2', '\x2', '\x1E', '(', '\x5', '\x6', '\x4', '\x2', '\x1F', 
		' ', '\a', '\a', '\x2', '\x2', ' ', '!', '\a', '\x4', '\x2', '\x2', '!', 
		'\"', '\x5', '\x4', '\x3', '\x2', '\"', '#', '\a', '\x11', '\x2', '\x2', 
		'#', '(', '\x3', '\x2', '\x2', '\x2', '$', '(', '\x5', '\n', '\x6', '\x2', 
		'%', '(', '\x5', '\b', '\x5', '\x2', '&', '(', '\x5', '\x6', '\x4', '\x2', 
		'\'', '\xF', '\x3', '\x2', '\x2', '\x2', '\'', '\x14', '\x3', '\x2', '\x2', 
		'\x2', '\'', '\x1F', '\x3', '\x2', '\x2', '\x2', '\'', '$', '\x3', '\x2', 
		'\x2', '\x2', '\'', '%', '\x3', '\x2', '\x2', '\x2', '\'', '&', '\x3', 
		'\x2', '\x2', '\x2', '(', '\x31', '\x3', '\x2', '\x2', '\x2', ')', '*', 
		'\f', '\a', '\x2', '\x2', '*', '+', '\t', '\x2', '\x2', '\x2', '+', '\x30', 
		'\x5', '\x4', '\x3', '\b', ',', '-', '\f', '\x6', '\x2', '\x2', '-', '.', 
		'\t', '\x3', '\x2', '\x2', '.', '\x30', '\x5', '\x4', '\x3', '\a', '/', 
		')', '\x3', '\x2', '\x2', '\x2', '/', ',', '\x3', '\x2', '\x2', '\x2', 
		'\x30', '\x33', '\x3', '\x2', '\x2', '\x2', '\x31', '/', '\x3', '\x2', 
		'\x2', '\x2', '\x31', '\x32', '\x3', '\x2', '\x2', '\x2', '\x32', '\x5', 
		'\x3', '\x2', '\x2', '\x2', '\x33', '\x31', '\x3', '\x2', '\x2', '\x2', 
		'\x34', '\x38', '\a', '\xF', '\x2', '\x2', '\x35', '\x37', '\x5', '\x4', 
		'\x3', '\x2', '\x36', '\x35', '\x3', '\x2', '\x2', '\x2', '\x37', ':', 
		'\x3', '\x2', '\x2', '\x2', '\x38', '\x36', '\x3', '\x2', '\x2', '\x2', 
		'\x38', '\x39', '\x3', '\x2', '\x2', '\x2', '\x39', ';', '\x3', '\x2', 
		'\x2', '\x2', ':', '\x38', '\x3', '\x2', '\x2', '\x2', ';', '<', '\a', 
		'\x10', '\x2', '\x2', '<', '\a', '\x3', '\x2', '\x2', '\x2', '=', '>', 
		'\a', '\a', '\x2', '\x2', '>', '?', '\a', '\x11', '\x2', '\x2', '?', '\t', 
		'\x3', '\x2', '\x2', '\x2', '@', '\x42', '\a', '\n', '\x2', '\x2', '\x41', 
		'@', '\x3', '\x2', '\x2', '\x2', '\x41', '\x42', '\x3', '\x2', '\x2', 
		'\x2', '\x42', '\x43', '\x3', '\x2', '\x2', '\x2', '\x43', 'I', '\a', 
		'\x5', '\x2', '\x2', '\x44', '\x46', '\a', '\n', '\x2', '\x2', '\x45', 
		'\x44', '\x3', '\x2', '\x2', '\x2', '\x45', '\x46', '\x3', '\x2', '\x2', 
		'\x2', '\x46', 'G', '\x3', '\x2', '\x2', '\x2', 'G', 'I', '\a', '\x6', 
		'\x2', '\x2', 'H', '\x41', '\x3', '\x2', '\x2', '\x2', 'H', '\x45', '\x3', 
		'\x2', '\x2', '\x2', 'I', '\v', '\x3', '\x2', '\x2', '\x2', '\n', '\x1A', 
		'\'', '/', '\x31', '\x38', '\x41', '\x45', 'H',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace ll
