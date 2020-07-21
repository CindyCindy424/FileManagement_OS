using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace file_system
{
    //FCB， 文件控制块建立

    //可序列化，以实现数据持久性
    [Serializable]
    public class FCB
    {
        public enum FileType { folder, txt };//系统中涉及两种新建元素类型，文件&文件夹

        public string fileName;
        public int filePointer = -1;
        public FileType fileType;
        public int intFather = -1, intSon = -1, intNext = -1, intPre = -1;//用于txt读入转换
        public FCB father = null, son = null, next = null, pre = null;
        public static int file_counter = 0;

       

        public FCB() { }

        public FCB(FileType filetype) //初始化文件夹
        {
            this.fileType = filetype;
            this.filePointer = file_counter++;//先计算后++，所以根节点的filePointer为0
        }

       

        public FCB(string fileName,FileType fileType)
        {
            this.fileName = fileName;
            this.fileType = fileType;
            this.filePointer = file_counter++;
        }

        public void addSonItem(FCB newItem)
        {
            

             if(this.son == null)
            {
                this.son = newItem;
                newItem.father = this;
            }
            else
            {
                FCB temp = this.son;
                while(temp.next != null)
                {
                    temp = temp.next;
                }
                temp.next = newItem;
                newItem.pre = temp;
            }
             
           
        }

        public void remove()
        {
            if(father != null)
            {
                father.son = next;
            }
            else if (pre != null)
            {
                pre.next = next;
            }
        }

        /*———————————用于txt文件读写的函数—————————————*/
        public int getFather(FCB item) //返回父亲的编号（用于txt文件读写）
        {
            int father;
            if (item.father != null)
            {
                father = item.father.filePointer;
            }
            else
                return -1;
            return father;
        }

        public int getSon(FCB item) //返回son的编号（用于txt文件读写）
        {
            int son;
            if (item.son != null)
            {
                son = item.son.filePointer;
            }
            else
                return -1;
            return son;
        }

        public int getNext(FCB item) //返回next的编号（用于txt文件读写）
        {
            int next;
            if (item.next!=null)
            {
                next = item.next.filePointer;
            }
            else
                return -1;
            return next;
        }

        public int getPre(FCB item) //返回pre的编号（用于txt文件读写）
        {
            int pre;
            if (item.pre!=null)
            {
                pre = item.pre.filePointer;
            }
            else
                return -1;
            return pre;
        }


        public void WStore(StreamWriter sw)
        {
            sw.WriteLine(fileName);
            sw.WriteLine(filePointer);
            sw.WriteLine(fileType);
            sw.WriteLine(getFather(this));
            sw.WriteLine(getSon(this));
            sw.WriteLine(getNext(this));
            sw.WriteLine(getPre(this));
            sw.WriteLine(file_counter);
        }

        public void RStore(StreamReader sr)
        {
            fileName = sr.ReadLine();
            filePointer =Convert.ToInt32( sr.ReadLine());
            // fileType =(FileType) Convert.ToInt32(sr.ReadLine());
            string readFileType = sr.ReadLine();
            if (readFileType == "txt")
                fileType = FileType.txt;
            else if(readFileType == "folder")
                fileType = FileType.folder;
            intFather = Convert.ToInt32(sr.ReadLine());
            intSon = Convert.ToInt32(sr.ReadLine());
            intNext = Convert.ToInt32(sr.ReadLine());
            intPre = Convert.ToInt32(sr.ReadLine());
            file_counter = Convert.ToInt32(sr.ReadLine());
        }


        
        public void RStore_Modify()    //完成Map映射后，将FCB的亲戚指针完善
        {
            int count = Directory.fcb_table.Count();
            for(int i=1; i <= count;i++)
            {
                if (Directory.fcb_table[i].intFather != -1&& Directory.fcb_table[i].intFather !=0)
                    Directory.fcb_table[i].father = Directory.fcb_table[Directory.fcb_table[i].intFather];
                if (Directory.fcb_table[i].intSon != -1)
                    Directory.fcb_table[i].son = Directory.fcb_table[Directory.fcb_table[i].intSon];
                if (Directory.fcb_table[i].intNext != -1)
                    Directory.fcb_table[i].next = Directory.fcb_table[Directory.fcb_table[i].intNext];
                if (Directory.fcb_table[i].intPre != -1)
                    Directory.fcb_table[i].pre = Directory.fcb_table[Directory.fcb_table[i].intPre];
            }
        }

    }
}
