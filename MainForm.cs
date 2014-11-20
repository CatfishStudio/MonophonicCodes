/*
 * Сделано в SharpDevelop.
 * Пользователь: SOMOV
 * Дата: 19.11.2014
 * Время: 21:30
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace MonophonicCodes
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			
		}
		
		
		void frequencyAnalysis()
		{
			string s = richTextBox1.Text;
            var x = from c in s
                    group c by c into g
                    let count = g.Count()
                    orderby g.Key ascending
                    select new
                    {
                        Value = g.Key,
                        Count = count,
                                          
                    };
            richTextBox5.Clear();
            richTextBox5.Text = "символ: " + "  " + "частота" +" | " + "отн. частота" + "    " +"\n\r";
            foreach (var count in x)
            {
               
                double a = Convert.ToDouble(count.Count)/s.Length;
                a=Math.Round(a,5);
                double b=Math.Round( -1*a*Math.Log(a,2),5);
                richTextBox5.Text += "  " + count.Value + "                     " + count.Count + "          " +a+"\n\r";
 				
            }
		}
		
		
		void ToolStripButton1Click(object sender, EventArgs e)
		{
			frequencyAnalysis();
		}
	}
}
