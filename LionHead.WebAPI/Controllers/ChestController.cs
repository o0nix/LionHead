using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LionHead.WebAPI.Controllers
{
    public class ChestController : ApiController
    {
        private IGameAPI _gameAPI;

        public ChestController(IGameAPI gameRepository)
        {
            _gameAPI = gameRepository;
        }

        [HttpGet]
        public string Loot(int playerId, string chestKey)
        {
            var chest = _gameAPI.GetChest(chestKey);

            var item = chest.Open(playerId);

            if (!string.IsNullOrWhiteSpace(item)) _gameAPI.LogMessage(string.Format("{0} found a {1}", playerId, item));
            else _gameAPI.LogMessage(string.Format("{0} found an empty chest", playerId));

            return item;
        }
    }
}
