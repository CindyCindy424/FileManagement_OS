using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_system
{
    //数据存储单元，1个Block可存储16个char
    //整个系统的block初始化在BitMap中一同初始化

    [Serializable]
    public class Block
    {
        //public char[] info;
        public string info;
        private int length;

        public Block()
        {
            //info = new char[100];
         }

        public void setInfo(string new_info)
        {
            length = (new_info.Length > 100) ? 100 : new_info.Length;
            /*for(int i = 0; i < length; i++)
            {
                info[i] = new_info[i];
                Console.WriteLine("写入info[" + i + "]，=" + new_info[i]);
            }*/
            info = new_info;
            //bool flag = (new_info.Length < 100) ? true: false;
            //return flag;
        }

        public string getInfo()
        {
            //string temp = new string(info);
            return info;
        }
    }
}
