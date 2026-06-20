using Microsoft.AspNetCore.Mvc;
using BookManage.Models;

namespace BookManage.Controllers
{
    public class BooksController : Controller
    {
        //Выводит список всех книг
        public IActionResult Index()
        {
            string howSort = Request.Query["sort"];
            List<Book> books = Library.Books;
            switch (howSort)
            {
                case "title":
                    books = books.OrderBy(x => x.Title).ToList();
                    break;
                case "author":
                    books = books.OrderBy(x => x.Author).ToList();
                    break;
                case "year":
                    books = books.OrderBy(x => x.Year).ToList();
                    break;
                default:
                    break;
            }
            return View(books);
        }

        //Выводит детали книги по id с помощью формы
        [HttpPost]
        public IActionResult Details()
        {
            int id = int.Parse(Request.Form["id"]);
            if (Library.Books.Count > id)
            {
                return View(Library.Books[id]);
            }
            return View(new Book { Id = -1 });
        }

        //Выводит детали книги по id
        [HttpGet]
        public IActionResult Details(int id)
        {
            if(id >= 0 && Library.Books.Count > id)
            {
                return View(Library.Books[id]);
            }
            return View(new Book { Id = -1 });
        }

        //Возвращает форму для создания новой книги
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Принимает данные формы и добавляет книгу
        [HttpPost]
        public IActionResult Create(Book book)
        {
            if(book != null)
            {
                book.Id = Library.Books.Count;
                Library.Books.Add(book);
            }
            return RedirectToAction("Index");
        }
    }
}
