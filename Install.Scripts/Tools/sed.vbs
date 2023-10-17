Dim pat, patparts, rxp, inp
pat = WScript.Arguments(0)
patparts = Split(pat,"/")
Set rxp = new RegExp
rxp.Global = True
rxp.Multiline = False
rxp.Pattern = patparts(1)
Do While Not WScript.StdIn.AtEndOfStream
  inp = WScript.StdIn.ReadLine()
  REM Script.Echo rxp.Replace(inp, patparts(2))
  WScript.StdOut.Write(rxp.Replace(inp, patparts(2)) & Chr(10))
Loop

REM Response.Write ("Here is my sentence" & vbCrLf)
REM The vbCrLf equates to doing a Chr(13) + Chr(10) combination. 
REM You could also use vbNewLine, which is replaced with whatever value the platform 
REM considers a newline. On Windows, the result of vbCrLf and vbNewLine should be the same.