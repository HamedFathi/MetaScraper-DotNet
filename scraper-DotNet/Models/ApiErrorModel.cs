using System;
using Newtonsoft.Json;

namespace scraperDotNet.Models
{
    public class ApiErrorModel
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public int StatusCode { get; private set; }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        /// <value>The status description.</value>
        public string StatusDescription { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:scraperDotNet.Models.ApiErrorModel.ApiErrorModel"/> class.
        /// </summary>
        /// <param name="statusCode">Status code.</param>
        /// <param name="statusDescription">Status description.</param>
        public ApiErrorModel(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:scraperDotNet.Models.ApiErrorModel.ApiErrorModel"/> class.
        /// </summary>
        /// <param name="statusCode">Status code.</param>
        /// <param name="statusDescription">Status description.</param>
        /// <param name="message">Message.</param>
        public ApiErrorModel(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            this.Message = message;
        }
    }
}
