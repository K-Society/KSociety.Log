namespace KSociety.Log.Pre.Web.App.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !System.String.IsNullOrEmpty(this.RequestId);
    }
}