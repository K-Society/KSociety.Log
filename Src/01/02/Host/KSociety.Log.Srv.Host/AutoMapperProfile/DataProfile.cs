using AutoMapper;
using KSociety.Log.App.Dto.Req.Biz;
using KSociety.Log.Biz.IntegrationEvent.Event;

namespace KSociety.Log.Srv.Host.AutoMapperProfile;

public class DataProfile : Profile
{
    public DataProfile()
    {
        CreateMap<WriteLog, WriteLogEvent>();
    }
}