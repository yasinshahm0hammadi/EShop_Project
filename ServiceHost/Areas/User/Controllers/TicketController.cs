using EShop.Application.Services.Implementation;
using EShop.Application.Services.Interface;
using EShop.Domain.DTOs.Contact.Ticket;
using EShop.Domain.Entities.Contact.Ticket;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.Controllers;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region Constructor

        private readonly IContactService _contactService;
        private readonly IUserService _userService;

        public TicketController(IContactService contactService, IUserService userService)
        {
            _contactService = contactService;
            _userService = userService;
        }

        #endregion

        #region Actions

        #region Tickets List

        [HttpGet("user-tickets-list")]
        public async Task<IActionResult> TicketsList(FilterTicketDto ticketList)
        {
            ticketList.UserId = User.GetUserId();
            ticketList.OrderBy = FilterTicketOrder.CreateDateDescending;

            var tickets = await _contactService.TicketsList(ticketList);
            return View(tickets);
        }

        #endregion

        #region Add Ticket

        [HttpGet("add-ticket")]
        public IActionResult AddUserTicket()
        {
            return View();
        }

        [HttpPost("add-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserTicket(AddTicketDto ticket)
        {
            if (ModelState.IsValid)
            {
                var creatorName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _contactService.AddUserTicket(ticket, User.GetUserId(), creatorName);

                switch (result)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "در فرایند ثبت تیکت خطایی رخ داد، لطفا بعدا تلاش کنید";
                        return RedirectToAction("TicketsList", "Ticket", new { area = "User"});
                    case AddTicketResult.EmptyText:
                        TempData[ErrorMessage] = "لطفا متن تیکت را وارد کنید";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد.";
                        TempData[InfoMessage] = "همکاران ما به زودی تیکت شما را بررسی خواهند کرد.";
                        return RedirectToAction("TicketsList", "Ticket", new { area = "User" });
                        
                }
            }

            return View(ticket);
        }

        #endregion

        #region Show Ticket Detail

        [HttpGet("user-tickets/{ticketId}")]
        public async Task<IActionResult> TicketDetails(long ticketId)
        {
            var ticket = await _contactService.GetTicketDetail(ticketId, User.GetUserId());
            var avatars = await _contactService.GetTicketAvatars(ticketId);
            ViewBag.OwnerAvatarImage = avatars.OwnerAvatar;
            ViewBag.AdminAvatarImage = avatars.AdminAvatar;

            if (ticket == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            return View(ticket);
        }

        #endregion

        #region Answer Ticket

        [HttpPost("answer-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerTicket(AnswerTicketDto answer)
        {
            if (ModelState.IsValid)
            {
                var creatorName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _contactService.OwnerAnswerTicket(answer, User.GetUserId(), creatorName);

                switch (result)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "عدم دسترسی";
                        TempData[WarningMessage] = "درصورت تکرار این مورد، دسترسی شما به صورت کلی از سیستم قطع خواهد شد";
                        return RedirectToAction("TicketsList", "Ticket");
                    case AnswerTicketResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                        return RedirectToAction("TicketsList", "Ticket");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ثبت شد";
                        break;
                }
            }

            return RedirectToAction("TicketDetails", "Ticket", new { area = "User", ticketId = answer.Id });
        }

        #endregion

        #endregion
    }
}
