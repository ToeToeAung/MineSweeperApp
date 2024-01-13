using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperApp
{ 
    public static class MSPConstants 
    {
        public static string welcomeMsg = "Welcome to Minesweeper!";
        public static string gridSizePromptMsg = "Enter the size of the grid (minimum is 2, maximum is 10): ";
        public static string mineNumberToPlaceMsg  = "Enter the number of mines to place on the grid(maximum is 35% of the total squares):";
        public static string incorrectInputMsg =  "Incorrect input. Please enter a valid number.";
        public static string selectSquareToRevealMsg = "Select a square to reveal (e.g. A1): "; 
    }
}
