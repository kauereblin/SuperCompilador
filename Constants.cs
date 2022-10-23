using System.Windows.Forms;

namespace SuperCompilador
{
    public class Constants : ScannerConstants
    {
        public enum ETokens
        { 
            EPSILON         = 0,
            DOLLAR          = 1,
            t_identificador = 2,
            t_tpInt         = 3,
            t_tpFloat       = 4,
            t_tpChar        = 5,
            t_tpString      = 6,
            t_int           = 7,
            t_float         = 8,
            t_char          = 9,
            t_string        = 10,
            t_boolean       = 11,
            t_break         = 12,
            t_end           = 13,
            t_if            = 14,
            t_then          = 15,
            t_else          = 16,
            t_while         = 17,
            t_do            = 18,
            t_readln        = 19,
            t_false         = 20,
            t_true          = 21,
            t_fun           = 22,
            t_main          = 23,
            t_print         = 24,
            t_println       = 25,
            t_val           = 26,
            t_var           = 27,
            t_TOKEN_28      = 28 ,//":"
            t_TOKEN_29      = 29 ,//","
            t_TOKEN_30      = 30 ,//";"
            t_TOKEN_31      = 31 ,//"="
            t_TOKEN_32      = 32 ,//"?"
            t_TOKEN_33      = 33 ,//"+"
            t_TOKEN_34      = 34 ,//"-"
            t_TOKEN_35      = 35 ,//"*"
            t_TOKEN_36      = 36 ,//"/"
            t_TOKEN_37      = 37 ,//"{"
            t_TOKEN_38      = 38 ,//"}"
            t_TOKEN_39      = 39 ,//"%"
            t_TOKEN_40      = 40 ,//"("
            t_TOKEN_41      = 41 ,//")"
            t_TOKEN_42      = 42 ,//">"
            t_TOKEN_43      = 43 ,//"<"
            t_TOKEN_44      = 44 ,//"=="
            t_TOKEN_45      = 45 ,//"!="
            t_TOKEN_46      = 46 ,//"&&"
            t_TOKEN_47      = 47 ,//"||"
            t_TOKEN_48      = 48 ,//"!"
        }

        public static string getClass(ETokens eId)
        {
            if (eId == ETokens.t_identificador)
                return "identificador";
            else if (eId >= ETokens.t_tpInt && eId <= ETokens.t_tpString)
            {
                switch (eId)
                {
                    case ETokens.t_tpInt   : return "constante int";
                    case ETokens.t_tpFloat : return "constante float";
                    case ETokens.t_tpChar  : return "constante char";
                    case ETokens.t_tpString: return "constante string";
                }
            }
            else if (eId >= ETokens.t_int && eId <= ETokens.t_var)
                return "palavra reservada";
            else if ((eId >= ETokens.EPSILON && eId <= ETokens.DOLLAR) || eId >= ETokens.t_TOKEN_28)
                return "símbolo especial";

            return "TOKEN INVÁLIDO";
        }
    }
}