using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WixVersionControl
{
    class Program
    {
        static int Main(string[] args)
        {
            //获取操作项目类型 WPF of MFC
            string ReadType = args[0];
            //AssemblyInfo.cs获取版本信息
            string[] ReadText = File.ReadAllLines(args[1], Encoding.Default);
            string fileversion = "";
            string version = "";
            string productversion = "";
            string product = "";
            if(ReadType.Equals("WPF"))
            {
                foreach (string item in ReadText)
                {
                    if (!item.Contains("//"))
                    {
                        if (item.Contains("AssemblyVersion"))
                        {
                            version = item.Substring(item.IndexOf("(\"") + 2, item.IndexOf("\")") - item.IndexOf("(\"") - 2);
                            //int version_short_len = version.IndexOf(".", version.IndexOf(".") + 1);
                            //version = version.Substring(0, version_short_len);
                        }
                        if (item.Contains("AssemblyFileVersion"))
                        {
                            fileversion = item.Substring(item.IndexOf("(\"") + 2, item.IndexOf("\")") - item.IndexOf("(\"") - 2);
                        }
                        if (item.Contains("AssemblyProduct"))
                        {
                            product = item.Substring(item.IndexOf("(\"") + 2, item.IndexOf("\")") - item.IndexOf("(\"") - 2);
                        }
                    }
                }
            }
            else
            {
                foreach (string item in ReadText)
                {
                    if (!item.Contains("//"))
                    {
                        if (item.Contains("FileVersion"))
                        {
                            fileversion = item.Substring(item.IndexOf("\", \"") + 4, (item.Length-1) - item.IndexOf("\", \"") - 4);
                        }
                        if (item.Contains("ProductVersion"))
                        {
                            productversion = item.Substring(item.IndexOf("\", \"") + 4, (item.Length - 1) - item.IndexOf("\", \"") - 4);
                        }
                        if (item.Contains("FileDescription"))
                        {
                            product = item.Substring(item.IndexOf("\", \"") + 4, (item.Length - 1) - item.IndexOf("\", \"") - 4);
                        }
                    }
                }
            }
            
            // .wxs 改变ProductVersion
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(args[2]);

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
                        item.InnerText= "ProductVersion=\""+ fileversion + "\"";
                    }
                }
            }
            doc1.Save(args[2]);

            // .wixproj 改变OutputName
            XmlDocument doc2 = new XmlDocument();
            doc2.Load(args[3]);

            //使用命名空间
            XmlNamespaceManager nsMgr2 = new XmlNamespaceManager(doc2.NameTable);
            nsMgr2.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlNode root2 = doc2.SelectSingleNode("/ns:Project/ns:PropertyGroup", nsMgr2);
            XmlNodeList xnl2 = root2.ChildNodes;
            foreach (XmlNode item in xnl2)
            {
                if (item.Name.Equals("ProductVersion"))
                {
                    item.InnerText = fileversion;
                }
                if (item.Name.Equals("OutputName"))
                {
                    item.InnerText = product+"_"+ fileversion;
                }
            }
            doc2.Save(args[3]);
            return 0;
        }
    }
}
