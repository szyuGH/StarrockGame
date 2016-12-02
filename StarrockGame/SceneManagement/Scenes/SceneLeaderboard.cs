using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using StarrockGame.GUI;
using StarrockGame.InputManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneLeaderboard : Scene
    {
        const float RETRIEVE_INTERVAL = 10;

        Dictionary<string, string>[] MOCK_LEADERBOARD = new Dictionary<string, string>[]
        {
            new Dictionary<string, string>(){ { "Rank", "1" }, { "Name", "Szyu" } , { "Ship", "Omega Destroyer" }, { "Difficulty", "Lost" }, { "Time", "04:20:30" }, { "Score", "9000" }, },
            new Dictionary<string, string>(){ { "Rank", "2" }, { "Name", "Szyu2" }, { "Ship", "Omega Destroyer" }, { "Difficulty", "Lost" }, { "Time", "02:40:10" }, { "Score", "4000" }, },
            new Dictionary<string, string>(){ { "Rank", "3" }, { "Name", "Szyu3" }, { "Ship", "Omega Destroyer" }, { "Difficulty", "Lost" }, { "Time", "01:20:30" }, { "Score", "3500" }, },
            new Dictionary<string, string>(){ { "Rank", "4" }, { "Name", "Szyu4" }, { "Ship", "Omega Destroyer" }, { "Difficulty", "Lost" }, { "Time", "01:10:50" }, { "Score", "3000" }, },
            new Dictionary<string, string>(){ { "Rank", "5" }, { "Name", "Szyu5" }, { "Ship", "Omega Destroyer" }, { "Difficulty", "Lost" }, { "Time", "00:50:30" }, { "Score", "1500" }, },
            new Dictionary<string, string>(){ { "Rank", "6" }, { "Name", "Szyu6" }, { "Ship", "Omega Destroyer" }, { "Difficulty", "Lost" }, { "Time", "00:45:20" }, { "Score", "1480" }, },
            new Dictionary<string, string>(){ { "Rank", "7" }, { "Name", "Szyu7" }, { "Ship", "Omega Destroyer" }, { "Difficulty", "Lost" }, { "Time", "00:45:10" }, { "Score", "1480" }, },
            new Dictionary<string, string>(){ { "Rank", "8" }, { "Name", "Szyu8" }, { "Ship", "Omega Destroyer" }, { "Difficulty", "Lost" }, { "Time", "00:20:44" }, { "Score", "20" }, },
            new Dictionary<string, string>(){ { "Rank", "9" }, { "Name", "Szyu9" }, { "Ship", "Omega Destroyer" }, { "Difficulty", "Lost" }, { "Time", "00:00:30" }, { "Score", "5" }, },
        };
        
        Table table;
        Menu menu;
        Label returnLabel;

        string ReturnText
        {
            get { return string.Format("Press \"{0}\" to return to the title screen.", Input.Device.InputTypeName(KeyboardInputType.MenuCancel)); }
        }

        float retrieveTimer = RETRIEVE_INTERVAL;
        

        public SceneLeaderboard(Game1 game) : base(game)
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
            table.Data = RetrieveLeaderboard();

            int cx = (Device.Viewport.Width - table.Bounding.Width) / 2;
            int cy = (Device.Viewport.Height - table.RealHeight) / 2;
            table.Move(cx,cy);


            menu = new Menu(menuFont, () => { SceneManager.Return(); });
            returnLabel = new Label(menu, 
                ReturnText, 
                new Vector2(100, table.Bounding.Y + table.RealHeight + 80), 
                1,
                Color.White, 0);
            returnLabel.CaptionMonitor = () => { return ReturnText; };
        }

        public override void Update(GameTime gameTime)
        {
            if (retrieveTimer <= 0)
            {
                table.Data = RetrieveLeaderboard();
                retrieveTimer = RETRIEVE_INTERVAL;
            }

            if (Input.Device.MenuCancel())
            {
                SceneManager.Return();
            }
            if (Input.Device.MenuUp())
                table.ScrollUp();
            else if (Input.Device.MenuDown())
                table.ScrollDown();
            
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            SpriteBatch.Begin();
            table.Render(SpriteBatch);
            menu.Render(SpriteBatch);
            SpriteBatch.End();
        }

        private List<Dictionary<string, string>> RetrieveLeaderboard()
        {
            return MOCK_LEADERBOARD.ToList();
        }
    }
}
