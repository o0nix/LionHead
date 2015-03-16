using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionHead.Tests
{
    [TestClass]
    public class GameChestUnitTest
    {
        [TestMethod]
        public void GivenAMessage_WhenIEncryptAndDecryptTheMessage_ThenDecryptedMessageIsSameAsOriginalMessage()
        {
            //Arrange
            var message = "Sword:10|Shield:10|Health Potion:30|Resurrection Phial:30|Scroll of wisdom:20";
            //Act
            var encryptedString = HttpUtility.UrlEncode(GameChest.Encrypt(message));
            var decryptedString = GameChest.Decrypt(HttpUtility.UrlDecode(encryptedString));
            //Assert
            Assert.AreEqual(message, decryptedString);
        }

        [TestMethod]
        public void GivenAListOfLootTableItemsAsString_WhenConvertToListAndBackToString_ThenConvertedStringIsSameAsOriginalString()
        {
            //Arrange
            var lootTable = "1:10|2:10|3:30|4:30|5:20";
            //Act
            var lootTableAsList = GameChest.LootTableFromString(lootTable);
            var lootTableAsString = GameChest.LootTableToString(lootTableAsList);
            //Assert
            Assert.AreEqual(lootTable, lootTableAsString);
        }

        [TestMethod]
        public void GivenAListOfLootTableItemsAsList_WhenConvertToStringAndBackToList_ThenConvertedListIsSameAsOriginalList()
        {
            //Arrange
            var lootTable = new List<LootTableItem>();
            lootTable.Add(new LootTableItem("Sword", 11));
            lootTable.Add(new LootTableItem("Shield", 8));
            lootTable.Add(new LootTableItem("Health Potion", 27));
            lootTable.Add(new LootTableItem("Resurrection Phial", 33));
            lootTable.Add(new LootTableItem("Scroll of wisdom", 21));
            //Act
            var lootTableAsString = GameChest.LootTableToString(lootTable);
            var lootTableAsList = GameChest.LootTableFromString(lootTableAsString);
            //Assert
            Assert.AreEqual(lootTable.Count, lootTableAsList.Count);
            foreach(var lootTableItemConverted in lootTableAsList)
            {
                Assert.IsTrue(lootTable.Any(li => li.ItemId == lootTableItemConverted.ItemId 
                                            &&  li.DropChance == lootTableItemConverted.DropChance));
            }
        }

        [TestMethod]
        public void GivenAListOfLootTableItemsAsString_WhenICreateAGameChest_ThenTheChestIsConstructed()
        {
            //Arrange
            var lootTable = "1:10|2:10|3:30|4:30|5:20";
            //Act
            var chest = new GameChest(GameChest.Encrypt(lootTable));
            //Assert
            Assert.IsNotNull(chest);
            Assert.IsNotNull(chest.LootTable);
            var lootTableList = GameChest.LootTableFromString(lootTable);
            Assert.AreEqual(lootTableList.Count(), chest.LootTable.Count());
            foreach (var lootTableItemChest in chest.LootTable)
            {
                Assert.IsTrue(lootTableList.Any(li => li.ItemId == lootTableItemChest.ItemId
                                            && li.DropChance == lootTableItemChest.DropChance));
            }
        }

        [TestMethod]
        public void GivenAChestWithNoItemsInLootTable_WhenIOpenTheChest_ThenNoItemIsGiven()
        {
            //Arrange
            var chest = new GameChest(new List<LootTableItem>());
            //Act
            var item = chest.Open(1);
            //Assert
            Assert.AreEqual(string.Empty, item);
        }

        [TestMethod]
        public void GivenAChestWithOneItemWithDropChanceZero_WhenIOpenTheChest_ThenNoItemIsGiven()
        {
            //Arrange
            var itemSword = new LootTableItem("Sword", 0);
            var chest = new GameChest(new List<LootTableItem>() { itemSword });
            //Act
            var item = chest.Open(1);
            //Assert
            Assert.AreEqual(string.Empty, item);
        }

        [TestMethod]
        public void GivenAChestWithOneItemWith100DropChance_WhenIOpenTheChest_ThenItemIsAlwaysGiven()
        {
            //Arrange
            var itemSword = new LootTableItem("Sword", 100);
            var chest = new GameChest(new List<LootTableItem>() { itemSword });

            for (var i = 0; i < 100; i++)
            {
                //Act
                var item = chest.Open(i);
                //Assert
                Assert.IsFalse(string.IsNullOrWhiteSpace(item));
                Assert.AreEqual(itemSword.ItemId, item);
            }
        }

        [TestMethod]
        public void GivenAChestWithMultipleItems_WhenIOpenTheChest_ThenItemIsBasedOnDistribution()
        {
            //Arrange
            var lootTable = new List<LootTableItem>();
            lootTable.Add(new LootTableItem("Sword", 11));
            lootTable.Add(new LootTableItem("Shield", 8));
            lootTable.Add(new LootTableItem("Health Potion", 27));
            lootTable.Add(new LootTableItem("Resurrection Phial", 33));
            lootTable.Add(new LootTableItem("Scroll of wisdom", 21));

            var chest = new GameChest(lootTable);

            var sampleSize = 10000;
            var itemFrequency = new Dictionary<string, int>();
            foreach (var item in chest.LootTable) itemFrequency[item.ItemId] = 0;
            //Act

            for (var i = 0; i < sampleSize; i++) itemFrequency[chest.Open(i)]++;

            //Assert
            foreach (var item in chest.LootTable) 
                Assert.AreEqual(item.DropChance,
                          (byte)Math.Round(itemFrequency[item.ItemId] * 100d / sampleSize, 0, MidpointRounding.AwayFromZero));
        }

        [TestMethod]
        public void GivenAChestWithMultipleItemsNotFullCoverage_WhenIOpenTheChest_ThenItemIsBasedOnDistribution()
        {
            //Arrange
            //drop chance sum of 79
            var lootTable = new List<LootTableItem>();
            lootTable.Add(new LootTableItem("Sword", 11));
            lootTable.Add(new LootTableItem("Shield", 8));
            lootTable.Add(new LootTableItem("Health Potion", 27));
            lootTable.Add(new LootTableItem("Resurrection Phial", 33));
            var chest = new GameChest(lootTable);

            var sampleSize = 10000;
            var itemFrequency = new Dictionary<string, int>();
            foreach (var item in chest.LootTable) itemFrequency[item.ItemId] = 0;
            var emptyChestFrequency = 0;
            //Act

            for (var i = 0; i < sampleSize; i++) {
                var item = chest.Open(i);
                if(!string.IsNullOrWhiteSpace(item)) itemFrequency[item]++; 
                else emptyChestFrequency++;
            } 

            //Assert
            Assert.AreEqual(100 - chest.LootTable.Sum(i => i.DropChance),
                           (byte)Math.Round(emptyChestFrequency * 100d / sampleSize, 0, MidpointRounding.AwayFromZero));

            foreach (var item in chest.LootTable) 
                Assert.AreEqual(item.DropChance,
                          (byte)Math.Round(itemFrequency[item.ItemId] * 100d / sampleSize, 0, MidpointRounding.AwayFromZero));
        }
    }
}
