In order to demo the service please use:
---------------------------------------------- 

http://[BASEURL]/api/chest?chestKey=TCX5D9hHSG3oS%2b9qFN9C6YQnJ6STblFH8cHgGma%2fh53h%2bdWqOKGIKwPSZjG1u9m%2bYFbIAHn3P3q6eAWLGbEhi4v5KoA%2fe02PXBKvJ5EGRaE%3d&playerId=1

*ChestKey is an encrypted string representing the loot table:

Item                   Drop chance 
Sword                 10              
Shield                  10              
Health Potion        30              
Resurrection Phial  30              
Scroll of wisdom    20    

*PlayerId is just a number. By changing the player id the chest generates random items          
		
		
To generate get the encrypted chest key use:
---------------------------------------------- 

http://[BASEURL]/Api/EncryptChest?lootTable=[LootTable]

LootTable item is defined as: <itemName>:<dropChance>
LootTable list is define as: <item1>|<item2>|<item3>

Example LootTable: Sword:10|Shield:10|Health%20Potion:30

Example Url: http://[BASEURL]/Api/EncryptChest?lootTable=Sword:10|Shield:10|Health%20Potion:30



