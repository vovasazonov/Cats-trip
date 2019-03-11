///*
//*	Copyright (c) NromaGames
//*	Developer Sazonov Vladimir (Emilio) 
//*	Email : futureNroma@yandex.ru
//*/

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MoveLocation : MonoBehaviour
//{

//	#region Variables
//	#endregion

//	#region Unity Methods
//	void Start()
//	{

//	}


//	void Update()
//	{

//	}

//	#endregion

//	/// <summary>
//	/// Create random material on area
//	/// </summary>
//	/// <param name="danElem"></param>
//	/// <param name="withCoin"></param>
//	/// <returns></returns>
//	GameObject GetRandomMaterial(bool danElem, bool withCoin)
//	{
//		GameObject material = null;
//		// True if position already has material
//		bool alreadyMat = false;

//		if (!alreadyMat && Random.Range(0, 3) == 0)
//		{
//			material = Instantiate(_emptyObj);
//			alreadyMat = true;
//		}

//		if (danElem && !alreadyMat)
//		{
//			material = Instantiate(_matDanLoc[_location - 1][
//				Random.Range(0, _matDanLoc[_location - 1].Count)]);
//			alreadyMat = true;
//		}
//		else if (!alreadyMat)
//		{
//			material = Instantiate(_matSafLoc[_location - 1][
//				Random.Range(0, _matSafLoc[_location - 1].Count)]);
//			alreadyMat = true;
//		}

//		return material;
//	}

//	/// <summary>
//	/// Create random road that
//	/// gamer can run without
//	/// dangerous materials
//	/// </summary>
//	/// <param name="posClear"></param>
//	/// <param name="danPosAdd"></param>
//	void RandomClearRoad(ref Vector2 posClear, ref bool[] danPosAdd)
//	{
//		int maxChanceY = 5;

//		switch ((int)posClear.x)
//		{
//			case -2:
//				if (Random.Range(0, maxChanceY) == 0)
//					posClear.y = 1;
//				else
//				{
//					if (!danPosAdd[1])
//						posClear.x = -1;
//					else
//						posClear.y = 1;
//				}
//				break;

//			case 2:
//				if (Random.Range(0, maxChanceY) == 0)
//					posClear.y = 1;
//				else
//				{
//					if (!danPosAdd[3])
//						posClear.x = 1;
//					else
//						posClear.y = 1;

//				}
//				break;

//			case 1:
//				if (Random.Range(0, maxChanceY) == 0)
//				{
//					posClear.y = 1;
//				}
//				else if (Random.Range(0, 3) == 0)
//				{
//					if (!danPosAdd[4])
//						posClear.x = 2;
//				}
//				else if (Random.Range(0, 3) == 0)
//					if (!danPosAdd[2])
//						posClear.x = 0;
//					else
//						posClear.y = 1;
//				break;

//			case -1:
//				if (Random.Range(0, maxChanceY) == 0)
//					posClear.y = 1;

//				else if (Random.Range(0, 3) == 0)
//				{
//					if (!danPosAdd[0])
//						posClear.x = -2;
//				}
//				else if (Random.Range(0, 2) == 0)
//					if (!danPosAdd[2])
//						posClear.x = 0;
//					else
//						posClear.y = 1;
//				break;

//			case 0:
//				if (Random.Range(0, maxChanceY) == 0)
//					posClear.y = 1;
//				else if (Random.Range(0, 3) == 0)
//				{
//					if (!danPosAdd[1])
//					{
//						posClear.x = -1;
//					}
//				}
//				else if (Random.Range(0, 2) == 0)
//					if (!danPosAdd[3])
//						posClear.x = 1;
//					else
//						posClear.y = 1;
//				break;

//			default:
//				break;
//		}
//	}

//	/// <summary>
//	/// Add five pieces of materials in line
//	/// </summary>
//	/// <param name="numberLine"></param>
//	/// <param name="location"></param>
//	/// <param name="posClear"></param>
//	/// <param name="withDanElem"></param>
//	void AddMaterialInLine(float numberLine, short location, ref Vector2 posClear, bool withDanElem = true)
//	{
//		// Position of piece 
//		Vector3 posInMethod;
//		bool[] danPosAdd = new bool[5];

//		for (int i = 0; i < 5; i++)
//		{
//			danPosAdd[i] = false;
//		}

//		for (int column = -2; column != 3; column++)
//		{
//			// Create safety material
//			if (!withDanElem || posClear.x == column || Random.Range(0, 2) == 0)
//			{
//				_material = GetRandomMaterial(false, true);

//				if (posClear.y == 1 && posClear.x == column)
//					posClear.y = 0;
//			}
//			// Create dangerous materials
//			else
//			{
//				_material = GetRandomMaterial(true, true);
//				danPosAdd[column + 2] = true;

//			}
//			// Create position of material
//			posInMethod = new Vector3(column, numberLine, 0);
//			// Transform material to position
//			_material.transform.position = posInMethod;
//			// Add new material to list
//			_piecesMaterials.Add(_material);
//		}

//		// Create new position road where player can move safety
//		RandomClearRoad(ref posClear, ref danPosAdd);
//	}

//	/// <summary>
//	/// Add five pieces of area in line
//	/// </summary>
//	/// <param name="numberLine"></param>
//	/// <param name="location"></param>
//	void AddAreaInLine(float numberLine, short location)
//	{
//		// Number of location in list
//		short locInList = (short)(location - 1);
//		// Position of piece 
//		Vector3 posInMethod;
//		// Piece of area
//		GameObject area;

//		// Create a left pieces
//		area = Instantiate(_areasLoc[locInList][0]);
//		posInMethod = new Vector3(-2, numberLine, 0);
//		area.transform.position = posInMethod;
//		_piecesAreas.Add(area);

//		// Create a center pieces
//		for (int column = -1; column != 2; column++)
//		{
//			area = Instantiate(_areasLoc[locInList][1]);
//			posInMethod = new Vector3(column, numberLine, 0);
//			area.transform.position = posInMethod;
//			_piecesAreas.Add(area);
//		}

//		// Create a right pieces
//		area = Instantiate(_areasLoc[locInList][2]);
//		posInMethod = new Vector3(2, numberLine, 0);
//		area.transform.position = posInMethod;
//		_piecesAreas.Add(area);
//	}


//}
