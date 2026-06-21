using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApp.DBLogic;
using NotesApp.Extensions;
using NotesApp.JwtLogic;
using NotesApp.Models;

namespace NotesApp.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly CryptoText cryptoText;

        private readonly DBConnection db;

        public NotesController(CryptoText _cryptoText, DBConnection _db)
        {
            cryptoText = _cryptoText;
            db = _db;
        }

        [HttpGet]
        public IActionResult Note(int id)
        {
            var note = db.Notes.FirstOrDefault(x => x.Id == id);
            note.Content = cryptoText.Decrypt(note.Content);
            return View(note);
        }

        [HttpPost]
        public IActionResult Note(Note note)
        {
            var existingNote = db.Notes.FirstOrDefault(n => n.Id == note.Id);
            existingNote.Title = note.Title;
            existingNote.Content = cryptoText.Encrypt(note.Content);
            db.SaveChanges();
            return RedirectToAction("Main", "Home");
        }

        public IActionResult NewNote()
        {
            string email = User.Claims.First(c => c.Type == "UserMail").Value;
            var user = db.UsersInfo.First(x => x.Email == email);
            Note newNote = new Note
            {
                UserId = user.Id,
                Title = "New note",
                Content = ""
            };
            db.Notes.Add(newNote);
            db.SaveChanges();
            return RedirectToAction("Note", new { id = newNote.Id });
        }

        public IActionResult DeleteNote(int id)
        {
            var note = db.Notes.FirstOrDefault(x => x.Id == id);
            if (note != null)
            {
                db.Notes.Remove(note);
                db.SaveChanges();
            }
            return RedirectToAction("Main", "Home");
        }
    }
}
