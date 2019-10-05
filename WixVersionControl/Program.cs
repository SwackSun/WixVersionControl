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
            //获取操作项目类型 -wpf of -mfc
            string ProjectType = args[0];
            //获取版本信息标识 -pv 产品版本；-fv 文件版本
            string VersionType = args[1];
            //AssemblyInfo.cs获取版本信息
            string[] VersionText = File.ReadAllLines(args[2], Encoding.Default);
            string fileversion = "";
            string productversion = "";
            string version = "";
            string product = "";
            if(ProjectType.Equals("-wpf"))
            {
                foreach (string item in VersionText)
                {
                    if (!item.Contains("//"))
                    {
                        if (item.Contains("AssemblyVersion"))
                        {
                            productversion = item.Substring(item.IndexOf("(\"") + 2, item.IndexOf("\")") - item.IndexOf("(\"") - 2);
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
            else if(ProjectType.Equals("-mfc"))
            {
                foreach (string item in VersionText)
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
            //获取版本信息
            if (VersionType.Equals("-pv"))
            {
                version = productversion;
            }
            else if (VersionType.Equals("-fv"))
            {
                version = fileversion;
            }

            // .wxs 改变ProductVersion
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(args[2]);

            //使用命名空间
            XmlNamespaceManager nsMgr1 = new XmlNamespaceManager(doc1.NameTable);
            nsMgr1.AddNamespace("ns", "http://schemas.microsoft.com/wix/2006/wi");

            //生成GUID
            string guidD = Guid.NewGuid().ToString("D");

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
                        item.InnerText= "ProductVersion=\""+ version + "\"";
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
                    item.InnerText = version;
                }
                if (item.Name.Equals("OutputName"))
                {
                    item.InnerText = product+"_"+ version;
                }
            }
            doc2.Save(args[3]);
            return 0;
        }
    }
}
