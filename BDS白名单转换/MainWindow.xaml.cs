using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BDS白名单转换
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void 转换_Click(object sender, RoutedEventArgs e)
        {
            #region 建立数组
            TextRange textRange = new TextRange(inputName.Document.ContentStart, inputName.Document.ContentEnd);
            string[] white_list = textRange.Text.Replace("\r", "").Split('\n');
            System.Collections.ArrayList al = new System.Collections.ArrayList(white_list);
            al.RemoveAt(white_list.Length - 1);
            white_list = (string[])al.ToArray(typeof(string));
            #endregion
            #region 信息统计
            statistics.Document.Blocks.Clear();
            statistics.AppendText("玩家总计:" + Convert.ToString(white_list.Length));
            string output = "[\n";
            for (int i = 0; i < white_list.Length; i++)
            {
                statistics.AppendText("\n" + Convert.ToString((decimal)i + 1) + ":" + white_list[i]);
            }
            #endregion
            #region 核心转换
            foreach (string name in white_list)
            {
                output = output + "  {\n  \"ignoresPlayerLimit\":" + Convert.ToString(ignores_player_limit.IsChecked).ToLower() + ",\n  \"name\":\"" + name.Replace(" ", "#空格") + "\"\n  },\n";
            }
            if ((bool)OneLine.IsChecked)
            { output = output.Replace("\n", "").Replace(" ", "").Replace("#空格", " "); }
            else
            {output = output.Substring(0, output.Length - 2).Replace("#空格", " ") + "\n]";}
            outputJson.Document.Blocks.Clear();
            outputJson.AppendText(output);
            #endregion
        }


    }
}