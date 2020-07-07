using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;

public class user
{
	[PrimaryKey]
	public int id { get; set; }
	public int money { get; set; }
	public int score { get; set; }
	public int level_user { get; set; }
	public int exp { get; set; }
	public int pro_account { get; set; }
}

public class animal_in_user
{
	[PrimaryKey]
	public int animal_in_user_id { get; set; }
	public int user_id { get; set; }
	public int animal_id { get; set; }
	public int current { get; set; }
}

public class animals
{
	[PrimaryKey]
	public int animals_id { get; set; }
	public string name { get; set; }
	public int cost { get; set; }
}

