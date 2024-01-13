using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Reflection;
using Serilog.Core;

namespace MineSweeperApp
{
    public interface IMineSweeper 
    {
        public void InitializeGrid();
        public void PlaceMines();
        public void CalculateNumbers();
        public int CountAdjacentMines(int row,int col);
        public void DisplayGrid();
        public void PlayGame();
        public void RevealCell(int row, int col);
        public void CheckStatus();
    }
    public class MineSweeper: IMineSweeper
    {
        public char[,] grid;
        private bool[,] isRevealed;
        private int gridSize;
        private int minesCount;
        private bool isGameOver;
        private readonly ILogger _logger;
        public MineSweeper(int gridSize, int minesCount, ILogger logger) 
        {
            _logger = logger;
            this.gridSize = gridSize;
            this.minesCount = minesCount;
            grid = new char[gridSize, gridSize];
            isRevealed = new bool[gridSize, gridSize];
            isGameOver = false;
            InitializeGrid();
            PlaceMines();
            CalculateNumbers();
            
        }

        // Initialize the grid of the MineSweeper Game App and set revealed as false
        public void InitializeGrid()
        {  
            try 
            {
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        _logger.Information("Cells InitializeGrid method are "+i+" and "+j);
                        grid[i, j] = '_';
                        isRevealed[i, j] = false;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        // In order to place mine in the grid randomly and minus after placing the mine
        public void PlaceMines()
        {
            Random random = new Random();
            int minesToPlace = minesCount;
            try{

                while (minesToPlace > 0)
                {
                    int x = random.Next(0, gridSize);
                    int y = random.Next(0, gridSize);

                    if (grid[x, y] != '*')
                    {
                        _logger.Information("Randomly place mine in grid's x and y places are " + x + " ,  " + y);
                        grid[x, y] = '*';
                        minesToPlace--;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

        }

        // To calculate the number of the adjacent mine count of cell which does not have mine
        public void CalculateNumbers()
        {
            try{
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (grid[i, j] != '*')
                        {
                            _logger.Information("Grid's x and y places in CalculateNumbers method are " + i + " ,  " + j);
                            int count = CountAdjacentMines(i, j);
                            grid[i, j] = count == 0 ? '_' : (char)(count + '0');
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            
        }

        // Counting Adjacent Mines with row and column
        public int CountAdjacentMines(int row, int col)
        {
            int count = 0;
            try{
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int x = row + dx;
                        int y = col + dy;

                        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize && grid[x, y] == '*')
                        {
                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
       
            return count;
        }
        //To display grid to user in console 
        public void DisplayGrid()
        {
            Console.WriteLine("  " + string.Join(" ", new string[gridSize]));
            try{
                for (int i = 0; i < gridSize; i++)
                {
                    Console.Write((char)('A' + i) + " ");
                    for (int j = 0; j < gridSize; j++)
                    {

                        if (isRevealed[i, j])
                        {
                            _logger.Information("Revealed row and column in DisplayGrid method are " + i + " and " + j);
                            Console.Write(grid[i, j] + " ");
                        }
                        else
                        {
                            Console.Write("_ ");
                        }
                    }
                    Console.WriteLine();
                }
            }            
            catch (Exception ex)
            {

                _logger.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
         
        }
        // Play Game method allows user to input the squar to reveal mines and handle the inputs
        public void PlayGame()
        {
            try{
                while (!isGameOver)
                {
                    DisplayGrid();
                    Console.WriteLine("Select a cell to reveal (e.g. A1): ");
                    string input = Console.ReadLine().ToUpper();

                    if (input.Length < 2 || input.Length > 3 || input[0] < 'A' || input[0] >= 'A' + gridSize ||
                        input[1] < '1' || input[1] >= '1' + gridSize)
                    {
                        Console.WriteLine("Invalid input.");
                        continue;
                    }

                    int row = input[0] - 'A';
                    int col = input[1] - '1';

                    if (isRevealed[row, col])
                    {
                        Console.WriteLine("Cell has already revealed.");
                        continue;
                    }

                    if (grid[row, col] == '*')
                    {
                        isGameOver = true;
                        Console.WriteLine("Game Over! You lose!");
                        DisplayGrid();
                        break;
                    }

                    RevealCell(row, col);
                    CheckStatus();
                }

                if (!isGameOver)
                {
                    Console.WriteLine("Yayy! Congratulations! You win!");
                    DisplayGrid();
                }
            }
            catch (Exception ex)
            {

                _logger.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
         
        }
        // To reveal cell with it's row and column
        public void RevealCell(int row, int col)
        {
            _logger.Information("Row and Column in RevealCell method are " + row + " and " + col);
            isRevealed[row, col] = true;
            try{
                if (grid[row, col] == '_')
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            int x = row + dx;
                            int y = col + dy;
                            if (x >= 0 && x < gridSize && y >= 0 && y < gridSize && !isRevealed[x, y])
                            {
                                RevealCell(x, y);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
       
        }
        //To check the current status whether game ovar or not
        public void CheckStatus()
        {
            int revealedCount = 0;
            try{
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        _logger.Information("Cell revealing in CheckStatus method at " + i + " and " + j);
                        if (isRevealed[i, j])
                        {
                            revealedCount++;
                        }
                    }
                }
                _logger.Information("Revealed Count in CheckStatus method at " + revealedCount);
                if (revealedCount == gridSize * gridSize - minesCount)
                {
                    isGameOver = true;
                }
            }
            catch (Exception ex)
            {

                _logger.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
         
        }
    }

}
