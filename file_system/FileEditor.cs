using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace file_system
{
    public class DelegateMethod
    {
        public delegate void delegateFunction();
    }

    public partial class FileEditor : Form
    {
        private File textFile;
        private My_BitMap bitMap = MainWindows.Overall_bitMap;
        public DelegateMethod.delegateFunction CallBack;

        public FileEditor()
        {
            InitializeComponent();
        }

        public FileEditor(File file)
        {
            InitializeComponent();
            textFile = file;
            showContent();
        }

        private void showContent()
        {
            //List<int> indexs = textFile.readTable();
            int indexs = textFile.readTable();
            string content = "";
            //foreach(int i in indexs)
            //{
            if(indexs != -1)
                content += bitMap.getBlock(indexs);
           // }
            textEditBox.Text = content;
        }

        private void FileEditor_Closing(object sender,EventArgs e)
        {
            if (MessageBox.Show("Save the changes?", "Tips",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                writeDisk();
                callBack();
            }
        }

        //关闭文件后 将文件内容进行保存
        private void writeDisk()
        {
            string content = textEditBox.Text;
            textFile.size = (content.Length * 4).ToString()+"B";
            //releaseBlock();
            textFile.indexTable = bitMap.write(content);
        }

        private void callBack()
        {
            if (CallBack != null)
                this.CallBack();
        }

        private void releaseBlock()
        {
            //List<int> indexs = textFile.readTable();
            int indexs = textFile.readTable();
            bitMap.withdraw(indexs);
        }

        private void FileEditor_Load(object sender, EventArgs e)
        {


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
