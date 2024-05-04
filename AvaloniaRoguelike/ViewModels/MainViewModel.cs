using AvaloniaRoguelike.Model;
using ReactiveUI;
using System.Xml.Linq;

namespace AvaloniaRoguelike.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Game _game;

        public MainViewModel(Game game)
        {
            _game = game;
        }


        public Game Game
        {
            get => _game;
            set
            {
                this.RaiseAndSetIfChanged(ref _game, value);
            }
        }
    }
}