Feature: Chest Items
	As a player
	I want to open a chest and receive an item
	So that I can use the item in the game

Background: 
	Given I have a player

@smoke-test @player @chest @loot
Scenario: Receive item from loot table when only one item in loot table
	Given a configured loot table: 
	    | Item               | Drop chance |
	    | Sword              | 100         |
	When I roll on this loot table
	Then I receive a sword from the loot table
	And a log is written with the players username and received item

@player @chest @loot
Scenario: Receive item from loot table when multiple drop chances
	Given a configured loot table: 
	    | Item               | Drop chance |
	    | Sword              | 10          |
	    | Shield             | 10          |
	    | Health Potion      | 30          |
	    | Resurrection Phial | 30          |
	    | Scroll of wisdom   | 20          |
	When I roll on this loot table
	Then I receive a random item from the loot table
	And a log is written with the players username and received item


@player @chest @loot @empty-chest
Scenario: No item from chest when empty loot table
	Given a configured loot table: 
		| Item               | Drop chance |
	When I roll on this loot table
	Then the chest is empty
	And a log is written with the players username and that the chest was empty
