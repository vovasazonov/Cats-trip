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

    //here are animals that for sale in shop
    private List<int> animalsInShop;
    public List<int> AnimalsInShop
    {
        get
        {
            return animalsInShop;
        }
    }

    //here the cost of each animal
    private Dictionary<int, int> costAnimals;
    public Dictionary<int, int> CostAnimals
    {
        get
        {
            return costAnimals;
        }
    }

    /// <summary>
    /// set all animals that will be in shop (fabric method)
    /// </summary>
    private void SetAnimalsInShop()
    {
        animalsInShop = new List<int>();

        animalsInShop.Add((int)Animal.GrayCat);        
        animalsInShop.Add((int)Animal.PinkRabbit);       
        animalsInShop.Add((int)Animal.BrownHamster);
        
    }

    /// <summary>
    /// set the cost of each animal (fabric method)
    /// </summary>
    private void SetCostAnimals()
    {
        costAnimals = new Dictionary<int, int>();

        costAnimals[(int)Animal.GrayCat] = costAnimal.grayCat;
        costAnimals[(int)Animal.PinkRabbit] = costAnimal.pinkRabbit;
        costAnimals[(int)Animal.BrownHamster] = costAnimal.brownHamster;
    }

    private struct costAnimal
    {
        public const int grayCat = 0;
        public const int pinkRabbit = 1000;
        public const int brownHamster = 7000;
    }

    
}
