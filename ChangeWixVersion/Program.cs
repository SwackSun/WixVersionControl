using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ChangeWixVersion
{
    class Program
    {
        static int Main(string[] args)
        {
            //获取版本信息
            string[] ReadText = File.ReadAllLines(args[0], Encoding.Default);
            string versionfile = "";
            string version = "";
            foreach (string item in ReadText)
            {
                if(!item.Contains("//"))
                {
                    if (item.Contains("AssemblyVersion"))
                    {
                        version = item.Substring(item.IndexOf("(\"")+2, item.Length - item.IndexOf("(\"") - 2 - 3);
                        int version_short_len = version.IndexOf(".", version.IndexOf(".") + 1);
                        version = version.Substring(0, version_short_len);
                    }
                    if (item.Contains("AssemblyFileVersion"))
                    {
                        versionfile = item.Substring(item.IndexOf("(\"") + 2, item.Length - item.IndexOf("(\"") - 2 - 3);
                    }
                }
            }
            //改变属性的值
            XmlDocument doc = new XmlDocument();
            doc.Load(args[1]);
            XmlElement root = doc.DocumentElement;
            XmlNodeList xnl = root.ChildNodes;
            foreach (XmlNode item in xnl)
            {
                //if(item.Attributes["Id"].Value.Equals("Version"))
                //{
                //    item.InnerText= versionfile;
                //}
                if (item.Name.Equals("define"))
                {
                    if(item.InnerText.IndexOf("ProductVersion")>=0)
                    {
                        item.InnerText= "ProductVersion=\""+ versionfile + "\"";
                    }
                }
            }
            doc.Save(args[1]);
            return 0;
        }
    }
}
