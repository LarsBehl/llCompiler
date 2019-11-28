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
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="llParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public interface IllVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.compileUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompileUnit([NotNull] llParser.CompileUnitContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] llParser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.compositUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompositUnit([NotNull] llParser.CompositUnitContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>blockSta</c>
	/// labeled alternative in <see cref="llParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlockSta([NotNull] llParser.BlockStaContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>lessOperator</c>
	/// labeled alternative in <see cref="llParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLessOperator([NotNull] llParser.LessOperatorContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>binOpAddSub</c>
	/// labeled alternative in <see cref="llParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinOpAddSub([NotNull] llParser.BinOpAddSubContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>unaryExpr</c>
	/// labeled alternative in <see cref="llParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnaryExpr([NotNull] llParser.UnaryExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>binOpMultDiv</c>
	/// labeled alternative in <see cref="llParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinOpMultDiv([NotNull] llParser.BinOpMultDivContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthes</c>
	/// labeled alternative in <see cref="llParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthes([NotNull] llParser.ParenthesContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>greaterOperator</c>
	/// labeled alternative in <see cref="llParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGreaterOperator([NotNull] llParser.GreaterOperatorContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>equalityOpertor</c>
	/// labeled alternative in <see cref="llParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEqualityOpertor([NotNull] llParser.EqualityOpertorContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>assignStatement</c>
	/// labeled alternative in <see cref="llParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignStatement([NotNull] llParser.AssignStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>instantiationStatement</c>
	/// labeled alternative in <see cref="llParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInstantiationStatement([NotNull] llParser.InstantiationStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>initializationStatement</c>
	/// labeled alternative in <see cref="llParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInitializationStatement([NotNull] llParser.InitializationStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>returnStatement</c>
	/// labeled alternative in <see cref="llParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReturnStatement([NotNull] llParser.ReturnStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>funcDefinitionStatement</c>
	/// labeled alternative in <see cref="llParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncDefinitionStatement([NotNull] llParser.FuncDefinitionStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.unaryExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnaryExpression([NotNull] llParser.UnaryExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.functionCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionCall([NotNull] llParser.FunctionCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.functionDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionDefinition([NotNull] llParser.FunctionDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.variableExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableExpression([NotNull] llParser.VariableExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>doubleAtomExpression</c>
	/// labeled alternative in <see cref="llParser.numericExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDoubleAtomExpression([NotNull] llParser.DoubleAtomExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>integerAtomExpression</c>
	/// labeled alternative in <see cref="llParser.numericExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIntegerAtomExpression([NotNull] llParser.IntegerAtomExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.boolExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolExpression([NotNull] llParser.BoolExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.blockStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlockStatement([NotNull] llParser.BlockStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="llParser.typeDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeDefinition([NotNull] llParser.TypeDefinitionContext context);
}
} // namespace ll
