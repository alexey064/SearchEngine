using System;
using System.IO;
using System.Linq;


namespace SearchEngine
{
    class SearchWithoutParams :SearchAdapter
    {
        //ищет искомое слово. Совпадение может быть в любой части имени файла. 
        public bool Search(string FullPath, string SearchName, bool DoSearchByContent, string contentSearch)
        {
            if (String.IsNullOrEmpty(FullPath))
            {
                return false;
            }
            string FileName = FullPath.Split('\\').Last();       //получаем имя файла
            for (int i = 0; i < FileName.Length - SearchName.Length; i++) //ищем первый совпадающий элемент поиска
            {
                if (FileName[i] == SearchName[0])
                {
                    for (int j = 1; j < SearchName.Length; j++) //ищем последующие совпадающие объекты
                    {
                        if (SearchName[j] == FileName[i + j])
                        {
                            if (j == SearchName.Length - 1) //Если совпала вся последовательность, то найден объект поиска
                            {
                                if (DoSearchByContent) 
                                {
                                    SearchByContent search = new SearchByContent();
                                    if (search.Search(FullPath, contentSearch))
                                    {
                                        return true;
                                    }
                                    else return false;
                                }
                                return true;
                            }
                        }
                        else break;
                    }
                }
            }
            return false;
        }
    }

    class SearchWithParams : SearchAdapter
    {
        //поиск файлов с учетом различных масок поиска
        public bool Search(string FullPath, string SearchName, bool DoSearchByContent, string contentSearch)
        {
            if (String.IsNullOrEmpty(FullPath))
            {
                return false;
            }
            string FileName = FullPath.Split('\\').Last();
            int j = 0;
            bool ContinueAble = false;
            for (int i = 0; i < SearchName.Length; i++)
            {
                switch (SearchName[i])
                {
                    case '*':
                        ContinueAble = true;
                        break;
                    case '?':
                        j++;
                        break;
                    default:
                        if (j < FileName.Length)
                        {
                            if (SearchName[i] == FileName[j])
                            {
                                j++;
                            }
                            else if (ContinueAble == true)
                            {
                                i = LastStar(SearchName, i);
                                j++;
                            }
                            else return false;
                        }
                        else return false;
                        break;
                }
            }
            if (SearchName[SearchName.Length-1]=='?')
            {
                return false;
            }
            if (DoSearchByContent)
            {
                SearchByContent search = new SearchByContent();
                if (search.Search(FullPath, contentSearch))
                {
                    return true;
                }
                else return false;
                
            }
            
            return true;
        }

        private int LastStar(string SearchName, int CurrentPos) 
        {
            for (int i = CurrentPos; i >-1; i--)
            {
                if (SearchName[i]=='*')
                {
                    return i++;
                }
            }
            return 0;
        }
    }

    class SearchHard : SearchAdapter
    {
        //поиск со строгим соответствием введенного. Ищет подслово только в начале имени файла
        public bool Search(string FullPath, string SearchName, bool DoSearchByContent, string contentSearch)
        {
            if (String.IsNullOrEmpty(FullPath))
            {
                return false;
            }
            string Filename = FullPath.Split('\\').Last();
            for (int i = 0; i < SearchName.Length; i++)
            {
                if (Filename[i]!=SearchName[i])
                {
                    return false;
                }
            }
            if (DoSearchByContent)
            {
                SearchByContent search = new SearchByContent();
                if (search.Search(FullPath, contentSearch))
                {
                    return true;
                }
                else return false;
            }
            return true;
        }
    }
    class SearchByContent 
    {
        public bool Search(string FullPath, string SearchString) 
        {
            using (StreamReader reader = new StreamReader(FullPath)) 
            {
                int CurrentStrok = 0;
                while (!reader.EndOfStream)
                {
                    CurrentStrok++;
                    string stroka = reader.ReadLine();
                    string[] words = stroka.Split(' ');
                    foreach (string word in words)
                    {
                        if (SearchHard(word, SearchString))
                        {
                            return true;
                        }
                    }
                }
            }
                return false;
        }

        private bool SearchHard(string FullName, string SearchName) 
        {
            if (String.IsNullOrEmpty(FullName))
            {
                return false;
            }
            if (SearchName.Length != FullName.Length)
            {
                return true;
            }
            for (int i = 0; i < SearchName.Length; i++)
            {
                if (FullName[i] != SearchName[i])
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}