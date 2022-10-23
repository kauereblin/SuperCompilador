
namespace SuperCompilador
{
    public class SyntaticError : AnalysisError
    {
        public string lexeme { get; set; }

        public SyntaticError(string msg, int position) : base(msg, position) {}

        public SyntaticError(string msg) : base(msg){}

        public SyntaticError(string msg, int position, string lexeme) : base(msg, position) { this.lexeme = lexeme; }
    }
}