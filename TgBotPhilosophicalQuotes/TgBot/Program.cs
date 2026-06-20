using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhilosophyTgBot
{
    internal class Program
    {
        static List<string> Quotes = new List<string>();
        static List<string> Stickers = new List<string>();
        static int counter = 0;
        static Random random = new Random();
        static bool flagFilter = false;

        static void Main(string[] args)
        {
            var bot = new TelegramBotClient("your_token");
            Parsing();
            ParsingStickers();
            bot.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //Если ошибка, то бот не закончит работу
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message: //Пришло сообщение, не реакция и т.д.
                        {
                            counter++;
                            var message = update.Message;
                            var id = message.Chat.Id;
                            switch (message.Type) //Тип сообщения(текст, аудио и т.д.)
                            {
                                case MessageType.Text:
                                    {
                                        var replyKeyboard = new ReplyKeyboardMarkup(
                                            new List<KeyboardButton[]>()
                                            {
                                            new KeyboardButton[]
                                            {
                                                new KeyboardButton("/start"),
                                                new KeyboardButton("/filter")
                                            }
                                            });
                                        
                                        if (message.Text == "/start")
                                        {
                                            await botClient.SendMessage(message.Chat.Id, "Привет, добро пожаловать в сборник цитат философов!", replyMarkup: replyKeyboard);
                                            await botClient.SendSticker(message.Chat.Id, "CAACAgIAAxkBAAENGV1nMK-W5_Bq9mkqqSMv65r3Ld8eQwACnxUAAl1ZyEtEDa1wW6NsuTYE");
                                            await botClient.SendMessage(message.Chat.Id, "Напиши любое сообщение, а я подберу тебе интересную цитату");
                                            flagFilter = false;
                                        }
                                        else if (message.Text == "/filter")
                                        {
                                            await botClient.SendMessage(message.Chat.Id, "Круто, что ты решил использовать фильтр. Введи сообщение или имя философа, и я найду тебе цитату с этим словом");
                                            flagFilter = true;
                                        }
                                        else if (flagFilter)
                                        {
                                            List<string> filterList = new List<string>();
                                            foreach (var str in Quotes)
                                            {
                                                if (str.ToLower().Contains(message.Text.ToLower()) && message.Chat.Id == id)
                                                {
                                                    filterList.Add(str);
                                                }
                                            }
                                            if (filterList.Count > 0)
                                            {
                                                await botClient.SendMessage(message.Chat.Id, filterList[random.Next(0, filterList.Count())]);
                                                if (counter >= 3)
                                                {
                                                    await botClient.SendSticker(message.Chat.Id, Stickers[random.Next(0, Stickers.Count())]);
                                                    counter = 0;
                                                }
                                            }
                                            else
                                            {
                                                await botClient.SendMessage(message.Chat.Id, "Не могу найти цитату с таким словом или именем, попробуй ещё раз(/filter)");
                                                await botClient.SendSticker(message.Chat.Id, "CAACAgIAAxkBAAENGV9nMLTskN0p8SVaDU8Y2k8r9JMa_QACWRcAAtSJ6UgInxcFXrXpEzYE");
                                            }
                                            flagFilter = false;
                                        }
                                        else
                                        {
                                            await botClient.SendMessage(message.Chat.Id, Quotes[random.Next(0, Quotes.Count())]);
                                            if(counter >= 3)
                                            {
                                                await botClient.SendSticker(message.Chat.Id, Stickers[random.Next(0, Stickers.Count())]);
                                                counter = 0;
                                            }
                                        }
                                    }
                                    return;
                                default:
                                    await botClient.SendMessage(message.Chat.Id, Quotes[random.Next(0, Quotes.Count())]);
                                    if (counter >= 3)
                                    {
                                        await botClient.SendSticker(message.Chat.Id, Stickers[random.Next(0, Stickers.Count())]);
                                        counter = 0;
                                    }
                                    return;

                            }
                        }
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Создаем переменную для сообщения об ошибке
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private static void Parsing()
        {
            var str = System.IO.File.ReadAllLines("Files/Quotes.txt");
            foreach (var line in str)
            {
                Quotes.Add(line);
            }
        }

        private static void ParsingStickers()
        {
            var str = System.IO.File.ReadAllLines("Files/Stickers.txt");
            foreach(var line in str)
            {
                Stickers.Add(line);
            }
        }
    }
}
