using Fissoft.EntityFramework.Fts;

namespace Termoservis.DAL.Extensions
{
    /// <summary>
    /// Extensions for <see cref="ApplicationDbContext"/>.
    /// </summary>
    public static class ApplicationDbContextExtensions
    {
        /// <summary>
        /// Enables the Entity Framework FTS interceptors.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <remarks>
        /// Using library:
        /// https://github.com/fissoft/Fissoft.EntityFramework.Fts
        /// </remarks>
        /// <example>
        ///     var text = FullTextSearchModelUtil.ContainsAll("code ef");
        ///     db.Tables.Where(c=>"*".Contains(text));
        /// </example>
        /// <example>
        ///     var text = FullTextSearchModelUtil.Contains("code");
        ///     db.Tables.Where(c=>c.Fullname.Contains(text));
        /// </example>
        public static void EnableEfFts(this ApplicationDbContext dbContext)
        {
            DbInterceptors.Init();
        }
    }
}
