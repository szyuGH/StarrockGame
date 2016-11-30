using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    //Contains all Attributes. Is filled with data from XMl-Files
    public class EntityTemplateData
    {
        //Name for the Picture
        public String TextureName;
        //Name for the actual Entity
        public String EntityName;
        public float Structure;
        public float Mass;        
        public float Scale;
        public float Inertia;
    }
}
