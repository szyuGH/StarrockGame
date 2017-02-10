using MySql.Data.MySqlClient;
using StarrockGame.SceneManagement;
using StarrockGame.SceneManagement.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame
{
    internal static class Dao
    {
        private const string CONNECTION_STRING = "SERVER=sql11.freesqldatabase.com;DATABASE=sql11158055;UID=sql11158055;PASSWORD=Z7jiIpPeuG;";

        internal static bool CheckAvailability()
        {
            MySqlConnection connection = new MySqlConnection(CONNECTION_STRING);
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception)
            {
                SceneManager.CallPopup<PopupNoLBConnection>();
            }
            return false;
        }

        internal static void PushSessionData()
        {
            MySqlConnection connection=null;
            try
            {
                connection = new MySqlConnection(CONNECTION_STRING);
                MySqlCommand command = connection.CreateCommand();
                // todo must be updated when db is up
                command.CommandText = string.Format("INSERT INTO leaderboard (player_name, ship_name, difficulty, time, score) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    Player.Get().Name,
                    SessionManager.UsedShipTemplate.Name,
                    SessionManager.Difficulty,
                    SessionManager.ElapsedTime.TotalMilliseconds,
                    SessionManager.Score);

                connection.Open();
                command.ExecuteNonQuery();

            } catch (Exception e)
            {
            }
            finally
            {
                connection?.Close();
            }
        }

        internal static List<Dictionary<string, string>> RetrieveLeaderboard(SessionDifficulty difficulty)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            MySqlConnection connection = null;

            try
            {
                connection = new MySqlConnection(CONNECTION_STRING);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT * FROM leaderboard where difficulty={0} order by score desc, time desc", (int)difficulty);
                MySqlDataReader Reader;
                connection.Open();
                Reader = command.ExecuteReader();
                int rank = 1;
                while (Reader.Read())
                {
                    string playerName = Reader.GetString(0);
                    string shipName = Reader.GetString(1);
                    long durationInMS = Reader.GetInt64(3);
                    int score = Reader.GetInt32(4);

                    result.Add(new Dictionary<string, string>() {
                    { "Rank", rank.ToString() },
                    { "Name", playerName },
                    { "Ship", shipName },
                    { "Difficulty", (difficulty).ToString() },
                    { "Time", string.Format("{0:hh\\:mm\\:ss}",TimeSpan.FromMilliseconds(durationInMS)) },
                    { "Score", score.ToString() },

                });
                    rank++;
                }
            }
            catch (Exception) { }
            finally
            {
                connection.Close();
            }
            return result;
        }
    }
}
