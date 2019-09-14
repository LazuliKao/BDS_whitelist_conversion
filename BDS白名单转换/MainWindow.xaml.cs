using System;
using System.Collections.Generic;
using System.IO;
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
            string white_list_all = textRange.Text.Replace("\r", "").Replace("\n", ";").Replace(";;", ";");
            statistics.AppendText(white_list_all);
            string[] white_list = white_list_all.Split(';');
            List<string> al = white_list.ToList();
            al.RemoveAt(white_list.Length - 1);
            white_list = al.ToArray();
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
            {
                output = output.Replace("\n", "").Replace(" ", "").Replace("#空格", " ");
                output = output.Substring(0, output.Length - 1) + "]";
            }
            else
            { output = output.Substring(0, output.Length - 2).Replace("#空格", " ") + "\n]"; }
            outputJson.Document.Blocks.Clear();
            outputJson.AppendText(output);
            #endregion
        }
        private void 提取_Click(object sender, RoutedEventArgs e)
        {
            #region 核心代码
            TextRange textRange = new TextRange(outputJson.Document.ContentStart, outputJson.Document.ContentEnd);
            string white_list_all = textRange.Text.Replace("\" ", "\"").Replace(" \"", "\"").Replace("\r", "");
            if (white_list_all != "")
            {
                string[] white_list = new string[0];
                while (true)
                {
                    try
                    {
                        int p1 = white_list_all.IndexOf("\"name\":\"");
                        int startIndex = p1 + 8;
                        int p2 = white_list_all.IndexOf("\"", startIndex);
                        List<string> al = white_list.ToList();
                        al.Add(white_list_all.Substring(startIndex, p2 - startIndex));
                        white_list = al.ToArray();
                        white_list_all = white_list_all.Substring(p2);
                        if (p1 < 0 | p2 < startIndex) { break; }

                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }

                }
                statistics.Document.Blocks.Clear();
                inputName.Document.Blocks.Clear();
                statistics.AppendText("玩家总计:" + white_list.Length);
                for (int i = 0; i < white_list.Length; i++)
                {
                    statistics.AppendText("\n" + Math.Truncate((decimal)i + 1) + ":" + white_list[i]);
                    inputName.AppendText(white_list[i]);
                    inputName.AppendText(Environment.NewLine);
                }
            }
            #endregion
        }

        private void MCBBS_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.mcbbs.net/thread-901503-1-1.html");
        }

        private void Github_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/littlegao233/BDS_whitelist_conversion/releases");

        }

        private void open_json_Click(object sender, RoutedEventArgs e)
        {
            var openJSONFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "JSON文件|whitelist.json"
            };
            var result = openJSONFileDialog.ShowDialog();
            if (result == true)
            {
                string File_path = openJSONFileDialog.FileName;
                open_whitelist_json.Items.Add(new MenuItem() { Header = File_path, Height = 30 });
                statistics.Document.Blocks.Clear();
                inputName.Document.Blocks.Clear();
                outputJson.Document.Blocks.Clear();
                outputJson.AppendText(File.ReadAllText(File_path, Encoding.Default));
            }

        }

        public delegate void BtnClickHandle(object sender, EventArgs e);
        private void open_text_Click(object sender, RoutedEventArgs e)
        {
            var openTXTFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "文本文件|*.txt"
            };
            var result = openTXTFileDialog.ShowDialog();
            if (result == true)
            {
                string File_path = openTXTFileDialog.FileName;
                MenuItem file_path_object = new MenuItem { Header = File_path, Height = 30,Name="file_history" };
                //file_path_object.Click += new EventHandler(open_history_path_Click);
                open_whitelist_text.Items.Add(file_path_object);
                statistics.Document.Blocks.Clear();
                inputName.Document.Blocks.Clear();
                outputJson.Document.Blocks.Clear();
                inputName.AppendText(File.ReadAllText(File_path, Encoding.Default));
              
            }
        }
        public void open_history_path_Click(object sender, RoutedEventArgs e)
        {

        }
      


}
}