using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin;
using NUnit.Framework;
using Moq;

namespace Owin.Hsts.Tests
{
    [TestFixture]
    public class HstsMiddlewareTests
    {
        private const string HstsHeaderName = "Strict-Transport-Security";
        private Mock<IOwinContext> _mockContext;
        private Mock<IOwinRequest> _mockRequest;
        private StubResponse _stubResponse;

        [SetUp]
        public void Setup()
        {            
            _mockRequest = new Mock<IOwinRequest>();
            _mockContext = new Mock<IOwinContext>();
            _mockContext.Setup(c => c.Request).Returns(_mockRequest.Object);
            _stubResponse = new StubResponse(_mockContext.Object);
            _mockContext.Setup(c => c.Response).Returns(_stubResponse);
            _mockRequest.Setup(r => r.IsSecure).Returns(true);  //Default the request to https
        }

        [Test]
        public async void WhenSchemeIsHttps_SetTheHeader()
        {
            // Arrange
            var middleware = new HstsMiddleware(null);

            // Act
            await middleware.Invoke(_mockContext.Object);
            _stubResponse.InvokeOnSendingHeaders();

            // Assert
            Assert.True(_mockContext.Object.Response.Headers.ContainsKey(HstsHeaderName));
        }

        [Test]
        public async void WhenSchemeIsNotHttps_DontSetTheHeader()
        {
            // Arrange
            var middleware = new HstsMiddleware(null);
            _mockRequest.Setup(r => r.IsSecure).Returns(false);

            // Act
            await middleware.Invoke(_mockContext.Object);
            _stubResponse.InvokeOnSendingHeaders();

            // Assert
            Assert.False(_mockContext.Object.Response.Headers.ContainsKey(HstsHeaderName));
        }

        [Test]
        public async void WhenHeaderIsAlreadySet_DontSetTheHeader()
        {
            // Arrange
            const string presetHeader = "max-age=1";
            _stubResponse.Headers.Add(HstsHeaderName, new[] { presetHeader });
            var middleware = new HstsMiddleware(null);

            // Act
            await middleware.Invoke(_mockContext.Object);
            _stubResponse.InvokeOnSendingHeaders();

            // Assert
            Assert.AreEqual(presetHeader, _mockContext.Object.Response.Headers[HstsHeaderName]);
        }

        [Test]
        public async void WhenHeaderIsAlreadySetAndOverrideIsSet_SetTheHeader()
        {
            // Arrange
            const string presetHeader = "max-age=1";
            var settings = new HstsSettings {OverwriteExisting = true};
            _stubResponse.Headers.Add(HstsHeaderName, new[] { presetHeader });
            var middleware = new HstsMiddleware(null, settings);
            
            // Act
            await middleware.Invoke(_mockContext.Object);
            _stubResponse.InvokeOnSendingHeaders();

            // Assert
            Assert.AreEqual(settings.GenerateResponseValue(), _mockContext.Object.Response.Headers[HstsHeaderName]);
        }
    }


    internal class StubResponse : IOwinResponse
    {

        private Action<object> _callback;
        private object _state;

        public StubResponse(IOwinContext context)
        {
            Context = context;
            Headers = new HeaderDictionary(new Dictionary<string, string[]>());
        }
        
        // Stubimplementation that only supports one callback and state
        public void OnSendingHeaders(Action<object> callback, object state)
        {
            _callback = callback;
            _state = state;
        }

        /// <summary>
        /// Simulates that another middleware has started writing to the stream.
        /// </summary>
        public void InvokeOnSendingHeaders()
        {
            if (_callback != null)
            {
                _callback.Invoke(_state);
            }            
        }

        #region Unimplemented methods

        public void Redirect(string location)
        {
            throw new NotImplementedException();
        }

        public void Write(string text)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] data, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(string text)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(string text, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(byte[] data)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(byte[] data, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(byte[] data, int offset, int count, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public IOwinResponse Set<T>(string key, T value)
        {
            throw new NotImplementedException();
        } 

        #endregion

        public IDictionary<string, object> Environment { get; private set; }
        public IOwinContext Context { get; private set; }
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Protocol { get; set; }
        public IHeaderDictionary Headers { get; private set; }
        public ResponseCookieCollection Cookies { get; private set; }
        public long? ContentLength { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public string ETag { get; set; }
        public Stream Body { get; set; }
    }
}