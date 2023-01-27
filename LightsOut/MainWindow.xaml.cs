using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LightsOut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LightsOutGame game;
        private About aboutForm;

        public MainWindow()
        {
            
        }

        private void CreateGrid()
        {
            int rectSize = (int)boardCanvas.Width / game.GridSize;
            // Create rectangles for grid
            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.White;
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;
                    rect.Stroke = Brushes.Black;
                    // Store each row and col as a Point
                    rect.Tag = new Point(r, c);
                    // Register event handler
                    rect.MouseLeftButtonDown += Rect_MouseLeftButtonDown;
                    // Put the rectangle at the proper location within the canvas
                    Canvas.SetTop(rect, r * rectSize);
                    Canvas.SetLeft(rect, c * rectSize);
                    // Add the new rectangle to the canvas' children
                    boardCanvas.Children.Add(rect);
                }
            }
            
        }

        private void Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            var rowCol = (Point)rect.Tag;
            int row = (int)rowCol.X;
            int col = (int)rowCol.Y;
            lblMoviments.Content = game.Move(row, col, Convert.ToInt32(lblMoviments.Content));
            DrawGrid();

            if (game.IsGameOver())
            {
                boardCanvas.Background = Brushes.Green;
                MessageBox.Show("You Win!!!","Playtech Games");
            }
        }

        private void DrawGrid()
        {
            
            BitmapImage theImageOff = new BitmapImage
               (new Uri(@"..\..\assets\PNG\IMAGE_off.png", UriKind.Relative));
            ImageBrush myImageOff = new ImageBrush(theImageOff);
            BitmapImage theImageOn = new BitmapImage
               (new Uri(@"..\..\assets\PNG\IMAGE_on.png", UriKind.Relative));
            ImageBrush myImageOn = new ImageBrush(theImageOn);

            int index = 0;

            // Set the colors of the rectangles
            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
                {
                    Rectangle rect = boardCanvas.Children[index] as Rectangle;
                    index++;
                    if (game.GetGridValue(r, c))
                    {
                        // On
                        rect.Fill = myImageOn;
                        rect.Stroke = myImageOff;
                    }
                    else
                    {
                        // Off
                        rect.Fill = myImageOff;
                        rect.Stroke = myImageOn;
                    }
                }
            }
        }

        private void MenuGame_New_Click(object sender, RoutedEventArgs e)
        {
            game.NewGame();
            boardCanvas.Background = Brushes.Transparent;
            lblMoviments.Content = 0;
            DrawGrid();
        }

        private void MenuGame_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuHelp_About_Click(object sender, RoutedEventArgs e)
        {
            aboutForm = new About();
            aboutForm.ShowDialog();
        }

        private void cbxLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lblMoviments != null) lblMoviments.Content = 0;
            InitializeComponent();
            
            ComboBoxItem typeItem = (ComboBoxItem)cbxLevel.SelectedItem;
            string level = typeItem.Content.ToString();
            int valueLevel = level == "Level Beginner" ? 3 : level == "Level 0" ? 4 :
                level == "Level 1" ? 5 : 9;

            boardCanvas.Children.Clear();

            game = new LightsOutGame(valueLevel);

            CreateGrid();
            boardCanvas.Background = Brushes.Transparent;
            
            DrawGrid();
            

        }
    }
}
