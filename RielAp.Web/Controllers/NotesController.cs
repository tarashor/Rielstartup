using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RielAp.Domain.Repositories;
using RielAp.Domain.Models;
using RielAp.Web.Models;

namespace RielAp.Web.Controllers
{
    public class NotesController : Controller
    {
        private IUsersRepository _userRepository;
        private IAnnouncementsRepository _announcementRepository;
        private const int PageSize = 20;

        public NotesController(IAnnouncementsRepository announcementRepository, IUsersRepository userRepository) 
        {
            _userRepository = userRepository;
            _announcementRepository = announcementRepository;
        }

        public ActionResult Index(int page = 1)
        {
            NoteViewModel model = new NoteViewModel();
            ICollection<Announcement> announcements = new List<Announcement>();
            IEnumerable<int> announcementIds = null;
            HttpCookie announcementIdsCookie = Request.Cookies["announcementsToSee"];
            if (announcementIdsCookie != null)
            {
                announcementIds = splitAnnouncementsIds(HttpUtility.UrlDecode(announcementIdsCookie.Value));
            }

            if (announcementIds != null) {
                foreach (int id in announcementIds) {
                    Announcement announcement = _announcementRepository.GetById(id);
                    if (announcement != null)
                    {
                        announcements.Add(announcement);
                    }
                }
            }

            model.Announcements = announcementsForPage(announcements, page);
            model.Page = createPageInfo(page, announcements.Count);

            return View(model);
        }

        private IEnumerable<int> splitAnnouncementsIds(string announcementsIds) {
            List<int> ids = null;
            IEnumerable<string> announcementsIdsStrings =  announcementsIds.Split(',').ToList();
            foreach (string strId in announcementsIdsStrings) { 
                int id;
                if (int.TryParse(strId, out id))
                {
                    if (ids == null) 
                        ids = new List<int>();

                    ids.Add(id);
                }
            }
            return ids;
        }

        private PageInfo createPageInfo(int page, int announcementCount)
        {
            PageInfo pageinfo = new PageInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = announcementCount
            };
            return pageinfo;
        }

        private IEnumerable<Announcement> announcementsForPage(IEnumerable<Announcement> announcements, int page)
        {
            return announcements.Skip((page - 1) * PageSize).Take(PageSize);
        }

        /*
        public ActionResult CurrentNote()
        {
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                Note currentNote = _notesRepository.GetCurrentNoteForUser(authenticatedUser);
                if (currentNote != null)
                {
                    return RedirectToAction("ApartmentsInNote", new { Id = currentNote.NoteId });
                }
            }
            return new EmptyResult();
        }

        public ActionResult ApartmentsInNote(int Id, int page = 1){
            Note note = _notesRepository.GetById(Id);
            if (note != null)
            {
                NoteViewModel viewModel = CreateNoteViewModel(page, note.Announcements);
                return View(viewModel);
            }
            return new EmptyResult();
            
        }

        private NoteViewModel CreateNoteViewModel(int page, IEnumerable<Announcement> announcements)
        {
            NoteViewModel viewModel = new NoteViewModel
            {
                Announcements = announcements.Skip((page - 1) * PageSize).Take(PageSize),
                Page = new PageInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = announcements.Count()
                }
            };
            return viewModel;
        }


        public ActionResult List()
        {
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            IEnumerable<Note> notes = null;
            if (authenticatedUser != null)
            {
                notes = authenticatedUser.Notes;
            }
            return View(notes);
        }



        [HttpPost]
        public ActionResult CreateNew()
        {
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                Note note = new Note();
                note.User = authenticatedUser;
                _notesRepository.SetCurrentNoteForUser(authenticatedUser, note);
                authenticatedUser.Notes.Add(note);
                note.Name = generateNameForNote(note);
                _notesRepository.Add(note);
                _notesRepository.SaveChanges();
            }
            return RedirectToAction("Apartments", "Announcements");
        }

        private string generateNameForNote(Note note)
        {
            string name = null;
            name = string.Format(Translation.Translation.NoteNamePattern, note.Created.ToShortDateString());
            int notesForThisDay = note.User.Notes.Where(n=>n.Created.Date == note.Created.Date).Count();
            bool moreThanOneNoteForDay = notesForThisDay  > 1;
            if (moreThanOneNoteForDay) {
                name += notesForThisDay.ToString();
            }
            return name;
        }

        public ActionResult DeleteNote(int id)
        {
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                Note note = _notesRepository.GetById(id);
                if (note.User == authenticatedUser)
                {
                    _notesRepository.Delete(note);
                    if ((note.IsCurrent) && (authenticatedUser.Notes.Count()>0)) {
                        _notesRepository.SetCurrentNoteForUser(authenticatedUser, authenticatedUser.Notes.Last());
                    }
                    _notesRepository.SaveChanges();
                    TempData["message"] = string.Format("{0} was deleted", note.Name);
                }
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult AddToNote(int id)
        {
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                Announcement apartment = _apartmentRepository.GetById(id);
                Note currentNote = _notesRepository.GetCurrentNoteForUser(authenticatedUser);
                _notesRepository.AddAnnouncementToNote(currentNote, apartment);
                _notesRepository.SaveChanges();
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteFromNote(int id)
        {
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                ApartmentAnnouncement apartment = _apartmentRepository.GetById(id) as ApartmentAnnouncement;
                Note currentNote = _notesRepository.GetCurrentNoteForUser(authenticatedUser);
                _notesRepository.DeleteAnnouncementFromNote(currentNote, apartment);
                _notesRepository.SaveChanges();
            }
            return new EmptyResult();
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _userRepository.Dispose();
                _announcementRepository.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
