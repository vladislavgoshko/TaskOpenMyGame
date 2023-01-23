using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TaskExam
{
    internal class TaskSolver
    {
        public static void Main(string[] args)
        {
            TestGenerateWordsFromWord();
            TestMaxLengthTwoChar();
            TestGetPreviousMaxDigital();
            TestSearchQueenOrHorse();
            TestCalculateMaxCoins();
            Console.WriteLine("All Test completed!");
        }


        /// задание 1) Слова из слова
        public static List<string> GenerateWordsFromWord(string word, List<string> wordDictionary)
        {
            List<string> rightWords = new List<string>();

            foreach (var _wordDic in wordDictionary.OrderBy(x => x))
            {
                List<char> _word = word.ToLower().ToList();
                bool isAdd = true;
                foreach (var letter in _wordDic.ToLower())
                {
                    if (!_word.Contains(letter))
                    {
                        isAdd = false;
                        break;
                    }
                    _word.Remove(letter);
                }
                if (isAdd)
                {
                    rightWords.Add(_wordDic);
                }
            }

            return rightWords;
        }

        /// задание 2) Два уникальных символа
        public static int MaxLengthTwoChar(string word)
        {
            return findMaxLength(word, 0);
        }

        static int findMaxLength(string word, int max)
        {
            var uniqueLetters = word.Distinct();
            if (uniqueLetters.Count() < 2)
                return 0;
            if (uniqueLetters.Count() == 2)
            {
                if (!isHasRepeats(word) && word.Length > max)
                    max = word.Length;
                return max;
            }
            foreach (char c in uniqueLetters)
            {
                max = findMaxLength(word.Replace(c.ToString(), ""), max);
            }
            return max;
        }

        static bool isHasRepeats(string word)
        {
            for (int i = 1; i < word.Length; ++i)
            {
                if (word[i] == word[i - 1])
                    return true;
            }

            return false;
        }

        /// задание 3) Предыдущее число
        public static long GetPreviousMaxDigital(long value)
        {
            return findMaxComb(value.ToString().ToArray<char>(), value.ToString().Length - 1, -1, value);
        }

        static long findMaxComb(char[] comb, int level, long max, long mainNum)
        {
            if (level > 0)
            {
                for (int i = 0; i <= level; i++)
                {
                    char[] _comb = new char[comb.Length];
                    Array.Copy(comb, _comb, comb.Length);

                    var tmp = _comb[i];
                    _comb[i] = _comb[level];
                    _comb[level] = tmp;

                    if (level == 1 && _comb[0] != '0')
                    {
                        long num = Convert.ToInt64(new string(_comb));
                        if (num > max && num < mainNum)
                            max = num;
                    }

                    max = findMaxComb(_comb, level - 1, max, mainNum);
                }
                max = findMaxComb(comb, level - 1, max, mainNum);
            }
            return max;
        }

        /// задание 4) Конь и Королева
        public static List<int> SearchQueenOrHorse(char[][] gridMap)
        {
            return new List<int> { findPathHorse(gridMap, new List<Point>()), findPathQueen(gridMap, new List<Point>()) };
        }
        static int findPathQueen(char[][] grid, List<Point> path, int minPath = 0)
        {
            if (minPath == 0)
            {
                Point start = new Point();
                for (int i = 0; i < grid.Length; ++i)
                    for (int j = 0; j < grid[i].Length; ++j)
                        if (grid[i][j] == 's')
                        {
                            start = new Point(j, i);
                            break;
                        }

                path.Add(start);
                minPath = -1;
            }
            if (minPath != -1 && path.Count > minPath)
                return minPath;
            for (int i = 0; i < 8; ++i)
            {
                int distance = maxStep(grid, path.Last(), i);
                for (int j = 1; j <= distance; ++j)
                {
                    Point step = doStepQueen(grid, path, i, j);
                    if (step.X != -1)
                    {
                        if (grid[step.Y][step.X] == 'e')
                        {
                            if (path.Count < minPath || minPath == -1)
                            {
                                minPath = path.Count;
                            }
                            break;
                        }
                        var newPath = new List<Point>(path);
                        newPath.Add(step);
                        int res = findPathQueen(grid, newPath, minPath);
                        if (res < minPath || minPath == -1)
                            minPath = res;
                    }
                }
            }

            return minPath;
        }
        static int maxStep(char[][] grid, Point point, int direction)
        {
            List<Point> steps = new List<Point> {
                new Point(-1, -1),
                new Point(-1, 0),
                new Point(-1, 1),
                new Point(0, 1),
                new Point(0, -1),
                new Point(1, -1),
                new Point(1, 0),
                new Point(1, 1)
            };

            int max = 0;
            for (int i = 1; i < Math.Max(grid.Length, grid[0].Length); ++i)
            {
                Point tmp = new Point(point.X + steps[direction].X * i, point.Y - steps[direction].Y * i);
                if (!(tmp.X >= 0 && tmp.X < grid[0].Length && tmp.Y >= 0 && tmp.Y < grid.Length))
                    return max;
                if (grid[tmp.Y][tmp.X] == 'x')
                    return max;
                max++;
            }
            return max;
        }
        static Point doStepQueen(char[][] grid, List<Point> path, int numStep, int stepSize)
        {
            Point point = new Point(-1, -1);

            List<Point> steps = new List<Point> {
                new Point(-1, -1),
                new Point(-1, 0),
                new Point(-1, 1),
                new Point(0, 1),
                new Point(0, -1),
                new Point(1, -1),
                new Point(1, 0),
                new Point(1, 1)
            };

            Point tmp = path.Last();
            tmp.X += steps[numStep].X * stepSize;
            tmp.Y -= steps[numStep].Y * stepSize;


            if (tmp.X >= 0 && tmp.X < grid[0].Length && tmp.Y >= 0 && tmp.Y < grid.Length)
            {

                if (grid[tmp.Y][tmp.X] != 'x')
                {
                    if (!path.Contains(tmp))
                    {
                        point = tmp;
                    }
                }
            }

            return point;
        }
        static int findPathHorse(char[][] grid, List<Point> path, int minPath = 0)
        {
            if (minPath == 0)
            {
                Point start = new Point();
                for (int i = 0; i < grid.Length; ++i)
                    for (int j = 0; j < grid[i].Length; ++j)
                        if (grid[i][j] == 's')
                        {
                            start = new Point(j, i);
                            break;
                        }

                path.Add(start);
                minPath = -1;
            }
            if (minPath != -1 && path.Count > minPath)
                return minPath;
            for (int i = 0; i < 8; ++i)
            {
                Point step = doStepHorse(grid, path, i);
                if (step.X != -1)
                {
                    if (grid[step.Y][step.X] == 'e')
                    {
                        if (path.Count < minPath || minPath == -1)
                        {
                            minPath = path.Count;
                        }
                        break;
                    }
                    var newPath = new List<Point>(path);
                    newPath.Add(step);
                    int res = findPathHorse(grid, newPath, minPath);
                    if (res < minPath || minPath == -1)
                        minPath = res;
                }
            }

            return minPath;
        }

        static Point doStepHorse(char[][] grid, List<Point> path, int numStep)
        {
            Point point = new Point(-1, -1);

            List<Point> steps = new List<Point> {
                new Point(-1, 2),
                new Point(-1, -2),
                new Point( 1, 2),
                new Point( 1, -2),
                new Point(-2, 1),
                new Point(-2, -1),
                new Point( 2, 1),
                new Point( 2, -1)
            };

            Point tmp = path.Last();
            tmp.X += steps[numStep].X;
            tmp.Y += steps[numStep].Y;

            if (tmp.X >= 0 && tmp.X < grid[0].Length && tmp.Y >= 0 && tmp.Y < grid.Length)
            {
                if (grid[tmp.Y][tmp.X] != 'x')
                {
                    if (!path.Contains(tmp))
                    {
                        point = tmp;
                    }
                }
            }

            return point;
        }
        /// задание 5) Жадина
        public static long CalculateMaxCoins(int[][] mapData, int idStart, int idFinish)
        {
            //код алгоритма
            return maxCoins(mapData, new List<int>() { idStart }, idFinish, 0);
        }
        static long maxCoins(int[][] map, List<int> path, int end, long coins)
        {
            if (path.Last() == end)
            {
                return coins;
            }
            long max = -1;
            foreach (var vector in map.Where(x => x[0] == path.Last() && !path.Contains(x[1])))
            {
                long tmp = maxCoins(map, new List<int>(path) { vector[1] }, end, coins + vector[2]);
                max = Math.Max(max, tmp);
            }
            if (max == -1)
                return -1;
            return max;
        }
        /// Тесты (можно/нужно добавлять свои тесты) 

        private static void TestGenerateWordsFromWord()
        {
            var wordsList = new List<string>
            {
                "кот", "ток", "око", "мимо", "гром", "ром", "мама",
                "рог", "морг", "огр", "мор", "порог", "бра", "раб", "зубр"
            };

            AssertSequenceEqual(GenerateWordsFromWord("арбуз", wordsList), new[] { "бра", "зубр", "раб" });
            AssertSequenceEqual(GenerateWordsFromWord("лист", wordsList), new List<string>());
            AssertSequenceEqual(GenerateWordsFromWord("маг", wordsList), new List<string>());
            AssertSequenceEqual(GenerateWordsFromWord("погром", wordsList), new List<string> { "гром", "мор", "морг", "огр", "порог", "рог", "ром" });
        }

        private static void TestMaxLengthTwoChar()
        {
            AssertEqual(MaxLengthTwoChar("beabeeab"), 5);
            AssertEqual(MaxLengthTwoChar("а"), 0);
            AssertEqual(MaxLengthTwoChar("ab"), 2);
        }

        private static void TestGetPreviousMaxDigital()
        {
            AssertEqual(GetPreviousMaxDigital(21), 12l);
            AssertEqual(GetPreviousMaxDigital(531), 513l);
            AssertEqual(GetPreviousMaxDigital(1027), -1l);
            AssertEqual(GetPreviousMaxDigital(2071), 2017l);
            AssertEqual(GetPreviousMaxDigital(207034), 204730l);
            AssertEqual(GetPreviousMaxDigital(135), -1l);
        }

        private static void TestSearchQueenOrHorse()
        {
            char[][] gridA =
            {
                new[] {'s', '#', '#', '#', '#', '#'},
                new[] {'#', 'x', 'x', 'x', 'x', '#'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', '#', '#', 'e'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridA), new[] { 3, 2 });

            char[][] gridB =
            {
                new[] {'s', '#', '#', '#', '#', 'x'},
                new[] {'#', 'x', 'x', 'x', 'x', '#'},
                new[] {'#', 'x', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'x', '#', '#', '#', '#', 'e'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridB), new[] { -1, 3 });

            char[][] gridC =
            {
                new[] {'s', '#', '#', '#', '#', 'x'},
                new[] {'x', 'x', 'x', 'x', 'x', 'x'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', 'e', 'x', '#'},
                new[] {'x', '#', '#', '#', '#', '#'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridC), new[] { 2, -1 });


            char[][] gridD =
            {
                new[] {'e', '#'},
                new[] {'x', 's'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridD), new[] { -1, 1 });

            char[][] gridE =
            {
                new[] {'e', '#'},
                new[] {'x', 'x'},
                new[] {'#', 's'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridE), new[] { 1, -1 });

            char[][] gridF =
            {
                new[] {'x', '#', '#', 'x'},
                new[] {'#', 'x', 'x', '#'},
                new[] {'#', 'x', '#', 'x'},
                new[] {'e', 'x', 'x', 's'},
                new[] {'#', 'x', 'x', '#'},
                new[] {'x', '#', '#', 'x'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridF), new[] { -1, 5 });
        }

        private static void TestCalculateMaxCoins()
        {
            var mapA = new[]
            {
                new []{0, 1, 1},
                new []{0, 2, 4},
                new []{0, 3, 3},
                new []{1, 3, 10},
                new []{2, 3, 6},
            };

            AssertEqual(CalculateMaxCoins(mapA, 0, 3), 11l);

            var mapB = new[]
            {
                new []{0, 1, 1},
                new []{1, 2, 53},
                new []{2, 3, 5},
                new []{5, 4, 10}
            };

            AssertEqual(CalculateMaxCoins(mapB, 0, 5), -1l);

            var mapC = new[]
            {
                new []{0, 1, 1},
                new []{0, 3, 2},
                new []{0, 5, 10},
                new []{1, 2, 3},
                new []{2, 3, 2},
                new []{2, 4, 7},
                new []{3, 5, 3},
                new []{4, 5, 8}
            };

            AssertEqual(CalculateMaxCoins(mapC, 0, 5), 19l);
        }

        /// Тестирующая система, лучше не трогать этот код

        private static void Assert(bool value)
        {
            if (value)
            {
                return;
            }

            throw new Exception("Assertion failed");
        }

        private static void AssertEqual(object value, object expectedValue)
        {
            if (value.Equals(expectedValue))
            {
                return;
            }

            throw new Exception($"Assertion failed expected = {expectedValue} actual = {value}");
        }

        private static void AssertSequenceEqual<T>(IEnumerable<T> value, IEnumerable<T> expectedValue)
        {
            if (ReferenceEquals(value, expectedValue))
            {
                return;
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (expectedValue is null)
            {
                throw new ArgumentNullException(nameof(expectedValue));
            }

            var valueList = value.ToList();
            var expectedValueList = expectedValue.ToList();

            if (valueList.Count != expectedValueList.Count)
            {
                throw new Exception($"Assertion failed expected count = {expectedValueList.Count} actual count = {valueList.Count}");
            }

            for (var i = 0; i < valueList.Count; i++)
            {
                if (!valueList[i].Equals(expectedValueList[i]))
                {
                    throw new Exception($"Assertion failed expected value at {i} = {expectedValueList[i]} actual = {valueList[i]}");
                }
            }
        }
    }
}