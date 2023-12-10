﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laba7
{
    enum UpdateType
    {
        UpdateName=1,
        UpdateSurname,
        UpdatePatronymic,
        UpdateisBlocked,
        UpdateTelephone,
        UpdateEmail
    }
    internal class UserBD
    {
        private List<User> users = new List<User>();


        public void AddUserWizard()
        {
            string answer = String.Empty;
            User tempUser;
            do {
                Console.WriteLine("Enter Surname");
                string surname = Console.ReadLine();

                Console.WriteLine("Enter Name");
                string name = Console.ReadLine();

                Console.WriteLine("Enter patronymic");
                string patronymic = Console.ReadLine();

                Console.WriteLine("Enter telephone number");
                string telnum = Console.ReadLine();

                Console.WriteLine("Enter email");
                string email = Console.ReadLine();

                tempUser = new User(surname, name, patronymic, false, email, telnum);

                Console.Write("YourUser: "); Console.WriteLine(tempUser.ToString());
                Console.WriteLine("Is all correct? [y/n]");
                answer=Console.ReadLine();
                if (answer == "n") continue;
                if (!CheckFIO(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.","ERROR");
                    Console.WriteLine("Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.");
                    return;
                }
                if (!CheckTelephoneNumber(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указан некорректный номер телефона", "ERROR");
                    Console.WriteLine("Указан некорректный номер телефона");
                    return;
                }
                if (!CheckEmail(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указан некорректный адрес e-mail", "ERROR");
                    Console.WriteLine("Указан некорректный адрес e-mail");
                    return;
                }
                break;
            }
            while (true);


            MyLogger.WriteLog($"Пользователь {Environment.UserName} добавил запись: {tempUser.ToString()}", "INFO");
            users.Add(tempUser);
        }

        public void AddUser(string surname, string name, string patronymic, string email, string telnum)
        {
            string answer = "n";
            bool isCorrcet = false;

            User tempUser = new User(surname, name, patronymic, false, email, telnum);

            if (!CheckFIO(tempUser))
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                    $"Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.", "ERROR");
                Console.WriteLine("Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.");
                return;
            }
            if (!CheckTelephoneNumber(tempUser))
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                    $"Указан некорректный номер телефона", "ERROR");
                Console.WriteLine("Указан некорректный номер телефона");
                return; 
            }
            if (!CheckEmail(tempUser))
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                    $"Указан некорректный адрес e-mail", "ERROR");
                Console.WriteLine("Указан некорректный адрес e-mail");
                return;
            }


            MyLogger.WriteLog($"Пользователь {Environment.UserName} добавил запись: {tempUser.ToString()}", "INFO");
            users.Add(tempUser);
        }

        public void ChangeUser(int id, UpdateType updateType)
        {
            User? delUser = users.Find((item) => item.Id == id);
            if(delUser == null)
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить несуществующую запись с id {id}", "ERROR");
                Console.WriteLine("Пользователь с данным id не найден");
                return;
            }

            switch (updateType)
            {
                case UpdateType.UpdateName:
                    break;

            }
        }

        public void RemoveUser(int id)
        {
            User? delUser = users.Find((item) => item.Id == id);
            if (delUser != null)
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} удалил запись: {delUser.ToString()}", "INFO");
                users.Remove(delUser);
            }
            else
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался удалить несуществующую запись с id {id}", "ERROR");
                Console.WriteLine("Пользователь с данным id не найден");
            }
        }

        public void ListUsers()
        {
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine((i+1)+")" + users[i]);
            }
        }






        public static bool CheckFIO(User testUser)
        {
            if (!char.IsUpper(testUser.Surname[0]))
            {
                return false;
            }
            if (!char.IsUpper(testUser.Name[0]))
            {
                return false;
            }
            if (!char.IsUpper(testUser.Patronymic[0]))
            {
                return false;
            }



            return true;
        }

        public static bool CheckEmail(User testUser)
        {
            try
            {
                MailAddress m = new MailAddress(testUser.Email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool CheckTelephoneNumber(User testUser)
        {
            //return Regex.Match(testUser.Telnum, @"^(\+[0-9]{11})$").Success;
            return Regex.Match(testUser.Telnum, @"\(?\+[0-9]{1,3}\)? ?-?[0-9]{1,3} ?-?[0-9]{3,5} ?-?[0-9]{4}( ?-?[0-9]{3})? ?(\w{1,10}\s?\d{1,6})?").Success;
        }





        public void GenerateTestUSers(int numOfUsers)
        {

            for (int i = 0; i < numOfUsers; i++)
            {
                users.Add(new User(RandomString(5), RandomString(5), RandomString(5), "Chel@mail.com", "+79885553535"));
            }
        }

        public static string RandomString(int length)
        {
            Random rnd = new Random();  
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }


    }
}
