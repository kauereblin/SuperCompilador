using System;
using System.Collections;
using System.Collections.Generic;

namespace SuperCompilador
{
    public class Semantico : Constants
    {
        private string        code;
        private string        relational_operator;
        private Stack<string> type_stack   = new Stack<string>();
        private Stack<string> symbol_table = new Stack<string>();
        private string        var_type;
        private string[]      ids;

        public void executeAction(int action, Token token)
        {
            switch (action)
            {
                case 1: 
                    {
                        string type1 = type_stack.Pop();
                        string type2 = type_stack.Pop();

                        if (type1 == "float64" || type2 == "float64")
                            type_stack.Push("float64");
                        else
                            type_stack.Push("int64");

                        code += "add\n";
                    }
                break;

                case 2: 
                    {
                        type_stack.Pop();
                        type_stack.Pop();

                        type_stack.Push("float64");

                        code += "sub\n";
                    }
                break;

                case 3: 
                    {
                        string type1 = type_stack.Pop(); 
                        string type2 = type_stack.Pop();
                        if (type1 == "float64" || type2 == "float64")
                            type_stack.Push("float64");
                        else
                            type_stack.Push("int64");

                        code += "mul\n";
                    }
                break;

                case 4: 
                    {
                        string type1 = type_stack.Pop();
                        string type2 = type_stack.Pop();
                        if (type1 == type2)
                            type_stack.Push(type1);

                        code += "div\n";
                    }
                break;

                case 5: 
                    {
                        type_stack.Push("int64");

                        code += $"ldc.i8 {token.getLexeme()}\n";
                        code += $"conv.r8\n";
                    }
                break;

                case 6: 
                    {
                        type_stack.Push("float64");

                        code += $"ldc.r8 {token.getLexeme()}\n";
                    }
                break;

                case 7: 
                    {
                        string type = type_stack.Pop();

                        if (type == "float64" || type == "int64")
                            type_stack.Push(type);
                    }
                break;

                case 8:
                    {
                        string type = type_stack.Pop();

                        if (type == "float64" || type == "int64")
                            type_stack.Push(type);

                        code += "ldc.i8 - 1\n";
                        code += "conv.r8\n";
                        code += "mul\n";

                    }
                break;

                case 9: relational_operator = token.getLexeme(); break;

                case 10:
                    {
                        string type1 = type_stack.Pop();
                        string type2 = type_stack.Pop();

                        if (type1 == type2)
                            type_stack.Push("bool");

                        switch (relational_operator)
                        {
                            case ">" : code += "cgt\n"; break;
                            case "<" : code += "clt\n"; break;
                            case "==": code += "ceq\n"; break;
                        }
                    }
                    break;

                case 11:
                    {
                        type_stack.Push("bool");

                        code += "ldc.i4.1\n";
                    }
                    break;

                case 12:
                    {
                        type_stack.Push("bool");

                        code += "ldc.i4.0\n";
                    }
                break;

                case 13:
                    {
                        string type = type_stack.Pop();

                        if (type == "bool")
                            type_stack.Push("bool");

                        code += "ldc.i4.1\n";
                        code += "xor\n";
                    }
                break;

                case 14:
                    {
                        string type = type_stack.Pop();

                        if (type == "int64")
                            code += "conv.i8\n";

                        code += $"call void [mscorlib]System.Console::Write({ type })";
                    }
                break;

                case 15:
                    {
                        code += ".assembly extern mscorlib { }\n"           +
                                ".assembly _codigo_objeto{ }\n"             +
                                ".module _codigo_objeto.exe\n\n"            +
                                ".class public _UNICA{ \n"                  +
                                ".method static public void _principal()\n" +
                                "{ .entrypoint\n";
                    }
                break;

                case 16:
                    {
                        code += "\tret\n" +
                                "\t}\n" +
                                "}\n";
                    }
                break;

                case 17:
                    {
                        
                    }
                break;

                case 18:
                    {
                        string type1 = type_stack.Pop();
                        string type2 = type_stack.Pop();

                        if (type1 == "bool" && type2 == "bool")
                            type_stack.Push("bool");

                        code += "and";
                    }
                break;

                case 19:
                    {
                    
                    }
                break;

                case 20:
                    {
                    
                    }
                break;

                case 21:
                    {
                    
                    }
                break;

                case 22:
                    {
                    
                    }
                break;

                case 23:
                    {
                    
                    }
                break;
                case 24:
                    {
                    
                    }
                break;

                case 25:
                    {
                    
                    }
                break;

                case 26:
                    {
                    
                    }
                break;

                case 27:
                    {
                    
                    }
                break;

                case 28:
                    {
                    
                    }
                break;

                case 29:
                    {
                    
                    }
                break;

                case 30:
                    {
                        
                    }
                break;

                case 31:
                    {
                    
                    }
                break;

                case 32:
                    {
                    
                    }
                break;

                case 33:
                    {
                    
                    }
                break;

                case 34:
                    {
                    
                    }
                break;

                case 35:
                    {
                    
                    }
                break;

                case 36:
                    {
                    
                    }
                break;

                case 37:
                    {
                    
                    }
                break;

                case 38:
                    {
                    
                    }
                break;

                case 39:
                    {
                    
                    }
                break;

                case 40:
                    {
                    
                    }
                break;

            }
        }
    }
}