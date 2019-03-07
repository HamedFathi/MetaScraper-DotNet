## ICTChallenge (Redstone Team)
Question ID: 515CD / Question 3
Score: 700

## Project Prerequisites:
Visual Studio 2017 on Mac OS HighSierra

## Build Instructions:
Open project. Use "Rebuild Solution" option in Build menu and then hit F5 to run the project.

## Documentation 

By calling the api `api/metaScraper` and set the url as parameter will get the meta JSON:

```
http://MY_DOMAIN.com/api/metaScraper/WEBSITE_URL
```

### Response

#### Success: 

Each parameter was available in html content will be in meta JSON.

``` json
{
  "title": "مستندات سرویس چابک پوش",
  "url": "doc.chabokpush.com",
  "imageInfo": {
    "imgUrl": "https://raw.githubusercontent.com/chabokpush/chabok-assets/master/chaboklogoblue.png",
    "imgWidth": 120,
    "imgHeight": 125
  },
  "hasData": true,
  "siteName": "چابک‌پوش",
  "descriptions": " پلتفرم‌ها پیاده سازی راحت بر روی هر پلتفرمیچابک روی هر پلتفرمی قابل استفاده استما برای هر پلتفرم راهنمای کاملی ایجاد کرده‌ایم تا شما در کوتاه‌ترین زمان چابک را به برنامه یا نرم‌...",
  "keywords": [
    "پوش‌نوتیفیکیشن",
    "ترکر نصب",
    "شمارنده نصب",
    "پلتفرم مارکتینگ موبایل و وب",
    "push notification",
    "install tracker",
    "segmentation",
    "کمپین نصب",
    "سگمنت",
    "track event",
    "ترک رویداد",
    "رصد رفتار کاربر",
    "پوش نوتیفیکیشن خودکار",
    "پوش جغرافیایی"
  ],
  "locale": "fa_IR"
}
```

#### Error: 

```
{
  "statusCode": 500,
  "statusDescription": "Couldn't resolve host name"
}
```

## Redstone Group
Mohammad Hadi Bayan
Hussein Habibi Juybari
Ali Noshahi
