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

        private List<TemplateData> templates;

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
            // TODO: unlock default ship template for new player
            //UnlockTemplates(); 
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
                this.templates = new List<TemplateData>(data.UnlockedTemplates);
            }
        }

        public void Save()
        {
            PlayerData data = new PlayerData();
            data.Credits = Credits;
            data.PlayerName = Name;
            data.UnlockedTemplates = templates.ToArray();

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(fs, data);
            }
        }

        public void UnlockTemplates(params TemplateData[] templates)
        {
            foreach (TemplateData template in templates)
            {
                if (!this.templates.Contains(template))
                {
                    this.templates.Add(template);
                }
            }
            Save();
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
