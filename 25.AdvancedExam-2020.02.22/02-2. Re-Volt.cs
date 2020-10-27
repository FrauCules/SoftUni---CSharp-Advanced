﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace _02._Re_Volt
{
    class Program
    {
        public class Position
        {
            public Position(int row, int col)
            {
                Col = col;
                Row = row;
            }

            public int Row { get; set; }

            public int Col { get; set; }

            public bool IsSafe(int rowCount, int colCount) // оставаме ли в матрицата? ДА - true / НЕ - false
            {
                if (Row < 0 || Col < 0)
                {
                    return false;
                }
                if (Row >= rowCount || Col >= colCount)
                {
                    return false;
                }

                return true;
            }

            public void CheckOtherSideMovement(int rowCount, int colCount) // ако сме излезли извън матрицата, влизаме от другата страна
            {
                if (Row < 0)
                {
                    Row = rowCount - 1;
                }
                if (Col < 0)
                {
                    Col = colCount - 1;
                }
                if (Row >= rowCount)
                {
                    Row = 0;
                }
                if (Col >= colCount)
                {
                    Col = 0;
                }
            }

            public static Position GetDirection(string command)
            {
                int row = 0;
                int col = 0;
                if (command == "left")
                {
                    col = -1;
                }
                if (command == "right")
                {
                    col = 1;
                }
                if (command == "up")
                {
                    row = -1;
                }
                if (command == "down")
                {
                    row = 1;
                }

                return new Position(row, col);
            }
        }
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine()); //размера на матрицата
            char[,] matrix = new char[n, n];
            int commands = int.Parse(Console.ReadLine()); // колко команди ще бъдат подадени
            
            ReadMatrix(matrix);
            var player = GetPlayerPosition(matrix);
            if (commands > 0)
            {
                matrix[player.Row, player.Col] = '-';
            }
            for (int i = 0; i < commands; i++)
            {
                string command = Console.ReadLine();

                MovePlayer(player, command, n);
                while (matrix[player.Row, player.Col] == 'B')
                {
                    MovePlayer(player, command, n);
                }

                while (matrix[player.Row, player.Col] == 'T')
                {
                    Position direction = Position.GetDirection(command); 
                    player.Row += direction.Row * -1; // връща ред назад
                    player.Col += direction.Col * -1; // връща колона назад
                }

                if (matrix[player.Row, player.Col] == 'F')
                {
                    Console.WriteLine($"Player won!");
                    matrix[player.Row, player.Col] = 'f';
                    PrintMatrix(matrix);
                    return;
                }
            }
            Console.WriteLine($"Player lost!");
            matrix[player.Row, player.Col] = 'f';
            PrintMatrix(matrix);
        }

        static Position MovePlayer(Position player, string command, int n) // 
        {
            Position movement = Position.GetDirection(command); // в каква посока се движим
            player.Row += movement.Row; // изчислява новата позиция на реда
            player.Col += movement.Col; // изчислява новата позиция на колоната
            player.CheckOtherSideMovement(n, n);    // ако сме излезли изнън матрицата, влизаме от другата страна

            return player;  // връща новата позиция
        }
        static Position GetPlayerPosition(char[,] matrix) // намиране позицията на играча
        {
            Position position = null;
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col] == 'f')
                    {
                        position = new Position(row, col);
                    }
                }
            }

            return position;
        }

        private static void ReadMatrix(char[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                string line = Console.ReadLine();
                for (int col = 0; col < line.Length; col++)
                {
                    matrix[row, col] = line[col];
                }
            }
        }   // четем матрицата от конзолата

        private static void PrintMatrix(char[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write($"{matrix[row, col]}");
                }
                Console.WriteLine();
            }
        }   // разпечатваме матрицата
    }
}
