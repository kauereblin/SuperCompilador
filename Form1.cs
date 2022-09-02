﻿using System;
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
            messageTextBox.Text = "compilação de programas ainda não foi implementada";
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
