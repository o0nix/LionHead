using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionHead.Core
{
    public class DummyGameAPI : IGameAPI
    {
        public void LogMessage(string Message)
        {
            //Do nothing
        }

        public GameChest GetChest(string chestKey)
        {
            return new GameChest(chestKey);
        }
    }
}
