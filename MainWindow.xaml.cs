using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlayerXTurn = true;
        private Button[] cells = new Button[9];


        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Grab all buttons from the UniformGrid
            cells = new Button[9];
            int index = 0;

            foreach (var child in LogicalTreeHelper.GetChildren(this))
            {
                if (child is Grid grid)
                {
                    foreach (var gridChild in grid.Children)
                    {
                        if (gridChild is UniformGrid uniformGrid)
                        {
                            foreach (var btn in uniformGrid.Children)
                            {
                                if (btn is Button button)
                                {
                                    button.Content = "";
                                    button.IsEnabled = true;
                                    button.Background = Brushes.White;
                                    cells[index++] = button;
                                }
                            }
                        }
                    }
                }
            }

            isPlayerXTurn = true;
            StatusText.Text = "Player X's Turn";
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button == null || !string.IsNullOrEmpty(button.Content.ToString()))
                return;

            button.Content = isPlayerXTurn ? "X" : "O";
            button.Foreground = isPlayerXTurn ? Brushes.Blue : Brushes.Red;
            button.IsEnabled = false;

            if (CheckForWinner())
            {
                StatusText.Text = $"Player {(isPlayerXTurn ? "X" : "O")} Wins!";
                DisableAllCells();
            }
            else if (IsDraw())
            {
                StatusText.Text = "It's a Draw!";
            }
            else
            {
                isPlayerXTurn = !isPlayerXTurn;
                StatusText.Text = $"Player {(isPlayerXTurn ? "X" : "O")}'s Turn";
            }
        }

        private bool CheckForWinner()
        {
            int[,] winConditions = new int[,]
            {
                {0,1,2}, {3,4,5}, {6,7,8}, // Rows
                {0,3,6}, {1,4,7}, {2,5,8}, // Columns
                {0,4,8}, {2,4,6}           // Diagonals
            };

            for (int i = 0; i < winConditions.GetLength(0); i++)
            {
                int a = winConditions[i, 0];
                int b = winConditions[i, 1];
                int c = winConditions[i, 2];

                if (!string.IsNullOrEmpty(cells[a].Content.ToString()) &&
                    cells[a].Content.ToString() == cells[b].Content.ToString() &&
                    cells[a].Content.ToString() == cells[c].Content.ToString())
                {
                    // Highlight winning cells
                    cells[a].Background = cells[b].Background = cells[c].Background = Brushes.LightGreen;
                    return true;
                }
            }

            return false;
        }

        private bool IsDraw()
        {
            foreach (var cell in cells)
            {
                if (string.IsNullOrEmpty(cell.Content.ToString()))
                    return false;
            }
            return true;
        }

        private void DisableAllCells()
        {
            foreach (var cell in cells)
            {
                cell.IsEnabled = false;
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
        }
    }
}