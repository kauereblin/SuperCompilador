using System;

namespace SuperCompilador
{
    public class Semantico : Constants
    {
        public void executeAction(int action, Token token)
        {
            Console.WriteLine("A��o #" + action + ", Token: " + token);
        }	
    }
}