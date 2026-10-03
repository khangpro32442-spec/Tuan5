using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace t7
{
    public partial class Form1 : Form
    {
        private string currentFilePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            foreach (FontFamily font in installedFonts.Families)
            {
                cmbFonts.Items.Add(font.Name);
            }

            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int size in sizes)
            {
                cmbSizes.Items.Add(size);
            }

            cmbFonts.SelectedItem = "Tahoma";
            cmbSizes.SelectedItem = 14;
            richText.Font = new Font("Tahoma", 14, FontStyle.Regular);

            UpdateWordCount();
        }

        private void btnTaoMoi_Click(object sender, EventArgs e)
        {
            richText.Clear();
            cmbFonts.SelectedItem = "Tahoma";
            cmbSizes.SelectedItem = 14;
            richText.Font = new Font("Tahoma", 14, FontStyle.Regular);
            currentFilePath = "";
        }

        private void btnMoTapTin_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openDlg.FileName;
                if (Path.GetExtension(currentFilePath).ToLower() == ".rtf")
                {
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.RichText);
                }
                else
                {
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.Filter = "Rich Text Format (*.rtf)|*.rtf";
                saveDlg.DefaultExt = "rtf";

                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveDlg.FileName;
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                    MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDinhDang_Click(object sender, EventArgs e)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.ShowColor = true;
            fontDlg.ShowApply = true;
            fontDlg.ShowEffects = true;
            fontDlg.ShowHelp = true;

            if (fontDlg.ShowDialog() != DialogResult.Cancel)
            {
                richText.ForeColor = fontDlg.Color;
                richText.Font = fontDlg.Font;
            }
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Bold);
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Italic);
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Underline);
        }

        private void ToggleFontStyle(FontStyle style)
        {
            if (richText.SelectionFont != null)
            {
                Font currentFont = richText.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ style;
                richText.SelectionFont = new Font(currentFont, newStyle);
            }
        }

        private void cmbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richText.SelectionFont != null && cmbFonts.SelectedItem != null)
            {
                string fontName = cmbFonts.SelectedItem.ToString();
                richText.SelectionFont = new Font(fontName, richText.SelectionFont.Size, richText.SelectionFont.Style);
            }
        }

        private void cmbSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richText.SelectionFont != null && cmbSizes.SelectedItem != null)
            {
                float fontSize = float.Parse(cmbSizes.SelectedItem.ToString());
                richText.SelectionFont = new Font(richText.SelectionFont.FontFamily, fontSize, richText.SelectionFont.Style);
            }
        }

        private void richText_TextChanged(object sender, EventArgs e)
        {
            UpdateWordCount();
        }

        private void UpdateWordCount()
        {
            string text = richText.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                lblWordCount.Text = "Tổng số từ: 0";
            }
            else
            {
                string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                lblWordCount.Text = $"Tổng số từ: {words.Length}";
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}