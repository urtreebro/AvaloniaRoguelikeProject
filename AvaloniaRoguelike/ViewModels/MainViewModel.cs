using AvaloniaRoguelike.Model;
using ReactiveUI;

namespace AvaloniaRoguelike.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Game _game;

        public MainViewModel()
        {
            var field = new GameField(0);
            _game = new Game(field);
            _game.Start();
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