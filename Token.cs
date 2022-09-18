namespace SuperCompilador
{
    public class Token
    {
        private int id;
        private string lexeme;
        private int position;

        public Token(int id, string lexeme, int position)
        {
            this.id = id;
            this.lexeme = lexeme;
            this.position = position;
        }

        public int getId()
        {
            return id;
        }

        public string getLexeme()
        {
            return lexeme;
        }

        public int getPosition()
        {
            return position;
        }

        public override string ToString()
        {
            return id+" ( "+lexeme+" ) @ "+position;
        }
    }
}