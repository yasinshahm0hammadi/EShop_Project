using EShop.Application.Services.Interface;
using EShop.Application.Utilities;
using Microsoft.Extensions.Configuration;

namespace EShop.Application.Services.Implementation;

public class SmsService : ISmsService
{
    #region Constructor

    private readonly IConfiguration _configuration;

    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region Services

    #region Send Verification Code

    public async Task SendVerificationSms(string mobile, string activationCode)
    {
        try
        {
            var apiKey = _configuration.GetSection("KavenegarSmsApiKey")["apiKey"];
            var api = new Kavenegar.KavenegarApi(apiKey);

            await api.VerifyLookup(mobile, activationCode, "VerifyWebsiteAccount");
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);
        }
    }

    #endregion

    #region Restore User Password

    public async Task SendRestorePasswordSms(string mobile, string newPassword)
    {
        try
        {
            var apiKey = _configuration.GetSection("KavenegarSmsApiKey")["apiKey"];
            var api = new Kavenegar.KavenegarApi(apiKey);

            await api.VerifyLookup(mobile, newPassword, "VerifyRecoverPassword");
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);
        }
    }

    #endregion

    #endregion

    #region Dispose

    public async ValueTask DisposeAsync() { }

    #endregion
}