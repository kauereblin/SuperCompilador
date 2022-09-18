using System;

namespace SuperCompilador
{
    public static class Util
    {
        public static int getLineNumber(string text, int position)
        {
            int lines = 1;

            for (int idxChar = 0; idxChar < position; idxChar++)
            {
                if (text[idxChar] == '\n')
                    lines++;
            }

            return lines;
        }
    }
}
