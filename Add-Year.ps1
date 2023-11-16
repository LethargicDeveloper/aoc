param([string]$year)

$sln = "$PSScriptRoot\src\AdventOfCode"
$yearDir = "$sln\$year"

New-Item -Path $yearDir -ItemType Directory

For ($i = 1; $i -le 25; $i++) {
  $day = "Day$($i.ToString('00'))"
  $dayDir = "$yearDir\$day"
  New-Item -Path $dayDir -ItemType Directory
  New-Item -Path "$dayDir\01.txt" -ItemType File
  
  $code = @"
using AdventOfCode.Abstractions;

namespace AdventOfCode._$year.$day;

public class Part01 : PuzzleSolver<long>
{
    public override long Solve()
    {
        return 0;
    }
}
"@

  $code | Out-File "$dayDir\Part01.cs"

  $code = @"
using AdventOfCode.Abstractions;

namespace AdventOfCode._$year.$day;

public class Part02 : PuzzleSolver<long>
{
    public override long Solve()
    {
        return 0;
    }
}
"@

  $code | Out-File "$dayDir\Part02.cs"
}
