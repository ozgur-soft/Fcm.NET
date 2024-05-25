# Fcm.NET
Firebase Cloud Messaging (Fcm) API with .NET

# Installation
```bash
dotnet add package Fcm --version 1.2.0
```

# Usage
```c#
namespace Fcm {
    internal class Program {
        static void Main(string[] args) {
            var fcm = new Fcm();
            fcm.SetApiKey("api key");
            fcm.Send(new() { To = "device id", Notification = new() { Title = "title", Body = "body" } });
        }
    }
}
```
