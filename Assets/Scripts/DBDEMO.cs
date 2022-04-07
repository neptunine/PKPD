using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

namespace Utility
{
    public class DBDEMO : MonoBehaviour
    {
        // Start is called before the first frame update

        public string[] GetWords(int topic,int quantity)
        {
            //string dbName = "URI=file:Assets/WordsDB.db";
            string dbName = $"Data Source={Application.streamingAssetsPath}/data.db";
            string[] wordarray = new string[quantity];
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"select * from Words WHERE GenreID = {topic} order by RANDOM() LIMIT {quantity};";

                    using (IDataReader reader = command.ExecuteReader())
                    { 
                        for (int i=0;reader.Read(); i++)
                        {
                            wordarray[i] = (string)reader["Name"];
                    
                    }
                        reader.Close();
                    }
                }
                connection.Close();
            }
            return wordarray;

        }

      




    }

}
