![logo](logos/codegator-167x79.png)

# CG.Purple: 
[![Build Status](https://dev.azure.com/codegator/CG.Purple/_apis/build/status/CodeGator.CG.Purple?branchName=main)](https://dev.azure.com/codegator/CG.Purple/_build/latest?definitionId=93&branchName=main)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/codegator/CG.Purple/93?logo=codecov&logoColor=white&style=flat-square&token=4BBNQPPATD)
[![Github docs](https://img.shields.io/static/v1?label=Documentation&message=online&color=blue)](https://codegator.github.io/CG.Purple/index.html)
[![GitHub last commit](https://img.shields.io/github/last-commit/CodeGator/CG.Purple?color=594ae2&style=flat-square&logo=github)](https://github.com/CodeGator/CG.Purple)


#### GitHub Stats
![Alt](https://repobeats.axiom.co/api/embed/d5fcf6901ac54bfa82dbafed01638aedd01047cc.svg "Repobeats analytics image")

### What does it do?
Purple is an idea for a self contained messaging microservice. The scenario is: You give Purple an email, or a text, via a REST call, and it takes care of storing that message, sending it, and tracking it's history and status, on your behalf.

### What's actually working, at this point?

* The service sends mail and text messages through SMTP, SendGrid, or Twillio.

* The service handles automatic retry, for failed messages.

* The service sends notifications, via SignalR, in real time.

* The service stores messages for a configurable amount of time. 

* The service deletes messages after a configurable amount of time. 

* The services has a simple REST interface.

* The service has a simple C# client that wraps the REST interface and makes it ridiculously easy to use from a C# application.


### What does it look like?
Here are a few early screen shots (subject to change):

Message page:
![messges](screens/messages.png)

Mime Type page:
![messges](screens/mimetypes.png)

Property Type page:
![messges](screens/propertytypes.png)

Parameter Type page:
![messges](screens/parametertypes.png)

Provider Type page:
![messges](screens/providertypes.png)

### What platform(s) does it support?
* [.NET 7.x](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) or higher

### What database(s) does it support?
* For now, SqlServer 2019, or higher.

### What 3rd party providers does it support?
* For sure, SMTP, Twillio, and SendGrid. Possibly others, who knows.

### How do I contact you?
If you've spotted a bug in the code please use the project Issues [HERE](https://github.com/CodeGator/CG.Purple/issues)

We also have a discussion group [HERE](https://github.com/CodeGator/CG.Purple/discussions)

### Is there any documentation?
There is developer documentation [HERE](https://codegator.github.io/CG.Purple/)  (when the blasted CI/CD pipeline works and it gets updated).

We also blog about projects like this one on our website, [HERE](http://www.codegator.com)

### Disclaimer
This project and it's contents are experimental in nature. There is no official support. Use at your own risk.