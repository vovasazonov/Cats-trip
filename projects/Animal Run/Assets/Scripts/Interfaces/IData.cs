/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

/// <summary>
/// Contains methods that must realise in data classes.
/// </summary>
public interface IData
{
	/// <summary>
	/// Set defoult data in class. Like constructor.
	/// </summary>
	void SetDefoultData();

	/// <summary>
	/// Clone object data
	/// </summary>
	/// <returns>cloned object</returns>
	object Clone();
}