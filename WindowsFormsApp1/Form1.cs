using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem aboutMenuItem;
        private Button browseButton;
        private TextBox pathTextBox;
        private ListBox foldersListBox;
        private DataGridView filesDataGridView;
        private Button processFilesButton;
        private FolderBrowserDialog folderBrowserDialog;

        public Form1()
        {
            InitializeComponent();
            InitializeComponents();
            InitializeForm();
        }

        public void InitializeComponents()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            int width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.8);
            int height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.8);
            this.Size = new System.Drawing.Size(width, height);


            this.menuStrip = menuStrip2;
            aboutMenuItem = toolStripMenuItem1;
            aboutMenuItem.Click += AboutMenuItem_Click;
            menuStrip.Items.Add(aboutMenuItem);
            Controls.Add(menuStrip);

            browseButton = button2;
            browseButton.Click += BrowseButton_Click;

            pathTextBox = textBox1;

            foldersListBox = listBox1;
            foldersListBox.DoubleClick += FoldersListBox_DoubleClick;

            filesDataGridView = dataGridView1;
            filesDataGridView.Columns.Add("FileName", "Name");
            filesDataGridView.Columns.Add("LastModified", "Last Modified");
            filesDataGridView.Columns.Add("FileSize", "Size (bytes)");
            filesDataGridView.DoubleClick += FilesDataGridView_DoubleClick;

            processFilesButton = button3;
            processFilesButton.Click += ProcessFilesButton_Click;


            folderBrowserDialog = new FolderBrowserDialog();
        }

        private void FilesDataGridView_DoubleClick(object sender, EventArgs e)
        {
            string fileName = filesDataGridView.CurrentRow.Cells["FileName"].Value.ToString();
            if (MessageBox.Show($"Do you want to duplicate {fileName}?", "Duplicate File", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sourcePath = Path.Combine(pathTextBox.Text, fileName);
                string destPath = Path.Combine(pathTextBox.Text, "Copy of " + fileName);
                File.Copy(sourcePath, destPath, overwrite: true);
                UpdateFileList(pathTextBox.Text);
            }
        }

        private void ProcessFilesButton_Click(object sender, EventArgs e)
        {
            filesDataGridView.Columns.Add("Delay", "Delay (seconds)");
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(pathTextBox.Text);
                FileInfo[] files = dirInfo.GetFiles();
                int fileCount = files.Length;
                Random random = new Random();

                foreach (FileInfo file in files)
                {
                    int delay = random.Next(1, fileCount);
                    Task.Delay(delay * 1000).ContinueWith(t =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            int rowIndex = filesDataGridView.Rows.Cast<DataGridViewRow>()
                                .First(row => row.Cells["FileName"].Value.ToString() == file.Name)
                                .Index;
                            filesDataGridView.Rows[rowIndex].Cells["Delay"].Value = delay;
                        }));
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FoldersListBox_DoubleClick(object sender, EventArgs e)
        {
            string selectedFolder = foldersListBox.SelectedItem.ToString();
            string fullPath = Path.Combine(pathTextBox.Text, selectedFolder);
            Info info1 = new Info();
            info1.textBox1.Text = selectedFolder;
            info1.textBox2.Text = Directory.GetLastWriteTime(fullPath);
            info1.Show();
            //MessageBox.Show($"Folder: {selectedFolder}\nLast Modified: {Directory.GetLastWriteTime(fullPath)}");
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                pathTextBox.Text = folderBrowserDialog.SelectedPath;
                UpdateFolderList(folderBrowserDialog.SelectedPath);
                UpdateFileList(folderBrowserDialog.SelectedPath);
                processFilesButton.Visible = true;
            }
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developer: Ruslan Gashikulin");
        }

        public void InitializeForm() {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void UpdateFolderList(string path)
        {
            foldersListBox.Items.Clear();
            var directories = Directory.GetDirectories(path);
            foreach (var dir in directories)
            {
                foldersListBox.Items.Add(Path.GetFileName(dir));
            }
        }

        private void UpdateFileList(string path)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                FileInfo[] files = dirInfo.GetFiles();

                filesDataGridView.Rows.Clear();
                foreach (FileInfo file in files)
                {
                    int index = filesDataGridView.Rows.Add();
                    filesDataGridView.Rows[index].Cells["FileName"].Value = file.Name;
                    filesDataGridView.Rows[index].Cells["LastModified"].Value = file.LastWriteTime;
                    filesDataGridView.Rows[index].Cells["FileSize"].Value = file.Length;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
