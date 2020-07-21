using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_system
{
    // 目录结构，存放一系列FCB信息；系统所有FCB和File的管理

    //可序列化，以实现数据持久性
    [Serializable]
    public class Directory
    {
        //用字典结构管理FCB和File
        public static Dictionary<int, File> file_table = new Dictionary<int, File>();
        public static Dictionary<int, FCB> fcb_table = new Dictionary<int, FCB>();

        //完成FCB和File的映射
        public void map(FCB item,File file) 
        {
            file_table[item.filePointer] = file;
            fcb_table[item.filePointer] = item;
        }

        public void map()//初始传入txt后的初始映射
        {
            int num = file_table.Count();
            for (int l = 1; l <= num; l++)
            {
                map(fcb_table[l], file_table[l]);
            }
        }


        //通过FCB 返回文件File
        public File getFile(FCB item)
        {
            if (fcb_table.ContainsKey(item.filePointer))
            {
                return file_table[item.filePointer];
            }
            else
                return null;
        }

        //通过FilePointer返回文件File
        public File getFile(int filePointer)
        {
            if (fcb_table.ContainsKey(filePointer))
            {
                return file_table[filePointer];
            }
            else
                return null;
        }

        //通过文件返回FCB
        public FCB getFCB(File file)
        {
            if (file_table.ContainsKey(file.filePointer))
            {
                return fcb_table[file.filePointer];
            }
            else
                return null;
        }

        //移除文件
        public void removeFile(FCB item)
        {
            file_table.Remove(item.filePointer);
        }

        //移除FCB
        public void removeFCB(File file)
        {
            fcb_table.Remove(file.filePointer);
        }
    }
}
