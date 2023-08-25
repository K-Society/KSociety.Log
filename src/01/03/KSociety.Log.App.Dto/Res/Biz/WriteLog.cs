namespace KSociety.Log.App.Dto.Res.Biz
{
    using KSociety.Base.App.Shared;
    using ProtoBuf;

    [ProtoContract]
    public class WriteLog : IResponse
    {
        [ProtoMember(1)] public bool Result { get; set; }

        public WriteLog()
        {

        }

        public WriteLog(bool result)
        {
            this.Result = result;
        }
    }
}