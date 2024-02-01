param([string]$year)

$src = "$PSScriptRoot\src"
$yearDir = "$src\_$year"

New-Item -Path $yearDir -ItemType Directory

& dotnet new console -n "_$year" -o $yearDir
& dotnet add "$yearDir\_$year.csproj" reference "$src\AocLib\AocLib.csproj"

$program = @"
using AocLib;

PuzzleRunner<_$year.Day01.Part01>.Solve();
"@

$program | Out-File "$yearDir\Program.cs"
$csproj = [XML](Get-Content -Path "$yearDir\_$year.csproj")
$itemGroup = $csproj.CreateElement("ItemGroup")

For ($i = 1; $i -le 25; $i++) {
  $day = "Day$($i.ToString('00'))"
  $dayDir = "$yearDir\$day"
  New-Item -Path $dayDir -ItemType Directory
  New-Item -Path "$dayDir\01.txt" -ItemType File
  New-Item -Path "$dayDir\sample.txt" -ItemType File

  $content = $csproj.CreateElement("None")
  $content.SetAttribute("Update", "$day\01.txt")
  $copyToOutputDir = $csproj.CreateElement("CopyToOutputDirectory")
  $copyToOutputDir.InnerText = "PreserveNewest"
  $content.AppendChild($copyToOutputDir)
  $itemGroup.AppendChild($content)

  $content = $csproj.CreateElement("None")
  $content.SetAttribute("Update", "$day\sample.txt")
  $copyToOutputDir = $csproj.CreateElement("CopyToOutputDirectory")
  $copyToOutputDir.InnerText = "PreserveNewest"
  $content.AppendChild($copyToOutputDir)
  $itemGroup.AppendChild($content)

  For ($part = 1; $part -le 2; $part++) {
    if ($i -eq 25 -and $part -eq 2) {
        break
    }

    $code = @"
using AocLib;

namespace _$year.$day;

public class Part0$part : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return 0;
    }
}
"@
    $code | Out-File "$dayDir\Part0$part.cs"
  }
}

$csproj.DocumentElement.AppendChild($itemGroup)
$csproj.Save("$yearDir\_$year.csproj")
& dotnet sln add "$yearDir\_$year.csproj"

