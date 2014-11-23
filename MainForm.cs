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
			
			public void SetLetter(string _nameLetter)
			{
				nameLetter = _nameLetter;
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
		
		private RichTextBox _selectRTB;
		
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
			
			for(int iRTB = 0; iRTB < richTextBox1.Text.Length; iRTB++){ // символы в тексте
				for(int i = 0; i < alphabet.Count; i++){ // алфавит повторений
					if(richTextBox1.Text[iRTB].ToString() == alphabet[i].name){ // найдена буква
						
						if(alphabet[i].index == alphabet[i].count){
							alphabet[i].index = 0;
							richTextBox2.Text += alphabet[i].variantsReplacement[alphabet[i].index].ToString();
						}else{
							richTextBox2.Text += alphabet[i].variantsReplacement[alphabet[i].index].ToString();
							alphabet[i].index++;
						}
						break;
					}
				}
				
			}
			
		}
		
		/* загрузка ранее сохраненного пропорционального шифра */
		void loadProportionalCode()
		{
			if(richTextBox3.Text != ""){
				alphabet = new List<Letter>(); // Таблиа пропорционального шифра
				for(int i = 0; i < richTextBox3.Lines.Length; i++){
					string lineText = richTextBox3.Lines[i].ToString();
					string name = "";
					int count = 0;
					for(int j = 0; j < lineText.Length; j++){
						if(lineText[j].ToString() == ":") name = lineText[j-1].ToString();
						if(lineText[j].ToString() == "]") count++;
					}
					alphabet.Add(new Letter(name, count));
					int index = 0;
					bool open = false;
					for(int k = 0; k < lineText.Length; k++){
						if(open == true){
							if(lineText[k].ToString() != "]") alphabet[i].variantsReplacement[index] += lineText[k].ToString();
							else{
								open = false;
								index++;
							}
						}
						if(lineText[k].ToString() == "[") open = true;
					}
					          
				}
				
				
			}else MessageBox.Show("У вас нет пропорционального шифра!");
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
		
		 /* кодировать: инициализация алфавита ключа ---------------*/
		void initEncodeAlphabetKey()
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
			}
			
		}
		
		/* кодировать Многоалфавитные подстановки -----*/
		void performEncodeMultiAlphabetSubstitution()
		{
			toolStripStatusLabel2.Text = "Действие: Кодирование многоалфавитными подстановками...";
			richTextBox2.Clear();
				
			for(int i = 0; i < alphabetKey.Count; i++){
				for(int j = 0; j < richTextBox4.Lines.Length; j++){
					
					string textLine = richTextBox4.Lines[j].ToString(); // строка многоалфавитного текста
					
					
					if(alphabetKey[i].nameKey.ToString() == textLine[0].ToString()){ // буква ключа = первой букве в строке многоалфавитного текста
						
						for(int k = 0; k < richTextBox4.Lines[0].Length; k++){
							// В ВЕРХНЕМ РЕГИСТРЕ
							if(char.IsUpper(alphabetKey[i].nameLetter[0])){
								// РУССКИЙ ТЕКСТ
								string alphabetLine = richTextBox4.Lines[0].ToString(); // первая строка многоалфавитного текста (верхний регистр)
								if(alphabetKey[i].nameLetter.ToString() == alphabetLine[k].ToString()){ // буква = букве в первой строке многоалфавитного текста
									alphabetKey[i].SetValue(textLine[k].ToString());
									richTextBox2.Text += alphabetKey[i].GetValue();
									break;
								}
								
								// АНГЛИЙСКИЙ ТЕКСТ
								alphabetLine = richTextBox4.Lines[64].ToString(); 
								if(alphabetKey[i].nameLetter.ToString() == alphabetLine[k].ToString()){ // буква = букве в первой строке многоалфавитного текста
									alphabetKey[i].SetValue(textLine[k].ToString());
									richTextBox2.Text += alphabetKey[i].GetValue();
									break;
								}
								
							}else{ // В НИЖНЕМ РЕГИСТРЕ
								// РУССКИЙ ТЕКСТ
								string alphabetLine = richTextBox4.Lines[32].ToString(); // тридцатьпервая строка многоалфавитного текста (нижний регистр)
								if(alphabetKey[i].nameLetter.ToString() == alphabetLine[k].ToString()){ // буква = букве в первой строке многоалфавитного текста
									alphabetKey[i].SetValue(textLine[k].ToString());
									richTextBox2.Text += alphabetKey[i].GetValue();
									break;
								}
								
								// АНГЛИЙСКИЙ ТЕКСТ
								alphabetLine = richTextBox4.Lines[91].ToString(); 
								if(alphabetKey[i].nameLetter.ToString() == alphabetLine[k].ToString()){ // буква = букве в первой строке многоалфавитного текста
									alphabetKey[i].SetValue(textLine[k].ToString());
									richTextBox2.Text += alphabetKey[i].GetValue();
									break;
								}
							}
						}
						
						break;
					}
				}
			}
			
			
		}
		
		/* декодировка: инициализация алфавита ключа ---------------*/
		void initDecodeAlphabetKey()
		{
			toolStripStatusLabel2.Text = "Действие: Инициализация алфавита ключа...";
			
			int index = 0;
			alphabetKey = new List<LetterAndKey>();
			
			for(int i = 0; i < richTextBox1.Text.Length; i++){
				if(index == toolStripTextBox1.Text.Length){
					index = 0;
					alphabetKey.Add(new LetterAndKey("", toolStripTextBox1.Text[index].ToString()));
					alphabetKey[i].SetValue(richTextBox1.Text[i].ToString());
					index++;
				}
				else {
					alphabetKey.Add(new LetterAndKey("", toolStripTextBox1.Text[index].ToString()));
					alphabetKey[i].SetValue(richTextBox1.Text[i].ToString());
					index++;
				}
			}
		}
		
		/* Определение языка текста */
		string determineLanguage(string _text)
		{
			 string text = _text.ToUpper();
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if ((c >= 'А') && (c <= 'Я'))
                    return "RUS";
                else  if ((c >= 'A') && (c <= 'Z')) return "ENG";
            }
            return "";
		}
		
		/* декодировка многоалфавитных подстановок ----*/
		void performDecodeMultiAlphabetSubstitution()
		{
			richTextBox2.Clear();
			
			for(int i = 0; i < alphabetKey.Count; i++){
				for(int j = 0; j < richTextBox4.Lines.Length; j++){
					
					string textLine = richTextBox4.Lines[j].ToString(); // строка многоалфавитного текста
					
					if(alphabetKey[i].nameKey.ToString() == textLine[0].ToString()){ // буква ключа = первой букве в строке многоалфавитного текста
						
						for(int k = 0; k < textLine.Length; k++){
							// В ВЕРХНЕМ РЕГИСТРЕ
							if(char.IsUpper(alphabetKey[i].codingLatter[0])){
								
								 
								string alphabetLineRus = richTextBox4.Lines[0].ToString();
								string alphabetLineEng = richTextBox4.Lines[64].ToString();
								
								if(alphabetKey[i].codingLatter.ToString() == textLine[k].ToString()){ 
									
									// РУССКИЙ ТЕКСТ
									if(determineLanguage(textLine[k].ToString()) == "RUS"){
										alphabetKey[i].SetLetter(alphabetLineRus[k].ToString());
										richTextBox2.Text += alphabetKey[i].nameLetter;
										break;
									}
									// АНГЛИЙСКИЙ ТЕКСТ
									if(determineLanguage(textLine[k].ToString()) == "ENG"){
										alphabetKey[i].SetLetter(alphabetLineEng[k].ToString());
										richTextBox2.Text += alphabetKey[i].nameLetter;
										break;
									}
								}
								
							}else{// В НИЖНЕМ РЕГИСТРЕ
								
								 
								string alphabetLineRus = richTextBox4.Lines[32].ToString(); 
								string alphabetLineEng = richTextBox4.Lines[91].ToString();
								
								if(alphabetKey[i].codingLatter.ToString() == textLine[k].ToString()){ 
									
									// РУССКИЙ ТЕКСТ
									if(determineLanguage(textLine[k].ToString()) == "RUS"){
										alphabetKey[i].SetLetter(alphabetLineRus[k].ToString());
										richTextBox2.Text += alphabetKey[i].nameLetter;
										break;
									}
									// АНГЛИЙСКИЙ ТЕКСТ
									if(determineLanguage(textLine[k].ToString()) == "ENG"){
										alphabetKey[i].SetLetter(alphabetLineEng[k].ToString());
										richTextBox2.Text += alphabetKey[i].nameLetter;
										break;
									}
								}
								
								
							}
						}
												
						break;
					}
				}
			}
			richTextBox2.Text = richTextBox2.Text.ToLower();
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
					initEncodeAlphabetKey();
					performEncodeMultiAlphabetSubstitution();
					toolStripStatusLabel2.Text = "Действие: Многоалфавитная подстановка завершена!";
				}else MessageBox.Show("Вы не ввели ключ!");
			}
			
		}
		
		void ToolStripButton2Click(object sender, EventArgs e)
		{
			if(toolStripComboBox1.Text == "Пропорциональное шифрование"){
				loadProportionalCode();
				performDecodeProportionalCode();
				toolStripStatusLabel2.Text = "Действие: Пропорциональная расшифровка завершена!";
			}
			
			if(toolStripComboBox1.Text == "Многоалфавитные подстановки"){
				initDecodeAlphabetKey();
				performDecodeMultiAlphabetSubstitution();
				toolStripStatusLabel2.Text = "Действие: Многоалфавитная расшифровка завершена!";
			}
		}
		
		
		/* Сохранить результат шифрования. */
		void СохранитьРезультатШифрованияToolStripMenuItemClick(object sender, EventArgs e)
		{
			tabControl2.SelectedIndex = 0;
			saveFileDialog1.FileName = "РезультатШифрования.txt";
			if(saveFileDialog1.ShowDialog() == DialogResult.OK){
				richTextBox2.SaveFile(saveFileDialog1.FileName);
			}
		}
		
		/* Сохранить результат частотного анализа. */
		void СохранитьРезультатЧастотногоАнализаToolStripMenuItemClick(object sender, EventArgs e)
		{
			tabControl2.SelectedIndex = 1;
			saveFileDialog1.FileName = "ЧастотныйАнализ.txt";
			if(saveFileDialog1.ShowDialog() == DialogResult.OK){
				richTextBox5.SaveFile(saveFileDialog1.FileName);
			}
		}
		
		/* Сохранить таблицу пропорционального шифра. */
		void СохранитьТаблицуПропорциональногоШифраToolStripMenuItemClick(object sender, EventArgs e)
		{
			tabControl2.SelectedIndex = 2;
			saveFileDialog1.FileName = "ПропорциональныйШифр.txt";
			if(saveFileDialog1.ShowDialog() == DialogResult.OK){
				richTextBox3.SaveFile(saveFileDialog1.FileName);
			}
		}
		
		/* Сохранить таблицу многоалфавитных посдатонок. */
		void СохранитьТаблицуМногоалфавитныхПосдатонокToolStripMenuItemClick(object sender, EventArgs e)
		{
			tabControl2.SelectedIndex = 3;
			saveFileDialog1.FileName = "МногоалфавитнаяТаблица.txt";
			if(saveFileDialog1.ShowDialog() == DialogResult.OK){
				richTextBox4.SaveFile(saveFileDialog1.FileName);
			}			
		}
		
		/* Сохранить файл исходных данных. */
		void СохранитьФайлИсходныхДанныхToolStripMenuItemClick(object sender, EventArgs e)
		{
			saveFileDialog1.FileName = "файл.txt";
			if(saveFileDialog1.ShowDialog() == DialogResult.OK){
				richTextBox1.SaveFile(saveFileDialog1.FileName);
			}
		}
		
		/* Открыть файл входных данных */
		void ОткрытьТектовыйФайлToolStripMenuItemClick(object sender, EventArgs e)
		{
			richTextBox1.Clear();
			if(openFileDialog1.ShowDialog() == DialogResult.OK){
				richTextBox1.LoadFile(openFileDialog1.FileName);
			}
		}
		
		/* Открыть таблицу пропорционального шифра */
		void ОткрытьТаблицуПропорциональногоШифрованияToolStripMenuItemClick(object sender, EventArgs e)
		{
			richTextBox3.Clear();
			if(openFileDialog1.ShowDialog() == DialogResult.OK){
				richTextBox3.LoadFile(openFileDialog1.FileName);
			}
		}
		
		/* Открыть таблицу многоалфавитных подстановок */
		void ОткрытьТаблицуМногоалфавитныхПодстановокToolStripMenuItemClick(object sender, EventArgs e)
		{
			richTextBox4.Clear();
			if(openFileDialog1.ShowDialog() == DialogResult.OK){
				richTextBox4.LoadFile(openFileDialog1.FileName);
			}
		}
		
		void RichTextBox1TextChanged(object sender, EventArgs e)
		{
			_selectRTB = richTextBox1;
		}
		
		void RichTextBox1Click(object sender, EventArgs e)
		{
			_selectRTB = richTextBox1;
		}
		
		void RichTextBox2TextChanged(object sender, EventArgs e)
		{
			_selectRTB = richTextBox2;
		}
		
		void RichTextBox2Click(object sender, EventArgs e)
		{
			_selectRTB = richTextBox2;
		}
		
		void RichTextBox5TextChanged(object sender, EventArgs e)
		{
			_selectRTB = richTextBox5;
		}
		
		void RichTextBox5Click(object sender, EventArgs e)
		{
			_selectRTB = richTextBox1;
		}
		
		void RichTextBox3TextChanged(object sender, EventArgs e)
		{
			_selectRTB = richTextBox3;
		}
		
		void RichTextBox3Click(object sender, EventArgs e)
		{
			_selectRTB = richTextBox3;
		}
		
		void RichTextBox4TextChanged(object sender, EventArgs e)
		{
			_selectRTB = richTextBox4;
		}
		
		void RichTextBox4Click(object sender, EventArgs e)
		{
			_selectRTB = richTextBox1;
		}
		
		/* Правка: Отмена, Повтор, Вырезать, Копировать, Вставить, Удалить */
		void editUndo() // отмена
		{
			_selectRTB.Undo();
		}
		
		void editRedo() // повтор
		{
			_selectRTB.Redo();
		}
		
		void editCut() // вырезать
		{
			_selectRTB.Cut();
		}
		
		void editCopy() // копировать
		{
			_selectRTB.Copy();
		}
		
		void editPaste() // вставить
		{
			_selectRTB.Paste();
		}
		
		void editDelete() // удалить
		{
			Clipboard.SetDataObject("");
			_selectRTB.Paste();
		}
		
		void editSelectAll() // Выделить всё
		{
			_selectRTB.SelectAll();
		}
		
		void ОтменитьToolStripMenuItemClick(object sender, EventArgs e)
		{
			editUndo();
		}
		
		void ПовторитьToolStripMenuItemClick(object sender, EventArgs e)
		{
			editRedo();
		}
		
		void ВырезатьToolStripMenuItemClick(object sender, EventArgs e)
		{
			editCut();
		}
		
		void КопироватьToolStripMenuItemClick(object sender, EventArgs e)
		{
			editCopy();
		}
		
		void ВставитьToolStripMenuItemClick(object sender, EventArgs e)
		{
			editPaste();
		}
		
		void УдалитьToolStripMenuItemClick(object sender, EventArgs e)
		{
			editDelete();
		}
		
		void ВыделитьВсёToolStripMenuItemClick(object sender, EventArgs e)
		{
			editSelectAll();
		}
		
		void ToolStripMenuItem2Click(object sender, EventArgs e)
		{
			editUndo();
		}
		
		void ToolStripMenuItem3Click(object sender, EventArgs e)
		{
			editRedo();
		}
		
		void ToolStripMenuItem4Click(object sender, EventArgs e)
		{
			editCut();
		}
		
		void ToolStripMenuItem5Click(object sender, EventArgs e)
		{
			editCopy();
		}
		
		void ToolStripMenuItem6Click(object sender, EventArgs e)
		{
			editPaste();
		}
		
		void ToolStripMenuItem7Click(object sender, EventArgs e)
		{
			editDelete();
		}
		
		void ВыделитьВсёToolStripMenuItem1Click(object sender, EventArgs e)
		{
			editSelectAll();
		}
		
		void PaintToolStripMenuItemClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("mspaint.exe");
		}
		
		void WordpadToolStripMenuItemClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("wordpad.exe");
		}
		
		void БлокнотToolStripMenuItem1Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("notepad.exe"); // блокнот
		}
		
		void ПроводникToolStripMenuItemClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("explorer.exe");
		}
		
		void КалькуляторToolStripMenuItem1Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("calc.exe"); // калькулятор
		}
		
		void КоманднаяСтрокаToolStripMenuItemClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("cmd.exe");
		}
		
		void ToolStripMenuItem8Click(object sender, EventArgs e)
		{
			MessageBox.Show("Программа: MonophonicCodes" + System.Environment.NewLine + "Версия: 1.0" + System.Environment.NewLine + "Автор: Сомов Евгений Павлович" + System.Environment.NewLine + "©  Somov Evgeniy, 2014", "О программе", MessageBoxButtons.OK);
		}
	}
}
