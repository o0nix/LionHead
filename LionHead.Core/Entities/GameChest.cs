using LionHead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LionHead.Core.Entities
{
    public class GameChest
    {
        #region constructor
        public GameChest(List<LootTableItem> lootTable)
        {
            if (lootTable.Sum(li => li.DropChance) > 100) throw new ArgumentException(string.Format("Total drop chance over 100%."));
            LootTable = lootTable.OrderBy(li => li.ItemId).ToList();
        }
        public GameChest(string chestKey)
            : this(LootTableFromString(Decrypt(chestKey)))
        {
        }
        #endregion constructor

        #region public
        public readonly List<LootTableItem> LootTable;
        public string Open(int playerId)
        {
            string result = string.Empty;
            var numberGenerator = new Random(playerId);
            var randomNumber = numberGenerator.Next(1, 101);
            var currentPercentage = 0U;
            foreach (var item in LootTable)
            {
                if (randomNumber <= (currentPercentage + item.DropChance))
                {
                    result = item.ItemId;
                    break;
                }
                currentPercentage += item.DropChance;
            }
            return result;
        }
        #endregion public

        #region private
        private static readonly byte[] _encriptionKey = Encoding.UTF8.GetBytes("12345678");
        private static readonly byte[] _IV = new byte[] { 11, 22, 33, 44, 55, 66, 77, 88 };

        private static readonly char[] _splitLootTableItems = new char[] { '|' };
        private static readonly char[] _splitLootItemValue = new char[] { ':' };

        public static string Encrypt(string inputString)
        {
            byte[] byteInput = Encoding.UTF8.GetBytes(inputString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            var memStream = new MemoryStream();
            ICryptoTransform transform = provider.CreateEncryptor(_encriptionKey, _IV);
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(byteInput, 0, byteInput.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memStream.ToArray(), Base64FormattingOptions.None);
        }
        public static string Decrypt(string inputString)
        {
            byte[] byteInput = new byte[inputString.Length];
            byteInput = Convert.FromBase64String(inputString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            var memStream = new MemoryStream();
            ICryptoTransform transform = provider.CreateDecryptor(_encriptionKey, _IV);
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(byteInput, 0, byteInput.Length);
            cryptoStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(memStream.ToArray());
        }
        public static List<LootTableItem> LootTableFromString(string lootTableString)
        {
            return lootTableString.Split(_splitLootTableItems).Select(li => {
                var lootTableItemValues = li.Split(_splitLootItemValue);
                return new LootTableItem(lootTableItemValues[0], byte.Parse(lootTableItemValues[1]));
            }).ToList();
        }
        public static string LootTableToString(List<LootTableItem> lootTable)
        {
            return string.Join(_splitLootTableItems[0].ToString(),
                               lootTable.Select(li => string.Concat(li.ItemId, _splitLootItemValue[0], li.DropChance)));
        }
        #endregion
    }
}
