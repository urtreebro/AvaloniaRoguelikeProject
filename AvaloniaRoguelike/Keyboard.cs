using System.Collections.Generic;
using System.Linq;

using Avalonia.Input;

namespace AvaloniaRoguelike
{
    static class Keyboard
    {
        public static readonly HashSet<Key> Keys = [];

        public static bool IsKeyDown(Key key) 
            => Keys.Contains(key);

        public static Key LastKeyPressed()
            => Keys.LastOrDefault();
    }
}