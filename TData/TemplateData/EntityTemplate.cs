using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    //Contains all Attributes. Is filled with data from XMl-Files
    public class EntityTemplate : AbstractTemplate
    {
        //Name for the Picture
        public String TextureName;
        [ContentSerializerAttribute(Optional = true)]
        public int Score;
        public float Structure;
        public float Mass;        
        public float Size;
        public float Inertia;
        public float ExplosionSize;
    }
}
