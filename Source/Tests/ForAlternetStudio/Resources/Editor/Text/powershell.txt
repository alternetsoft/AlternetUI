using namespace System.Text
using namespace System.IO
using module CardGames

[string]$string = "Hello World"
## Valid values are "SHA1", "SHA256", "SHA384", "SHA512", "MD5"
[string]$algorithm = "SHA256"
$algorithm.

[byte[]]$stringbytes = [UnicodeEncoding]::Unicode.GetBytes($string)

[Stream]$memorystream = [MemoryStream]::new($stringbytes)
$hashfromstream = Get-FileHash -InputStream $memorystream `
  -Algorithm $algorithm
$hashfromstream.Hash.ToString()

# Windows PowerShell Version Check
get-alias -definition get-childitem
set-alias -list get-childitem

Set-MyProcess -Strict
Clear-Host
$Host

function Add-Numbers {
 $args[0] + $args[1]
}

function F {"Hello Form F"}
$Function:F
Set-Alias A F
$Alias:A
$Count=10
$Variable:Count
$Env:Path

$a = Get-Date

"Month: " + $a.Dayofyear

"Day of the Week: " + $a.Dayofweek

"Hour: " + $a.Hour

"Day:" + $a.day

For ($i=0; $i -le 10; $i++) {
    "10 * $i = " + (10 * $i)
    }

get-service | where { $_.Status -eq "running"} | select-object -last 10

$colors = @("Red","Orange","Yellow","Green","Blue","Indigo","Violet")
For ($i=0; $i -lt $colors.Length; $i++) {
    $colors[$i]
    }
$i=1
Do {
    $i
    $i++
    }
While ($i -le 10)

$i=1
Do {
    $i
    $i++
    }
Until ($i -gt 10)

$i=1
While ($true)
    {
    $i
    $i++
    if ($i -gt 10) {
        Break
        }
    }

workflow foo
{
  $b = "Hello"
  Inlinescript {$using:b}
}
foo # returns "Hello"