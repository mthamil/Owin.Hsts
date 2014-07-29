using System;
using System.Text;

namespace Owin.Hsts
{
    /// <summary>
    ///     Settings for the HSTS Middleware
    /// </summary>
    public class HstsSettings
    {

        /// <summary>
        ///     The amount of time the browser should keep the HSTS policy enabled for. (Default: 28 days)
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        ///     Whether sub-domains should be covered by the browsers HSTS policy. (Default: true)
        /// </summary>
        public bool IncludeSubDomains { get; set; }

        /// <summary>
        ///     Determines whether or not to overwrite an existing value. (Default: false)
        /// </summary>
        public bool OverwriteExisting { get; set; }

        public HstsSettings()
        {
            Duration = TimeSpan.FromDays(28d);
            IncludeSubDomains = true;
        }

        /// <summary>
        ///     Generates the value of the response header
        /// </summary>
        public string GenerateResponseValue()
        {
            var sb = new StringBuilder(String.Format("max-age={0}", (int) Duration.TotalSeconds));
            if (IncludeSubDomains)
            {
                sb.Append("; includeSubDomains");
            }
            return sb.ToString();
        }

    }
}
