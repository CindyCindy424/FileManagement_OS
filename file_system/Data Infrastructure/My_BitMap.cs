using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_system
{
    //Use bitmap to manage the blocks
    [Serializable]
    public class My_BitMap
    {
        //public static int Capcity = 100 * 100 * 100;  //分配初始存储空间大小
        public static int Capcity = 120 ;  //分配初始存储空间大小
        public static Block[] blocks = new Block[Capcity];  //初始化capacity个block
        public static bool[] my_inside_bitMap = new bool[Capcity]; //block空间置1，占用置0
        private int bitPointer = 0;

        public My_BitMap()
        {
            //初始化BitMap，所有block置空
            for (int i = 0; i < Capcity; i++)
            {
                my_inside_bitMap[i] = false;

            }
        }

        //获取某个block的数据
        public string getBlock(int i)
        {
            return blocks[i].getInfo();
        }

        //搜索空位并放置数据
        public int allocateBlock(int num,string data)
        {
            //bitPointer = bitPointer % Capcity;
           // Console.WriteLine("bitPointer=" + bitPointer);

            //int tempPointer = bitPointer;
           // while (true)
           // {
                if (my_inside_bitMap[num]==false)//可以分配
                {
                    blocks[num] = new Block();
                    Console.WriteLine("初始化第" + num + "个block");
                    
                    blocks[num].setInfo(data);
                    my_inside_bitMap[num] = true;
                    Console.WriteLine("把第" + num + "组设为false");
                    //bitPointer = tempPointer + 1;
                    return num;  //返回Block号
                }
                //else
                  //  tempPointer = (tempPointer + 1) % Capcity;
                //if (tempPointer == bitPointer)
                  //  break;
                  else
                        return -1;//录入数据失败
            // }

        }

        //Withdraw a block,set its status to empty
        public void withdraw(int index)
        {
            my_inside_bitMap[index] = false;
        }

        public void withdraw(List<int> indexs)
        {
            foreach(int i in indexs)
            {
                my_inside_bitMap[i] = false;
            }
        }

        //建立索引记录文件的block块数
       /* public int[] write(string data)
        {
            // IndexTable table = new IndexTable();

            int[] table = new int[10];
            int tablePointer = 0;
            for (int k = 0; k < 10; k++) table[k] = -1;
            int newIndex;
            while (data.Count() > 16)
            {
                //table.addIndex(allocateBlock(data.Substring(0, 15)));
                newIndex = allocateBlock(data.Substring(0, 15));//分配的block号
                if(newIndex!=-1)
                {
                    table[tablePointer] = newIndex; //存入索引
                    tablePointer++;
                }
                data = data.Remove(0, 15);
            }
            //table.addIndex(allocateBlock(data));
            newIndex = allocateBlock(data);
            table[tablePointer] = newIndex;

            return table;  //返回索引指示数组
        }*/


        public int write(string data)//返回第几个block
        {
            for(int k=0;k<Capcity;k++  )
            {
                if (my_inside_bitMap[k] == false)
                {
                    allocateBlock(k,data);
                   return k;
                }    
            }
            Console.WriteLine(data + "写入失败，返回-1");
            return -1;//寻找失败
            
        }

    }
}
