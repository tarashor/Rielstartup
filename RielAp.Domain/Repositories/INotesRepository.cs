using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories
{
    public interface INotesRepository:IRepository<Note>
    {
        void AddAnnouncementToNote(Note note, Announcement apartment);
        void DeleteAnnouncementFromNote(Note note, Announcement apartment);
        Note GetCurrentNoteForUser(User user);
        void SetCurrentNoteForUser(User user, Note note);
    }
}
