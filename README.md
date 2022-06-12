# DevNet.ChromeDriverManager
## _Manage chromedriver distribution and autoinstall_
Version 1.0.3 (Latest)
![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)

.Net library that sets up and give access to manage chromedriver 
(built for .Net Framework 4.5 and above)

## Usase
Set ChromeDriver Location (Optional)

```sh
DevNet.ChromeDriverManager.Location = "C:/WebDrivers";
```
Get Installed Version

```sh
string installedVersion = DevNet.ChromeDriverManager.INSTALLED_VERSION;
Console.WriteLine(installedVersion);
```
Get Latest Version

```sh
string latestVersion = DevNet.ChromeDriverManager.LATEST_VERSION;
Console.WriteLine(latestVersion);
```
Install Latest Version

```sh
DevNet.ChromeDriverManager.InstallLatest();
```
Install Specific Version

```sh
DevNet.ChromeDriverManager.InstallSpecified(version: "103.0.5060.24");
```
Install Compatible with Chrome

```sh
DevNet.ChromeDriverManager.InstallWithCompatiblity();
```
Installation Status (Only while installing ChromeDriver)

```sh
int? status = DevNet.ChromeDriverManager.DOWNLOAD_PROGRESS_PERCENTAGE;
Console.WriteLine(status);
```
Uninstall

```sh
DevNet.ChromeDriverManager.Uninstall();
```

## Developer

Developed by Akash Shaw
##### Reach us
@[Github](https://github.com/itsalfredakku)
@[Facebook](https://facebook.com/itsalfredakku)
@[Instagram](https://instagram.com/itsalfredakku)

## License

MIT
