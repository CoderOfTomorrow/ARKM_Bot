namespace ARKM_Bot.Models
{
    public abstract class BaseRequest
    {
        public abstract HttpMethod HttpMethod { get; set; }
        public abstract string Method { get; set; }
        public abstract string Endpoint { get; set; }
        public abstract string Body { get; set; }
    }
}