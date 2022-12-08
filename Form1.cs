using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            var lexico    = new Lexico();
            var sintatico = new Sintatico();
            var semantico = new Semantico();

            semantico.objFile = fileStripStatusLabel.Text;

            lexico.setInput(editor.Text);
            bool noError = true;

            try
            {
                sintatico.parse(lexico, semantico);
            }
            catch (LexicalError err)
            {
                noError = false;

                int line = Util.getLineNumber(editor.Text, err.getPosition());
                string invalidSymbol = "";

                if (err.Message == ScannerConstants.SCANNER_ERROR[0])
                    invalidSymbol = char.ToString(editor.Text[err.getPosition()]);

                messageTextBox.Text = $"Erro na linha {line} - {invalidSymbol} {err.Message}";
                return;
            }
            catch (SyntaticError err)
            {
                noError = false;

                int line = Util.getLineNumber(editor.Text, err.getPosition());

                messageTextBox.Text = $"Erro na linha {line} - encontrado {err.lexeme} {err.Message}";
                return;
            }
            catch (SemanticError err)
            {
                noError = false;

                int line = Util.getLineNumber(editor.Text, err.getPosition());
                string invalidSymbol = "";

                if (err.Message == ScannerConstants.SCANNER_ERROR[0])
                    invalidSymbol = char.ToString(editor.Text[err.getPosition()]);

                messageTextBox.Text = $"Erro na linha {line} - {invalidSymbol} {err.Message}";
                return;
            }

            if (noError)
                messageTextBox.Text = "programa compilado com sucesso";

            var enviromentPath = System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);

            Console.WriteLine(enviromentPath);
            var paths = enviromentPath.Split(';');
            var exePath = paths.Select(x => Path.Combine(x, "ilasm.exe"))
                               .Where(x => File.Exists(x))
                               .FirstOrDefault();

            var p = new Process
            {
                StartInfo =
                 {
                     FileName = exePath,
                     WorkingDirectory = $"{sintatico.fileIL.Substring(0, sintatico.fileIL.LastIndexOf("\\"))}",
                     Arguments = $"/exe {sintatico.fileIL}"
                 }
            }.Start();

            string exe = sintatico.fileIL;
            exe = exe.Substring(0, exe.IndexOf(".")) + ".exe";

            var p2 = new Process
            {
                StartInfo = { FileName = exe, }
            }.Start();
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
