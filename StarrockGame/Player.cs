using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;

namespace StarrockGame
{
    public class Player
    {
        

        private int _credits;
        public int Credits
        {
            get { return _credits; }
            set { _credits = value; }
        }

        private List<AbstractTemplate> templates;

        public string Name { get; private set; }

        private int playerIndex;
        private string filePath;

        private Player(int index)
        {
            playerIndex = index;
            filePath = string.Format(@"{0}\{1}\PlayerData\Player{2}.urd", 
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                playerIndex);
            Load();
        }

        private void Initialize()
        {
            this.Credits = 0;
            this.Name = Environment.UserName;
            this.templates = new List<AbstractTemplate>();
            UnlockTemplates("Spaceship"); 
        }

        private void Load()
        {
            if (!File.Exists(filePath))
                Initialize();
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                PlayerData data;
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    data = (PlayerData)formatter.Deserialize(fs);
                }

                this.Credits = data.Credits;
                this.Name = data.PlayerName;
                this.templates = new List<AbstractTemplate>(data.UnlockedTemplates.Select(s => Cache.LoadTemplate<AbstractTemplate>(s)));
            }
        }

        public void Save()
        {
            PlayerData data = new PlayerData();
            data.Credits = Credits;
            data.PlayerName = Name;
            data.UnlockedTemplates = templates.Select(t => t.File).ToArray();

            FileInfo fi = new FileInfo(filePath);
            DirectoryInfo di = fi.Directory;
            if (!di.Exists) di.Create();

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(fs, data);
            }
        }

        public void UnlockTemplates(params string[] templates)
        {
            foreach (string template in templates)
            {
                AbstractTemplate data = Cache.LoadTemplate<AbstractTemplate>(template);
                if (!this.templates.Contains(data))
                {
                    this.templates.Add(data);
                }
            }
            Save();
        }

        public List<T> GetTemplates<T>() where T : AbstractTemplate
        {
            return templates.Where(t => t is T).Cast<T>().ToList();
        }

        #region setup Multiton
        const int MAX_PLAYER_INSTANCES = 1;
        private static Player[] instances = new Player[MAX_PLAYER_INSTANCES];
        public static Player Get(int index = 0)
        {
            if (instances[index] == null)
                instances[index] = new Player(index);
            return instances[index];
        }
        #endregion
    }
}
