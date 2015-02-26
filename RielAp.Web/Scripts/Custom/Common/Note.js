/// <reference path="../_references.js" />
var Note = {
    announcementsCookiesKey: "announcementsToSee",
    announcementIdSeparator: ',',

    addAnnouncementId: function (announcementId) {
        if (!Note.isAnnouncementIdAddedAlready(announcementId)) {
            var announcements = Note.getAnnouncementsIDs();
            announcements.push(announcementId);
            var d = new Date();
            d.setFullYear(d.getFullYear()+1);
            var c = new Cookie(Note.announcementsCookiesKey, announcements.join(Note.announcementIdSeparator), d);
            Cookie.setCookie(c);
        }
    },

    removeAnnouncementId: function (announcementId) {
        if (Note.isAnnouncementIdAddedAlready(announcementId)) {
            var announcements = Note.getAnnouncementsIDs();
            Note.removeFromArray(announcements, announcementId);
            var d = new Date();
            d.setFullYear(d.getFullYear() + 1);
            var c = new Cookie(Note.announcementsCookiesKey, announcements.join(Note.announcementIdSeparator), d);
            Cookie.setCookie(c);
        }
    },

    removeAllAnnouncements: function () {
        Cookie.deleteCookie(Note.announcementsCookiesKey);
    },

    getAnnouncementsIDs: function () {
        var announcementsIdsStr = Cookie.getCookie(Note.announcementsCookiesKey);
        if (announcementsIdsStr) {
            var announcementIds = announcementsIdsStr.split(Note.announcementIdSeparator);
            return announcementIds;
        }
        return [];

    },

    removeFromArray:function(ar, item){
        while((ax= ar.indexOf(item))!= -1){
            ar.splice(ax, 1);
        }
    },

    isAnnouncementIdAddedAlready: function (announcementId) {
        var announcements = Note.getAnnouncementsIDs();
        var res = false;
        if (announcements) {
            res = announcements.indexOf(announcementId) > -1;
        }
        return res;
    }
};