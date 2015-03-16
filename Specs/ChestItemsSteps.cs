using LionHead;
using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using LionHead.WebAPI;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TechTalk.SpecFlow;

namespace Specs
{
    [Binding]
    public class ChestItemsSteps
    {
        private const string GameAPIMockKey = "GameAPIMock";
        private const string PlayerKey = "Player";
        private const string ChestKey = "Chest";
        private const string ItemKey = "Item";

        private static string _baseUrl = "http://localhost:9999/";

        private static IDisposable _service;

        [BeforeFeature]
        public static void InitializeFeatureTest()
        {
            _service = WebApp.Start<Startup>(url: _baseUrl);
        }

        [BeforeScenario]
        public static void InitializeScenarioTest() 
        {
            var gameAPIMock = new Mock<IGameAPI>();
            UnityConfig.RegisteredTypes.RegisterInstance<IGameAPI>(gameAPIMock.Object);
            ScenarioContext.Current.Set<Mock<IGameAPI>>(gameAPIMock, GameAPIMockKey);
        }

        [AfterFeature]
        public static void CleanUp()
        {
            _service.Dispose();
        }

        [Given(@"I have a player")]
        public static void GivenIHaveAPlayer()
        {
            var player = 1;
            ScenarioContext.Current.Set<int>(player, PlayerKey);
        }

        [Given(@"a configured loot table:")]
        public static void GivenAConfiguredLootTable(Table table)
        {
            var lootTable = new List<LootTableItem>();
            foreach (var row in table.Rows)
            {
                lootTable.Add(new LootTableItem(row[0], byte.Parse(row[1])));
            }
            var chest = new GameChest(lootTable);
            var gameAPIMock = ScenarioContext.Current.Get<Mock<IGameAPI>>(GameAPIMockKey);

            gameAPIMock.Setup(x => x.GetChest(It.IsAny<string>())).Returns(chest);

            ScenarioContext.Current.Set<GameChest>(chest, ChestKey);
        }

        [When(@"I roll on this loot table")]
        public static void WhenIRollOnThisLootTable()
        {
            using(var client = new HttpClient())
            {
                var player = ScenarioContext.Current.Get<int>(PlayerKey);
                var chest = ScenarioContext.Current.Get<GameChest>(ChestKey);

                var chestKey = GameChest.Encrypt(GameChest.LootTableToString(chest.LootTable));
                var encodedChestKey = HttpUtility.UrlEncodeUnicode(chestKey);
                var resp = client.GetAsync(string.Format("{0}api/chest?playerId={1}&chestKey={2}", ChestItemsSteps._baseUrl, 
                                                                                                    player,
                                                                                                    encodedChestKey)).Result;

                var item = resp.Content.ReadAsAsync<string>().Result;

                ScenarioContext.Current.Set<string>(item, ItemKey);
            }
        }

        [Then(@"I receive a random item from the loot table")]
        public static void ThenIReceiveARandomItemFromTheLootTable()
        {
            var item = ScenarioContext.Current.Get<string>(ItemKey);
            var chest = ScenarioContext.Current.Get<GameChest>(ChestKey);

            Assert.IsNotNull(item);
            Assert.IsNotNull(chest.LootTable.FirstOrDefault(x => x.ItemId == item));
        }

        [Then(@"I receive a sword from the loot table")]
        public void ThenIReceiveASwordFromTheLootTable()
        {
            var item = ScenarioContext.Current.Get<string>(ItemKey);
            Assert.IsNotNull(item);
            Assert.AreEqual("Sword", item);
        }

        [Then(@"the chest is empty")]
        public void ThenTheChestIsEmpty()
        {
            Assert.IsTrue(string.IsNullOrWhiteSpace(ScenarioContext.Current.Get<string>(ItemKey)));
        }

        [Then(@"a log is written with the players username and received item")]
        public static void ThenALogIsWrittenWithThePlayersUsernameAndReceivedItem()
        {
            var player = ScenarioContext.Current.Get<int>(PlayerKey);
            var item = ScenarioContext.Current.Get<string>(ItemKey);

            var gameAPIMock = ScenarioContext.Current.Get<Mock<IGameAPI>>(GameAPIMockKey);
            gameAPIMock.Verify(x => x.LogMessage(string.Format("{0} found a {1}", player, item)), Times.Once);
        }

        [Then(@"a log is written with the players username and that the chest was empty")]
        public void ThenALogIsWrittenWithThePlayersUsernameAndThatTheChestWasEmpty()
        {
            var player = ScenarioContext.Current.Get<int>(PlayerKey);

            var gameAPIMock = ScenarioContext.Current.Get<Mock<IGameAPI>>(GameAPIMockKey);
            gameAPIMock.Verify(x => x.LogMessage(string.Format("{0} found an empty chest", player)), Times.Once);
        }

    }
}

