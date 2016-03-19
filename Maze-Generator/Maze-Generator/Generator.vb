Public Class Generator

    Enum directions
        up
        down
        left
        right
    End Enum

    Public grid As Grid
    Public open As New List(Of Cell)

    Public currentcell As Cell
    Public previousdirection As Byte

    Public random As Random

    Public Sub New(ByRef usegrid)
        grid = usegrid
        open.Add(grid.cells(grid.startpoint.X, grid.startpoint.Y))
        currentcell = open(0)

        grid.cells(grid.startpoint.X, grid.startpoint.Y).state = Cell.states.wall
        grid.cells(grid.finishpoint.X, grid.finishpoint.Y).state = Cell.states.wall

        random = New Random()

    End Sub

    Public Sub drawOpen(ByRef vbgame As VBGame)

        For Each Cell As Cell In open
            vbgame.drawRect(Cell.getRect(), Color.FromArgb(0, 128, 0))
        Next

    End Sub

    Public Function doDirection(direction As Byte, opoint As Point, amount As Integer) As Point
        Dim point As Point
        If direction = directions.up Then
            point = New Point(opoint.X, opoint.Y - amount)
        ElseIf direction = directions.down Then
            point = New Point(opoint.X, opoint.Y + amount)
        ElseIf direction = directions.left Then
            point = New Point(opoint.X - amount, opoint.Y)
        ElseIf direction = directions.right Then
            point = New Point(opoint.X + amount, opoint.Y)
        End If
        Return point
    End Function

    Public Sub backtrack()
        open.Remove(currentcell)
        Try
            currentcell = open(random.Next(0, open.ToArray().Length))
        Catch
        End Try
    End Sub

    Public Function handle() As Boolean
        Dim direction As Byte
        Dim donedirection As Point
        Dim donedirectionone As Point
        Dim donedirections As New List(Of Byte)
        Dim done As Boolean
        Dim backs As Integer

        done = False
        backs = 0
        While Not done

            If donedirections.ToArray().Length = 4 Then
                backtrack()
                backs += 1
                If backs >= open.ToArray().Length Then
                    Return False
                End If
                donedirections.Clear()
            End If

            direction = random.Next(0, 4)

            If donedirections.Contains(direction) Then
                Continue While
            Else
                donedirections.Add(direction)
            End If

            donedirection = doDirection(direction, currentcell.getIndexXY(), 2)

            If donedirection.X > 0 And donedirection.X < grid.width And donedirection.Y > 0 And donedirection.Y < grid.height Then
                If grid.cells(donedirection.X, donedirection.Y).state = Cell.states.wall Then
                    done = True
                End If
            End If

        End While

        donedirectionone = doDirection(direction, currentcell.getIndexXY(), 1)

        grid.cells(donedirectionone.X, donedirectionone.Y).state = Cell.states.empty
        grid.cells(donedirection.X, donedirection.Y).state = Cell.states.empty

        grid.dirtycells.Add(grid.cells(donedirectionone.X, donedirectionone.Y))
        grid.dirtycells.Add(grid.cells(donedirection.X, donedirection.Y))

        If direction <> previousdirection Then
            open.Add(grid.cells(currentcell.ix, currentcell.iy))
        End If

        'open.Add(grid.cells(donedirection.X, donedirection.Y))

        currentcell = grid.cells(donedirection.X, donedirection.Y)

        previousdirection = direction

        Return True

    End Function

End Class
