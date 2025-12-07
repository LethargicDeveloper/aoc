using System.Collections;
using System.Security.Cryptography;

namespace _2015.Day04;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var num = 0;
        var hash = "";

        while (!hash.StartsWith("00000"))
        {
            hash = MD5.ComputeHash($"{input}{num++}");
        }
        
        return num - 1;
    }
}
