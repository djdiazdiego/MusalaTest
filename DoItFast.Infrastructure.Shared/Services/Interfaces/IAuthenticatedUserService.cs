namespace DoItFast.Infrastructure.Shared.Services.Interfaces
{
    public interface IAuthenticatedUserService
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public Guid? UserId { get; }
        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; }
        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; }
        /// <summary>
        /// User ip address.
        /// </summary>
        public string Ip { get; }
    }
}
