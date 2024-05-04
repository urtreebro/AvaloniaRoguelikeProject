using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaRoguelike.Model.GameObjects
{
    public abstract class Loot
    {
        int condition;
        //TODO: ? - if picked and ? - if not????

        public Loot(int condition) 
        {
            this.condition = condition;
        }

        public int Condition { get; set; }
    }
}
