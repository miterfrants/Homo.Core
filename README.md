# How to use Homo.Core centralize error handling

- Add config block in appsettings.json

```json
"Config": {
    "Common": {
        "ErrorMappingPath": "Your Error Mapping Ressource Folder"
    }
}
```

- Add error mapping json in ur project ex: `./Resources/{ISO 639-1}-{ISO 3166-1}.json` `./Resources/zh-TW.json`

```json
{
  "INVALID_EMAIL": "錯誤 EMAIL 格式"
}
```

- using Homo.Core namespaces in Startup.cs

```cs
using Homo.Core.Constants;
```

- Add code into `ConfigureServices` in Startup.cs

```cs
AppSettings appSettings = new AppSettings();
Configuration.GetSection("Config").Bind(appSettings);
services.Configure<AppSettings>(Configuration.GetSection("Config"));
```

- Use Middleware in `Configure` in Startup.cs

```cs
app.UseMiddleware(typeof(Homo.Core.Middleware.ErrorHandlingMiddleware))
```
