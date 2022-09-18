using System.Windows.Forms;

namespace SuperCompilador
{
    public class Constants : ScannerConstants
    {
        public enum EIdentifiers
        { 
            EPSILON         = 0,
            DOLLAR          = 1,
            t_identificador = 2,
            t_tpInt         = 3,
            t_tpFloat       = 4,
            t_tpChar        = 5,
            t_tpString      = 6,
            t_comentLinha   = 7,
            t_comentBloco   = 8,
            t_int           = 9,
            t_float         = 10,
            t_char          = 11,
            t_string        = 12,
            t_boolean       = 13,
            t_break         = 14,
            t_end           = 15,
            t_if            = 16,
            t_then          = 17,
            t_else          = 18,
            t_while         = 19,
            t_do            = 20,
            t_readln        = 21,
            t_false         = 22,
            t_true          = 23,
            t_fun           = 24,
            t_main          = 25,
            t_print         = 26,
            t_println       = 27,
            t_val           = 28,
            t_var           = 29,
            t_TOKEN_30      = 30, //":"
            t_TOKEN_31      = 31, //","
            t_TOKEN_32      = 32, //";"
            t_TOKEN_33      = 33, //"="
            t_TOKEN_34      = 34, //"?"
            t_TOKEN_35      = 35, //"+"
            t_TOKEN_36      = 36, //"-"
            t_TOKEN_37      = 37, //"*"
            t_TOKEN_38      = 38, //"/"
            t_TOKEN_39      = 39, //"{"
            t_TOKEN_40      = 40, //"}"
            t_TOKEN_41      = 41, //"%"
            t_TOKEN_42      = 42, //"("
            t_TOKEN_43      = 43, //")"
            t_TOKEN_44      = 44, //">"
            t_TOKEN_45      = 45, //"<"
            t_TOKEN_46      = 46, //"=="
            t_TOKEN_47      = 47, //"!="
            t_TOKEN_48      = 48, //"&&"
            t_TOKEN_49      = 49, //"||"
            t_TOKEN_50      = 50, //"!"
            t_TOKEN_51      = 51, //"++"
            t_TOKEN_52      = 52, //"--"
        }

        public static string getClass(EIdentifiers eId)
        {
            if (eId == EIdentifiers.t_identificador)
                return "identificador";
            else if (eId >= EIdentifiers.t_tpInt && eId <= EIdentifiers.t_tpString)
            {
                switch (eId)
                {
                    case EIdentifiers.t_tpInt   : return "constante int";
                    case EIdentifiers.t_tpFloat : return "constante float";
                    case EIdentifiers.t_tpChar  : return "constante char";
                    case EIdentifiers.t_tpString: return "constante string";
                }
            }
            else if (eId >= EIdentifiers.t_boolean && eId <= EIdentifiers.t_var || eId >= EIdentifiers.t_int && eId <= EIdentifiers.t_string)
                return "palavra reservada";
            else if (eId >= EIdentifiers.t_TOKEN_30)
                return "símbolo especial";
            else
                return "símbolo especial";

            return "TOKEN INVÁLIDO";
        }
    }
}