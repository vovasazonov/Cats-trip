using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
	public static IDatabaseable Database;

	#region Variables
	private static Server _instance;
	#endregion

	/// <summary>
	/// Destroy object if there are object
	/// that exist. Do not destroy first object.
	/// </summary>
	void Awake()
	{
		// There are not object yet.
		if (_instance == null)
		{
			_instance = this;
		}
		// There are object that exist.
		// Delete this object.
		else if (_instance != this)
		{
			Destroy(gameObject);
		}

		// On load another scene, don't destroy object.
		DontDestroyOnLoad(gameObject);
	}

	public void SetDataBase(string nameDatabase)
	{
		switch (nameDatabase)
		{
			case "MongoDB":
				Database = new ServerMongoDB();
				break;
			case "MariaDB":
				Database = new ServerMariaDB();
				break;
			case "MySQL":
				Database = new ServerMySQL();
				break;
			case "MSSQL":
				Database = new ServerMSSQL();
				break;
			default:
				break;
		}

		Database.RunServer();

		// Go to game scene.
		SceneManager.LoadScene("MainMenu");
	}
}
