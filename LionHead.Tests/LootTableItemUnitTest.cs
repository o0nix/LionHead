using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LionHead.Tests
{
    [TestClass]
    public class LootTableItemUnitTest
    {
        [TestMethod]
        public void GivenAnItemAndDropChanceOfZero_WhenConstructANewItem_ThenItemIsConstructed()
        {
            //Arrange
            var itemName = "Test Item";
            var dropChance = (byte)0;
            
            //Act
            var item = new LootTableItem(itemName, dropChance);

            //Assert
            Assert.IsNotNull(item);
            Assert.AreEqual(itemName, item.ItemId);
            Assert.AreEqual(dropChance, item.DropChance);
        }

        [TestMethod]
        public void GivenAnItemAndDropChanceOf100_WhenConstructANewItem_ThenItemIsConstructed()
        {
            //Arrange
            var itemName = "Test Item";
            var dropChance = (byte)100;

            //Act
            var item = new LootTableItem(itemName, dropChance);

            //Assert
            Assert.IsNotNull(item);
            Assert.AreEqual(itemName, item.ItemId);
            Assert.AreEqual(dropChance, item.DropChance);
        }

        [TestMethod]
        public void GivenAnItemAndDropChanceOver101_WhenConstructANewItem_ThenExceptionIsThrown()
        {
            //Arrange
            var itemName = "Test Item";
            var dropChance = (byte)101;

            Exception result = null;
            //Act
            try
            {
                new LootTableItem(itemName, dropChance);
            }
            catch (Exception ex)
            {
                result = ex;
            }
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ArgumentOutOfRangeException), result.GetType());
        }
    }
}
