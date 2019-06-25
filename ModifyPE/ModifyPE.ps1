function ModifyPe([string]$fileName, [string]$dosStubStr)
{
    $fs = 
    $bw = New-Object -TypeName System.IO.BinaryWriter([System.IO.File]::Open($fileName,[System.IO.FileMode]::Open,[System.IO.FileAccess]::Write,[System.IO.FileShare]::Write))
    $bw.Seek(5*16-2,[System.IO.SeekOrigin]::Begin)
    $bw.Write([System.Text.Encoding]::Default.GetBytes($dosStubStr))
}

function Main()
{
    $dosStub = "COMP temporary modification by aa.     ";
    ModifyPe "D:\Workspaces\Code\temp\aadtb.dll.mui" $dosStub;
}

Main
