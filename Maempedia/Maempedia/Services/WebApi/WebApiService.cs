namespace Maempedia.Services.WebApi
{
    public class WebApiService
    {
        private static WebApiService instance;
        public static WebApiService Instance
        {
            get { return instance ?? (instance = new WebApiService()); }
        }

        private readonly AccountApi account;
        public AccountApi Account
        {
            get { return this.account; }
        }

        private readonly MenuApi menu;
        public MenuApi Menu
        {
            get { return this.menu; }
        }

        private readonly OwnerApi owner;
        public OwnerApi Owner
        {
            get { return this.owner; }
        }

        private CommentApi comment;
        public CommentApi Comment
        {
            get { return this.comment; }
        }

        private WebApiService()
        {
            this.account = new AccountApi();
            this.menu = new MenuApi();
            this.owner = new OwnerApi();
            this.comment = new CommentApi();
        }
    }
}
