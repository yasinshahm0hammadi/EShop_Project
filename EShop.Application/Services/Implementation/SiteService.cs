using EShop.Application.Services.Interface;
using EShop.Application.Utilities;
using EShop.Domain.DTOs.Site;
using EShop.Domain.Entities.Site;
using EShop.Domain.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Services.Implementation;

public class SiteService : ISiteService
{
    #region Constructor

    private readonly IGenericRepository<SiteSetting> _siteSettingRepository;
    private readonly IGenericRepository<AboutUs> _aboutUsRepository;
    private readonly IGenericRepository<Feature> _featureRepository;
    private readonly IGenericRepository<Question> _questionRepository;

    public SiteService(IGenericRepository<SiteSetting> siteSettingRepository, IGenericRepository<AboutUs> aboutUsRepository, IGenericRepository<Feature> featureRepository, IGenericRepository<Question> questionRepository)
    {
        _siteSettingRepository = siteSettingRepository;
        _aboutUsRepository = aboutUsRepository;
        _featureRepository = featureRepository;
        _questionRepository = questionRepository;
    }

    #endregion

    #region Services

    #region SiteSetting

    public async Task<SiteSettingDto> GetDefaultSiteSetting()
    {
        try
        {
            var siteSetting = await _siteSettingRepository
           .GetQuery()
           .Select(x => new SiteSettingDto
           {
               SiteName = x.SiteName,
               FooterText = x.FooterText,
               Email = x.Email,
               Phone = x.Phone,
               Mobile = x.Mobile,
               CopyRight = x.CopyRight,
               Address = x.Address,
               MapScript = x.MapScript,
               IsDefault = x.IsDefault,
               CreateDate = x.CreateDate.ToStringShamsiDate(),
               LastUpdateDate = x.LastUpdateDate.ToStringShamsiDate()
           }).FirstOrDefaultAsync(x => x.IsDefault);

            return siteSetting ?? new SiteSettingDto();
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new SiteSettingDto();
        }
    }
    public async Task<EditSiteSettingDto> GetSiteSettingForEdit(long settingId)
    {
        try
        {
            var setting = await _siteSettingRepository.GetEntityById(settingId);

            if (setting != null)
            {
                return new EditSiteSettingDto
                {
                    Id = setting.Id,
                    SiteName = setting.SiteName,
                    IsDefault = setting.IsDefault,
                    Address = setting.Address,
                    CopyRight = setting.CopyRight,
                    Email = setting.Email,
                    FooterText = setting.FooterText,
                    MapScript = setting.MapScript,
                    Mobile = setting.Mobile,
                    Phone = setting.Phone,

                };
            }

            return null;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new EditSiteSettingDto();
        }
    }
    public async Task<EditSiteSettingResult> EditSiteSetting(EditSiteSettingDto newSetting, string userName)
    {
        try
        {
            var mainSetting = await _siteSettingRepository.GetEntityById(newSetting.Id);

            if (mainSetting != null)
            {
                mainSetting.SiteName = newSetting.SiteName;
                mainSetting.Address = newSetting.Address;
                mainSetting.CopyRight = newSetting.CopyRight;
                mainSetting.Email = newSetting.Email;
                mainSetting.FooterText = newSetting.FooterText;
                mainSetting.MapScript = newSetting.MapScript;
                mainSetting.Mobile = newSetting.Mobile;
                mainSetting.Phone = newSetting.Phone;
                mainSetting.IsDefault = newSetting.IsDefault;
                mainSetting.LastUpdateDate = DateTime.Now.ToShamsiDateTime();

                _siteSettingRepository.EditEntityByEditor(mainSetting, userName);
                await _siteSettingRepository.SaveChanges();

                return EditSiteSettingResult.Success;
            }

            return EditSiteSettingResult.NotFound;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return EditSiteSettingResult.Error;
        }
    }

    #endregion

    #region AboutUs

    public async Task<List<AboutUsDto>> GetAboutUs()
    {
        try
        {
            return await _aboutUsRepository
            .GetQuery()
            .Where(x => !x.IsDelete)
            .Select(x => new AboutUsDto
            {
                Id = x.Id,
                HeaderTitle = x.HeaderTitle,
                Description = x.Description,
                CreateDate = x.CreateDate.ToStringShamsiDate(),
                LastUpdateDate = x.LastUpdateDate.ToStringShamsiDate()
            }).OrderByDescending(x => x.Id)
            .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new List<AboutUsDto>();
        }
    }
    public async Task<CreateAboutUsResult> CreateAboutUs(CreateAboutUsDto about)
    {
        try
        {
            var newAboutUs = new AboutUs
            {
                HeaderTitle = about.HeaderTitle,
                Description = about.Description,
            };

            await _aboutUsRepository.AddEntity(newAboutUs);
            await _aboutUsRepository.SaveChanges();

            return CreateAboutUsResult.Success;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return CreateAboutUsResult.Error;
        }
    }
    public async Task<EditAboutUsDto> GetAboutUsForEdit(long aboutId)
    {
        try
        {
            var aboutUs = await _aboutUsRepository.GetEntityById(aboutId);

            if (aboutUs == null)
            {
                return null;
            }

            return new EditAboutUsDto
            {
                Id = aboutUs.Id,
                HeaderTitle = aboutUs.HeaderTitle,
                Description = aboutUs.Description
            };
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new EditAboutUsDto();
        }
    }
    public async Task<EditAboutUsResult> EditAboutUs(EditAboutUsDto about, string userName)
    {
        try
        {
            var aboutUs = await _aboutUsRepository.GetEntityById(about.Id);

            if (about != null)
            {
                aboutUs.HeaderTitle = about.HeaderTitle;
                aboutUs.Description = about.Description;
                aboutUs.LastUpdateDate = DateTime.Now;

                _aboutUsRepository.EditEntityByEditor(aboutUs, userName);
                await _aboutUsRepository.SaveChanges();

                return EditAboutUsResult.Success;
            }

            return EditAboutUsResult.NotFound;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return EditAboutUsResult.Error;
        }
    }


    #region Features

    public async Task<List<FeatureDto>> GetAllFeatures()
    {
        try
        {
            return await _featureRepository
            .GetQuery()
            .Where(x => !x.IsDelete)
            .Select(x => new FeatureDto
            {
                Id = x.Id,
                FeatureTitle = x.FeatureTitle,
                Image = x.Image,
            }).OrderByDescending(x => x.Id)
            .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new List<FeatureDto>();
        }
    }

    #endregion

    #region Questions

    public async Task<List<QuestionDto>> GetAllQuestions()
    {
        try
        {
            return await _questionRepository
            .GetQuery()
            .Where(x => !x.IsDelete)
            .Select(x => new QuestionDto
            {
                Id = x.Id,
                QuestionTitle = x.QuestionTitle,
                Answer = x.Answer
            }).OrderByDescending(x => x.Id)
            .ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new List<QuestionDto>();
        }
    }

    #endregion

    #endregion

    #endregion

    #region Dispose

    public async ValueTask DisposeAsync() { }

    #endregion
}