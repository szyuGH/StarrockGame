using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TData.TemplateData;

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

        internal void CalculateTest()
        {
            XDocument doc = XDocument.Load(TemplatePath);
            doc.Descendants("Checksum").Remove();
            string templateType = doc.Descendants("Asset").First().Attribute("Type").Value.Split('.').Last();

            XDocument tmpDoc = new XDocument();
            XElement templateElement = new XElement(templateType);
            tmpDoc.Add(templateElement);
            List<XElement> children = doc.Descendants("Asset").First().Descendants().ToList();
            templateElement.Add(children);

            MemoryStream ms2 = new MemoryStream() { Position = 0 };
            tmpDoc.Save(ms2);
            ms2.Position = 0;

            SpaceshipTemplate template;
            XmlSerializer ser = new XmlSerializer(typeof(SpaceshipTemplate));
            template = (SpaceshipTemplate)ser.Deserialize(ms2);

            Checksum = CC(template);

            XElement node = doc.Descendants("Asset").FirstOrDefault();
            node.AddFirst(new XElement("Checksum", Checksum));
            doc.Save(TemplatePath);
        }

        internal int CC(AbstractTemplate currentTemplate)
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
            return GenerateHashCode(xml);
        }

        internal void Calculate()
        {
            XmlSerializer ser = new XmlSerializer(typeof(AsteroidTemplate));


            XDocument doc = XDocument.Load(TemplatePath);
            doc.Descendants("Checksum").Remove();
            string templateType = doc.Descendants("Asset").First().Attribute("Type").Value.Split('.').Last();

            XDocument tmpDoc = new XDocument();
            XElement templateElement = new XElement(templateType);
            tmpDoc.Add(templateElement);
            templateElement.Add(doc.Descendants("Asset").First().Descendants());

            string xml = tmpDoc.ToString();
            Checksum = GenerateHashCode(xml);
            XElement node = doc.Descendants("Asset").FirstOrDefault();
            node.AddFirst(new XElement("Checksum", Checksum));
            doc.Save(TemplatePath);
        }

        private int GenerateHashCode(string s)
        {
            int res = 13;
            unchecked
            {
                for (int i = 0; i< s.Length;i++)
                {
                    res += (int)(s[i]+757)*(i+211);
                }
            }
            return res;
        }
    }
}
