Imports System.Data.SqlClient
Imports System.Data

Public Class Form1
    Private connectionString As String = "Server=OVERLOAD-LORD;Database=LibraryDB;Integrated Security=True;"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim query As String = "SELECT * FROM Books"
                Dim adapter As New SqlDataAdapter(query, connection)
                Dim table As New DataTable()
                adapter.Fill(table)
                dgvBooks.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            Using connection As New SqlConnection(connectionString)
                Dim query As String = "INSERT INTO Books (Title, Author, YearPublished, Genre) VALUES (@Title, @Author, @YearPublished, @Genre)"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Title", txtTitle.Text)
                    command.Parameters.AddWithValue("@Author", txtAuthor.Text)
                    command.Parameters.AddWithValue("@YearPublished", Convert.ToInt32(txtYearPublished.Text))
                    command.Parameters.AddWithValue("@Genre", txtGenre.Text)
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using
            LoadData()
            ClearFields()
        Catch ex As Exception
            MessageBox.Show("Error adding book: " & ex.Message)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If dgvBooks.SelectedRows.Count > 0 Then
            Try
                Dim id As Integer = Convert.ToInt32(dgvBooks.SelectedRows(0).Cells("Id").Value)
                Using connection As New SqlConnection(connectionString)
                    Dim query As String = "UPDATE Books SET Title = @Title, Author = @Author, YearPublished = @YearPublished, Genre = @Genre WHERE Id = @Id"
                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@Id", id)
                        command.Parameters.AddWithValue("@Title", txtTitle.Text)
                        command.Parameters.AddWithValue("@Author", txtAuthor.Text)
                        command.Parameters.AddWithValue("@YearPublished", Convert.ToInt32(txtYearPublished.Text))
                        command.Parameters.AddWithValue("@Genre", txtGenre.Text)
                        connection.Open()
                        command.ExecuteNonQuery()
                    End Using
                End Using
                LoadData()
                ClearFields()
            Catch ex As Exception
                MessageBox.Show("Error updating book: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvBooks.SelectedRows.Count > 0 Then
            Try
                Dim id As Integer = Convert.ToInt32(dgvBooks.SelectedRows(0).Cells("Id").Value)
                Using connection As New SqlConnection(connectionString)
                    Dim query As String = "DELETE FROM Books WHERE Id = @Id"
                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@Id", id)
                        connection.Open()
                        command.ExecuteNonQuery()
                    End Using
                End Using
                LoadData()
                ClearFields()
            Catch ex As Exception
                MessageBox.Show("Error deleting book: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        txtTitle.Clear()
        txtAuthor.Clear()
        txtYearPublished.Clear()
        txtGenre.Clear()
    End Sub

    Private Sub dgvBooks_SelectionChanged(sender As Object, e As EventArgs) Handles dgvBooks.SelectionChanged
        If dgvBooks.SelectedRows.Count > 0 Then
            txtTitle.Text = dgvBooks.SelectedRows(0).Cells("Title").Value.ToString()
            txtAuthor.Text = dgvBooks.SelectedRows(0).Cells("Author").Value.ToString()
            txtYearPublished.Text = dgvBooks.SelectedRows(0).Cells("YearPublished").Value.ToString()
            txtGenre.Text = dgvBooks.SelectedRows(0).Cells("Genre").Value.ToString()
        End If
    End Sub
End Class
