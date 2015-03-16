using LionHead.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionHead.Core.Interfaces
{
    public interface IGameAPI
    {
        void LogMessage(string Message);

        GameChest GetChest(string chestKey);
    }
}
