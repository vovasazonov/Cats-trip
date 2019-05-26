using UnityEngine;
using System; //
using System.Collections;
using System.Collections.Generic;  // Lists

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

public class ServerMongoDB : MonoBehaviour, IDatabaseable
{
	private string _connectionString = "mongodb://localhost:27017";
	private MongoDatabase _database = null;

	// Start is called before the first frame update
	void Start()
    {
		// Start server
		_database = new MongoClient(_connectionString).GetServer().GetDatabase("catstripdb");
		Debug.Log("ESTABLISHED CONNECTION TO MONGODB");
	}



}
