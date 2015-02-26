using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Models;
using RielAp.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Controllers
{
    public class FeedbackController : Controller
    {
        //
        // GET: /Feedback/
        private int PageSize = 20;
        private string adminEmail = "info@aparts.in.ua";
        private readonly IEmailsRepository _emailsRepository;
        public FeedbackController(IEmailsRepository emailsRepository)
        {
            _emailsRepository = emailsRepository;
        }

        public ActionResult Index(int page=1)
        {
            IEnumerable<Email> feedbacks = _emailsRepository.GetVisibleParentEmails().OrderBy(e => e.Created);
            IEnumerable<Email> feedbacksPerPage = feedbacks.Skip((page - 1) * PageSize).Take(PageSize);
            FeedbackListModel model = new FeedbackListModel();
            model.Page = new PageInfo()
            {
                TotalItems = feedbacks.Count(),
                ItemsPerPage = PageSize,
                CurrentPage = page
            };
            model.FeedbacksPerPage = feedbacksPerPage;

            return View(model);
        }

        public ActionResult Add(int? parentEmailId = null)
        {
            Email feedback = new Email();
            feedback.ParentEmailId = parentEmailId;
            feedback.IsShown = true;
            return View(feedback);
        }

        [HttpPost]
        public ActionResult Add(Email feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.Created = DateTime.Now;
                _emailsRepository.Add(feedback);
                _emailsRepository.SaveChanges();

                MailService.SendEmail(adminEmail, "Відгук: " + feedback.Subject, bodyfromFeedback(feedback));

                return RedirectToAction("Index");
            }

            return View(feedback);
        }
        private string bodyfromFeedback(Email email) {
            return string.Format("<p>Ви отримали відгук від: {0} номер: {1}</p><p>{2}</p>", email.AuthorEmail, email.Id, email.Body);
        }


    }
}
