using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IndexLooper
{
    public static int Increment(int index, int count)
    {
        if (index == count - 1)
        {
            return 0;
        }
        else
        {
            return index++;
        }
    }

    public static int Decrement(int index, int count)
    {
        if (index == 0)
        {
            return count - 1;
        }
        else
        {
            return index--;
        }
    }
}