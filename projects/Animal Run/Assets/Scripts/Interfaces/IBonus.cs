/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Contains values and methods that must be 
/// realise in bonus clases.
/// </summary>
public interface IBonus
{
	// Name of bonus.
	BonusName Name { get; }

	// Contains the levels of each feature of bonus.
	Dictionary<BonusFeatures, int> LevelFeature { get; set; }

	#region BonusFeatures that must be ralise in each bonus
	// Lifetime bonus on the player when 
	// he picked it up in the game.
	float Lifetime { get; set; }
	// A chance to appearance the bonus in the game.
	float Chance { get; set; }
	#endregion

	/// <summary>
	/// Destroy bonus after the time out or
	/// after met a new same bonus.
	/// </summary>
	void DestroyBonus();
	/// <summary>
	/// When the player collide with this bonus,
	/// the bonus set as a child of player in hierarchy.
	/// </summary>
	void SetAsChildOfPlayer();
	/// <summary>
	/// Hide the sprite of bonus when collide with player.
	/// </summary>
	void HideSprite();
	/// <summary>
	/// Describes the bonus operation algorithm.
	/// </summary>
	void BonusAction();
}
