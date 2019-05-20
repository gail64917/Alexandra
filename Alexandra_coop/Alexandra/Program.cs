using System;
using System.Collections.Generic;

namespace Alexandra
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Считывание S
            Console.WriteLine("Введите Количество шагов через пробел");
            string UserInput = Console.ReadLine();
            uint steps = ParseInput(UserInput);

            while (steps == 0)
            {
                Console.WriteLine("Неверный формат! Введите Количество шагов через пробел");
                UserInput = Console.ReadLine();
                steps = ParseInput(UserInput);
            }
            if (steps != 0)
            {
                Console.WriteLine("Количество шагов: S = {0} ", steps);
            }
            #endregion

            #region Считывание матрицы
            Console.WriteLine("\r\nВведите через пробел A, B, C для матрицы: \r\n AA B0 \r\n 0B CC");
            string UserInputMatrix = Console.ReadLine();
            List<float> inputsMatrix = ParseInputMatrix(UserInputMatrix);
            while (inputsMatrix.Count < 3)
            {
                Console.WriteLine("Неверный формат! Введите через пробел A, B, C для матрицы: \r\n AA B0 \r\n 0B CC");
                UserInputMatrix = Console.ReadLine();
                inputsMatrix = ParseInputMatrix(UserInputMatrix);
            }
            Console.WriteLine("Введенная матрица: \r\n {0},{0}   {1},0 \r\n 0,{1}   {2},{2}", inputsMatrix[0], inputsMatrix[1], inputsMatrix[2]);
            #endregion

            #region Инстанс класса рандом
            Random rd = new Random();
            #endregion


            #region Инициализация 1 шага

            int step = 0;

            // r - вероятность скооперироваться. Высчитывается на каждом шаге
            float A = inputsMatrix[0];
            float B = inputsMatrix[1];
            float C = inputsMatrix[2];
            float r = A / (A + C);      // r = A / (A + C)

            //Первый шаг
            if (rd.Next(0, 101) > r * 100)
            {
                Console.WriteLine("Coop = 1");
            }
            else
            {
                Console.WriteLine("Coop = 0");
            }

            
            #endregion

            #region 2 и следующие шаги
            //ВТОРОЙ И СЛЕДУЮЩИЙ ШАГИ
            while (step < steps)
            {
                if (rd.Next(0, 101) > r * 100)
                {
                    Console.WriteLine("Coop = 1");
                }
                else
                {
                    Console.WriteLine("Coop = 0");
                }
                step++;
            }
            #endregion
        }

        /// <summary>
        /// Парсим ввод пользователем S
        /// </summary>
        /// <param name="UserInput"></param>
        /// <returns></returns>
        static uint ParseInput(string UserInput)
        {
            UserInput = UserInput.TrimStart().TrimEnd();
            List<uint> result = new List<uint>();
            string[] inputs = UserInput.Split();
            foreach (string input in inputs)
            {
                uint value;
                uint.TryParse(input, out value);
                if(value != 0)
                {
                    result.Add(value);
                }
            }

            return result.Count != 0 ? result[0] : 0;
        }


        /// <summary>
        /// Парсим ввод пользователем матрицы
        /// </summary>
        /// <param name="UserInput"></param>
        /// <returns></returns>
        static List<float> ParseInputMatrix(string UserInput)
        {
            UserInput = UserInput.TrimStart().TrimEnd().ToUpper();
            List<float> result = new List<float>();
            string[] inputs = UserInput.Split();
            foreach (string input in inputs)
            {
                float value;
                bool success = float.TryParse(input, out value);
                if (success)
                {
                    result.Add(value);
                }
            }

            return result;
        }
    }
}
