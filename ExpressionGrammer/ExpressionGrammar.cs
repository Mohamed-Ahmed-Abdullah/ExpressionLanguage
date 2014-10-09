using System.Globalization;
using System.Linq;
using Irony.Compiler;
//using Irony.Parsing;

namespace ExpressionGrammer {

  //[Language("ex", "1.0", "Expression Language Grammer")]
  public class ExpressionGrammar : Grammar {
    //TerminalSet _skipTokensInPreview = new TerminalSet(); //used in token preview for conflict resolution
    public ExpressionGrammar()
    {
      //this.GrammarComments = "NOTE: This grammar is just a demo, and it is a broken demo.\r\n" + 
      //                       "Demonstrates token preview technique to help parser resolve conflicts.\r\n";
      #region Lexical structure
      StringLiteral StringLiteral = TerminalFactory.CreateCSharpString("StringLiteral");
      StringLiteral CharLiteral = TerminalFactory.CreateCSharpChar("CharLiteral");
      NumberLiteral Number = TerminalFactory.CreateCSharpNumber("Number");
      IdentifierTerminal identifier = TerminalFactory.CreateCSharpIdentifier("Identifier");

      CommentTerminal SingleLineComment = new CommentTerminal("SingleLineComment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
      CommentTerminal DelimitedComment = new CommentTerminal("DelimitedComment", "/*", "*/");
      NonGrammarTerminals.Add(SingleLineComment);
      NonGrammarTerminals.Add(DelimitedComment);
      //Temporarily, treat preprocessor instructions like comments
      CommentTerminal ppInstruction = new CommentTerminal("ppInstruction", "#", "\n");
      NonGrammarTerminals.Add(ppInstruction);

      //Symbols
      Terminal colon = new Terminal(":");//ToTerm(":", "colon");
        Terminal semi = ToTerm(";");//, "semi");
      NonTerminal semi_opt = new NonTerminal("semi?");
      semi_opt.Rule = Empty | semi;
      Terminal dot = ToTerm(".");//, "dot");
      Terminal comma = ToTerm(",");//, "comma");
      NonTerminal comma_opt = new NonTerminal("comma_opt", Empty | comma);
      NonTerminal commas_opt = new NonTerminal("commas_opt");
      commas_opt.Rule = MakeStarRule(commas_opt, null, comma);
      Terminal qmark = ToTerm("?");//, "qmark");
      NonTerminal qmark_opt = new NonTerminal("qmark_opt", Empty | qmark);
      Terminal Lbr = ToTerm("{");
      Terminal Rbr = ToTerm("}");
      Terminal Lpar = ToTerm("(");
      Terminal Rpar = ToTerm(")");
      Terminal tgoto = ToTerm("goto");
      Terminal yld = ToTerm("yield");

      Terminal Lparx = ToTerm("(*");
      #endregion

      #region NonTerminals
      //B.2.1. Basic concepts
      var qual_name_with_targs = new NonTerminal("qual_name_with_targs");
      var base_type_list = new NonTerminal("base_type_list");
      var generic_dimension_specifier = new NonTerminal("generic_dimension_specifier");
      var qual_name_segment = new NonTerminal("qual_name_segment");
      var qual_name_segments_opt = new NonTerminal("qual_name_segments_opt");
      var type_or_void = new NonTerminal("type_or_void", "type or void");
      var builtin_type = new NonTerminal("builtin_type", "built-in type");
      var type_ref_list = new NonTerminal("type_ref_list");
      var identifier_ext = new NonTerminal("identifier_ext");
      var identifier_or_builtin = new NonTerminal("identifier_or_builtin");

      //B.2.2. Types
      var type_ref = new NonTerminal("type_ref");
      var type_argument_list = new NonTerminal("type_argument_list");
      var typearg_or_gendimspec_list = new NonTerminal("typearg_or_gendimspec_list");
      var type_argument_list_opt = new NonTerminal("type_argument_list_opt");
      var integral_type = new NonTerminal("integral_type");

      //B.2.4. Expressions
      var argument = new NonTerminal("argument");
      var argument_list = new NonTerminal("argument_list");
      var argument_list_opt = new NonTerminal("argument_list_opt");
      var expression = new NonTerminal("expression", "expression");
      var expression_list = new NonTerminal("expression_list");
      var expression_opt = new NonTerminal("expression_opt");
      var conditional_expression = new NonTerminal("conditional_expression");
      var lambda_expression = new NonTerminal("lambda_expression");
      var query_expression = new NonTerminal("query_expression");
      var unary_operator = new NonTerminal("unary_operator");
      var assignment_operator = new NonTerminal("assignment_operator");
      var primary_expression = new NonTerminal("primary_expression");
      var unary_expression = new NonTerminal("unary_expression");
      var pre_incr_decr_expression = new NonTerminal("pre_incr_decr_expression");
      var post_incr_decr_expression = new NonTerminal("post_incr_decr_expression");
      var literal = new NonTerminal("literal");
      var parenthesized_expression = new NonTerminal("parenthesized_expression");
      var member_access = new NonTerminal("member_access");
      var member_access_segment = new NonTerminal("member_access_segment");
      var member_access_segments_opt = new NonTerminal("member_access_segments_opt");
      var array_indexer = new NonTerminal("array_indexer");
      var argument_list_par = new NonTerminal("argument_list_par");
      var argument_list_par_opt = new NonTerminal("argument_list_par_opt");
      var incr_or_decr = new NonTerminal("incr_or_decr");
      var incr_or_decr_opt = new NonTerminal("incr_or_decr_opt");
      var creation_args = new NonTerminal("creation_args");
      var object_creation_expression = new NonTerminal("object_creation_expression");
      var anonymous_object_creation_expression = new NonTerminal("anonymous_object_creation_expression");
      var typeof_expression = new NonTerminal("typeof_expression");
      var checked_expression = new NonTerminal("checked_expression");
      var unchecked_expression = new NonTerminal("unchecked_expression");
      var default_value_expression = new NonTerminal("default_value_expression");
      //var anonymous_method_expression = new NonTerminal("anonymous_method_expression");

      var elem_initializer = new NonTerminal("elem_initializer");
      var elem_initializer_list = new NonTerminal("elem_initializer_list");
      var elem_initializer_list_ext = new NonTerminal("elem_initializer_list_ext");
      var initializer_value = new NonTerminal("initializer_value");

      var anonymous_object_initializer = new NonTerminal("anonymous_object_initializer");
      var member_declarator = new NonTerminal("member_declarator");
      var member_declarator_list = new NonTerminal("member_declarator_list");
      var unbound_type_name = new NonTerminal("unbound_type_name");
      var generic_dimension_specifier_opt = new NonTerminal("generic_dimension_specifier_opt");
      var bin_op_expression = new NonTerminal("bin_op_expression");
      var typecast_expression = new NonTerminal("typecast_expression");
      var bin_op = new NonTerminal("bin_op", "operator symbol");

      //B.2.5. Statements
      var statement = new NonTerminal("statement", "statement");
      var statement_list = new NonTerminal("statement_list");
      var statement_list_opt = new NonTerminal("statement_list_opt");
      var declaration_statement = new NonTerminal("declaration_statement");
      var embedded_statement = new NonTerminal("embedded_statement");
      var selection_statement = new NonTerminal("selection_statement");
      var iteration_statement = new NonTerminal("iteration_statement");
      var block = new NonTerminal("block");
      var statement_expression = new NonTerminal("statement_expression");
      var statement_expression_list = new NonTerminal("statement_expression_list");
      var local_variable_declaration = new NonTerminal("local_variable_declaration");
      //var local_constant_declaration = new NonTerminal("local_constant_declaration");
      var local_variable_type = new NonTerminal("local_variable_type");
      var local_variable_declarator = new NonTerminal("local_variable_declarator");
      //var local_variable_declarators = new NonTerminal("local_variable_declarators");
      var if_statement = new NonTerminal("if_statement");
      var else_clause_opt = new NonTerminal("else_clause_opt");
      var while_statement = new NonTerminal("while_statement");
      var do_statement = new NonTerminal("do_statement");
      var for_statement = new NonTerminal("for_statement");
      var for_initializer_opt = new NonTerminal("for_initializer_opt");
      var for_condition_opt = new NonTerminal("for_condition_opt");
      var for_iterator_opt = new NonTerminal("for_iterator_opt");
      var break_statement = new NonTerminal("break_statement");
      var continue_statement = new NonTerminal("continue_statement");
      var return_statement = new NonTerminal("return_statement");
      var identifier_opt = new NonTerminal("identifier_opt");

      var resource_acquisition = new NonTerminal("resource_acquisition");

      //namespaces, compilation units
      var qualified_identifier = new NonTerminal("qualified_identifier");
      var qualified_alias_member = new NonTerminal("qualified_alias_member");

      //B.2.9. Arrays
      var rank_specifier = new NonTerminal("rank_specifier");
      var rank_specifiers = new NonTerminal("rank_specifiers");
      var rank_specifiers_opt = new NonTerminal("rank_specifiers_opt");
      var dim_specifier = new NonTerminal("dim_specifier");
      var dim_specifier_opt = new NonTerminal("dim_specifier_opt");
      var list_initializer = new NonTerminal("array_initializer");
      var list_initializer_opt = new NonTerminal("array_initializer_opt");


     
      var new_opt = new NonTerminal("new_opt");
     
      #endregion

      #region operators, punctuation and delimiters
      RegisterOperators(1, "||");
      RegisterOperators(2, "&&");
      RegisterOperators(3, "|");
      RegisterOperators(4, "^");
      RegisterOperators(5, "&");
      RegisterOperators(6, "==", "!=");
      RegisterOperators(7, "<", ">", "<=", ">=", "is", "as");
      RegisterOperators(8, "<<", ">>");
      RegisterOperators(9, "+", "-");
      RegisterOperators(10, "*", "/", "%");
     
      RegisterOperators(-3, "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^=", "<<=", ">>=");
      RegisterOperators(-2, "?");
      RegisterOperators(-1, "??");

      this.MarkPunctuation(";", ",", "(", ")", "{", "}", "[", "]", ":");
      //this.MarkTransient(/*namespace_member_declaration, member_declaration, type_declaration,*/ statement, embedded_statement, expression, 
      //  literal, bin_op, primary_expression, expression);
       
      //this.AddTermsReportGroup("assignment", "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^=", "<<=", ">>=");
      //this.AddTermsReportGroup("typename", "bool", "decimal", "float", "double", "string", "object", 
      //  "sbyte", "byte", "short", "ushort", "int", "uint", "long", "ulong", "char");
      //this.AddTermsReportGroup("statement", "if", "switch", "do", "while", "for", "foreach", "continue", "goto", "return", "try", "yield", 
      //                                      "break", "throw", "unchecked", "using");
      //this.AddTermsReportGroup("type declaration", "public", "private", "protected", "static", "internal", "sealed", "abstract", "partial", 
      //                                             "class", "struct", "delegate", "interface", "enum");
      //this.AddTermsReportGroup("member declaration", "virtual", "override", "readonly", "volatile", "extern");
      //this.AddTermsReportGroup("constant", Number, StringLiteral, CharLiteral);
      //this.AddTermsReportGroup("constant", "true", "false", "null");

      //this.AddTermsReportGroup("unary operator", "+", "-", "!", "~");
      
      //this.AddToNoReportGroup(comma, semi);
      //this.AddToNoReportGroup("var", "const", "new", "++", "--", "this", "base", "checked", "lock", "typeof", "default",
      //                         "{", "}", "[");
     
      //
      #endregion

      #region "<" conflict resolution
      //var gen_lt = new NonTerminal("gen_lt");
      //gen_lt.Rule = CustomActionHere(this.ResolveLessThanConflict) + "<";
      #endregion
      
      // RULES
      //B.2.1. Basic concepts
      //qual_name_with_targs is an alias for namespace-name, namespace-or-type-name, type-name,

      generic_dimension_specifier.Rule = /*gen_lt +*/ commas_opt + ">";
      qual_name_segments_opt.Rule = MakeStarRule(qual_name_segments_opt, null, qual_name_segment);
      identifier_or_builtin.Rule = identifier | builtin_type;
      identifier_ext.Rule = identifier_or_builtin | "this" | "base";
      qual_name_segment.Rule = dot + identifier
                              | "::" + identifier
                              | type_argument_list;
      //generic_dimension_specifier.Rule = lt + commas_opt + ">";
      generic_dimension_specifier.Rule = /*gen_lt +*/ commas_opt + ">";
      qual_name_with_targs.Rule = identifier_or_builtin + qual_name_segments_opt;

      type_argument_list.Rule = /*gen_lt +*/ type_ref_list + ">";
      type_argument_list_opt.Rule = Empty | type_argument_list;
      typearg_or_gendimspec_list.Rule = type_argument_list | generic_dimension_specifier_opt;

      //B.2.2. Types
      type_or_void.Rule = /*qual_name_with_targs |*/ "void";
      builtin_type.Rule = integral_type | "bool" | "decimal" | "float" | "double" | "string" | "object";

      type_ref.Rule = type_or_void + qmark_opt + rank_specifiers_opt + typearg_or_gendimspec_list;
      type_ref_list.Rule = MakePlusRule(type_ref_list, comma, type_ref);

      var comma_list_opt = new NonTerminal("comma_list_opt");
      comma_list_opt.Rule = MakeStarRule(comma_list_opt,null, comma);
      rank_specifier.Rule = "[" + comma_list_opt + "]";
      rank_specifiers.Rule = MakePlusRule(rank_specifiers, null, rank_specifier);
      rank_specifiers_opt.Rule = rank_specifiers.Q();
      integral_type.Rule = ToTerm("sbyte") | "byte" | "short" | "ushort" | "int" | "uint" | "long" | "ulong" | "char";

      
      //B.2.4. Expressions
      argument.Rule = expression | "ref" + identifier | "out" + identifier;
      argument_list.Rule = MakePlusRule(argument_list, comma, argument);
      argument_list_opt.Rule = Empty | argument_list;
      expression.Rule = conditional_expression
                    | bin_op_expression
                    | typecast_expression
                    | primary_expression;
      expression_opt.Rule = Empty | expression;
      expression_list.Rule = MakePlusRule(expression_list, comma, expression);
      unary_operator.Rule = ToTerm("+") | "-" | "!" | "~" | "*";
      assignment_operator.Rule = ToTerm("=") | "+=" | "-=" | "*=" | "/=" | "%=" | "&=" | "|=" | "^=" | "<<=" | ">>=";
      conditional_expression.Rule = expression /*+ PreferShiftHere()*/ + qmark + expression + colon + expression;// + ReduceThis();
      bin_op_expression.Rule = expression + bin_op + expression;

      typecast_expression.Rule = parenthesized_expression + primary_expression;
      primary_expression.Rule =
        literal
        | unary_expression
        | parenthesized_expression
        | member_access
        | pre_incr_decr_expression
        | post_incr_decr_expression
        | object_creation_expression
        | anonymous_object_creation_expression
        | typeof_expression
        | checked_expression
        | unchecked_expression
        | default_value_expression
        /*| anonymous_method_expression*/;
      unary_expression.Rule = unary_operator + primary_expression;
      dim_specifier.Rule = "[" + expression_list + "]";
      dim_specifier_opt.Rule = dim_specifier.Q();
      literal.Rule = Number | StringLiteral | CharLiteral | "true" | "false" | "null";
      parenthesized_expression.Rule = Lpar + expression + Rpar;
      pre_incr_decr_expression.Rule = incr_or_decr + member_access;
      post_incr_decr_expression.Rule = member_access + incr_or_decr;

      //joined invocation_expr and member_access; for member access left the most general variant
      member_access.Rule = identifier_ext + member_access_segments_opt;
      member_access_segments_opt.Rule = MakeStarRule(member_access_segments_opt, null, member_access_segment);
      member_access_segment.Rule = dot + identifier
                                 | array_indexer
                                 | argument_list_par
                                 | type_argument_list;
      array_indexer.Rule = "[" + expression_list + "]";

      argument_list_par.Rule = Lpar + argument_list_opt + Rpar;

      argument_list_par_opt.Rule = Empty | argument_list_par;

      list_initializer.Rule = Lbr + elem_initializer_list_ext + Rbr;
      list_initializer_opt.Rule = list_initializer.Q();

      elem_initializer.Rule = initializer_value | identifier + "=" + initializer_value;
      elem_initializer_list.Rule = MakePlusRule(elem_initializer_list, comma, elem_initializer);
      elem_initializer_list_ext.Rule = Empty | elem_initializer_list + comma_opt;
      initializer_value.Rule = expression | list_initializer;

      //delegate, anon-object, object
      object_creation_expression.Rule = "new" + qual_name_with_targs + qmark_opt + creation_args + list_initializer_opt;
      creation_args.Rule = dim_specifier | rank_specifier | argument_list_par;

      anonymous_object_creation_expression.Rule = "new" + anonymous_object_initializer;
      anonymous_object_initializer.Rule = Lbr + Rbr | Lbr + member_declarator_list + comma_opt + Rbr;
      member_declarator.Rule = expression | identifier + "=" + expression;
      member_declarator_list.Rule = MakePlusRule(member_declarator_list, comma, member_declarator);
      //typeof
      typeof_expression.Rule = "typeof" + Lpar + type_ref + Rpar;
      generic_dimension_specifier_opt.Rule = Empty /*| gen_lt*/ + commas_opt + ">";
      //checked, unchecked
      checked_expression.Rule = "checked" + parenthesized_expression;
      unchecked_expression.Rule = "unchecked" + parenthesized_expression;
      //default-value
      default_value_expression.Rule = "default" + Lpar + type_ref + Rpar;
     
      bin_op.Rule = ToTerm("<")       
                  | "||" | "&&" | "|" | "^" | "&" | "==" | "!=" | ">" | "<=" | ">=" | "<<" | ">>" | "+" | "-" | "*" | "/" | "%"
                  | "=" | "+=" | "-=" | "*=" | "/=" | "%=" | "&=" | "|=" | "^=" | "<<=" | ">>="
                  | "is" | "as" | "??";

      //Queries
      query_expression.Rule = "from";

      //B.2.5. Statements
      statement.Rule = /*labeled_statement |*/ declaration_statement | embedded_statement;
      statement.ErrorRule = SyntaxError + semi; //skip all until semicolon
      statement_list.Rule = MakePlusRule(statement_list, null, statement);
      statement_list_opt.Rule = Empty | statement_list;
      declaration_statement.Rule = local_variable_declaration + semi /*| local_constant_declaration*/ + semi;
      local_variable_declaration.Rule = local_variable_type /*+ local_variable_declarators*/;
      local_variable_type.Rule = member_access | "var"; // | builtin_type; //to fix the conflict, changing to member-access here
      local_variable_declarator.Rule = identifier | identifier + "=" + initializer_value;
      //local_variable_declarators.Rule = MakePlusRule(local_variable_declarators, comma, local_variable_declarator);
      //local_constant_declaration.Rule = "const" + type_ref /*+ constant_declarators*/;
      //embedded_statement
      embedded_statement.Rule = block | semi /*empty_statement*/ | statement_expression + semi | selection_statement
                               | iteration_statement /* | jump_statement | try_statement | checked_statement | unchecked_statement
                               | lock_statement | using_statement | yield_statement*/;
      block.Rule = Lbr + statement_list_opt + Rbr;
      //selection (if and switch)
      selection_statement.Rule = if_statement /*| switch_statement*/;
      if_statement.Rule = ToTerm("if") + Lpar + expression + Rpar + embedded_statement + else_clause_opt;
      else_clause_opt.Rule = Empty /*| PreferShiftHere()*/ + "else" + embedded_statement;
      iteration_statement.Rule = while_statement | do_statement | for_statement /*| foreach_statement*/;
      while_statement.Rule = "while" + parenthesized_expression + embedded_statement;
      do_statement.Rule = "do" + embedded_statement + "while" + parenthesized_expression + semi;
      for_statement.Rule = "for" + Lpar + for_initializer_opt + semi + for_condition_opt + semi + for_iterator_opt + Rpar + embedded_statement;
      for_initializer_opt.Rule = Empty | local_variable_declaration | statement_expression_list;
      for_condition_opt.Rule = Empty | expression;
      for_iterator_opt.Rule = Empty | statement_expression_list;
      break_statement.Rule = "break" + semi;
      continue_statement.Rule = "continue" + semi;
      return_statement.Rule = "return" + expression_opt + semi;
      identifier_opt.Rule = Empty | identifier;
      
      resource_acquisition.Rule = local_variable_declaration | expression;

      statement_expression.Rule = object_creation_expression
                                | member_access | member_access + assignment_operator + expression
                                | pre_incr_decr_expression | post_incr_decr_expression
                                ;
      statement_expression_list.Rule = MakePlusRule(statement_expression_list, comma, statement_expression);
      incr_or_decr_opt.Rule = Empty | ToTerm("++") | "--";
      incr_or_decr.Rule = ToTerm("++") | "--";

      //B.2.6. Namespaces
      this.Root = block;
      qualified_identifier.Rule = MakePlusRule(qualified_identifier, dot, identifier);

      base_type_list.Rule = MakePlusRule(base_type_list, comma, qual_name_with_targs);

      new_opt.Rule = Empty | "new";

      //_skipTokensInPreview.UnionWith(new Terminal[] { dot, identifier, comma, ToTerm("::"), comma, ToTerm("["), ToTerm("]") });
    
    }

      public Terminal ToTerm(string terminal)
      {
          return new Terminal(terminal);
      }

      #region conflict resolution for "<"
    //Here is an elaborate generic declaration which can be used as a good test. Perfectly legal, uncomment it to check that c#
    // accepts it:
    // List<Dictionary<string, object[,]>> genericVar; 
    //private void ResolveLessThanConflict(ParsingContext context, CustomParserAction customAction) {
    //  var scanner = context.Parser.Scanner;
    //  string previewSym = null;
    //  if (context.CurrentParserInput.Term.Name == "<") {
    //    scanner.BeginPreview(); 
    //    int ltCount = 0;
    //    while(true) {
    //      //Find first token ahead (using preview mode) that is either end of generic parameter (">") or something else
    //      Token preview;
    //      do {
    //        preview = scanner.GetToken();
    //      } while (_skipTokensInPreview.Contains(preview.Terminal) && preview.Terminal != base.Eof);
    //      //See what did we find
    //      previewSym = preview.Terminal.Name; 
    //      if (previewSym == "<")
    //        ltCount++;
    //      else if (previewSym == ">" && ltCount > 0) {
    //        ltCount--;
    //        continue;               
    //      } else 
    //        break; 
    //    }
    //    scanner.EndPreview(true); //keep previewed tokens; important to keep ">>" matched to two ">" symbols, not one combined symbol (see method below)
    //  }//if
    //  //if we see ">", then it is type argument, not operator
    //  ParserAction action;
    //  if (previewSym == ">")
    //    action = customAction.ShiftActions.First(a => a.Term.Name == "<");
    //  else
    //    action = customAction.ReduceActions.First();
    //  // Actually execute action
    //  action.Execute(context);
    //}

    //In preview, we may run into combination '>>' which is a comletion of nested generic parameters.
    // It should be recognized as two ">" symbols, not a single ">>" operator
    // By default, the ">>" has higher priority over single ">" symbol because it is longer. 
    // When this method is called we force the selection to a single ">"
    //public override void OnScannerSelectTerminal(ParsingContext context) {
    //  if (context.Source.PreviewChar == '>' && context.Status == ParserStatus.Previewing) {
    //    context.CurrentTerminals.Clear();
    //    context.CurrentTerminals.Add(ToTerm(">")); //select the ">" terminal
    //  }
    //  base.OnScannerSelectTerminal(context); 
    //}
    #endregion

    // See    http://www.jaggersoft.com/csharp_standard/9.3.3.htm
    //public override void SkipWhitespace(ISourceStream source) {
    //  while (!source.EOF()) {
    //    var ch = source.PreviewChar;
    //    switch (ch) {
    //      case ' ':
    //      case '\t':
    //      case '\r':
    //      case '\n':
    //      case '\v':
    //      case '\u2085':
    //      case '\u2028':
    //      case '\u2029':
    //        source.PreviewPosition++;
    //        break;
    //      default:
    //        //Check unicode class Zs
    //        UnicodeCategory chCat = char.GetUnicodeCategory(ch);
    //        if (chCat == UnicodeCategory.SpaceSeparator) //it is whitespace, continue moving
    //          continue;//while loop 
    //        //Otherwize return
    //        return;
    //    }//switch
    //  }//while
    //}
      public void MarkPunctuation(params string[] symbols)
      {
          foreach (string symbol in symbols)
          {
              var term = ToTerm(symbol);
              //term.SetFlag(TermFlags.IsPunctuation | TermFlags.NoAstNode);
          }
      }
  }//class


}//namespace

