using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.GUI;
using System.IO;
using TData.TemplateData;
using StarrockGame.Caching;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneCheckTemplates : Scene
    {
        Gauge checkingGauge;
        Label captionLabel;
        Label checkingLabel;

        Menu menu;


        IEnumerable<string> files;
        IEnumerator<string> fileIterator;
        AbstractTemplate currentTemplate;
        bool failed;
        bool isExiting;

        private string checkingLabelCaption
        {
            get
            {
                return currentTemplate?.Name ?? "";
            }
        }

        public SceneCheckTemplates(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            files = from fullFileName
                    in Directory.EnumerateFiles(@"Content\Data\Templates")
                    select Path.GetFileNameWithoutExtension(fullFileName);
            fileIterator = files.GetEnumerator();

            int width = Device.Viewport.Width - 128;
            int height = 48;
            int x = (Device.Viewport.Width - width)/2;
            int y = (Device.Viewport.Height - height)/2;
            Rectangle bounding = new Rectangle(x,y, width, height);
            checkingGauge = new Gauge(files.Count(), bounding, Color.Red);

            menu = new Menu(Cache.LoadFont("MenuFont"), null);

            checkingLabel = new Label(menu, "", new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height * .5f), 1, Color.White)
            {
                CaptionMonitor = () => { return checkingLabelCaption; }
            };

            captionLabel = new Label(menu, "Checking Template Integrity", new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height * .5f - 48), 1.25f, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (isExiting)
                return;
            if (failed)
            {
                System.Windows.Forms.MessageBox.Show("There are corrupted templates in your game folder!");
                SceneManager.Exit();
                isExiting = true;
                return;
            }

            CheckFiles();
            checkingGauge.Value++;
            checkingLabel.Update(gameTime, false);

            if (checkingGauge.Full)
            {
                SceneManager.Set<SceneTitle>();
            }
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            SpriteBatch.Begin();
            checkingGauge.Render(SpriteBatch);
            checkingLabel.Render(SpriteBatch);
            captionLabel.Render(SpriteBatch);
            SpriteBatch.End();
        }



        private void CheckFiles()
        {
            if (fileIterator.MoveNext())
            {
                currentTemplate = Cache.LoadTemplate<AbstractTemplate>(fileIterator.Current);

                if (!CheckCurrentTemplate())
                {
                    failed = true;
                }
            }
            else
            {
                currentTemplate = null;
            }
            
        }

        private bool CheckCurrentTemplate()
        {
            XmlSerializer xmlserializer = new XmlSerializer(currentTemplate.GetType());
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            xmlserializer.Serialize(ms, currentTemplate);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string xmlString = sr.ReadToEnd();
            ms.Dispose();
            sr.Dispose();
            

            XDocument doc = XDocument.Parse(xmlString);
            doc.Descendants("Checksum").Remove();
            doc.Descendants(currentTemplate.GetType().Name).First().RemoveAttributes();
            string xml = doc.ToString();
            int checksum = GenerateHashCode(xml);
            return currentTemplate.Checksum == checksum;
        }

        private int GenerateHashCode(string s)
        {
            int res = 13;
            unchecked
            {
                for (int i = 0; i < s.Length; i++)
                {
                    res += (int)(s[i] + 757) * (i + 211);
                }
            }
            return res;
        }

        private IEnumerable<string> NextFile()
        {
            foreach (string file in files)
            {
                yield return file;
            }
        }
    }
}
