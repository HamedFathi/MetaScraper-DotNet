using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using scraperDotNet.Models;

namespace scraperDotNet.Controllers
{
    public class MetaDataModel
    {
        private string _title;
        private ImageInfoModel _imageInfo;
        private string _another;
        private string _siteName;
        private string _descriptions;
        private List<String> _keywords;
        private string _locale;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Website title
        /// </summary>
        /// <value>The website title.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String Url { get; set; }

        /// <summary>
        /// Website thumbnail image with width and height values.
        /// </summary>
        /// <value>The thumbnail image URL.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ImageInfoModel ImageInfo
        {
            get { return _imageInfo; }
            set
            {
                _imageInfo = value;
                OnPropertyChanged(nameof(ImageInfo));
            }
        }

        /// <summary>
        /// Gets or sets website another.
        /// </summary>
        /// <value>Another.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String Another
        {
            get { return _another; }
            set
            {
                _another = value;
                OnPropertyChanged(nameof(Another));
            }
        }

        /// <summary>
        /// If website has data should get true value.
        /// </summary>
        /// <value>The website has data.</value>
        public Boolean HasData { get; set; }

        /// <summary>
        /// Gets or sets the name of the site.
        /// </summary>
        /// <value>The name of the site.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String SiteName
        {
            get { return _siteName; }
            set
            {
                _siteName = value;
                OnPropertyChanged(nameof(SiteName));
            }
        }

        /// <summary>
        /// Gets or sets the website descriptions.
        /// </summary>
        /// <value>The descriptions.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String Descriptions
        {
            get { return _descriptions; }
            set
            {
                _descriptions = value;
                OnPropertyChanged(nameof(Descriptions));
            }
        }

        /// <summary>
        /// Gets or sets the website keywords. Note: this tag has been deprecated by google bots to scrape websites for SEO.
        /// </summary>
        /// <value>The keywords.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<String> Keywords
        {
            get { return _keywords; }
            set
            {
                _keywords = value;
                OnPropertyChanged(nameof(Keywords));
            }
        }

        /// <summary>
        /// Gets or sets the website locale.
        /// </summary>
        /// <value>The locale.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String Locale
        {
            get { return _locale; }
            set
            {
                _locale = value;
                OnPropertyChanged(nameof(Locale));
            }
        }


        private MetaDataModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:scraperDotNet.Controllers.MetaDataModel"/> class.
        /// If website hasn't data this class will be called.
        /// Note: The url is require paramter and should not be null.
        /// </summary>
        /// <param name="url">URL.</param>
        public MetaDataModel(String url)
        {
            Contract.Requires(url != null);

            this.HasData = false;
            this.Url = url;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
