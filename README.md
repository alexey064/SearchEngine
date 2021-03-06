# SearchEngine

## Название: SearchEngine.

## Язык:
c#.

## Используемые технологии:
Windows forms, .NET Framework.

## Краткое описание:
Программа, которая поможет найти потерянные файлы.Она может учитывать различные маски ввода и даже искать по содержимому в файлах.

## Краткая документация:
В программе имеется всего одна форма, на ней несколько текстовых полей и одна кнопка. В коде формы находятся методы для каждого элемента управления, а методы также управления поиском.
Методы start отвечает за запуск поиска, а restore - продолжает остановившийся поиск. Метод OutText используется для вывода информации о результатах поиска, и других вспомогательных элементах.
После завершения поиска программа сохраняет последний запрос в файл. Программа позволяет возобновлять поиск даже после перезапуска программы.

Класс Search выполняет рекурсивный поиск подходящего файла. В подготовительной фазе поиска выполняется проверка входных данных на корректность, а также выбирается тип поиска.
В программе есть три вида поиска: обычный - ищет название файла в любой части названия. Параметры поискового запроса (*, ?) считает как часть названия. Строгий - ищет совпадение по точному названию файла, сравнивать начинает по первому символу.
Параметризированный - поиск с учетом параметров. Параметр ? означает что на месте этого символа может быть один любой символ. Параметр * означает что на месте этого символа может быть много любых символов.

Если поиск возобновляется, то поиск ускоренно проходит все папки пока не дойдет до той, на которой он остановился. Выполнение этой операции начнется формирования пути, на основе последнего проверенного файла.

Файл SearchAdapter.cs представляет собой интерфейс для раличных типов поиска.

Файл SearchType состоит из нескольких классов, каждый из которых определяет свой тип поиска и реализует интерфейс SearchAdapter.

TreeViewExtension представляет собой статический класс, который расширяет функциональность TreeView и позволяет легко добавлять в него элементы.
