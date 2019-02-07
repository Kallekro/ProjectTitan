using System;
using System.Collections;
using System.Data.SQLite;
using System.IO;

namespace ProjectTitan
{
    public class DB_Manager
    {
        private SQLiteConnection m_dbconn;
        
        public DB_Manager()
        {
           
        }

        public bool CreateSaveDB(string db_name)
        {
            if ( File.Exists("Src/DB/DB_Content/db_saves/" + db_name + ".sqlite") == false )
            {
                this.m_dbconn = new SQLiteConnection(String.Format("Data Source=Src/DB/DB_Content/db_saves/{0}.sqlite;Version=3;", db_name));
                m_dbconn.Open();

                string create_tables_sql = ReadFileToString("Src/DB/DB_Content/sql_scripts/generate_db.sql");
                SQLiteCommand cdb_cmd = new SQLiteCommand
                {
                    CommandText = create_tables_sql,
                    Connection = m_dbconn
                };
                cdb_cmd.ExecuteNonQuery();
                m_dbconn.Close();
                return true;
            }
            else {
                return false;
            } 
        }

        public bool DeleteSaveDB(string db_name)
        {
            string file = "Src/DB/DB_Content/db_saves/" + db_name + ".sqlite";
            if (File.Exists(file)) {
                File.Delete(file);
                return true;
            } else {
                return false;
            }
        }

        public bool LoadDB(string db_name)
        {
            if (File.Exists("Src/DB/DB_Content/db_saves/" + db_name + ".sqlite") == true) 
            {
                this.m_dbconn = new SQLiteConnection(String.Format("Data Source=Src/DB/DB_Content/db_saves/{0}.sqlite;Version=3;", db_name));
                return true;
            }
            else 
            {
                return false;
            }
        }


        /// Manipulate dabase
        public bool InsertIntoTable(string table_name, string columns, ArrayList values)
        {
            if (m_dbconn != null) {
                m_dbconn.Open();

                // construct sql insert statement with given arguments
                string vals = "";
                for (int i = 0; i < values.Count - 1; i++) { vals += String.Format("@{0},", i); }
                vals += "@" + (values.Count - 1);
                string insert_sqltext = String.Format("INSERT INTO {0} {1} VALUES ({2})", table_name, columns, vals);

                // create new sqlite-command and add values to parameters
                SQLiteCommand insert_sql = new SQLiteCommand(insert_sqltext, m_dbconn);
                for (int i = 0; i < values.Count; i++) {
                    insert_sql.Parameters.AddWithValue("@" + i, values[i]);
                }
                insert_sql.ExecuteNonQuery();

                // close connection
                m_dbconn.Close();
                return true;
             } 
             else {
                return false;
             }
        }

        /// <summary>
        /// Loads all default data into the database-schema.
        /// Data-files are defined in DB_Content/data/
        /// </summary>
        public void LoadDataIntoDB()
        {
            if (m_dbconn != null)
            {
                // READ AND LOAD TEAMS DATA
                string teams_data_path = "Src/DB/DB_Content/data/teams.data";
                var teams_data_lines = File.ReadAllLines(teams_data_path);
                foreach (var line in teams_data_lines)
                {
                    string[] values = line.Split(',');
                    ArrayList args = new ArrayList
                {
                    Int32.Parse(values[0]), // team_id
                    values[1],              // name
                    values[2],              // team_abbr
                    values[3],              // nationality
                    Int32.Parse(values[4])  // budget
                };
                    InsertIntoTable("Teams", "(team_id,name,name_abbr,nationality,budget)", args);
                }

                // READ AND LOAD RIDER DATA
                string riders_data_path = "Src/DB/DB_Content/data/riders.data";
                var riders_data_lines = File.ReadAllLines(riders_data_path);
                foreach (var line in riders_data_lines)
                {
                    string[] values = line.Split(',');
                    ArrayList args = new ArrayList
                {
                    Int32.Parse(values[0]), // rider_id
                    values[1],              // name
                    values[2],              // birthday
                    Int32.Parse(values[3]), // salary
                    values[4],              // rider type
                    values[5],              // nationality
                    Int32.Parse(values[6])  // team_id -> Teams.team_id
                };
                    InsertIntoTable("Riders", "(rider_id,name,birthday,salary,rider_type,nationality,team_id)", args);
                }
            }

        }


        /// Helper methods
        public string ReadFileToString(string file_name)
        {
            if (!File.Exists(file_name)) {
                return "";
            }
            else
            {
                return File.ReadAllText(file_name);
            }

        }

    }
}
