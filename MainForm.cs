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
		
		/* Класс буквы (пропорциональное шиврование)*/
		public class Letter
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
		
		/* Класс буквы и пренадлежащего ей ключа*/
		public class LetterAndKey
		{
			public string nameLetter;
			public string nameKey;
			public string codingLatter;
			
			public LetterAndKey(string _nameLetter, string _nameKey)
			{
				nameLetter = _nameLetter;
				nameKey = _nameKey;
				codingLatter = "";
			}
			
			public void SetValue(string _codingLetter)
			{
				codingLatter = _codingLetter;
			}
			
			public string GetValue()
			{
				return codingLatter;
			}
		}
		
		public List<Letter> alphabet = new List<Letter>(); // Таблиа пропорционального шифра
						
		public List<LetterAndKey> alphabetKey = new List<LetterAndKey>(); // Таблиа букв и соответствующего ключа
		
		
		/*===========================================================================
		  * ПРОПОРЦИОНАЛЬНЫЙ ШИФР
		  * =========================================================================*/
			
		/* Частотный анализ ----------------------------*/
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
		
		/* инициализация пропорционального шифра -------*/
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
		
		/* кодировать пропорциональный шифр ------------*/
		void performEncodeProportionalCode()
		{
			toolStripStatusLabel2.Text = "Действие: Кодирование пропорционального шифра...";
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
		
		/* декодировать пропорциональный шифр ----------*/
		void performDecodeProportionalCode()
		{
			toolStripStatusLabel2.Text = "Действие: Декодирование пропорционального шифра...";
			richTextBox2.Clear();
			
			int index = 0;
			string code = "";
			for(int iRTB = 0; iRTB < richTextBox1.Text.Length; iRTB++){ // символы в тексте
				if(index == 3 || iRTB == richTextBox1.Text.Length - 1){
					if(iRTB == richTextBox1.Text.Length -1) code += richTextBox1.Text[iRTB].ToString();
					MessageBox.Show(code);
					for(int i = 0; i < alphabet.Count; i++){
						for(int j = 0; j < alphabet[i].variantsReplacement.Length; j++){
							if(alphabet[i].variantsReplacement[j].ToString() == code.ToString()){
								richTextBox2.Text += alphabet[i].name;
								index = 0;
								code = "";
								break;
							}
						}
					}
				}
				if(index < 3){
					code += richTextBox1.Text[iRTB].ToString();
					index++;
				}
			}
		}
		
		/*===========================================================================
		  * МНОГОАЛФАВИТНЫЕ ПОДСТАНОВКИ
		  * =========================================================================*/
		
		/* инициализация алфавита ключа */
		void initAlphabetKey()
		{
			toolStripStatusLabel2.Text = "Действие: Инициализация алфавита ключа...";
			
			int index = 0;
			alphabetKey = new List<LetterAndKey>();
			
			for(int i = 0; i < richTextBox1.Text.Length; i++){
				if(index == toolStripTextBox1.Text.Length){
					index = 0;
					alphabetKey.Add(new LetterAndKey(richTextBox1.Text[i].ToString(), toolStripTextBox1.Text[index].ToString()));
					index++;
				}
				else {
					alphabetKey.Add(new LetterAndKey(richTextBox1.Text[i].ToString(), toolStripTextBox1.Text[index].ToString()));
					index++;
				}
				//richTextBox2.Text += alphabetKey[i].nameKey;
			}
			
		}
		
		/* кодировать Многоалфавитные подстановки */
		void performEncodeMultiAlphabetSubstitution()
		{
			richTextBox2.Clear();
				
			for(int i = 0; i < alphabetKey.Count; i++){
				for(int j = 0; j < richTextBox4.Lines.Length; j++){
					
					string textLine = richTextBox4.Lines[j].ToString(); // строка многоалфавитного текста
					
					
					if(alphabetKey[i].nameKey.ToString() == textLine[0].ToString()){ // буква ключа = первой букве в строке многоалфавитного текста
						
						// РУССКИЙ ТЕКСТ В ВЕРХНЕМ РЕГИСТРЕ
						for(int k = 0; k < richTextBox4.Lines[0].Length; k++){
							string alphabetLine = richTextBox4.Lines[0].ToString(); // первая строка многоалфавитного текста (верхний регистр)
							if(alphabetKey[i].nameLetter.ToString() == alphabetLine[k].ToString()){ // буква = букве в первой строке многоалфавитного текста
								alphabetKey[i].SetValue(textLine[k].ToString());
								richTextBox2.Text += alphabetKey[i].GetValue();
								break;
							}
						}
						
						// РУССКИЙ ТЕКСТ В НИЖНЕМ РЕГИСТРЕ
						for(int k = 0; k < richTextBox4.Lines[0].Length; k++){
							string alphabetLine = richTextBox4.Lines[31].ToString(); // тридцатьпервая строка многоалфавитного текста (нижний регистр)
							if(alphabetKey[i].nameLetter.ToString() == alphabetLine[k].ToString()){ // буква = букве в первой строке многоалфавитного текста
								alphabetKey[i].SetValue(textLine[k].ToString());
								richTextBox2.Text += alphabetKey[i].GetValue();
								break;
							}
						}
						
						break;
					}
					
					
					
				}
			}
			
			
		}
		
		
		/*=ВЫПОЛНЕНИЕ ================================================================*/
		void ToolStripButton1Click(object sender, EventArgs e)
		{
			if(toolStripComboBox1.Text == "Пропорциональное шифрование"){
				frequencyAnalysis();
				initProportionalCode();
				performEncodeProportionalCode();
				toolStripStatusLabel2.Text = "Действие: Пропорциональное шифрование завершено!";
			}
			if(toolStripComboBox1.Text == "Многоалфавитные подстановки"){
				if(toolStripTextBox1.Text != ""){
					initAlphabetKey();
					performEncodeMultiAlphabetSubstitution();
					toolStripStatusLabel2.Text = "Действие: Многоалфавитная подстановка завершена!";
				}else MessageBox.Show("Вы не ввели ключ!");
			}
			
		}
		
		void ToolStripButton2Click(object sender, EventArgs e)
		{
			if(toolStripComboBox1.Text == "Пропорциональное шифрование"){
				performDecodeProportionalCode();
				toolStripStatusLabel2.Text = "Действие: Пропорциональная шифровка завершена!";
			}
		}
	}
}
