using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MySql.Data.MySqlClient;
using StarrockGame.Caching;
using StarrockGame.GUI;
using StarrockGame.InputManagement;
using StarrockGame.SceneManagement.Popups;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneLeaderboard : Scene
    {
        const float RETRIEVE_INTERVAL = 10; // in seconds

        internal const string ConnectionString = "SERVER=sql7.freesqldatabase.com;DATABASE=sql7148244;UID=sql7148244;PASSWORD=ImvNHhy7Ld;";
            

        
        Table table;
        Menu menu;
        Label returnLabel;
        Label changeDiffLabel;
        bool initialized;

        private SessionDifficulty currentDifficulty = SessionDifficulty.Easy;

        string ReturnText
        {
            get { return string.Format("Press \"{0}\" to return to the title screen.", Input.Device.InputTypeName(KeyboardInputType.MenuCancel)); }
        }
        string ChangeDifficultyText
        {
            get { return string.Format("Press \"{0}\" and \"{1}\" to changed difficulty.", 
                Input.Device.InputTypeName(KeyboardInputType.MenuLeft), Input.Device.InputTypeName(KeyboardInputType.MenuRight)); }
        }

        float retrieveTimer = RETRIEVE_INTERVAL;
        

        public SceneLeaderboard(Game1 game) : base(game)
        {
        }

        

        public override void Initialize()
        {
            SpriteFont tableFont = Cache.LoadFont("TableFont");
            SpriteFont menuFont = Cache.LoadFont("MenuFont");

            // setup table
            table = new Table(tableFont, new Rectangle(10, 10, 500, 20));
            table.AddColumn("Rank", 80)
                .AddColumn("Name", 200)
                .AddColumn("Ship", 240)
                .AddColumn("Difficulty", 160)
                .AddColumn("Time", 130)
                .AddColumn("Score", 130);
            table.Data = RetrieveLeaderboard(currentDifficulty);

            int cx = (Device.Viewport.Width - table.Bounding.Width) / 2;
            int cy = (Device.Viewport.Height - table.RealHeight) / 2;
            table.Move(cx, cy);


            menu = new Menu(menuFont, () => { SceneManager.Return(); });
            returnLabel = new Label(menu,
                ReturnText,
                new Vector2(100, table.Bounding.Y + table.RealHeight + 80),
                1,
                Color.White, 0);
            returnLabel.CaptionMonitor = () => { return ReturnText; };

            changeDiffLabel = new Label(menu,
                ChangeDifficultyText,
                new Vector2(100, table.Bounding.Y + table.RealHeight + 80 + menuFont.LineSpacing),
                1,
                Color.White, 0);
            changeDiffLabel.CaptionMonitor = () => { return ChangeDifficultyText; };

            initialized = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!initialized)
                return;

            if (Input.Device.MenuCancel())
            {
                SceneManager.Return();
            }
            if (Input.Device.MenuUp())
                table.ScrollUp();
            else if (Input.Device.MenuDown())
                table.ScrollDown();
            else if (Input.Device.MenuLeft())
            {
                if (currentDifficulty == SessionDifficulty.Easy)
                    currentDifficulty = SessionDifficulty.Lost;
                else
                    currentDifficulty--;
                retrieveTimer = 0;
            }
            else if (Input.Device.MenuRight())
            {
                if (currentDifficulty == SessionDifficulty.Lost)
                    currentDifficulty = SessionDifficulty.Easy;
                else
                    currentDifficulty++;
                retrieveTimer = 0;
            }

            if (retrieveTimer <= 0)
            {
                table.Data = RetrieveLeaderboard(currentDifficulty);
                retrieveTimer = RETRIEVE_INTERVAL;
            }
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);
            
            SpriteBatch.Begin();
            table?.Render(SpriteBatch);
            menu?.Render(SpriteBatch);
            SpriteBatch.End();
        }

        private List<Dictionary<string, string>> RetrieveLeaderboard(SessionDifficulty difficulty)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            
            MySqlConnection connection = new MySqlConnection(ConnectionString);

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
            connection.Close();
            return result;
        }
    }
}
