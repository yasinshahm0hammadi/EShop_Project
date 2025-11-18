using EShop.Domain.DTOs.Contact;
using EShop.Domain.DTOs.Contact.Ticket;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Application.Services.Interface;

public interface IContactService : IAsyncDisposable
{
    #region Contact Us

    Task SendNewContactMessage(SendContactMessageDto contact, string userIp, long? userId);
    Task<FilterContactMessagesDto> FilterContactMessages(FilterContactMessagesDto message);

    #endregion

    #region Ticket

    Task<AddTicketResult> AddUserTicket(AddTicketDto ticket, long userId, string? creatorName);
    Task<FilterTicketDto> TicketsList(FilterTicketDto ticket);
    Task<TicketDetailDto> GetTicketDetail(long ticketId, long userId);
    Task<(string? OwnerAvatar, string? AdminAvatar)> GetTicketAvatars(long ticketId);
    Task<AnswerTicketResult> OwnerAnswerTicket(AnswerTicketDto answer, long userId, string? creatorName);
    Task<AnswerTicketResult> AdminAnswerTicket(AnswerTicketDto answer, long userId, string? creatorName);

    #endregion
}