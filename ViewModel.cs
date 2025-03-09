using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace TravelingSquares
{
    public class ViewModel : INotifyPropertyChanged
    {
        private bool isBusy = false;
        private double resolution = 30;
        private int whiteDotSize = 20;
        private int blackDotSize = 20;
        private bool showDots = true;
        private bool showWhiteDots = true;
        private bool showBlackDots = true;
        private bool showLines = true;
        private int lineThickness = 1;        
        private int[,] Array2D;
        private double canvasWidth;
        private double canvasHeight;
        private int columns;
        private int rows;
        private Canvas canvas;

        public RelayCommand NewModelCommand { get; }

        public RelayCommand ShowDotsCommand { get; }

        public RelayCommand ShowWhiteDotsCommand { get; }

        public RelayCommand ShowBlackDotsCommand { get; }

        public RelayCommand ShowLinesCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(Canvas canvas)
        {
            this.canvas = canvas;

            canvasWidth = canvas.ActualWidth;
            canvasHeight = canvas.ActualHeight;

            NewModelCommand = new RelayCommand((param) =>
            {
                Init();
                Draw();
            });

            ShowDotsCommand = new RelayCommand((param) =>
            {
                if ((bool)param) DrawDots();
                else HideAllDots();
            });

            ShowWhiteDotsCommand = new RelayCommand((param) =>
            {
                if ((bool)param) DrawDots();
                else
                {
                    HideAllDots();
                    DrawDots();
                }
            });

            ShowBlackDotsCommand = new RelayCommand((param) =>
            {
                if ((bool)param) DrawDots();
                else
                {
                    HideAllDots();
                    DrawDots();
                }
            });

            ShowLinesCommand = new RelayCommand((param) =>
            {
                if ((bool)param) DrawLines();
                else HideLines();
            });

            Init();
        }

        public void Init(bool reInitArray2D = true)
        {
            IsBusy = true;

            try
            {
                columns = 1 + (int)Math.Floor(canvasWidth / resolution);
                rows = 1 + (int)Math.Floor(canvasHeight / resolution);

                if (reInitArray2D)
                {
                    Array2D = new int[columns, rows];

                    var random = new Random();
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            Array2D[c, r] = random.Next(2);
                        }
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void Draw()
        {
            canvas.Children.Clear();
            canvas.Background = new SolidColorBrush(Colors.LightGray);

            DrawDots();
            DrawLines();
        }

        public void DrawLine(Point p1, Point p2)
        {
            var line = new Line();
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;
            line.Stroke = new SolidColorBrush(Colors.Black);
            line.StrokeThickness = LineThickness;
            canvas.Children.Add(line);
        }

        public void DrawDots()
        {
            IsBusy = true;

            try
            {
                for (int c = 0; c < columns; c++)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        if (Array2D[c, r] == 0 && !ShowBlackDots) continue;
                        if (Array2D[c, r] == 1 && !ShowWhiteDots) continue;

                        double xoffset = c * resolution;
                        double yoffset = r * resolution;

                        var circle = new Ellipse();
                        if (Array2D[c, r] == 0)
                        {
                            circle.Stroke = new SolidColorBrush(Colors.Black);
                            circle.StrokeThickness = resolution * BlackDotSize / 100;
                        }
                        else
                        {
                            circle.Stroke = new SolidColorBrush(Colors.White);
                            circle.StrokeThickness = resolution * WhiteDotSize / 100;
                        }

                        canvas.Children.Add(circle);
                        Canvas.SetTop(circle, yoffset - circle.StrokeThickness * 0.5);
                        Canvas.SetLeft(circle, xoffset - circle.StrokeThickness * 0.5);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void HideAllDots()
        {
            IsBusy = true;

            try
            {
                var list = new List<UIElement>();
                foreach (UIElement child in canvas.Children)
                {
                    if (child is Ellipse) list.Add(child);
                }

                foreach (UIElement child in list)
                {
                    canvas.Children.Remove(child);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void DrawLines()
        {
            IsBusy = true;

            try
            {
                for (int column = 0; column < columns - 1; column++)
                {
                    for (int row = 0; row < rows - 1; row++)
                    {
                        double xoffset = column * resolution;
                        double yoffset = row * resolution;

                        var a = new Point(xoffset + resolution * 0.5, yoffset);
                        var b = new Point(xoffset + resolution, yoffset + resolution * 0.5);
                        var c = new Point(xoffset + resolution * 0.5, yoffset + resolution);
                        var d = new Point(xoffset, yoffset + resolution * 0.5);

                        switch (GetState(Array2D[column, row], Array2D[column + 1, row], Array2D[column + 1, row + 1], Array2D[column, row + 1]))
                        {
                            case 1:
                                DrawLine(c, d);
                                break;
                            case 2:
                                DrawLine(b, c);
                                break;
                            case 3:
                                DrawLine(b, d);
                                break;
                            case 4:
                                DrawLine(a, b);
                                break;
                            case 5:
                                DrawLine(a, d);
                                DrawLine(b, c);
                                break;
                            case 6:
                                DrawLine(a, c);
                                break;
                            case 7:
                                DrawLine(a, d);
                                break;
                            case 8:
                                DrawLine(a, d);
                                break;
                            case 9:
                                DrawLine(a, c);
                                break;
                            case 10:
                                DrawLine(a, b);
                                DrawLine(c, d);
                                break;
                            case 11:
                                DrawLine(a, b);
                                break;
                            case 12:
                                DrawLine(b, d);
                                break;
                            case 13:
                                DrawLine(b, c);
                                break;
                            case 14:
                                DrawLine(c, d);
                                break;
                        }
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }

        }

        // TODO: can this be turned into a generic method?
        //       HideLines() and HideDots() are almost identical
        public void HideLines()
        {
            IsBusy = true;

            try
            {
                var list = new List<UIElement>();
                foreach (UIElement child in canvas.Children)
                {
                    if (child is Line) list.Add(child);
                }

                foreach (UIElement child in list)
                {
                    canvas.Children.Remove(child);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public int GetState(int a, int b, int c, int d)
        {
            return a * 8 + b * 4 + c * 2 + d * 1;
        }

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool IsBusy
        {
            get { return isBusy; }
            private set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        public double Resolution
        {
            get { return resolution; }
            set
            {
                resolution = value;
                OnPropertyChanged();
            }
        }

        public int WhiteDotSize
        {
            get { return whiteDotSize; }
            set 
            {
                whiteDotSize = value;
                OnPropertyChanged();
            }
        }

        public int BlackDotSize
        {
            get { return blackDotSize; }
            set 
            {
                blackDotSize = value;
                OnPropertyChanged();
            }
        }

        public bool ShowDots
        {
            get { return showDots; }
            set
            {
                showDots = value;
                OnPropertyChanged();
            }
        }

        public bool ShowWhiteDots
        {
            get { return showWhiteDots; }
            set 
            { 
                showWhiteDots = value;
                OnPropertyChanged();
            }
        }

        public bool ShowBlackDots
        {
            get { return showBlackDots; }
            set 
            {
                showBlackDots = value;
                OnPropertyChanged();
            }
        }

        public bool ShowLines
        {
            get { return showLines; }
            set 
            {
                showLines = value;
                OnPropertyChanged();
            }
        }
        
        public int LineThickness
        {
            get { return lineThickness; }
            set
            {
                lineThickness = value;
                OnPropertyChanged();
            }
        }
    }
}
