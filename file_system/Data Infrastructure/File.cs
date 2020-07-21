using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace file_system
{
    //文件结构建立

    //可序列化，以实现数据持久性
    [Serializable]

    public class File
    {
        public string name, size, type, path;
        public DateTime createTime;
        public int filePointer;
        //public int[] indexTable = new int[10];
        //public IndexTable indexPointer;
        public int indexTable = -1;//初始等于-1，

        public File() { }

        public File(string name,string size)
        {
            this.name = name;
            this.size = size;
            this.createTime = DateTime.Now;
            // indexPointer = new IndexTable();
            //for (int i=0;i<10;i++)
            //{
             //   indexTable[i] = -1; //空索引赋值为-1
            //}
        }

        //1个File对应1个FCB
        public File(FCB item,string fatherPath = "")
        {
            name = item.fileName;
            size = "0";
            type = item.fileType.ToString();
            path = fatherPath + '\\' + name;
            filePointer = item.filePointer;
            // indexPointer = new IndexTable();
           // for (int i = 0; i < 10; i++)
            //{
             //   indexTable[i] = -1; //空索引赋值为-1
           // }
            createTime = DateTime.Now;
        }

        /*public List<int> readTable()
        {
            List<int> content = new List<int>();
            int i = 0;
            while (indexTable[i] != -1)
            {
                content.Add(indexTable[i]);
                i++;
            }
            return content;
        }*/

        public int readTable()
        {
            return indexTable;
        }


        public void WStore(StreamWriter sw)
        {
            sw.WriteLine(name);
            sw.WriteLine(size);
            sw.WriteLine(type);
            sw.WriteLine(path);
            sw.WriteLine(createTime);
            sw.WriteLine(filePointer);
            //sw.WriteLine(indexTable);
            // for(int k=0;k<10;k++) sw.WriteLine(indexTable[k]);
            sw.WriteLine(indexTable);

        }
        
        public void RStore(StreamReader sr)
        {
            name = sr.ReadLine();
            size = sr.ReadLine();
            type = sr.ReadLine();
            path = sr.ReadLine();
            createTime =Convert.ToDateTime( sr.ReadLine());
            filePointer =Convert.ToInt32(sr.ReadLine());
            //for (int k = 0; k < 10; k++) indexTable[k] =Convert.ToInt32 ( sr.ReadLine());
            indexTable = Convert.ToInt32(sr.ReadLine());
        }

    }
}
