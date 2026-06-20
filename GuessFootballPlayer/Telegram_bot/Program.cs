using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parsing.Parssing(); //Файл с футболистами
            Parsing.ParssingStickers(); //Файлы со стикерами
            Parsing.DownloadingDataBase(); //Файл с пользователями

            var client = new TelegramBotClient("your_token");

            client.StartReceiving(Game.Update, Game.Error);
            Console.ReadLine();
        }
    }
}
