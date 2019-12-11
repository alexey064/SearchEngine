using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SearchEngine
{
    class Search
    {
        public bool Running { get; private set; } = false; // false - задача не запущена, true - Задача запущена
        private byte RestLevel = 0;//Уровень вложености пути. Используется для восстановления рекурсивного поиска
        public string RestoredElem; //хранит элемент на котором поиск был остановлен.
        public bool Restored = false; // обозначает необходимость восстанавливать поиск
        public string SearchString { get; set; } // определяет какое название файла мы ищем
        private string CurrentDir { get; set; } //текущая обрабатываемая папка
        public string StartDir; //папка начального поиска
        private int ScannedFiles = 0; // количество просканированных файлов
        public string contentSearch { get; set; } //определяет текст, который используется для поиска содержимого файла.
        private bool DoSearchContent=true; // определяет необходимость выполнять поиск по содержимому файла
        public delegate void Output(string str, int type);
        public Output output;
        public SearchAdapter SearchType=new SearchWithoutParams();

        public void stop(bool Resumeable) 
        {// данный метод выполняет остановку поиска
            if (Resumeable)
            {
                Running = false;
            }
            else OnEnd();
            
        }
        public void start() 
        {//Проверка различных условий для начала поиска
            if (String.IsNullOrEmpty(contentSearch))//если не введены данные для поиска по содержимому, то не будем проверять содержимое файла
            {
                DoSearchContent = false;
            }
            else DoSearchContent = true;
            if (SearchType==null)//Если не выбран тип поиска, то ставим тип поиска по умолчанию
            {
                SearchType = new SearchWithoutParams();
            }
            if (string.IsNullOrEmpty(StartDir))//если не задан каталог, с которого начинаем поиск, выводим соответствующее сообщение
            {
                output("Не задан путь для поиска", 3);
                return;
            }
            if (string.IsNullOrEmpty(SearchString))//если не задано имя файла, которое мы ищем, то выводим соответствующее сообщение
            {
                output("Не задано имя файла для поиска", 3);
                return;
            }
            if (!Directory.Exists(StartDir)) //проверяем правильность введенного каталога
            {
                output("неправильно задан путь поиска. Возможно данный путь отсутствует", 3);
                return;
            }
            if (Restored) //Если мы продолжаем поиск, то выполняем соответствующий метод
            {
                Continue();
            } else CurrentDir = StartDir;
            Running = true;
            Find();
            if (!Running )
            {
                return;
            }
            OnEnd();
        }

        private void OnEnd() 
        {//данный метод обнуляет все значения используемых переменных
            Running = false;
            contentSearch = null;
            StartDir = null;
            CurrentDir = null;
            Restored = false;
            SearchString = null;
            RestoredElem = null;
            RestLevel = 0;
            ScannedFiles = 0;
        }
        public void Find()
        {
            List<string> elems = Directory.GetDirectories(CurrentDir).ToList<string>();
            elems.AddRange(Directory.GetFiles(CurrentDir).ToList<string>());

            //Следующий код пропускает поиск файлов в уже проверенных папках
            //используется только при возобновлении поиска.
            if (Restored == true)
            {//цель данного блока обход уже проверенных папок на основе полного пути последнего проверяемого файла 
                RestLevel++;
                List<string> temp = new List<string>();
                int j = 0;
                foreach (string elem in elems)
                {
                    if (elem == BuildPathUntil(RestoredElem))
                    {
                        for (int i = j; i < elems.Count; i++)
                        {
                            temp.Add(elems[i]);
                        }
                        break;
                    }
                    j++;
                }
                elems = temp;
                if (elems[0] == RestoredElem)
                {
                    Restored = false;
                }
            }
            
            //обход папок и файлов
            foreach (string element in elems)
            {
                if (Running == false)//используется для остановки поиска по требованию 
                {
                    if (Restored == true)
                    {
                        return;
                    }
                    else { RestoredElem = element; Restored = true; return; }
                }
                else
                        if (Directory.Exists(element)) //Если просматриваемый элемент является папкой, то ищем содержимое данной папки
                        {
                            Find(element);
                        }
                        else
                        {
                            if (File.Exists(element))
                            {
                                ScannedFiles++;
                                output(ScannedFiles.ToString(), 0);
                                output(element, 1);
                                if (SearchType.Search(element, SearchString, DoSearchContent, contentSearch))
                                {
                                    output(element, 2);
                                }
                            }
                        }
                    
            }
        }
        private void Find(string path)
        {
            CurrentDir = path;
            Find();
        }
        private void Continue()
        {//подготовка к восстановлению поиска
            string[] CollapsedPath = RestoredElem.Split('\\');
            string temp = "";
            RestLevel = 0;
            int startlevel = StartDir.Split('\\').Length;
            for (int i = 0; i < startlevel; i++)
            {
                temp += CollapsedPath[RestLevel];
                if (i!=startlevel-1)
                {
                    temp += "\\";
                    RestLevel++;
                }
            }
            CurrentDir = temp;
        }

        private string BuildPathUntil(string elem)
        {//Строит путь. Используется для восстановления полного пути.
            string[] CollapsedPath = elem.Split('\\');
            string temp = null;
            for (int i = 0; i <= RestLevel; i++)
            {
                temp += CollapsedPath[i];
                if (i<RestLevel)
                {
                    temp += "\\";
                }
            }
            return temp;
        }
    }
}