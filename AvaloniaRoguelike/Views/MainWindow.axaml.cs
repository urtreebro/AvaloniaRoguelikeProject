using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

using AvaloniaRoguelike.Model;
using AvaloniaRoguelike.ViewModels;

namespace AvaloniaRoguelike.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Keyboard.Keys.Add(e.Key);
            base.OnKeyDown(e);
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            //Keyboard.Keys.Remove(e.Key);
            Keyboard.Keys.Clear();
            base.OnKeyUp(e);
        }
    }
}