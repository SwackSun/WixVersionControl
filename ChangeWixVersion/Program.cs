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
            //AssemblyInfo.cs获取版本信息
            string[] ReadText = File.ReadAllLines(args[0], Encoding.Default);
            string versionfile = "";
            string version = "";
            string product = "";
            foreach (string item in ReadText)
            {
                if(!item.Contains("//"))
                {
                    if (item.Contains("AssemblyVersion"))
                    {
                        version = item.Substring(item.IndexOf("(\"")+2, item.IndexOf("\")") - item.IndexOf("(\"") - 2);
                        //int version_short_len = version.IndexOf(".", version.IndexOf(".") + 1);
                        //version = version.Substring(0, version_short_len);
                    }
                    if (item.Contains("AssemblyFileVersion"))
                    {
                        versionfile = item.Substring(item.IndexOf("(\"") + 2, item.IndexOf("\")") - item.IndexOf("(\"") - 2);
                    }
                    if (item.Contains("AssemblyProduct"))
                    {
                        product = item.Substring(item.IndexOf("(\"") + 2, item.IndexOf("\")") - item.IndexOf("(\"") - 2);
                    }
                }
            }
            // .wxs 改变ProductVersion
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(args[1]);

            //使用命名空间
            XmlNamespaceManager nsMgr1 = new XmlNamespaceManager(doc1.NameTable);
            nsMgr1.AddNamespace("ns", "http://schemas.microsoft.com/wix/2006/wi");

            XmlElement root1 = doc1.DocumentElement;
            XmlNodeList xnl1 = root1.ChildNodes;
            foreach (XmlNode item in xnl1)
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
            doc1.Save(args[1]);

            // .wixproj 改变OutputName
            XmlDocument doc2 = new XmlDocument();
            doc2.Load(args[2]);

            //使用命名空间
            XmlNamespaceManager nsMgr2 = new XmlNamespaceManager(doc2.NameTable);
            nsMgr2.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlNode root2 = doc2.SelectSingleNode("/ns:Project/ns:PropertyGroup", nsMgr2);
            XmlNodeList xnl2 = root2.ChildNodes;
            foreach (XmlNode item in xnl2)
            {
                if (item.Name.Equals("ProductVersion"))
                {
                    item.InnerText = versionfile;
                }
                if (item.Name.Equals("OutputName"))
                {
                    item.InnerText = product+"_"+ versionfile;
                }
            }
            doc2.Save(args[2]);
            return 0;
        }
    }
}
