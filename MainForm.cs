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
using System.Collections;
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
		
		/* Структура буквы */
		public struct Letter
		{
			public string name;
			public int count;
			public string[] variantsReplacement;
			public int index;
			
			public Letter(String _name, int _count) // конструктор
			{
				name = _name;	// имя
				count = _count;	// частота повторений
				index = 0;		// индекс выборки
				variantsReplacement = new string[count]; // массив значений
			}
			
			public void addValue(int _index, string _value) // метод
			{
				variantsReplacement[_index] = _value;
			}
			
			public void changeIndex(int _index)
			{
				index = _index;
			}
		}
		
		public List<Letter> alphabet = new List<Letter>(); // Таблиа пропорционального шифра
		
		
		/* Частотный анализ */
		void frequencyAnalysis()
		{
			toolStripStatusLabel2.Text = "Действие: Частотный анализ...";
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
            
            alphabet = new List<Letter>(); // Таблиа пропорционального шифра
            richTextBox5.Clear();
            richTextBox5.Text = "символ: " + "  " + "частота" +" | " + "отн. частота" + "    " +"\n\r";
            foreach (var count in x)
            {
               
                double a = Convert.ToDouble(count.Count)/s.Length;
                a=Math.Round(a,5);
                double b=Math.Round( -1*a*Math.Log(a,2),5);
                richTextBox5.Text += "  " + count.Value + "                     " + count.Count + "          " +a+"\n\r";
                alphabet.Add(new Letter(count.Value.ToString(), count.Count));
            }
		}
		
		/* инициализация пропорционального шифра */
		void initProportionalCode()
		{
			toolStripStatusLabel2.Text = "Действие: Инициализация пропорционального шифра...";
			richTextBox3.Clear();
			
			int index = 100;
			for(int i = 0; i < alphabet.Count; i++){
				
				richTextBox3.Text += alphabet[i].name + ":";
				for(int j = 0; j < alphabet[i].count; j++){
					alphabet[i].variantsReplacement[j] = index.ToString();
					richTextBox3.Text += "[" + alphabet[i].variantsReplacement[j] + "]";
					index++;					
				}
				richTextBox3.Text += System.Environment.NewLine;
				
			}
		}
		
		/* выполнение пропорционального шифрования */
		void performProportionalCode()
		{
			toolStripStatusLabel2.Text = "Действие: Выполнение пропорционального шифрования...";
			richTextBox2.Clear();
			
			int index = 0;
			for(int iRTB = 0; iRTB < richTextBox1.Text.Length; iRTB++){ // символы в тексте
				for(int i = 0; i < alphabet.Count; i++){ // алфавит повторений
					if(richTextBox1.Text[iRTB].ToString() == alphabet[i].name){ // найдена буква
						richTextBox2.Text += alphabet[i].variantsReplacement[alphabet[i].index].ToString();
						if(alphabet[i].index == alphabet[i].count) alphabet[i].changeIndex(0);
						else{
							index++;
							alphabet[i].changeIndex(index);
						}
						break;
					}
				}
			}
			
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripButton1Click(object sender, EventArgs e)
		{
			if(toolStripComboBox1.Text == "Пропорциональное шифрование"){
				frequencyAnalysis();
				initProportionalCode();
				performProportionalCode();
				toolStripStatusLabel2.Text = "Действие: Пропорциональное шифрование завершено!";
			}
			
			
		}
	}
}
