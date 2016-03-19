Imports System.Threading
Public Class Form1

    Public thread As New Thread(AddressOf mainloop)
    Public vbgame As New VBGame

    Public side As Integer = 10

    Public mazewidth As Integer = 51
    Public mazeheight As Integer = 51

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        adjustSize()
        thread.IsBackground = True
        thread.Start()
    End Sub

    Sub adjustSize()
        vbgame.setDisplay(Me, New Size(mazewidth * side, mazeheight * side), "Maze Generator")
    End Sub

    Sub mainloop()

        Dim start As New Button(vbgame, "Start", New Rectangle(10, 10, 50, 20))
        start.setColor(vbgame.red, vbgame.green)
        start.setTextColor(vbgame.white, vbgame.white)

        While True

            For Each e As MouseEvent In vbgame.getMouseEvents()

                If start.handle(e) = MouseEvent.buttons.left Then

                    mazeloop()

                End If

            Next

            start.draw()

            vbgame.update()
            vbgame.clockTick(30)

        End While

    End Sub

    Sub mazeloop()

        Dim frames As Integer = 0

        Dim grid As New Grid(side, mazewidth, mazeheight, New Point(1, 1), New Point(mazewidth - 2, mazeheight - 2))
        Dim generator As New Generator(grid)

        vbgame.fill(vbgame.black)

        While True

            For Each e As KeyEventArgs In vbgame.getKeyDownEvents()
                If e.KeyCode = Keys.R Then
                    vbgame.fill(vbgame.black)
                    grid = New Grid(side, mazewidth, mazeheight, New Point(1, 1), New Point(mazewidth - 2, mazeheight - 2))
                    generator = New Generator(grid)
                End If
            Next
            generator.handle()


            grid.drawDirtyCells(vbgame)

            generator.drawOpen(vbgame)

            vbgame.update()

            vbgame.clockTick(30)

        End While

    End Sub

End Class
