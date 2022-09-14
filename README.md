[![license](https://img.shields.io/:license-mit-blue.svg)](https://github.com/ozgur-soft/FCM.NET/blob/main/LICENSE.md)

# FCM.NET
Firebase Cloud Messaging (FCM) API with .NET

# Installation
```bash
dotnet add package FCM --version 1.0.1
```

# Usage
```c#
using FCM;

var fcm = new FCM();
fcm.SetApiKey("api key");
fcm.Send(new() { To = "device id", Notification = new() { Title = "title", Body = "body" } });
```
