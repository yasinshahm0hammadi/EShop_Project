using System.Security.Cryptography.X509Certificates;
using EShop.Application.Services.Interface;
using EShop.Application.Utilities;
using EShop.Domain.DTOs.Contact;
using EShop.Domain.DTOs.Contact.Ticket;
using EShop.Domain.DTOs.Paging;
using EShop.Domain.Entities.Contact;
using EShop.Domain.Entities.Contact.Ticket;
using EShop.Domain.Migrations;
using EShop.Domain.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats;

namespace EShop.Application.Services.Implementation;

public class ContactService : IContactService
{
    #region Contructor

    private readonly IGenericRepository<ContactUs> _contactRepository;
    private readonly IGenericRepository<Ticket> _ticketRepository;
    private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;

    public ContactService(IGenericRepository<ContactUs> contactRepository, IGenericRepository<Ticket> ticketRepository, IGenericRepository<TicketMessage> ticketMessageRepository)
    {
        _contactRepository = contactRepository;
        _ticketRepository = ticketRepository;
        _ticketMessageRepository = ticketMessageRepository;
    }

    #endregion

    #region Services

    #region Contact Us

    public async Task SendNewContactMessage(SendContactMessageDto contact, string userIp, long? userId)
    {
        try
        {
            // todo: Use Sanitizer to Sanitize The input data

            var newMessage = new ContactUs
            {
                UserId = (userId != null && userId.Value != 0) ? userId.Value : (long?)userId,
                UserIp = userIp,
                Fullname = contact.Fullname,
                Email = contact.Email,
                MessageSubject = contact.MessageSubject,
                MessageText = contact.MessageText,
            };

            await _contactRepository.AddEntity(newMessage);
            await _contactRepository.SaveChanges();
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);
        }
    }
    public async Task<FilterContactMessagesDto> FilterContactMessages(FilterContactMessagesDto message)
    {
        try
        {
            var query = _contactRepository
           .GetQuery()
           .Include(x => x.User)
           .OrderByDescending(x => x.Id);

            #region Filter

            if (!string.IsNullOrWhiteSpace(message.Fullname))
            {
                query = query.Where(x => EF.Functions.Like(x.Email, $"%{message.Fullname}%")).OrderByDescending(x => x.CreateDate);
            }
            if (!string.IsNullOrWhiteSpace(message.Email))
            {
                query = query.Where(x => EF.Functions.Like(x.Email, $"%{message.Email}%")).OrderByDescending(x => x.CreateDate);
            }

            #endregion

            #region Paging

            var contactCount = await query.CountAsync();

            var pager = Pager.Build(message.PageId, contactCount, message.TakeEntity,
                message.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return message.SetPaging(pager).SetContactUs(allEntities);
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new FilterContactMessagesDto();
        }
    }

    #endregion

    #region Ticket

    public async Task<AddTicketResult> AddUserTicket(AddTicketDto ticket, long userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ticket.Text))
            {
                return AddTicketResult.EmptyText;
            }

            var newTicket = new Ticket
            {
                OwnerId = userId,
                Title = ticket.Title,
                IsReadByOwner = true,
                TicketSection = ticket.TicketSection,
                TicketPriority = ticket.TicketPriority,
                TicketState = TicketState.UnderProgress
            };

            await _ticketRepository.AddEntity(newTicket);
            await _ticketRepository.SaveChanges();

            var newTicketMessage = new TicketMessage
            {
                TicketId = newTicket.Id,
                SenderId = userId,
                Text = ticket.Text
            };

            await _ticketMessageRepository.AddEntity(newTicketMessage);
            await _ticketMessageRepository.SaveChanges();

            return AddTicketResult.Success;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return AddTicketResult.Error;
        }
    }
    public async Task<FilterTicketDto> TicketsList(FilterTicketDto ticket)
    {
        try
        {
            var query = _ticketRepository
            .GetQuery()
            .AsQueryable()
            .Include(x => x.Owner)
            .Where(x => !x.IsDelete);

            #region State

            switch (ticket.TicketState)
            {
                case TicketState.All:
                    query = query.Where(x => !x.IsDelete);
                    break;
                case TicketState.UnderProgress:
                    query = query.Where(x => x.TicketState == TicketState.UnderProgress);
                    break;
                case TicketState.Answered:
                    query = query.Where(x => x.TicketState == TicketState.Answered);
                    break;
                case TicketState.Closed:
                    query = query.Where(x => x.TicketState == TicketState.Closed);
                    break;
            }

            switch (ticket.OrderBy)
            {
                case FilterTicketOrder.CreateDateDescending:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
                case FilterTicketOrder.CreateDateAscending:
                    query = query.OrderBy(x => x.CreateDate);
                    break;
            }

            #endregion

            #region Filter

            if (ticket.TicketState != null)
            {
                query = query.Where(x => x.TicketState == ticket.TicketState.Value);
            }

            if (ticket.TicketPriority != null)
            {
                query = query.Where(x => x.TicketPriority == ticket.TicketPriority.Value);
            }

            if (ticket.TicketSection != null)
            {
                query = query.Where(x => x.TicketSection == ticket.TicketSection);
            }

            if (ticket.UserId != null && ticket.UserId != 0)
            {
                query = query.Where(x => x.OwnerId == ticket.UserId);
            }

            if (Equals(!string.IsNullOrWhiteSpace(ticket.Title)))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{ticket.Title}%"));
            }

            #endregion

            #region Paging

            var ticketCount = await query.CountAsync();

            var pager = Pager.Build(ticket.PageId, ticketCount, ticket.TakeEntity,
                ticket.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return ticket.SetPaging(pager).SetTickets(allEntities);
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new FilterTicketDto();
        }
    }
    public async Task<TicketDetailDto> GetTicketDetail(long ticketId, long userId)
    {
        try
        {
            var ticket = await _ticketRepository
            .GetQuery()
            .Include(x => x.Owner)
            .OrderByDescending(x => x.CreateDate)
            .SingleOrDefaultAsync(x => x.Id == ticketId && !x.IsDelete);

            var ticketMessage = await _ticketMessageRepository
                .GetQuery()
                .Include(x => x.Sender)
                .Include(x => x.Ticket)
                .ThenInclude(x => x.Owner)
                .Where(x => x.TicketId == ticketId && !x.IsDelete)
                .OrderBy(x => x.CreateDate)
                .ToListAsync();

            if (ticket == null || ticket.OwnerId != userId)
            {
                return null;
            }

            return new TicketDetailDto
            {
                Ticket = ticket,
                TicketMessage = ticketMessage,
                Owner = ticket.Owner
            };
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new TicketDetailDto();
        }
    }
    public async Task<(string? OwnerAvatar, string? AdminAvatar)> GetTicketAvatars(long ticketId)
    {
        try
        {
            var ticket = await _ticketRepository
            .GetQuery()
            .Include(x => x.Owner)
            .Include(x => x.TicketMessages)
            .ThenInclude(x => x.Sender)
            .FirstOrDefaultAsync(t => t.Id == ticketId && !t.IsDelete);

            if (ticket == null)
                return ("Not Found", "Not Found");

            // آواتار Owner
            var ownerAvatar = ticket.Owner.AvatarPath ?? "Not Found";

            // پیدا کردن اولین پیام ادمین (SenderId != OwnerId)
            var adminMessage = ticket.TicketMessages
                .FirstOrDefault(x => x.SenderId != ticket.OwnerId);

            var adminAvatar = adminMessage?.Sender.AvatarPath ?? "Not Found";

            return (ownerAvatar, adminAvatar);
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return (null, null);
        }
    }
    public async Task<AnswerTicketResult> OwnerAnswerTicket(AnswerTicketDto answer, long userId)
    {
        try
        {
            var ticket = await _ticketRepository.GetEntityById(answer.Id);

            if (ticket == null)
            {
                return AnswerTicketResult.NotForUser;
            }

            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text
            };

            await _ticketMessageRepository.AddEntity(ticketMessage);
            await _ticketMessageRepository.SaveChanges();

            ticket.IsReadByOwner = true;
            ticket.IsReadByAdmin = false;
            ticket.TicketState = TicketState.UnderProgress;
            await _ticketRepository.SaveChanges();

            return AnswerTicketResult.Success;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return AnswerTicketResult.Error;
        }
    }
    public async Task<AnswerTicketResult> AdminAnswerTicket(AnswerTicketDto answer, long userId)
    {
        try
        {
            var ticket = await _ticketRepository.GetEntityById(answer.Id);

            if (ticket == null)
            {
                return AnswerTicketResult.NotFound;
            }

            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text
            };

            await _ticketMessageRepository.AddEntity(ticketMessage);
            await _ticketMessageRepository.SaveChanges();

            ticket.IsReadByOwner = false;
            ticket.IsReadByAdmin = true;
            ticket.TicketState = TicketState.Answered;
            await _ticketRepository.SaveChanges();

            return AnswerTicketResult.Success;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return AnswerTicketResult.Error;
        }
    }

    #endregion

    #endregion

    #region Dispose

    public async ValueTask DisposeAsync() { }

    #endregion
}