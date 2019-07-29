using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wixsimpleversion
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string versionfile = "";
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (attributes.Length != 0)
            {
                versionfile = ((AssemblyFileVersionAttribute)attributes[0]).Version;
            }
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            int version_short_len = version.IndexOf(".", version.IndexOf(".") + 1);
            version = version.Substring(0, version_short_len);
            this.Title = "Swack Test Tool v" + versionfile;
            ver.Content = "v" + versionfile;
        }
    }
}
