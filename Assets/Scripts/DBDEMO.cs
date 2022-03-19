using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class DBDEMO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DisplayWords();
    }

    public void DisplayWords()
    {
        string dbName = "URI=file:Assets/WordsDB.db";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using(var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Words";

                using(IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log("Words:" + reader["Name"] );
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        

    }


    
}
