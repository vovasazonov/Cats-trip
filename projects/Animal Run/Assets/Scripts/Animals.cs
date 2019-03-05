using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals {

    //do default values before using object 
    public Animals()
    {
        SetAnimalsInShop();
        SetCostAnimals();
    }

    //contains the id of each animal
    public enum animals
    {
        grayCat = 0,
        pinkRabbit = 1,
        brownHamster = 2
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

        animalsInShop.Add((int)animals.grayCat);        
        animalsInShop.Add((int)animals.pinkRabbit);       
        animalsInShop.Add((int)animals.brownHamster);
        
    }

    /// <summary>
    /// set the cost of each animal (fabric method)
    /// </summary>
    private void SetCostAnimals()
    {
        costAnimals = new Dictionary<int, int>();

        costAnimals[(int)animals.grayCat] = costAnimal.grayCat;
        costAnimals[(int)animals.pinkRabbit] = costAnimal.pinkRabbit;
        costAnimals[(int)animals.brownHamster] = costAnimal.brownHamster;
    }

    private struct costAnimal
    {
        public const int grayCat = 0;
        public const int pinkRabbit = 1000;
        public const int brownHamster = 7000;
    }

    
}
