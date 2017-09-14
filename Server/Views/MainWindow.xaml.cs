using System.Windows;
using System.Windows.Controls;
using Server.Core;
using Server.ViewModels;

namespace Server
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ServerViewModel _viewModel;

        public MainWindow()
        {
            _viewModel = new ServerViewModel(new ServerTcp(), 'X');
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