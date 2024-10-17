Imports System
Imports System.Runtime.InteropServices

Namespace DebugRemoteScript
    Public Class Program
        Public Shared Sub Main(ByVal args As String())
            Dim remote = RemoteAPI.InitializeAPI(args)
            Dim text = remote.GetEditorText()
            Dim str = String.Empty

            For Each c In text
                str = c + str
            Next

            remote.ShowMessage(str)
        End Sub
    End Class
End Namespace
