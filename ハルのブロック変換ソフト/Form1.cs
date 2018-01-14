using SilverNBTLibrary;
using SilverNBTLibrary.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ハルのブロック変換ソフト
{
    public partial class Form1 : Form
    {
        private Util.Convert convert = new Util.Convert();
        private string[] fileNames;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            convert.pgb = progressBar1;
            convert.form = this;
            convert.bw = backgroundWorker1;
            backgroundWorker1.WorkerReportsProgress = true;
            richTextBox1.AllowDrop = true;
            this.richTextBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.richTextBox1_DragEnter);
            this.richTextBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.richTextBox1_DragDrop);
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {

            //コントロール内にドロップされたとき実行される
            //ドロップされたすべてのファイル名を取得する
            fileNames =
                (string[])e.Data.GetData(DataFormats.FileDrop, false);
            backgroundWorker1.RunWorkerAsync();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            //コントロール内にドラッグされたとき実行される
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                //ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
                e.Effect = DragDropEffects.Copy;
            else
                //ファイル以外は受け付けない
                e.Effect = DragDropEffects.None;
        }

        private void richTextBox1_DragDrop(object sender,System.Windows.Forms.DragEventArgs e)
        {
            //コントロール内にドロップされたとき実行される
            //ドロップされたすべてのファイル名を取得する
            fileNames =
                (string[])e.Data.GetData(DataFormats.FileDrop, false);
            backgroundWorker1.RunWorkerAsync();
        }

        private void richTextBox1_DragEnter(object sender,System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach(var file in fileNames)
            {
                switch(Path.GetExtension(file))
                {
                    case ".schematic":
                        LogAppend(file + "を変換します...");
                        convert.ToStructure(file);
                        LogAppend(file + "を変換しました");
                        break;
                    case ".nbt":
                        LogAppend(file + "を変換します...");
                        convert.ToSchematic(file);
                        LogAppend(file + "を変換しました");
                        break;
                    default:
                        LogAppend(file + "の処理をスキップしました");
                        break;
                }
            }
        }

        private void LogAppend(string log)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                richTextBox1.AppendText(log + "\n");
            });
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value++;
        }
    }
}
