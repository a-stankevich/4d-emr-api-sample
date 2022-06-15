## Update ApiClient.cs

1. Update `public-api.json` from <host>/swagger
2. Install [nswag cli](https://github.com/RicoSuter/NSwag/wiki/CommandLine)
3. Run from root folder
```
nswag openapi2csclient /input:public-api.json /classname:ApiClient /namespace:Stage4 /output:PatientLoader4/Generated/ApiClient.cs /UseBaseUrl:false
```
