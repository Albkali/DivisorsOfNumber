Imports System.Text.RegularExpressions

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Function dbms(ByVal query As String)
        Try
            Dim db As String() = IO.File.ReadAllLines(TextBox1.Text)
            Dim ColumnsCount As Integer = CountCharacter(db(0), ",")
            Dim Columns As String() = db(0).Split(",")
            Dim itemindex As Integer = 0
            Dim condition = False
            Dim value As String = ""
            If query.Contains("select all") Then
                If query.StartsWith("where") Then
                    Dim patt As String = "select all where (.*?) = (.*?);"
                    Dim rx As Regex = New Regex(patt)
                    Dim colname As String = rx.Match(query).Groups(1).ToString
                    value = rx.Match(query).Groups(2).ToString
                    condition = True
                    itemindex = Array.IndexOf(Columns, colname)
                End If
                For i As Integer = 0 To ColumnsCount
                    If (i <> ColumnsCount) Then
                        TextBox3.AppendText(Columns(i) & " | ")
                    Else
                        TextBox3.AppendText(Columns(i) & vbNewLine)
                    End If
                Next
                For i As Integer = 1 To db.Length
                    Dim rows As String() = db(i).Split(",")
                    If condition = True Then
                        If rows(itemindex) = value Then
                            For j As Integer = 0 To ColumnsCount
                                If (j <> ColumnsCount) Then
                                    TextBox3.AppendText(rows(j) & " | ")
                                Else
                                    TextBox3.AppendText(rows(j) & " | " & vbNewLine)
                                End If
                            Next
                        End If
                    Else
                        For j As Integer = 0 To ColumnsCount
                            If (j <> ColumnsCount) Then
                                TextBox3.AppendText(rows(j) & " | ")
                            Else
                                TextBox3.AppendText(rows(j) & " | " & vbNewLine)
                            End If
                        Next
                    End If

                Next
            ElseIf query.StartsWith("singleselect") Then
                Dim patt As String = Nothing
                If query.Contains("where") Then
                    patt = "singleselect (.*?) where"
                Else
                    patt = "singleselect (.*?);"
                End If
                Dim rx As Regex = New Regex(patt)
                Dim column As String = rx.Match(query).Groups(1).ToString
                Dim itemindexz As Integer = Array.IndexOf(Columns, column)
                If query.Contains("where") Then
                    Dim patts As String = "singleselect " & column & " where (.*?) = (.*?);"
                    Dim rxs As Regex = New Regex(patts)
                    Dim colname As String = rxs.Match(query).Groups(1).ToString
                    value = rxs.Match(query).Groups(2).ToString
                    condition = True
                    itemindex = Array.IndexOf(Columns, colname)
                End If
                TextBox3.AppendText(column & " ----- " & vbNewLine)
                For i As Integer = 1 To db.Length
                    Dim rows As String() = db(i).Split(",")
                    If condition = True Then
                        If rows(itemindex) = value Then
                            TextBox3.AppendText(rows(itemindexz) & vbNewLine)
                        End If
                    Else
                        TextBox3.AppendText(rows(itemindexz) & vbNewLine)
                    End If
                Next
            ElseIf query.StartsWith("insert") Then
                query = query.Replace("insert ", "")
                If CountCharacter(query, ",") <> ColumnsCount Then
                    MsgBox("You should insert " & ColumnsCount + 1 & " values only")
                Else
                    My.Computer.FileSystem.WriteAllText(TextBox1.Text, query & vbNewLine, True)
                    TextBox3.AppendText("Executed successfully." & vbNewLine)
                End If
            ElseIf query.StartsWith("delete") Then
                Dim patts As String = "delete where (.*?) = (.*?);"
                Dim rxs As Regex = New Regex(patts)
                Dim colname As String = rxs.Match(query).Groups(1).ToString
                value = rxs.Match(query).Groups(2).ToString
                condition = True
                itemindex = Array.IndexOf(Columns, colname)
                For i As Integer = 1 To db.Length
                    Dim rows As String() = db(i).Split(",")
                    If rows(itemindex) = value Then
                        Dim lines As List(Of String) = db.ToList
                        lines.RemoveAt(i)
                        System.IO.File.WriteAllLines(TextBox1.Text, lines)
                        TextBox3.AppendText("Deleted .." & vbNewLine)
                        Exit For ' remove it to delete all
                    End If
                Next
            ElseIf query.StartsWith("update") Then
                Dim patts As String = "update (.*?) value (.*?) to (.*?);"
                Dim rxs As Regex = New Regex(patts)
                Dim colname As String = rxs.Match(query).Groups(1).ToString
                value = rxs.Match(query).Groups(2).ToString
                Dim newvalue = rxs.Match(query).Groups(3).ToString
                condition = True
                itemindex = Array.IndexOf(Columns, colname)
                For i As Integer = 1 To db.Length
                    Dim rows As String() = db(i).Split(",")
                    If rows(itemindex) = value Then
                        db(i) = db(i).Replace(value, newvalue)
                        System.IO.File.WriteAllLines(TextBox1.Text, db)
                        TextBox3.AppendText("Updated .." & vbNewLine)
                        Exit For ' remove it to update all
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
    End Function

    Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
        Dim cnt As Integer = 0
        For Each c As Char In value
            If c = ch Then
                cnt += 1
            End If
        Next
        Return cnt
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
        TextBox1.Text = OpenFileDialog1.FileName
        Button2.Enabled = True
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox3.Clear()
        dbms(TextBox2.Text)
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label4.Click, Label5.Click

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

    End Sub
End Class
