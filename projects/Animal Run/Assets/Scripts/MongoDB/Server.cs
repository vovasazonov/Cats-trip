using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
	public static IDatabaseable Database;

	public void SetDataBase(string nameDatabase)
	{
		switch (nameDatabase)
		{
			case "MongoDB":
				Database = new ServerMongoDB();
				break;
			case "MariaDB":
				break;
			case "MySQL":
				break;
			case "MSSQL":
				break;
			default:
				break;
		}

		Database.RunServer();
		// Удалить это после теста
		Database.LoadData();
		DataPlayer data = new DataPlayer();
		data.Coins = 23342;
		Database.SaveData(data);

		// Переключиться на игровую сцену.
	}
}
