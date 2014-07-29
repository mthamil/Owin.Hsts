using System.Threading.Tasks;
using Microsoft.Owin;

namespace Owin.Hsts
{
    /// <summary>
    ///     Appends the HTTP Strict Transport Security Header if the application does not specify it.
    /// </summary>
    public class HstsMiddleware : OwinMiddleware
    {

        private const string hstsHeaderName = "Strict-Transport-Security";
        private readonly HstsSettings _currentSettings;

        public HstsMiddleware(OwinMiddleware next)
            : base(next)
        {
            _currentSettings = new HstsSettings();
        }

        public HstsMiddleware(OwinMiddleware next, HstsSettings settings)
            : base(next)
        {
            _currentSettings = settings ?? new HstsSettings();
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (Next != null)
            {
                await Next.Invoke(context);
            }

            // We need to check to see if the application has already added the key
            if (context.Response.Headers.ContainsKey(hstsHeaderName))
            {
                // it has, so check to see if we can override the application
                if (_currentSettings.OverwriteExisting)
                {
                    context.Response.Headers[hstsHeaderName] = _currentSettings.GenerateResponseValue();
                }
            }
            else
            {
                // we are safe to add the new header
                context.Response.Headers.Add(hstsHeaderName, new[] {_currentSettings.GenerateResponseValue()});
            }
        }

    }

}
