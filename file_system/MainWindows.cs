using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;

namespace file_system
{
    public partial class MainWindows : Form
    {
        //初始化基础数据结构
        FCB root_fcb = new FCB(0);  //初始化根目录
        public FCB current_directory;
        Stack<FCB> folderStack = new Stack<FCB>(); //文件夹栈
        Directory directory = new Directory();  //新建目录
        public static My_BitMap Overall_bitMap = new My_BitMap();  //新建BitMap
        public string dir = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        private Dictionary<int, ListViewItem> list_table = new Dictionary<int, ListViewItem>();
        TreeNode root_node;
        bool Format = false;//是否发生格式化


        public MainWindows()
        {
            InitializeComponent();
            current_directory = root_fcb;
            folderStack.Push(current_directory);//根节点加入当前文件夹栈
            InitializeWindows();
            //加上读入记录
            loadFormerData();
        }

        public void loadFormerData()
        {
            readStore();//程序启动时将txt里的数据加载回文件系统
            directory.map();
            int countNum = Directory.fcb_table.Count();
            for (int h = 1; h <= countNum; h++)
            {
                Directory.fcb_table[h].RStore_Modify();
                if (Directory.fcb_table[h].filePointer == 1)
                {
                    Directory.fcb_table[h].intFather = 0;
                    Directory.fcb_table[h].father = root_fcb;
                    root_fcb.son = Directory.fcb_table[h];
                }
            }
                UpdateView();
        }



       // readStore();//程序启动时将txt里的数据加载回文件系统
      //  treeView1.ExpandAll();


        
        public void InitializeWindows()
        {
            InitializeListView();
            InitializeTreeView();
            textBox1.Text = "root\\";
        }


        public void InitializeListView()
        {
            listView1.Items.Clear();
        }

        public void InitializeTreeView()
        {
            treeView1.Nodes.Clear();//清除当前所有结点
            root_node = new TreeNode("root");//加入根目录结点
            treeView1.Nodes.Add(root_node);
        }

        public void UpdateView()
        {
            
               

            UpdateTreeView();
            UpdateListView(current_directory);
        }

        public void UpdateTreeView()
        {
            treeView1.Nodes.Clear();
            root_node = new TreeNode("root");
            nodeDFS(root_node,root_fcb);//维护root_node实例（treenode类）
            treeView1.Nodes.Add(root_node);//把实例付给窗口

        }

        public void UpdateListView(FCB item)
        {
            list_table = new Dictionary<int, ListViewItem>();
            listView1.Items.Clear();
            if(item.son != null)
            {
                FCB son = item.son;
                do
                {
                    File temp = directory.getFile(son);
                    ListViewItem file = new ListViewItem(new string[]
                    {
                        temp.name,
                        temp.size,
                        temp.type,
                        temp.createTime.ToString()
                });
                    if (temp.type == "folder")
                        file.ImageIndex = 0;
                    else
                        file.ImageIndex = 1;

                    listMap(temp, file);
                    listView1.Items.Add(file);
                    son = son.next;
                } while (son != null);
            }
        }

        private void nodeDFS(TreeNode node,FCB dir)//维护目录
        {
            if(dir.son != null)
            {
                FCB son = dir.son;
                do
                {
                    if (son.fileType == FCB.FileType.folder)
                    {
                        TreeNode new_node = new TreeNode(son.fileName);
                        nodeDFS(new_node, son);
                        node.Nodes.Add(new_node);
                    }
                    son = son.next;
                } while(son != null);
            }
        }

        private string nameCheck(string s,string ext = "")
        {
            FCB current_dir = current_directory.son;
            int counter = 0;
            while(current_dir != null)
            {
                string[] sArray = current_dir.fileName.Split('(');
                if (sArray[0] != current_dir.fileName && sArray[0] == s) 
                {
                    counter++;
                }
                else if (current_dir.fileName == s + ext)
                {
                    counter++;
                }
                current_dir = current_dir.next;
            }
            if(counter > 0)
                s += "(" + counter.ToString() + ")";
            return s + ext;
        }

        /*——————————————————————————————————————————*/
        //将文件系统数据存储到txt文件

        void writeStore()  
        {
            /* FileStream BlockStore = new FileStream("txtRecord\\Block.txt", FileMode.Create, FileAccess.Write);   //将block数据写到txt文件
             StreamWriter bw = new StreamWriter(BlockStore);
             //for (int i = 100; i < BitMap.Capcity; i++)
                 for (int i = 0; i < My_BitMap.Capcity; i++)
             {
                 // if(My_BitMap.blocks[i] == null)
                 //   break;
                 if (My_BitMap.my_inside_bitMap[i] == true)//这一块没有数据，为空
                 {
                     //没有操作
                 }
                 else//该块有数据
                 {
                     //for (int j = 0; j < 16; j++)//该块有数据
                     //{
                         //if (My_BitMap.blocks[i].info[j] == 0)
                          //  bw.Write("|");
                        // else
                         //{
                             bw.Write(My_BitMap.blocks[i]);
                             Console.WriteLine("写入block，info：" + i + "内容：" + My_BitMap.blocks[i]);
                        // }

                     //}
                 }

             }
             bw.Flush();
             bw.Close();
             BlockStore.Close();*/
            //for (int i = 0; i < My_BitMap.Capcity; i++)
            //{

            FileStream BlockStore = new FileStream("txtRecord\\Block.txt", FileMode.Create, FileAccess.Write);   //将block数据写到txt文件
            StreamWriter bw = new StreamWriter(BlockStore);
            for (int i=0;i<My_BitMap.Capcity;i++)
            {
                if (My_BitMap.my_inside_bitMap[i] == false)//这一块没有数据，为空
                {
                    bw.WriteLine('N');
                }
                else
                {
                    bw.WriteLine(My_BitMap.blocks[i].info);
                    Console.WriteLine("写入block，info：" + i + "内容：" + My_BitMap.blocks[i]);
                }
            }
            bw.Flush();
            bw.Close();
            BlockStore.Close();


            FileStream BitMapStore = new FileStream("txtRecord\\BitMap.txt", FileMode.Create, FileAccess.Write);   //将bitmap数据写到txt文件
            StreamWriter mw = new StreamWriter(BitMapStore);
            for (int k = 0; k < My_BitMap.Capcity; k++)
            {
                mw.WriteLine(My_BitMap.my_inside_bitMap[k]);
                //Console.WriteLine("改bitmap第" + i);
            }
            mw.Flush();
            mw.Close();
            BitMapStore.Close();

            FileStream FCBStore = new FileStream("txtRecord\\FCB.txt", FileMode.Create, FileAccess.Write); //将FCB数据写到txt文件
            StreamWriter fcbw = new StreamWriter(FCBStore);
            int count = Directory.fcb_table.Count();
            fcbw.WriteLine(count);
            for (int k = 1; k <= count; k++)
                Directory.fcb_table[k].WStore(fcbw);
            fcbw.Flush();
            fcbw.Close();
            FCBStore.Close();


            FileStream FileStore = new FileStream("txtRecord\\File.txt", FileMode.Create, FileAccess.Write); //File写到txt文件
            StreamWriter fw = new StreamWriter(FileStore);
            // int countFile = Directory.file_table.Count();
            for (int k = 1; k <= count; k++)
                Directory.file_table[k].WStore(fw);
            fw.Flush();
            fw.Close();
            FileStore.Close();

        }


        //将txt里的数据加载回文件系统
        void readStore()   
        {
            if (System.IO.File.Exists("txtRecord\\Block.txt") && System.IO.File.Exists("txtRecord\\BitMap.txt") && System.IO.File.Exists("txtRecord\\File.txt") && System.IO.File.Exists("txtRecord\\FCB.txt"))
            {
                FileStream BlockStore = new FileStream("txtRecord\\Block.txt", FileMode.Open);     //读取Block数据
                StreamReader br = new StreamReader(BlockStore, Encoding.Default);
                //for (int i = 100; i < BitMap.Capcity; i++)
                    for (int i = 0; i < My_BitMap.Capcity; i++)
                    {
                    string readContent = br.ReadLine();
                    if (readContent == null || Convert.ToString(readContent) == "N" )
                    {
                        //这个block没有内容
                    }
                    else
                    {
                        My_BitMap.blocks[i] = new Block();
                            My_BitMap.blocks[i].info = readContent;
                        }
                    }
                br.Close();
                BlockStore.Close();

                FileStream BitMapStore = new FileStream("txtRecord\\BitMap.txt", FileMode.Open);    //读取BitMap数据
                StreamReader mr = new StreamReader(BitMapStore);
                for (int i = 0; i < My_BitMap.Capcity; i++)
                    My_BitMap.my_inside_bitMap[i] = Convert.ToBoolean(mr.ReadLine());
                mr.Close();
                BitMapStore.Close();


                FileStream fcbStorage = new FileStream("txtRecord\\FCB.txt", FileMode.Open);   //读取FCB数据
                StreamReader fcbr = new StreamReader(fcbStorage);
                string content = fcbr.ReadLine();
                Console.WriteLine("读取FCB文件，第一行内容：" + content);
                int dcount = Convert.ToInt32(content); //FCB的数量

                for (int i = 1; i <= dcount; i++)
                {
                    FCB fcb = new FCB();
                    fcb.RStore(fcbr);
                    Directory.fcb_table[i]=fcb;
                    //directory.Add(fcb);
                }
                fcbr.Close();
                fcbStorage.Close();

                


                FileStream FileStore = new FileStream("txtRecord\\File.txt", FileMode.Open);   //读取File
                StreamReader fr = new StreamReader(FileStore);
                for (int i = 1; i <= dcount; i++)
                {
                    Directory.file_table[i] = new File();
                    Directory.file_table[i].RStore(fr);
                }
                fr.Close();
                FileStore.Close();



            }
        }
       


        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem current_item = new ListViewItem();
            if (listView1.SelectedItems.Count != 0 )
            {
               current_item = listView1.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Please select a item");
                return;
            }

            File current_file = directory.getFile(getPointer(current_item));
            FCB current_fcb = directory.getFCB(current_file);

            openClick(current_fcb, current_file);
        }

        private void openClick(FCB fcb,File file)
        {
            switch (fcb.fileType){
                case FCB.FileType.folder: //双击文件夹进入当前文件夹
                    current_directory = fcb;
                    folderStack.Push(fcb);
                    textBox1.Text = directory.getFile(current_directory).path;
                    UpdateListView(fcb);
                    break;
                case FCB.FileType.txt: //双击文件弹出FileEditor
                    FileEditor fileEditor = new FileEditor(file);
                    fileEditor.Show();
                    fileEditor.CallBack = UpdateView;
                    break;
                default:
                    break;
            }
        }

        //新建文件
        private void 文件ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string file_name = nameCheck("New text", ".txt");
            string fatherPath;
            //新建FCB
            FCB new_fcb = new FCB(file_name, FCB.FileType.txt);
            current_directory.addSonItem(new_fcb);

            //建立文件路径
            File father = directory.getFile(current_directory);
            fatherPath = (father == null) ? "root" : father.path;

            //新建文件
            File new_file = new File(new_fcb, fatherPath);
            directory.map(new_fcb, new_file);

            UpdateListView(current_directory);
        }

        //新建文件夹
        private void 文件夹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string file_name = nameCheck("New folder");
            string fatherPath;
            //Add new FCB
            FCB new_fcb = new FCB(file_name, FCB.FileType.folder);
            current_directory.addSonItem(new_fcb);

            //Add new File
            //Build new file path
            File father = directory.getFile(current_directory);
            fatherPath = (father == null) ? "root" : father.path;
            File new_file = new File(new_fcb, fatherPath);
            directory.map(new_fcb, new_file);

            UpdateView();
        }

        private void 删除ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ListViewItem current_item = new ListViewItem();
            if (listView1.SelectedItems.Count != 0)
            {
                current_item = listView1.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Please select a item");
                return;
            }

            File current_file = directory.getFile(getPointer(current_item));
            FCB current_fcb = directory.getFCB(current_file);

            //List<int> indexs = current_file.readTable();
            int indexs = current_file.readTable();
            Overall_bitMap.withdraw(indexs);

            current_fcb.remove();
            directory.removeFile(current_fcb);

            UpdateView();
        }

        private void 返回ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (current_directory == root_fcb)
            {
                MessageBox.Show("⚠ Attention：Already at root!");
                return;
            }
            while(current_directory.father == null)
                {
                    current_directory = current_directory.pre;
                }
            current_directory = current_directory.father;
            
            if(current_directory == root_fcb)
                textBox1.Text = "root\\";
            else
                textBox1.Text = directory.getFile(current_directory).path;
            UpdateListView(current_directory);
        }

        private void 格式化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            root_fcb = new FCB(0);
            directory = new Directory();
            folderStack = new Stack<FCB>();
            Overall_bitMap = new My_BitMap();
            current_directory = root_fcb;
            UpdateView();

            Console.WriteLine("执行格式化函数！");
            FileStream fs = new FileStream("txtRecord\\Block.txt", FileMode.Truncate, FileAccess.Write);
            
            //fs.Seek(0, SeekOrigin.Begin);
            //fs.SetLength(0);
            fs.Close();
            FileStream fs02 = new FileStream("txtRecord\\BitMap.txt", FileMode.Truncate, FileAccess.Write);
            //fs02.SetLength(0);
            //fs02.Seek(0, SeekOrigin.Begin);
            fs02.Close();
            FileStream fs03 = new FileStream("txtRecord\\File.txt", FileMode.Truncate, FileAccess.Write);
            //fs03.SetLength(0);
            fs03.Close();
            FileStream fs04 = new FileStream("txtRecord\\FCB.txt", FileMode.Truncate, FileAccess.Write);
            //fs04.SetLength(0);
            fs04.Close();

            Format = true;
        }

        public void listMap(File file, ListViewItem item)
        {
            list_table[file.filePointer] = item;
        }

        public ListViewItem getItem(File file)
        {
            if (list_table.ContainsKey(file.filePointer))
            {
                return list_table[file.filePointer];
            }
            else
                return null;
        }

        public int getPointer(ListViewItem item)
        {
            if (list_table.ContainsValue(item))
            {
                foreach (KeyValuePair<int, ListViewItem> kvp in list_table)
                {
                    if (kvp.Value.Equals(item))
                        return kvp.Key;
                }
                return -1;
            }
            else
            {
                MessageBox.Show("Can't get the pointer");
                return -1;
            }
        }

        private void 文件ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string file_name = nameCheck("New text", ".txt");
            string fatherPath;
            //新建FCB
            FCB new_fcb = new FCB(file_name, FCB.FileType.txt);
            current_directory.addSonItem(new_fcb);

            //建立文件路径
            File father = directory.getFile(current_directory);
            fatherPath = (father == null) ? "root" : father.path;

            //新建文件
            File new_file = new File(new_fcb, fatherPath);
            directory.map(new_fcb, new_file);

            UpdateListView(current_directory);
        }

        private void 文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file_name = nameCheck("New folder");
            string fatherPath;
            //Add new FCB
            FCB new_fcb = new FCB(file_name, FCB.FileType.folder);
            current_directory.addSonItem(new_fcb);

            //Add new File
            //Build new file path
            File father = directory.getFile(current_directory);
            fatherPath = (father == null) ? "root" : father.path;
            File new_file = new File(new_fcb, fatherPath);
            directory.map(new_fcb, new_file);

            UpdateView();
        }

        private void 返回ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (current_directory == root_fcb)
            {
                MessageBox.Show("⚠ Attention：Already at root!");
                return;
            }
            while (current_directory.father == null)
            {
                current_directory = current_directory.pre;
            }
            current_directory = current_directory.father;
            if (current_directory == root_fcb)
                textBox1.Text = "root\\";
            else
                textBox1.Text = directory.getFile(current_directory).path;
            UpdateListView(current_directory);
        }

        private void 打开ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ListViewItem current_item = new ListViewItem();
            if (listView1.SelectedItems.Count != 0)
            {
                current_item = listView1.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Please select a item");
                return;
            }

            File current_file = directory.getFile(getPointer(current_item));
            FCB current_fcb = directory.getFCB(current_file);

            openClick(current_fcb, current_file);
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ListViewItem current_item = new ListViewItem();
            if (listView1.SelectedItems.Count != 0)
            {
                current_item = listView1.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Please select a item");
                return;
            }

            File current_file = directory.getFile(getPointer(current_item));
            FCB current_fcb = directory.getFCB(current_file);

            //List<int> indexs = current_file.readTable();
            int indexs = current_file.readTable();
            Overall_bitMap.withdraw(indexs);

            current_fcb.remove();
            directory.removeFile(current_fcb);

            UpdateView();
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem current_item = new ListViewItem();
            if (listView1.SelectedItems.Count != 0)
            {
                current_item = listView1.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Please select a item");
                return;
            }

            File current_file = directory.getFile(getPointer(current_item));
            FCB current_fcb = directory.getFCB(current_file);

            openClick(current_fcb, current_file);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem current_item = new ListViewItem();
            if (listView1.SelectedItems.Count != 0)
            {
                current_item = listView1.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Please select a item");
                return;
            }

            File current_file = directory.getFile(getPointer(current_item));
            FCB current_fcb = directory.getFCB(current_file);

            InputBox inputBox = new InputBox(current_file,current_fcb);
            inputBox.Show();
            inputBox.CallBack = UpdateView;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void 重命名ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ListViewItem current_item = new ListViewItem();
            if (listView1.SelectedItems.Count != 0)
            {
                current_item = listView1.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Please select a item");
                return;
            }

            File current_file = directory.getFile(getPointer(current_item));
            FCB current_fcb = directory.getFCB(current_file);

            InputBox inputBox = new InputBox(current_file, current_fcb);
            inputBox.Show();
            inputBox.CallBack = UpdateView;
        }

        

        public void MainWindows_Closing(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save data?", "Tips", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //serialize();
                if(Format==false)
                writeStore();  //程序结束后将文件系统数据存储到txt文件
            }
        }

        private void 加载之前数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           //deserialize();
           UpdateView();
        }
    
        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
