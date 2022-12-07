using System;
using System.Collections;
using System.Collections.Generic;

namespace SuperCompilador
{
    public class Semantico : Constants
    {
        public  string                     code;
        private string                     relationalOperator;
        private Stack<string>              typeStack   = new Stack<string>();
        private Dictionary<string, string> symbolTable = new Dictionary<string, string>();
        private string                     varType;
        private List<string>               ids = new List<string>();
        private string                     parseClass;
        private Stack<string>              labelStack  = new Stack<string>();
        private int                        labelCounter = 1;
        private const string               LABEL = "L";

        public string objFile { get; set; }

        public void executeAction(int action, Token token)
        {
            switch (action)
            {
                case 1: 
                    {
                        string type1 = typeStack.Pop();
                        string type2 = typeStack.Pop();

                        if (type1 == "float64" || type2 == "float64")
                            typeStack.Push("float64");
                        else
                            typeStack.Push("int64");

                        code += "add\n";
                    }
                break;

                case 2: 
                    {
                        typeStack.Pop();
                        typeStack.Pop();

                        typeStack.Push("float64");

                        code += "sub\n";
                    }
                break;

                case 3: 
                    {
                        string type1 = typeStack.Pop(); 
                        string type2 = typeStack.Pop();

                        if (type1 == "float64" || type2 == "float64")
                            typeStack.Push("float64");
                        else
                            typeStack.Push("int64");

                        code += "mul\n";
                    }
                break;

                case 4: 
                    {
                        string type1 = typeStack.Pop();
                        string type2 = typeStack.Pop();

                        if (type1 == type2)
                            typeStack.Push(type1);

                        code += "div\n";
                    }
                break;

                case 5: 
                    {
                        typeStack.Push("int64");

                        string number = token.getLexeme();
                        int charD = number.IndexOf("d");
                        int result;

                        if (charD == -1)
                        {
                            result = Convert.ToInt32(number);
                        }
                        else
                        {
                            string before = number.Substring(0, charD);
                            string after  = number.Substring(charD);

                            result = Convert.ToInt32(before) * (int)Math.Pow(10, Convert.ToInt32(after));

                        }

                        code += $"ldc.i8 {result}\n";
                        code += $"conv.r8\n";
                    }
                break;

                case 6: 
                    {
                        typeStack.Push("float64");

                        code += $"ldc.r8 {token.getLexeme()}\n";
                    }
                break;

                case 7: 
                    {
                        string type = typeStack.Pop();

                        if (type == "float64" || type == "int64")
                            typeStack.Push(type);
                    }
                break;

                case 8:
                    {
                        string type = typeStack.Pop();

                        if (type == "float64" || type == "int64")
                            typeStack.Push(type);
                        
                        code += "ldc.i8 - 1\n";
                        code += "conv.r8\n";
                        code += "mul\n";

                    }
                break;

                case 9: relationalOperator = token.getLexeme(); break;

                case 10:
                    {
                        string type1 = typeStack.Pop();
                        string type2 = typeStack.Pop();

                        if (type1 == type2)
                            typeStack.Push("bool");

                        switch (relationalOperator)
                        {
                            case ">" : code += "cgt\n"; break;
                            case "<" : code += "clt\n"; break;
                            case "==": code += "ceq\n"; break;
                        }
                    }
                    break;

                case 11:
                    {
                        typeStack.Push("bool");

                        code += "ldc.i4.1\n";
                    }
                    break;

                case 12:
                    {
                        typeStack.Push("bool");

                        code += "ldc.i4.0\n";
                    }
                break;

                case 13:
                    {
                        string type = typeStack.Pop();

                        if (type == "bool")
                            typeStack.Push("bool");

                        code += "ldc.i4.1\n";
                        code += "xor\n";
                    }
                break;

                case 14:
                    {
                        string type = typeStack.Pop();

                        if (type == "int64")
                            code += "conv.i8\n";

                        code += $"call void [mscorlib]System.Console::Write({ type })\n";
                    }
                break;

                case 15:
                    {
                        code += ".assembly extern mscorlib { }\n"      +
                                ".assembly _object_code{ }\n"          +
                                ".module _object_code.exe\n\n"         +
                                ".class public _UNIC{ \n"              +
                                ".method static public void _main() {\n" +
                                ".entrypoint\n";
                    }
                break;

                case 16:
                    {
                        code += "ret\n" +
                                "}\n" +
                                "}\n";
                    }
                break;

                case 17: code += $"ldstr \"\"\ncall void [mscorlib]System.Console::WriteLine(string)\n"; break;

                case 18:
                    {
                        string type1 = typeStack.Pop();
                        string type2 = typeStack.Pop();

                        if (type1 == "bool" && type2 == "bool")
                            typeStack.Push("bool");

                        code += "and";
                    }
                break;

                case 19:
                    {
                        string type1 = typeStack.Pop();
                        string type2 = typeStack.Pop();

                        if (type1 == "bool" && type2 == "bool")
                            typeStack.Push("bool");

                        code += "or";
                    }
                break;

                case 20:
                    {
                        string type1 = typeStack.Pop();
                        string type2 = typeStack.Pop();

                        if (type1 == type2 && (type1 == "int64" || type1 == "float64"))
                            typeStack.Push(type1);

                        code += "rem";
                    }
                break;

                case 21:
                case 22:
                    {
                        typeStack.Push("string");

                        code += $"ldstr {token.getLexeme()}\n";
                    }
                break;

                case 24:
                    {
                        string label = LABEL + labelCounter++;

                        code += "brfalse " + label;

                        labelStack.Push(label);
                    }
                break;

                case 25:
                    {
                        string label2 = LABEL + labelCounter++;

                        code += "br " + label2;

                        string label1 = labelStack.Pop();

                        code += label1 + ":";

                        labelStack.Push(label2);
                    }
                break;

                case 26:
                    {
                        string label = labelStack.Pop();

                        code += label + ":";
                    }
                break;

                case 27:
                    {
                        string label = LABEL + labelCounter++;

                        code += label + ":";

                        labelStack.Push(label);
                    }
                break;

                case 28:
                    {
                        string label = labelStack.Pop();

                        code += "brtrue " + label;
                    }
                break;

                case 30:
                    {
                        switch (token.getLexeme())
                        {
                            case "int"    : varType = "int64"  ; break;
                            case "float"  : varType = "float64"; break;
                            case "char"   : varType = "char"   ; break;
                            case "string" : varType = "string" ; break;
                            case "boolean": varType = "bool"   ; break;
                        }
                    }
                break;

                case 31:
                    {
                        foreach (var id in ids)
                        {
                            symbolTable[id] = varType;

                            code += $".locals({varType} {id})\n";
                        }

                        ids.Clear();
                    }
                    break;

                case 32: ids.Add(token.getLexeme()); break;

                case 33:
                    {
                        string id = token.getLexeme();

                        string idType = symbolTable[id];

                        typeStack.Push(idType);

                        code += $"ldloc {id} { (idType == "int64" ? "\nconv.r8" : "") }\n";
                    }
                break;

                case 34:
                    {
                        string id = ids[ids.Count - 1];
                        ids.RemoveAt(ids.Count - 1);

                        string idType = symbolTable[id];
                        typeStack.Pop();

                        if (idType == "int64")
                            code += "conv.i8\n";

                        code += $"stloc {id}\n";
                    }
                break;

                case 35:
                    {
                        foreach (var id in ids)
                        {
                            string idType = symbolTable[id];

                            switch (idType)
                            {
                                case "int64"  : parseClass = "Int64" ; break;
                                case "float64": parseClass = "Double"; break;
                            }

                            code += "call string[mscorlib]System.Console::ReadLine()\n";
                            code += $"call {idType} [mscorlib]System.{ parseClass }::Parse(string)\n";
                            code += $"stloc {id}\n";
                        }

                        ids.Clear();
                    }
                break;
            }
        }
    }
}