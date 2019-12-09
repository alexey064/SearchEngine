using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SearchEngine
{
    public partial class Form1 : Form
    {
        Search search = new Search();
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {//запуск и остановка поиска
            switch (search.Running)
            {
                case false: //Если задача не запущена
                    search.output = OutText; //делегат для обратного вызова формы и вывода результата
                    search.SearchString = textBox1.Text;
                    search.StartDir = textBox3.Text;
                    search.contentSearch = textBox2.Text;
                    button1.Text = "Пауза";
                    ThreadPool.QueueUserWorkItem(Start);
                    PrepareUI(false);
                    button2.Enabled = false;
                    if (!search.Restored)
                    {
                        Time.Text = "0:0";
                        treeView1.Nodes.Clear();
                    }
                    timer1.Start();
                    break;

                case true: //Если задача запущена
                    search.stop(true); // останавливаем задачу
                    timer1.Stop();
                    button1.Text = "Продолжить";
                    button2.Enabled = true;
                    break;
            }
        }

        private void PrepareUI(bool state)
        {//данный метод блокирует и разблокирует ui по необходимости. 
            textBox1.Enabled = state;
            поискToolStripMenuItem.Enabled = state;
            textBox2.Enabled = state;
            textBox3.Enabled = state;
            button3.Enabled = state;
        }

        public void OutText(string stroka, int type)
        {//Данный метод предоставляет связь класса Search с формой, или другим пользовательским интерфейсом.
            //типы: 0=количество просканированных файлов, 1 = сканирующийся файл, 2=добавить найденный файл в список. 3=вывод об ошибках
            switch (type)
            {
                case 0: Scanned.Invoke((MethodInvoker)(() => Scanned.Text = stroka)); break;
                case 1: Scaning.Invoke((MethodInvoker)(() => Scaning.Text = stroka)); break;
                case 2: treeView1.Invoke((MethodInvoker)(() => treeView1.add(stroka))); break;
                case 3: MessageBox.Show(stroka); break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {// кнопка выбора папки
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            textBox3.Text = folderBrowser.SelectedPath;
        }

        private void Start(object state)
        {// данный метод начинает выполнять поиск. После выполнения поиска делает небольшие изменения в ui. 
            search.start();
            if (!search.Restored)
            {
                button1.Invoke((MethodInvoker)(() => button1.Text = "Старт"));
                button2.Invoke((MethodInvoker)(() => button2.Enabled = false));
                timer1.Stop();
                this.Invoke((MethodInvoker)(() => PrepareUI(true)));
            }
            SaveAfter();
        }

        public void Restore()
        {//Данный метод загружает начальные условия последнего запроса.
            //Если во время приостановленовленного поиска было выключено приложение, то восстанавливает найденные результаты, и позволяет продолжить поиск.
            if (File.Exists("TempInfo"))
            {
                using (StreamReader stream = new StreamReader("TempInfo"))
                {
                    textBox2.Text = stream.ReadLine();
                    textBox3.Text = stream.ReadLine();
                    textBox1.Text = stream.ReadLine();
                    search.RestoredElem = stream.ReadLine();
                }
            }
            if (search.RestoredElem != null)
            {
                if (File.Exists("TempRes"))
                {
                    List<string> FoundedResult = new List<string>();
                    using (StreamReader reader = new StreamReader("TempRes"))
                    {
                        while (!reader.EndOfStream)
                        {
                            FoundedResult.Add(reader.ReadLine());
                        }
                    }
                    foreach (string stroka in FoundedResult)
                    {
                        treeView1.add(stroka);
                    }
                    search.Restored = true;
                    PrepareUI(false);
                    button1.Text = "Продолжить";
                    button2.Enabled = true;
                }
                
            }
        }
        public void SaveAfter()
        {//сохраняет в файл параметры последнего поиска
            string Filename = textBox3.Text;
            using (StreamWriter stream = new StreamWriter("TempInfo", false))
            {
                stream.WriteLine(textBox2.Text);// имя для поиса содержимого
                stream.WriteLine(textBox3.Text); //имя для поиска
                stream.WriteLine(textBox1.Text); // путь поиска
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Restore();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //выводит сколько времени затрачено на поиск
            string[] time = Time.Text.Split(':');
            int seconds = int.Parse(time[1]);
            int minutes = int.Parse(time[0]);
            seconds++;
            if (seconds == 60)
            {
                minutes++;
                seconds = 0;
            }
            Time.Text = minutes + ":" + seconds;
        }

        private void button2_Click(object sender, EventArgs e)
        {// Кнопка стоп
            search.stop(false);
            PrepareUI(true);
            timer1.Stop();
            button1.Text = "Старт";
            button2.Enabled = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {//данный метод сохраняет введённые параметры последнего запроса в файл
            SaveAfter();
            if (button2.Enabled) //Если приложение закрывается при приостановленном запросе, то записываем в файл найденные результаты. 
            {
                List<string> AllNodes = treeView1.GetAll();

                using (StreamWriter writer = new StreamWriter("TempRes"))
                {
                    foreach (string stroka in AllNodes)
                    {
                        writer.WriteLine(stroka);
                    }
                }
            }
        }
        private void NormalSearch_Click(object sender, EventArgs e)
        {
            //устанавливает тип поиска: без параметров 
            search.SearchType = new SearchWithoutParams();
            NormalSearch.Checked = true;
            HardSearch.Checked = false;
            Parametrized.Checked = false;
        }

        private void HardSearch_Click(object sender, EventArgs e)
        {
            //устанавливает тип поиска: жесткий
            search.SearchType = new SearchHard();
            NormalSearch.Checked = false;
            HardSearch.Checked = true;
            Parametrized.Checked = false;
        }

        private void Parametrized_Click(object sender, EventArgs e)
        {
            //устанавливает тип поиска: с параметрами
            search.SearchType = new SearchWithParams();
            NormalSearch.Checked = false;
            HardSearch.Checked = false;
            Parametrized.Checked = true;
        }
    }
}