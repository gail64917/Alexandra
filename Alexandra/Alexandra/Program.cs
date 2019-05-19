using System;
using System.Collections.Generic;

namespace Alexandra
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Считывание q и S
            Console.WriteLine("Введите вероятность z=1 и Количество шагов через пробел");
            string UserInput = Console.ReadLine();
            List<float> inputs = ParseInput(UserInput);
            uint stepValue;

            bool stepsValueCorrect = false;
            if(inputs.Count >= 2)
            {
                stepsValueCorrect = uint.TryParse(inputs[1].ToString(), out stepValue);
            }
            while (!(inputs.Count >= 2 && stepsValueCorrect && inputs[0] >= 0 && inputs[0] <= 1))
            {
                Console.WriteLine("Неверный формат! Введите вероятность P(z=1) и Количество шагов через пробел");
                UserInput = Console.ReadLine();
                inputs = ParseInput(UserInput);
                if (inputs.Count >= 2)
                {
                    stepsValueCorrect = uint.TryParse(inputs[1].ToString(), out stepValue);
                }
            }
            stepsValueCorrect = uint.TryParse(inputs[1].ToString(), out stepValue);
            if (stepsValueCorrect)
            {
                Console.WriteLine("Введенные числа: q = {0}, S = {1} ", inputs[0], stepValue);
            }
            #endregion

            #region Считывание усилий игроков
            Console.WriteLine("\r\nВведите через пробел усилия (C - дорогостоящие; D - дешевые), прилагаемые игроком 1 и игроком 2");
            string UserInputStrategies = Console.ReadLine();
            List<char> inputsStrategies = ParseInputStrategies(UserInputStrategies);
            while(inputsStrategies.Count < 2)
            {
                Console.WriteLine("Неверный формат! Введите через пробел усилия (C - дорогостоящие; D - дешевые), прилагаемые игроком 1 и игроком 2");
                UserInputStrategies = Console.ReadLine();
                inputsStrategies = ParseInputStrategies(UserInputStrategies);
            }
            Console.WriteLine("Введенные стратегии: 1 = {0}, 2 = {1} ", inputsStrategies[0], inputsStrategies[1]);
            #endregion
            
            #region Инициализация L
            Random rd = new Random();
            float q = inputs[0];
            int leftSide = (int)Math.Round((1 - q) * 1000);
            int rightSide = (int)Math.Round((1 - q) * 1000 / q);
            float L = Convert.ToSingle(rd.Next(leftSide, rightSide)) / 1000;
            #endregion


            #region Инициализация 1 шага
            float[] result = new float[2] { 0, 0 };

            int step = 0;
            int r1, r2, z;

            
            char CurrentPlayer1Choice = inputsStrategies[0];
            char CurrentPlayer2Choice = inputsStrategies[1];

            string matrixResult = "";

            //Первый шаг
            char y1 = CurrentPlayer1Choice;
            char y2 = CurrentPlayer2Choice;

            matrixResult = GetMatrixResult(CurrentPlayer1Choice, CurrentPlayer2Choice, L, result);

            Console.WriteLine("\r\n\r\nШаг {0}: игрок1 = {1}, игрок2 = {2}", step, CurrentPlayer1Choice, CurrentPlayer2Choice);
            Console.WriteLine("у1 = {0}, y2 = {1}", y1, y2);
            Console.WriteLine(matrixResult);
            #endregion

            #region 2 и следующие шаги
            //ВТОРОЙ И СЛЕДУЮЩИЙ ШАГИ
            while (step < stepValue)
            {
                L = Convert.ToSingle(rd.Next(leftSide, rightSide)) / 1000;

                int threshold = 30; //30% - вероятность, что примет 0, 70% - вероятность, что примет 1. Значение threshold располагается в диапазоне 0-100.

                r1 = rd.Next(0, 101) > threshold ? 1 : 0;
                r2 = rd.Next(0, 101) > threshold ? 1 : 0;

                z = rd.Next(0, 2);


                //ДЛЯ ИГРОКА 1 НА ТЕКУЩЕМ ШАГЕ
                if(CurrentPlayer1Choice == 'C')
                {
                    if(r1 == 1)
                    {
                        y1 = CurrentPlayer2Choice;
                    }
                    else //r1 = 0
                    {
                        y1 = CurrentPlayer2Choice == 'C' ? 'D' : 'C';               //Другой игрок на предыдущем шаге НАОБОРОТ
                    }
                    CurrentPlayer1Choice = (z == 0 && y1 == 'D') ? 'D' : 'C';
                }
                else
                {
                    CurrentPlayer1Choice = (z == 1) ? 'C' : 'D';
                }


                //ДЛЯ ИГРОКА 2 НА ТЕКУЩЕМ ШАГЕ
                if (CurrentPlayer2Choice == 'C')
                {
                    if (r2 == 1)
                    {
                        y2 = CurrentPlayer1Choice;
                    }
                    else //r2 = 0
                    {
                        y2 = CurrentPlayer1Choice == 'C' ? 'D' : 'C';               //Другой игрок на предыдущем шаге НАОБОРОТ
                    }
                    CurrentPlayer2Choice = (z == 0 && y2 == 'D') ? 'D' : 'C';
                }
                else
                {
                    CurrentPlayer2Choice = (z == 1) ? 'C' : 'D';
                }


                step++;


                matrixResult = GetMatrixResult(CurrentPlayer1Choice, CurrentPlayer2Choice, L, result);

                Console.WriteLine("\r\n\r\nШаг {0}: игрок1 = {1}, игрок2 = {2}. ", step, CurrentPlayer1Choice, CurrentPlayer2Choice);
                Console.WriteLine("у1 = {0}, y2 = {1}, z = {2}", y1, y2, z);
                Console.WriteLine(matrixResult);
            }
            #endregion

            Console.WriteLine("\r\n\r\nИтог Игр: {0}; {1}", result[0], result[1]);
        }

        /// <summary>
        /// Парсим ввод пользователем q и S. Закидываем все в лист float
        /// </summary>
        /// <param name="UserInput"></param>
        /// <returns></returns>
        static List<float> ParseInput(string UserInput)
        {
            UserInput = UserInput.TrimStart().TrimEnd();
            List<float> result = new List<float>();
            string[] inputs = UserInput.Split();
            foreach (string input in inputs)
            {
                float value;
                float.TryParse(input, out value);
                if(value != 0)
                {
                    result.Add(value);
                }
            }

            return result;
        }

        /// <summary>
        /// Парсим ввод пользователем стратегий (усилий) для игроков
        /// </summary>
        /// <param name="UserInput"></param>
        /// <returns></returns>
        static List<char> ParseInputStrategies(string UserInput)
        {
            UserInput = UserInput.TrimStart().TrimEnd().ToUpper();
            List<char> result = new List<char>();
            string[] inputs = UserInput.Split();
            foreach (string input in inputs)
            {
                char value;
                bool success = char.TryParse(input, out value);
                if(success && (value == 'C' || value == 'D'))
                {
                    result.Add(value);
                }
            }

            return result;
        }

        /// <summary>
        /// Высчитывание результата шага
        /// </summary>
        /// <param name="CurrentPlayer1Choice"></param>
        /// <param name="CurrentPlayer2Choice"></param>
        /// <returns></returns>
        static string GetMatrixResult(char CurrentPlayer1Choice, char CurrentPlayer2Choice, float L, float[] result)
        {
            string matrixResult = "";
            if (CurrentPlayer1Choice == 'C')
            {
                if (CurrentPlayer2Choice == 'C')
                {
                    matrixResult = "1,1";
                    result[0] += 1;
                    result[1] += 1;
                }
                else
                {
                    matrixResult = (-L).ToString() + "; " + (1 + L).ToString();
                    result[0] += -L;
                    result[1] += 1+L;
                }
            }
            else
            {
                if (CurrentPlayer2Choice == 'C')
                {
                    matrixResult = (1 + L).ToString() + "; " + (-L).ToString();
                    result[0] += 1+L;
                    result[1] += -L;
                }
                else
                {
                    matrixResult = "0,0";
                    result[0] += 0;
                    result[1] += 0;
                }
            }

            return matrixResult;
        }
    }
}
