Feature: Calculator

Scenario: Test SeaWars
	Given I start a game with random rival with randomly setted ships
	When I play seaWars and save game results as 'gameResultMessage' and 'isWin'
	Then I see results of the game based on 'gameResultMessage' and 'isWin'

