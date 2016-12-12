using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace THC
{
    public class Template
    {
        public string TemplateName;
        public string TemplatePath;
        public int Checksum;

        public Template(string path)
        {
            TemplatePath = path;
            TemplateName = Path.GetFileNameWithoutExtension(path);
        }

        public override string ToString()
        {
            return TemplateName;
        }

        internal void Calculate()
        {
            XDocument doc = XDocument.Load(TemplatePath);
            doc.Descendants("Checksum").Remove();
            string xml = doc.ToString();
            Checksum = xml.GetHashCode();
            XElement node = doc.Descendants("Asset").FirstOrDefault();
            node.Add(new XElement("Checksum", Checksum));
            doc.Save(TemplatePath);
        }
    }
}
