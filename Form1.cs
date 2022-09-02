using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            editor.Text = "";
            messageTextBox.Text = "";
            fileStripStatusLabel.Text = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile.Filter = "Text Files | *.txt";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                editor.LoadFile(openFile.FileName, RichTextBoxStreamType.PlainText);
                messageTextBox.Text = "";
                fileStripStatusLabel.Text = openFile.FileName;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                editor.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
                messageTextBox.Text = "";
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
            messageTextBox.Text = "compilação de programas ainda não foi implementada";
        }

        private void helpStripMenuItem_Click(object sender, EventArgs e)
        {
            messageTextBox.Text = "Super Grupo: Arthur Bezerra Pinotti, Kaue Reblin, Luiz Gustavo Klitzke";
        }

        private void editor_TextChanged(object sender, EventArgs e)
        {
            updateLineNumbers();
        }

        private void editor_VScroll(object sender, EventArgs e)
        {
            int startY = editor.GetPositionFromCharIndex(0).Y % (editor.Font.Height + 1);
            lineNumbers.Location = new Point(0, startY);

            updateLineNumbers();
        }

        private void editor_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            splitContainerEditor.SplitterDistance = (25 * (int)editor.ZoomFactor);
            lineNumbers.Width = (25 * (int)editor.ZoomFactor);
            lineNumbers.ZoomFactor = editor.ZoomFactor;

            updateLineNumbers();
        }
    }
}
