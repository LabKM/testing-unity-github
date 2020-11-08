using System;
using System.Collections;


public static class Utility
{
    public static T[] ShuffleArr<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);
        for (int i = 0; i <  array.Length -1; ++i)
        {
            int randindex = prng.Next(i, array.Length);
            T tempItem = array[randindex];
            array[randindex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }

    public static float linearSin(float n) // 주기 0.0~1.0 위상 -1 ~ 1 시작 0
    {
        float x = (n - (int)n) - 0.25f;
        return x > 0 ? -4 * x + 1: 4 * x + 1;
    }

    public static float sin(float n) // 주기 0.0~1.0 위상 -1 ~ 1 시작 0
    { 
        return (float)(Math.Sin((double)n * Math.PI * 2));
    }

    public static float pi
    {
        get{
            return (float)Math.PI;
        }
    }

    //public struct Coord
    //{
    //    public int x;
    //    public int y;

    //    public Coord(int _x, int _y)
    //    {
    //        x = _x;
    //        y = _y;
    //    }
    //    public Coord(float _x, float _y)
    //    {
    //        x = (int)_x;
    //        y = (int)_y;
    //    }

    //    public static bool operator ==(Coord a, Coord b)
    //    {
    //        return a.x == b.x && a.y == b.y;
    //    }
    //    public static bool operator !=(Coord a, Coord b)
    //    {
    //        return !(a == b);
    //    }
    //    public override bool Equals(object o)
    //    {
    //        if (o is Coord) {
    //            return this == (Coord)o;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    public override int GetHashCode()
    //    {
    //        return x * y;
    //    }
    //}
}
