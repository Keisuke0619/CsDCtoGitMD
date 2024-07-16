using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ConvertUnityInfoToMarkDown
{
	public class WildCard
	{
		public string DeleteStr(string input, string wildCard)
		{
			var pattern = ConvertToRegex(wildCard);
			return Regex.Replace(input, pattern, "");
		}
		public string ReplaceStr(string input, string wildCard, string replace)
		{
			var pattern = ConvertToRegex(wildCard);
			return Regex.Replace(input, pattern, replace);
		}
		public bool IsMatch(string input, string wildCard)
		{
			var pattern = ConvertToRegex(wildCard);
			return Regex.IsMatch(input, pattern);
		}

		private string ConvertToRegex(string input)
		{
			string pattern = string.Empty;
			foreach (char c in input)
			{
				
				switch (c)
				{
					case '?': pattern += "."; break;
					case '*': pattern += ".*"; break;
					default: pattern += Regex.Escape(c.ToString()); break;
				};
			}
			return pattern;
		}
	}


	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		void Replace()
		{
			var conv = new WildCard();
			var text = this.InputBox.Text;
			text = text.Replace("/// <summary>\r\n", "概　要：");
			text = text.Replace("/// <summary>\r", "概　要：");
			text = text.Replace("/// <summary>\n", "概　要：");
			text = text.Replace("/// </summary>\r\n", "");
			text = text.Replace("/// </summary>\r", "");
			text = text.Replace("/// </summary>\n", "");
			text = text.Replace("<br/>\r\n", "\r\n　　　　");
			text = text.Replace("<br/>\r", "\r　　　　");
			text = text.Replace("<br/>\n", "\n　　　　");
			text = text.Replace("</param>", "");
			text = text.Replace("public static", "###");
			text = text.Replace("/// ", "");
			text = text.Replace("\t", "");
			text = text.Replace("<returns>", "戻り値＜");
			text = text.Replace("</returns>", "");

			text = conv.ReplaceStr(text, "<param name*>", "引数リスト");
			text = conv.DeleteStr(text, "{*}");
			text = conv.DeleteStr(text, "=>*);");

			List<string> lines = new List<string>();
			var split = text.Split('\n');
			text = "";
			int idx = 0;
			string[] head = { "１", "２", "３", "４", "５", "６", "７", "８", "９" };
			foreach (var line in split)
			{
				if (conv.IsMatch(line, "### *"))
				{
					lines.Insert(0, line);
					foreach (var l in lines) { text += l; }
					lines.Clear();
					idx = 0;
					continue;
				}
				var rp = line.Replace("引数リスト", "");
				if (line != rp)
				{
					string num = (idx + 1).ToString();
					if (idx < head.Length)
					{
						num = head[idx];
					}
					rp = "引数" + num + "＞" + rp;
					idx++;
				}
				lines.Add(rp);
			}
			this.OutputBox.Text = text;
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(OutputBox.Text);
		}

		private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			Replace();
		}
	}
}
