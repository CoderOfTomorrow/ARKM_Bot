namespace ARKM_Bot.Models
{
    public class PostRequest : BaseRequest
    {
        public override HttpMethod HttpMethod { get; set; } = HttpMethod.Post;
        public override string Method { get; set; } = "POST";
        public override string Body { get; set; } = "{}";
        public override string Endpoint { get; set; }
    }
}