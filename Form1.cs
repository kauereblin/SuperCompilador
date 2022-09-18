using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SuperCompilador.Constants;

namespace SuperCompilador
{
    public partial class Interface : Form
    {
        public Interface()
        {
            InitializeComponent();
        }

        private void newStripMenuItem_Click(object sender, EventArgs e)
        {
            float zfLines = lineNumbers.ZoomFactor;

            editor.Clear();
            messageTextBox.Clear();
            fileStripStatusLabel.Text = "";

            editor.ZoomFactor = 1.0F;
            editor.ZoomFactor = zfLines;
            lineNumbers.ZoomFactor = zfLines;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float zfLines = lineNumbers.ZoomFactor;

            openFile.Filter = "Text Files | *.txt";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                editor.LoadFile(openFile.FileName, RichTextBoxStreamType.PlainText);
                messageTextBox.Clear();
                fileStripStatusLabel.Text = openFile.FileName;
            }

            editor.ZoomFactor = 1.0F;
            editor.ZoomFactor = zfLines;
            lineNumbers.ZoomFactor = zfLines;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile.Filter = "Text Files | *.txt";

            if (!string.IsNullOrEmpty(fileStripStatusLabel.Text))
            {
                if (fileStripStatusLabel.Text[fileStripStatusLabel.Text.Length - 1].Equals('*'))
                    fileStripStatusLabel.Text = fileStripStatusLabel.Text.Remove(fileStripStatusLabel.Text.Length - 1);

                editor.SaveFile(fileStripStatusLabel.Text, RichTextBoxStreamType.PlainText);
                return;
            }

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                editor.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
                messageTextBox.Clear();
                fileStripStatusLabel.Text = saveFile.FileName;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Paste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Cut();
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lexico = new Lexico();
            lexico.setInput(editor.Text);
            bool noError = true;

            try
            {
                messageTextBox.Text = "linha\t\tclasse\t\t\tlexema\n";

                Token token;
                while ((token = lexico.nextToken()) != null)
                {
                    EIdentifiers eCLass = (EIdentifiers)token.getId();
                    if (eCLass == EIdentifiers.t_comentLinha || eCLass == EIdentifiers.t_comentBloco)
                        continue;

                    int    line         = Util.getLineNumber(editor.Text, token.getPosition());
                    string lexicalClass = getClass(eCLass);
                    string lexeme       = token.getLexeme();

                    messageTextBox.Text += $"{line}\t\t{lexicalClass}\t\t{lexeme}\n";
                    // só escreve o lexema, necessário escrever t.getId, t.getPosition()

                    // t.getId () - retorna o identificador da classe. Olhar Constants.java e adaptar, pois 
                    // deve ser apresentada a classe por extenso
                    // t.getPosition () - retorna a posição inicial do lexema no editor, necessário adaptar 
                    // para mostrar a linha	

                    // esse código apresenta os tokens enquanto não ocorrer erro
                    // no entanto, os tokens devem ser apresentados SÓ se não ocorrer erro, necessário adaptar 
                    // para atender o que foi solicitado
                }
            }
            catch (LexicalError err)
            {
                noError = false;

                int line = Util.getLineNumber(editor.Text, err.getPosition());
                messageTextBox.Text = $"Erro na linha {line} - {editor.Text[err.getPosition()]} {err.Message}";
                // e.getMessage() - retorna a mensagem de erro de SCANNER_ERRO (olhar ScannerConstants.java 
                // e adaptar conforme o enunciado da parte 2)
                // e.getPosition() - retorna a posição inicial do erro, tem que adaptar para mostrar a 
                // linha  
            }

            if (noError)
                messageTextBox.Text += "programa compilado com sucesso";
        }

        private void helpStripMenuItem_Click(object sender, EventArgs e)
        {
            messageTextBox.Text = "Equipe 06: Arthur Bezerra Pinotti, Kaue Reblin, Luiz Gustavo Klitzke";
        }

        private void editor_TextChanged(object sender, EventArgs e)
        {
            updateLineNumbers();

            if (!string.IsNullOrEmpty(fileStripStatusLabel.Text) && !fileStripStatusLabel.Text[fileStripStatusLabel.Text.Length - 1].Equals('*'))
                fileStripStatusLabel.Text += "*";
        }

        private void editor_VScroll(object sender, EventArgs e)
        {
            int startY = editor.GetPositionFromCharIndex(0).Y % (editor.Font.Height + 1);
            lineNumbers.Location = new Point(0, startY);

            updateLineNumbers();
        }

        private void editor_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            splitContainerEditor.SplitterDistance = (40 * (int)editor.ZoomFactor);
            lineNumbers.ZoomFactor = editor.ZoomFactor;

            updateLineNumbers();
        }
    }
}
