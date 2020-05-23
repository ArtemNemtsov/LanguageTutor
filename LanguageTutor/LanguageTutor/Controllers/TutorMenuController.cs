using LanguageTutorService;
using LanguageTutorService.Models;
using LanguageTutorService.Services;
using LanguageTutorService.ViewModels;
using MediaStudioService.Core.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace LanguageTutor.Controllers
{
    [Authorize]
    public class TutorMenuController : Controller
    {
       private TopicViewModel tutorMenuModel;
       private readonly TopicService _topicService;
       private readonly TutorService _tutorService;


        public TutorMenuController(TopicService topicService, TutorService tutorService)
        {
            _tutorService = tutorService;
            _topicService = topicService;
        }

        public IActionResult Main()
        {
            return View();
        }
        
        public IActionResult SelectLang()
        {
            var topics = _topicService.GetTopics();

            tutorMenuModel = new TopicViewModel
            {
                TopicList = new SelectList(topics, "IdTopic", "NameTopic"),
            };

            return View(tutorMenuModel);
        }
    }
}