# Fcm.NET
Firebase Cloud Messaging (FCM) API with .NET

# Installation
```bash
dotnet add package FCM --version 1.1.0
```

# Usage
```c#
namespace FCM {
    internal class Program {
        static void Main(string[] args) {
            var fcm = new FCM();
            fcm.SetApiKey("api key");
            fcm.Send(new() { To = "device id", Notification = new() { Title = "title", Body = "body" } });
        }
    }
}
```
