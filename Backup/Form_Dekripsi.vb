Imports System.Data.OleDb
Imports System.Security.Cryptography
Public Class Form_dekripsi
    Dim outputLength As Integer 'not used here, DES class returns as number of bytes processed

    Dim desProv As New DESCryptoServiceProvider()

    Dim myDes As New DES()
    Dim iv(7) As Byte 'blank, not supported by DES class
    Dim key8() As Byte 'DES key
    Dim input() As Byte
    Dim output() As Byte
    Sub tampil()
        SQL2 = "Select * From tbl_hasilenkrip"
        Perintah = New OleDbCommand(SQL2, Koneksi)
        Pembaca = Perintah.ExecuteReader
        Dim x As Integer
        ListView1.Items.Clear()
        While Pembaca.Read
            ListView1.Items.Add(Pembaca("id_produk"))
            ListView1.Items(x).SubItems.Add(Pembaca("nama_produk"))
            ListView1.Items(x).SubItems.Add(Pembaca("pagi_produksi"))
            ListView1.Items(x).SubItems.Add(Pembaca("siang_produksi"))
            ListView1.Items(x).SubItems.Add(Pembaca("malam_produksi"))
            ListView1.Items(x).SubItems.Add(Pembaca("rata_produksi"))
            ListView1.Items(x).SubItems.Add(Pembaca("tanggal"))
            x = x + 1
        End While
    End Sub
    Private Function GetHexString(ByVal bytes() As Byte, Optional ByVal len As Integer = -1, Optional ByVal spaces As Boolean = False) As String
        If len = -1 Then len = bytes.Length
        Dim i As Integer
        Dim s As String = ""
        For i = 0 To len - 1
            s += bytes(i).ToString("x2")
            If spaces Then s += " "
        Next
        If spaces Then s = s.TrimEnd()
        Return s
    End Function
    Function HexStringToBytes(ByVal hexstring As String) As Byte()
        Dim out((hexstring.Length / 2) - 1) As Byte
        For i = 0 To (hexstring.Length / 2) - 1
            out(i) = Convert.ToByte(hexstring.Substring(i * 2, 2), 16)
        Next
        Return out
    End Function
    Public Function StrToHex(ByRef Data As String) As String
        Dim sVal As String
        Dim sHex As String = ""
        While Data.Length > 0
            sVal = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()))
            Data = Data.Substring(1, Data.Length - 1)
            sHex = sHex & sVal
        End While
        Return sHex
    End Function
    Private Sub SetupBytes()
        'If (InputTextBox.Text.Length / 2) Mod 8 <> 0 Then Throw New Exception("Input length not multiple of 8")
        'If DESKeyTextBox.Text.Length <> (8 * 2) Then Throw New Exception("DES key length not 8")
        'If DES3KeyTextBox.Text.Length <> (16 * 2) Then Throw New Exception("DES3 key length not 16")

        key8 = HexStringToBytes(DES3KeyTextBox.Text)
        'key16 = HexStringToBytes(DES3KeyTextBox.Text)
        'Input = HexStringToBytes(InputTextBox.Text)
        'ReDim output(input.Length - 1)
    End Sub

    Private Sub Form_dekripsi_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Bukadatabase()
        Call tampil()
        SQL = "Select * From tbl_user"
        Perintah = New OleDbCommand(SQL, Koneksi)
        Pembaca = Perintah.ExecuteReader
        Dim x As Integer
        While Pembaca.Read
            Label15.Text = (Pembaca("id"))
            Label16.Text = (Pembaca("nama"))
            x = x + 1
        End While
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        Dim tStr As String '= Me.TextBox4.Text
        Dim tStrA() As String
        Dim tStrD As String = ""
        Dim tStrH As String = ""
        Dim i As Integer
        Dim PanjangAwal As Single
        Dim tIntPenambah As Integer
        Dim tStrPenambah As String
        Dim StrAngkaPenambah As String

        PanjangAwal = Me.TextBox1.TextLength

        If PanjangAwal < 8 Then


            tIntPenambah = 8 - PanjangAwal

            If Len(tIntPenambah) < 2 Then
                StrAngkaPenambah = "0" & tIntPenambah

            Else
                StrAngkaPenambah = tIntPenambah
            End If

            For Ulang = 1 To tIntPenambah
                tStrPenambah = tStrPenambah & "0"
            Next

            tStr = Me.TextBox1.Text & tStrPenambah



            For Each ch As Char In tStr
                i = Asc(ch)
                'tStrD &= i.ToString.PadLeft(2, "0") & " "
                tStrD = tStrD & i.ToString.PadLeft(2, "0") & " "
            Next

            tStrA = tStrD.Split(" ")
            For i = 0 To UBound(tStrA)
                If tStrA(i) <> "" Then

                    If Len(Hex(tStrA(i))) < 2 Then
                        'tStrH &= "0" & Hex(tStrA(i)) & " "
                        tStrH = tStrH & "0" & Hex(tStrA(i)) & " "
                    Else
                        'tStrH &= Hex(tStrA(i)) & " "
                        tStrH = tStrH & Hex(tStrA(i)) & " "
                    End If

                End If

            Next

            Me.DES3KeyTextBox.Text = Replace(tStrH, " ", "")



        Else
            tStr = Me.TextBox1.Text '& tStrPenambah

            For Each ch As Char In tStr
                i = Asc(ch)
                'tStrD &= i.ToString.PadLeft(2, "0") & " "
                tStrD = tStrD & i.ToString.PadLeft(2, "0") & " "
            Next

            tStrA = tStrD.Split(" ")
            For i = 0 To UBound(tStrA)
                If tStrA(i) <> "" Then

                    If Len(Hex(tStrA(i))) < 2 Then
                        'tStrH &= "0" & Hex(tStrA(i)) & " "
                        tStrH = tStrH & "0" & Hex(tStrA(i)) & " "
                    Else
                        'tStrH &= Hex(tStrA(i)) & " "
                        tStrH = tStrH & Hex(tStrA(i)) & " "
                    End If

                End If

            Next

            Me.DES3KeyTextBox.Text = Replace(tStrH, " ", "")
        End If
    End Sub
    Sub Cari()
        SQL = "select * from tbl_hasilenkrip " & _
                              "where StrComp('" & Trim(TextBox1.Text) & "',katakunci,0)=0"
        Perintah = New OleDbCommand(SQL, Koneksi)
        Pembaca = Perintah.ExecuteReader
    End Sub
    Function HexToString(ByVal hex As String) As String
        Dim text As New System.Text.StringBuilder(hex.Length \ 2)
        For i As Integer = 0 To hex.Length - 2 Step 2
            text.Append(Chr(Convert.ToByte(hex.Substring(i, 2), 16)))
        Next
        Return text.ToString
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ListView1.SelectedItems.Count = 0 Then
            MessageBox.Show("Data yang mau di enkripsi belum di pilih! Harap klik datanya dulu!")
        Else
            
            For i As Integer = 0 To ListView1.Columns.Count - 1
                ' DtSet.Rows(i)(0) = decrypt(DtSet.Rows(i)(0), salt)
                'MsgBox("clomun: " & DtSet.Rows(i)(0))
                'place all the columns here 1 by 1 =)

                Dim iSuccessCount As Integer
                Dim iFailedCount As Integer
                Try

                    Dim inputhex As String
                    inputhex = ListView1.SelectedItems(0).SubItems(i).Text.ToString
                    input = HexStringToBytes(inputhex)
                    ReDim output(input.Length - 1)
                    iSuccessCount += 1
                    SetupBytes()

                    myDes.decrypt_des(key8, input, input.Length, output, outputLength)
                    Dim o = GetHexString(output)

                    Dim hasil = HexToString(o)

                    Call Cari()
                    If Pembaca.Read = False Then
                        iFailedCount += 1
                    Else
                        
                        ListView1.SelectedItems(0).SubItems(i).Text = hasil.TrimEnd(" ")

                    End If


                Catch ex As System.Security.Cryptography.CryptographicException
                    iFailedCount += 1

                End Try

                If iFailedCount = 1 Then

                    MsgBox("Data tidak dapat di dekripsi, Password Salah")
                End If
            Next
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim txtidproduk As String
        Dim txtproduksi As String
        Dim txtpagi As String
        Dim txtsiang As String
        Dim txtmalam As String
        Dim txtratarata As String
        Dim katakunci As String
        Dim DateTimePicker1 As String



        txtidproduk = ListView1.SelectedItems(0).Text.ToString
        txtproduksi = ListView1.SelectedItems(0).SubItems(1).Text.ToString
        txtpagi = ListView1.SelectedItems(0).SubItems(2).Text.ToString
        txtsiang = ListView1.SelectedItems(0).SubItems(3).Text.ToString
        txtmalam = ListView1.SelectedItems(0).SubItems(4).Text.ToString
        txtratarata = ListView1.SelectedItems(0).SubItems(5).Text.ToString
        DateTimePicker1 = ListView1.SelectedItems(0).SubItems(6).Text.ToString
        katakunci = TextBox1.Text
        Try
            SQL2 = "Insert Into tbl_hasildekrip Values('" & txtidproduk & _
                 "','" & txtproduksi & _
                 "','" & txtpagi & _
                 "','" & txtsiang & _
                 "','" & txtmalam & _
                 "','" & txtratarata & _
                 "','" & katakunci & _
                 "','" & DateTimePicker1 & "')"
            Perintah = New OleDbCommand(SQL2, Koneksi)
            Perintah.CommandType = CommandType.Text
            Perintah.ExecuteNonQuery()
            MsgBox("Berhasil Menambah Data", MsgBoxStyle.Information, "Simpan Data")

        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim txtidproduk As String
        txtidproduk = ListView1.SelectedItems(0).Text.ToString
        Dim pil As String
        pil = MsgBox("Anda yakin ingin menghapus data record ini?", _
                     MsgBoxStyle.YesNo, "Konfirmasi")
        If pil = vbYes Then
            Try
                SQL = "Delete * From tbl_hasilenkrip Where id_produk='" & _
                                       txtidproduk & "'"
                Perintah = New OleDbCommand(SQL, Koneksi)
                Perintah.CommandType = CommandType.Text
                Perintah.ExecuteNonQuery()
                MsgBox("Berhasil Menghapus Data", MsgBoxStyle.Information, "Hapus Data")
                Call tampil()

            Catch ex As Exception
                MsgBox(ex.Message())
            End Try
        Else
            Exit Sub
        End If
    End Sub
End Class