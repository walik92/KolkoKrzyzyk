using System.Windows;
using System.Windows.Controls;
using Client.Core;
using Client.ViewModels;

namespace Client
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ClientViewModel _viewModel;

        public MainWindow()
        {
            _viewModel = new ClientViewModel(new ClientTcp(), 'O');
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void ClickButton(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var numerPola = int.Parse(button.Tag.ToString());
            _viewModel.Select(numerPola);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Connect();
        }
    }
}