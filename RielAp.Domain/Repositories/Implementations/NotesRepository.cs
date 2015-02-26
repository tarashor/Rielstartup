using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class NotesRepository:Repository<Note>, INotesRepository
    {
        public NotesRepository(RealtyDBContext context)
            : base(context)
        {
        }

        public void AddAnnouncementToNote(Note note, Announcement apartment)
        {
            if ((note != null) && (apartment != null))
            {
                if (!note.Announcements.Contains(apartment))
                {
                    note.Announcements.Add(apartment);
                }
            }
        }

        public void DeleteAnnouncementFromNote(Note note, Announcement apartment)
        {
            if ((note != null) && (apartment != null))
            {
                if (note.Announcements.Contains(apartment))
                {
                    note.Announcements.Remove(apartment);
                }
            }
        }


        public Note GetCurrentNoteForUser(User user)
        {
            if (user == null) return null;
            return SearchFor(n => (n.UserId == user.UserID) && (n.IsCurrent)).FirstOrDefault();
        }
        
        public void SetCurrentNoteForUser(User user, Note note)
        {
            Note currentNoteOld = GetCurrentNoteForUser(user);
            if (currentNoteOld != null)
            {
                currentNoteOld.IsCurrent = false;
            }
            note.IsCurrent = true;
        }
    }
}
