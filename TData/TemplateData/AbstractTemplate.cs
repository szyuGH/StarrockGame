using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public abstract class AbstractTemplate
    {
        public string Name;
        public string File; // the same as the file the xml is in


        public override bool Equals(object obj)
        {
            if (obj is AbstractTemplate)
                return Name.Equals((obj as AbstractTemplate).Name);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
