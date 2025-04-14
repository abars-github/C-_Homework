const int Free = 0;
const int Kentavr = 1;
const int Attacked = 2;
const int BoardSize = 8;

int[,] board = new int[BoardSize, BoardSize];

System.Console.WriteLine("Kentavr (Rook+Knight) placement using Maximum Free Fields Heuristic");
System.Console.WriteLine("Board size is 8x8. Rows and Columns are numbered 1 to 8.");
Console.WriteLine("\n");

int initRow, initCol;
while (true)
{
    System.Console.Write("Enter the row (1-8) for the first kentavr: ");
    if (!int.TryParse(System.Console.ReadLine(), out initRow) || initRow < 1 || initRow > 8)
    {
        System.Console.WriteLine("Invalid input. Try again.");
        continue;
    }

    System.Console.Write("Enter the column (1-8) for the first kentavr: ");
    if (!int.TryParse(System.Console.ReadLine(), out initCol) || initCol < 1 || initCol > 8)
    {
        System.Console.WriteLine("Invalid input. Try again.");
        continue;
    }
    initRow--;
    initCol--;
    break;
}

if (board[initRow, initCol] != Free)
{
    System.Console.WriteLine("Selected cell is not free. Exiting.");
    return;
}

PlaceKentavr(board, initRow, initCol);
System.Console.WriteLine("\nAfter placing first kentavr:");
PrintBoard(board);

while (true)
{
    (int bestRow, int bestCol, int bestFreeCount) = (-1, -1, -1);
    for (int r = 0; r < BoardSize; r++)
    {
        for (int c = 0; c < BoardSize; c++)
        {
            if (board[r, c] == Free)
            {
                int freeAfter = SimulatePlacement(board, r, c);
                if (freeAfter > bestFreeCount)
                {
                    bestFreeCount = freeAfter;
                    bestRow = r;
                    bestCol = c;
                }
            }
        }
    }

    if (bestRow == -1 || bestFreeCount < 0)
    {
        break;
    }

    PlaceKentavr(board, bestRow, bestCol);
    System.Console.WriteLine($"\nPlaced kentavr at ({bestRow + 1}, {bestCol + 1}) using heuristic (free fields left: {bestFreeCount}).");
    PrintBoard(board);
}

var freeCells = new System.Collections.Generic.List<(int, int)>();
for (int r = 0; r < BoardSize; r++)
{
    for (int c = 0; c < BoardSize; c++)
    {
        if (board[r, c] == Free)
        {
            freeCells.Add((r, c));
        }
    }
}
foreach (var cell in freeCells)
{
    if (board[cell.Item1, cell.Item2] == Free)
    {
        PlaceKentavr(board, cell.Item1, cell.Item2);
    }
}

System.Console.WriteLine("\nFinal board after filling in all free cells:");
PrintBoard(board);
System.Console.WriteLine("Press any key to exit.");
System.Console.ReadKey();



void PlaceKentavr(int[,] board, int row, int col)
{
    board[row, col] = Kentavr;

    for (int c = 0; c < BoardSize; c++)
    {
        if (board[row, c] == Free)
            board[row, c] = Attacked;
    }
    for (int r = 0; r < BoardSize; r++)
    {
        if (board[r, col] == Free)
            board[r, col] = Attacked;
    }

    int[,] knightMoves = new int[,] { { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 } };
    for (int i = 0; i < knightMoves.GetLength(0); i++)
    {
        int newRow = row + knightMoves[i, 0];
        int newCol = col + knightMoves[i, 1];
        if (InBoard(newRow, newCol) && board[newRow, newCol] == Free)
            board[newRow, newCol] = Attacked;
    }
    board[row, col] = Kentavr;
}
int SimulatePlacement(int[,] board, int row, int col)
{
    int[,] copy = new int[BoardSize, BoardSize];
    System.Array.Copy(board, copy, board.Length);

    if (copy[row, col] != Free)
        return -1;

    copy[row, col] = Kentavr;

    for (int c = 0; c < BoardSize; c++)
    {
        if (copy[row, c] == Free)
            copy[row, c] = Attacked;
    }
    for (int r = 0; r < BoardSize; r++)
    {
        if (copy[r, col] == Free)
            copy[r, col] = Attacked;
    }

    int[,] knightMoves = new int[,] { { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 } };
    for (int i = 0; i < knightMoves.GetLength(0); i++)
    {
        int newRow = row + knightMoves[i, 0];
        int newCol = col + knightMoves[i, 1];
        if (InBoard(newRow, newCol) && copy[newRow, newCol] == Free)
            copy[newRow, newCol] = Attacked;
    }

    int freeCount = 0;
    for (int r = 0; r < BoardSize; r++)
    {
        for (int c = 0; c < BoardSize; c++)
        {
            if (copy[r, c] == Free)
                freeCount++;
        }
    }
    return freeCount;
}

bool InBoard(int row, int col)
{
    return row >= 0 && row < BoardSize && col >= 0 && col < BoardSize;
}

void PrintBoard(int[,] board)
{
    System.Console.WriteLine("   1 2 3 4 5 6 7 8");
    for (int r = 0; r < BoardSize; r++)
    {
        System.Console.Write((r + 1) + "  ");
        for (int c = 0; c < BoardSize; c++)
        {
            char symbol = '.';
            if (board[r, c] == Kentavr)
                symbol = 'K';
            else if (board[r, c] == Attacked)
                symbol = '*';
            System.Console.Write(symbol + " ");
        }
        System.Console.WriteLine();
    }
}
