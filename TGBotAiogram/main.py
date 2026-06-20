#Aiogram - асинхронная библиотека для написания Telegram Bot

import asyncio

from aiogram import Bot #Отправляет запросы и принимает ответы
from aiogram import Dispatcher #Обрабатывает события
from aiogram import types
from aiogram.filters import CommandStart, Command
from aiogram.utils import markdown
from aiogram.enums import ParseMode
from aiogram import F

BOT_TOKEN=#your_token

dp = Dispatcher()


@dp.message(CommandStart())
async def handle_start(message: types.Message):
    url = "https://w7.pngwing.com/pngs/547/380/png-transparent-robot-waving-hand-bot-ai-robot-thumbnail.png"
    await message.answer(f"{markdown.hide_link(url=url)}Hello, {markdown.hbold(message.from_user.full_name)}!",parse_mode=ParseMode.HTML)


@dp.message(Command("help"))
async def handle_help(message: types.Message):
    #1 вариант
    # entity_bold = types.MessageEntity(type="bold", offset=len("I'm an echo bot.\nSend me "), length=3)
    # entities = [entity_bold]
    # await message.answer("I'm an echo bot.\nSend me any message!",entities=entities)
    #2 вариант(sep = разделитель)
    # text = markdown.text(
    #     markdown.blockquote("I'm an echo bot."), #Чтобы самим не экранировать спец. символы(в видосе было markdown_decoration.quote)
    #     markdown.text( #автоматический разделитель - пробел
    #         "Send me",
    #         markdown.bold("any"),
    #         markdown.blockquote("message!")),
    #     sep="\n"
    # )
    #3 вариант
    text = "I'm an echo bot\\.\nSend me *any* message\\!"
    await message.answer(text=text,parse_mode=ParseMode.MARKDOWN_V2)
    message.contac


@dp.message(Command("code", prefix="/!%")) #Какие символы могут быть в начале команды
async def handle_command_code(message: types.Message):
    text = markdown.text(
        "Here's Python code:",
        "",
        markdown.pre("print(1)"),
        sep="\n"
    )
    await message.answer(text=text, parse_mode=ParseMode.MARKDOWN_V2)


# def is_photo(message: types.Message):
#     return message.photo

# @dp.message(is_photo)
#@dp.message(lambda message: message.photo) #Проверим, что у message есть фото. Если есть фотография, то заходим в функцию
@dp.message(F.photo, ~F.caption) #То есть должно быть фото, но без описания
async def handle_photo_without_caption(message: types.Message):
    caption = "I can't see, sorry. Could you describe it please"
    message.photo
    await message.reply(caption)


@dp.message(F.photo, F.caption.contains("please"))
async def handle_photo_with_please_caption(message: types.Message):
    await message.reply("Don't beg me. I can't see, sorry.")


any_media_filter = F.photo | F.video | F.document
@dp.message(any_media_filter, ~F.caption) #Фото, видео или документ, но без описания
async def handle_any_media_wo_caption(message: types.Message):
    await message.reply("I can't see")


@dp.message(any_media_filter, F.caption) #Фото, видео или документ с описанием
async def handle_any_media_w_caption(message: types.Message):
    await message.reply(f"Something is no media. Your text: {message.caption!r}") #!r - строчка в кавычках


@dp.message(F.from_user.id.in_({42, 1111}), F.text == "secret") #Если напишет человек с одним из таких id и сообщение secret
async def sercet_admin_message(message: types.Message):
    await message.reply("Hi, admin!")


@dp.message()
async def echo_message(message: types.Message):
    await message.answer("Wait a second...")
    # if message.text:
    #     await message.answer(message.text,entities=message.entities)
    try:
        await message.copy_to(message.chat.id) #Отправляет такое же сообщение, от телеграма
        # await message.forward(message.chat.id) Показывает, что сообщение переслано
        # await message.send_copy(message.chat.id) Отправляет такое же сообщение, от aiogram
    except TypeError:
        await message.reply("Something new")

async def main():
    bot = Bot(token=BOT_TOKEN)
    await dp.start_polling(bot) #Ждёт обновления

if __name__ == "__main__": #позволяет запускать функцию только в том случае, если мы запускаем файл, а не импорт его например
    asyncio.run(main()) #запуск асинхронной функции вне синхронной, просто так не можем