namespace Owin.Hsts
{
    public static class HstsExtensions
    {
        /// <summary>
        ///     Adds the Http Strict Transport Security (HSTS) header using the default settings
        /// </summary>
        /// <param name="builder">The current OWIN App Builder</param>
        public static void UseHttpStrictTransportSecurity(this IAppBuilder builder)
        {
            builder.Use<HstsMiddleware>();
        }

        /// <summary>
        ///     Adds the Http Strict Transport Security (HSTS) header using the specified settings
        /// </summary>
        /// <param name="builder">The current OWIN App Builder</param>
        /// <param name="settings">The settings to apply to the HSTS header</param>
        public static void UseHttpStrictTransportSecurity(this IAppBuilder builder, HstsSettings settings)
        {
            builder.Use<HstsMiddleware>(settings);
        }

    }
}
