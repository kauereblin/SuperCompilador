using System;
using System.Collections;
using System.IO;
using System.Text;

namespace SuperCompilador
{
    public class Sintatico : Constants
    {
        private Stack stack = new Stack();
        private Token currentToken;
        private Token previousToken;
        private Lexico scanner;
        private Semantico semanticAnalyser;

        public string fileIL { set; get; }

        private static bool isTerminal(int x)
        {
            return x < ParserConstants.FIRST_NON_TERMINAL;
        }

        private static bool isNonTerminal(int x)
        {
            return x >= ParserConstants.FIRST_NON_TERMINAL && x < ParserConstants.FIRST_SEMANTIC_ACTION;
        }

        private static bool isSemanticAction(int x)
        {
            return x >= ParserConstants.FIRST_SEMANTIC_ACTION;
        }

        private bool step()
        {
            if (currentToken == null)
            {
                int pos = 0;
                if (previousToken != null)
                    pos = previousToken.getPosition() + previousToken.getLexeme().Length;

                currentToken = new Token((int) ETokens.DOLLAR, "$", pos);
            }

            int x = (int) stack.Pop();
            int a = currentToken.getId();

            if (x == (int) ETokens.EPSILON)
            {
                return false;
            }
            else if (isTerminal(x))
            {
                if (x == a)
                {
                    if (stack.Count == 0)
                        return true;
                    else
                    {
                        previousToken = currentToken;
                        currentToken = scanner.nextToken();
                        return false;
                    }
                }
                else
                {
                    string sLexeme = currentToken.getLexeme() == "$" ? "EOF" : currentToken.getLexeme();
                    throw new SyntaticError(ParserConstants.PARSER_ERROR[x], currentToken.getPosition(), sLexeme);
                }
            }
            else if (isNonTerminal(x))
            {
                if (pushProduction(x, a))
                    return false;
                else
                {
                    string sLexeme = currentToken.getLexeme() == "$" ? "EOF" : currentToken.getLexeme();
                    throw new SyntaticError(ParserConstants.PARSER_ERROR[x], currentToken.getPosition(), sLexeme);
                }
            }
            else
            {
                semanticAnalyser.executeAction(x - ParserConstants.FIRST_SEMANTIC_ACTION, previousToken);
                return false;
            }
        }

        private bool pushProduction(int topStack, int tokenInput)
        {
            int p = ParserConstants.PARSER_TABLE[topStack - ParserConstants.FIRST_NON_TERMINAL, tokenInput - 1];
            if (p >= 0)
            {
                int[] production = ParserConstants.PRODUCTIONS[p];
                for (int i = production.Length - 1; i >= 0; i--)
                    stack.Push(production[i]);

                return true;
            }
            else
                return false;
        }

        public void parse(Lexico scanner, Semantico semanticAnalyser)
        {
            this.scanner = scanner;
            this.semanticAnalyser = semanticAnalyser;

            stack.Clear();
            stack.Push(ETokens.DOLLAR);
            stack.Push(ParserConstants.START_SYMBOL);

            currentToken = scanner.nextToken();

            while (!step());

            string filePath = semanticAnalyser.objFile;
            filePath = filePath.Substring(0, filePath.IndexOf(".")) + ".il";

            File.WriteAllText(filePath, semanticAnalyser.code);

            fileIL = filePath;
        }
    }
}

