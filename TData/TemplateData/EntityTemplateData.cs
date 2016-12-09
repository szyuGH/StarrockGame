using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    //Contains all Attributes. Is filled with data from XMl-Files
    public class EntityTemplateData : TemplateData
    {
        //Name for the Picture
        public String TextureName;        
        public float Structure;
        public float Mass;        
        public float Size;
        public float Inertia;
       
    }
}
