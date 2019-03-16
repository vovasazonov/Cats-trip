/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals {

    // Set default values before using object.
    public Animals()
    {
        SetAnimalsInShop();
        SetCostAnimals();
    }

    // Animals that are for sale in shop.
    private List<int> _animalsInShop;
    public List<int> AnimalsInShop
    {
        get
        {
            return _animalsInShop;
        }
    }

    // Show the cost of animal by name of animal.
    private Dictionary<int, int> _costAnimals;
    public Dictionary<int, int> CostAnimals
    {
        get
        {
            return _costAnimals;
        }
    }

    /// <summary>
    /// Set all animals that user can buy in shop (fabric method)
    /// </summary>
    private void SetAnimalsInShop()
    {
        _animalsInShop = new List<int>();

        _animalsInShop.Add((int)Animal.GrayCat);        
        _animalsInShop.Add((int)Animal.PinkRabbit);       
        _animalsInShop.Add((int)Animal.BrownHamster);
        
    }

    /// <summary>
    /// Set the cost of each animal to dictionary (fabric method)
    /// </summary>
    private void SetCostAnimals()
    {
        _costAnimals = new Dictionary<int, int>();

        _costAnimals[(int)Animal.GrayCat] = CostAnimal.grayCat;
        _costAnimals[(int)Animal.PinkRabbit] = CostAnimal.pinkRabbit;
        _costAnimals[(int)Animal.BrownHamster] = CostAnimal.brownHamster;
    }

	/// <summary>
	/// The cost of each animal.
	/// </summary>
    private struct CostAnimal
    {
        public const int grayCat = 0;
        public const int pinkRabbit = 1000;
        public const int brownHamster = 7000;
    }

    
}
