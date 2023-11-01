using System.Collections;
using System.Collections.Generic;

public class intM
{
    public int Min;
    public int Max;
    public intM(int a)
    {
        Min = Max = a;
    }
    public intM(int a, int b)
    {
        Min = a;
        Max = b;
    }
    public intM(string str)
    {
        string[] com = str.Split('-');
        Max = int.Parse(com[1]);
        Min = int.Parse(com[0]);
    }
    public intM(string str1, string str2)
    {
        Max = int.Parse(str1);
        Min = int.Parse(str2);
    }
}
