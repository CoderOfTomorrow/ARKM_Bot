namespace ARKM_Bot.Models
{
    public class GetRequest : BaseRequest
    {
        public override HttpMethod HttpMethod { get; set; } = HttpMethod.Get;
        public override string Method { get; set; } = "GET";
        public override string Body { get; set; } = "";
        public override string Endpoint { get; set; }
    }
}