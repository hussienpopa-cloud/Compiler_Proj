using System;
using System.Collections.Generic;

public class TinyParser
{
    private List<Token> tokens;
    private int index = 0;
    private Token currentToken;

    public TinyParser(List<Token> tokens)
    {
        this.tokens = tokens;

        if (tokens.Count > 0)
            currentToken = tokens[index];
    }

    private void Match(string expected)
    {
        if (index < tokens.Count &&
            (currentToken.Value == expected || currentToken.Type == expected))
        {
            index++;

            if (index < tokens.Count)
                currentToken = tokens[index];
        }
        else
        {
            throw new Exception($"Syntax Error: Expected '{expected}' but found '{currentToken?.Value}'");
        }
    }

    public void ParseProgram()
    {
        ParseStatements();
    }

    private void ParseStatements()
    {
        ParseStatement();

        while (index < tokens.Count &&
               currentToken.Value != "end" &&
               currentToken.Value != "}" &&
               currentToken.Value != "until" &&
               currentToken.Value != "else")
        {
            ParseStatement();
        }
    }

    private void ParseStatement()
    {
        if (index >= tokens.Count) return;

        if (currentToken.Value == "int" ||
            currentToken.Value == "float" ||
            currentToken.Value == "string")
        {
            ParseDeclaration();
            Match(";");
        }
        else if (currentToken.Type == "Identifier")
        {
            ParseAssignment();
            Match(";");
        }
        else if (currentToken.Value == "if")
        {
            ParseIf();
        }
        else if (currentToken.Value == "repeat")
        {
            ParseRepeat();
        }
        else if (currentToken.Value == "{")
        {
            ParseBlock();
        }
        else if (currentToken.Value == "write")
        {
            ParseWrite();
            Match(";");
        }
        else if (currentToken.Value == "read")
        {
            ParseRead();
            Match(";");
        }
        else
        {
            throw new Exception(
                $"Error: A statement cannot start with '{currentToken.Value}' ({currentToken.Type})"
            );
        }
    }

    private void ParseDeclaration()
    {
        if (currentToken.Value == "int") Match("int");
        else if (currentToken.Value == "float") Match("float");
        else Match("string");

        Match("Identifier");

        if (currentToken.Value == ":=")
        {
            Match(":=");
            ParseExpression();
        }

        while (currentToken.Value == ",")
        {
            Match(",");
            Match("Identifier");

            if (currentToken.Value == ":=")
            {
                Match(":=");
                ParseExpression();
            }
        }
    }

    private void ParseAssignment()
    {
        if (currentToken.Type != "Identifier")
            throw new Exception($"Error: Cannot assign value to {currentToken.Type}");

        Match("Identifier");
        Match(":=");
        ParseExpression();
    }

    private void ParseIf()
    {
        Match("if");
        ParseCondition();
        Match("then");

        ParseStatements();

        if (index < tokens.Count && currentToken.Value == "else")
        {
            Match("else");
            ParseStatements();
        }

        Match("end");
    }

    private void ParseRepeat()
    {
        Match("repeat");

        ParseStatements();

        Match("until");
        Match("(");
        ParseCondition();
        Match(")");
    }

    private void ParseBlock()
    {
        Match("{");
        ParseStatements();
        Match("}");
    }

    private void ParseWrite()
    {
        Match("write");
        ParseExpression();
    }

    private void ParseRead()
    {
        Match("read");
        Match("Identifier");
    }

    private void ParseCondition()
    {
        ParseExpression();

        if (currentToken.Value == "<" ||
            currentToken.Value == ">" ||
            currentToken.Value == "=" ||
            currentToken.Value == "<>")
        {
            Match(currentToken.Value);
        }
        else
        {
            throw new Exception("Expected Relational Operator");
        }

        ParseExpression();
    }

    private void ParseExpression()
    {
        ParseTerm();

        while (index < tokens.Count &&
              (currentToken.Value == "+" ||
               currentToken.Value == "-"))
        {
            Match(currentToken.Value);
            ParseTerm();
        }
    }

    private void ParseTerm()
    {
        ParseFactor();

        while (index < tokens.Count &&
              (currentToken.Value == "*" ||
               currentToken.Value == "/"))
        {
            Match(currentToken.Value);
            ParseFactor();
        }
    }

    private void ParseFactor()
    {
        if (currentToken.Type == "Identifier")
            Match("Identifier");

        else if (currentToken.Type == "Number")
            Match("Number");

        else if (currentToken.Value == "(")
        {
            Match("(");
            ParseExpression();
            Match(")");
        }
        else
        {
            throw new Exception(
                $"Expected Identifier or Number but found '{currentToken.Value}'"
            );
        }
    }
}