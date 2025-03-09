using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TravelingSquares
{
    public sealed partial class MainPage : Page
    {
        public double resolution;

        private ViewModel vm;

        public MainPage()
        {
            this.InitializeComponent();
        }
        
        private void ResolutionSlider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {            
            if (resolution > vm.Resolution)
            {
                resolution = vm.Resolution;
                vm.Init(reInitArray2D: true);
            }
            else 
            {
                resolution = vm.Resolution;
                vm.Init(reInitArray2D: false);
            }

            vm.Draw();
        }        

        private void DotSizeSlider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            vm.HideAllDots();
            vm.DrawDots();
        }

        private void LineThickness_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            vm.HideLines();
            vm.DrawLines();
        }

        private void DrawingCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new ViewModel(DrawingCanvas);

            DataContext = vm;
            resolution = vm.Resolution;

            vm.Draw();
        }
    }
}
