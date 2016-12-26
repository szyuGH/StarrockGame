using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using TData.TemplateData;
using System.Linq;
using System.Xml.Serialization;

namespace CSC
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private string location = @"..\..\..\..\Content\";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            List<string> files = Directory.EnumerateFiles("Content").Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
            foreach (string file in files)
            {
                AbstractTemplate template = Content.Load<AbstractTemplate>("" + file);
                string formatted = null;
                using (MemoryStream ms = new MemoryStream() { Position = 0 })
                {
                    XmlSerializer ser = new XmlSerializer(template.GetType());
                    template.Checksum = 7;
                    ser.Serialize(ms, template);
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        formatted = sr.ReadToEnd().Replace("\r\n","");
                    }

                }
                int checksum = GenerateHashCode(formatted);
                XDocument doc = XDocument.Load(location + file + ".xml");
                XElement checksumElement = doc.Descendants("Checksum").FirstOrDefault();
                if (checksumElement != null)
                {
                    checksumElement.Value = checksum.ToString();
                }
                else
                {
                    doc.Descendants("Asset").First().AddFirst(new XElement("Checksum") { Value = checksum.ToString() });
                }
                doc.Save(location + file + ".xml");
            }
            
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

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
