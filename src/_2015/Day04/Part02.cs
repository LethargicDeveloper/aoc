using System.Security.Cryptography;
using AocLib;
using MoreLinq;

namespace _2015.Day04;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var num = 0;
        var hash = "";

        while (!hash.StartsWith("000000"))
        {
            hash = MD5.ComputeHash($"{input}{num++}");
        }
        
        return num - 1;
    }
}
