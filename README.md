# CG.Purple: 
---
[![Build Status](https://dev.azure.com/codegator/CG.Purple/_apis/build/status/CodeGator.CG.Purple?branchName=main)](https://dev.azure.com/codegator/CG.Purple/_build/latest?definitionId=92&branchName=main)
[![Github docs](https://img.shields.io/static/v1?label=Documentation&message=online&color=blue)](https://codegator.github.io/CG.Purple/index.html)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/codegator/CG.Purple/92)
[![Github discussion](https://img.shields.io/badge/Discussion-online-blue)](https://github.com/CodeGator/CG.Purple/discussions)

### What does it do?
Purple is an idea for a self contained messaging microservice. The scenario is: You give Purple an email, or text, via a REST call, and it takes care of sending that message on your behalf.

* The service will handle retry logic, fallbacks, notifications, etc. 

* Messages are kept for some number of days before they are deleted. 

* Notifications are sent, via SignalR, to the caller, for updated status.

* There will be a REST interface, for information about messages, history, etc.

* There will (hopefully) be a mobile app, for administering the service remotely.

### What platform(s) does it support?
* .NET 7.x or higher

### What databases does it support?
* For now, SqlServer 2019, or higher.

### What databases does it support?
* For sure, SMTP and SendGrid. Possibly also Twillio. Maybe others, at some point.

### How do I contact you?
If you've spotted a bug in the code please use the project Issues [HERE](https://github.com/CodeGator/CG.Purple/issues)

We also have a discussion group [HERE](https://github.com/CodeGator/CG.Purple/discussions)

### Is there any documentation?
There is developer documentation [HERE](https://codegator.github.io/CG.Purple/)

We also blog about projects like this one on our website, [HERE](http://www.codegator.com)

---

## Disclaimer
This project and it's contents are experimental in nature. There is no official support. Use at your own risk.