
### CG.Purple Solution - BACKLOG

Things to be done on the project, as of 12/06/2022

### General

* Eventually, we want to capture metrics, both message and provider related. 

* Eventually, we want a dashboard with pretty graphs for displaying metrics.

* Eventually, need to deal with provider failover, for times when a provider is overworked, or rate limited, or just dies for some reason.

### Database

* Should we map all CRUD operations to stored procedures?

* Should we write stored procedures for all queries?

### Libraries

* Should we add 'tags' for messages? (like properties, but without a value)

* Should we add caching to the managers?

### Host (UI)

* All dialogs should use the MudBlazor extensions.

* We've removed all cascading deletes from the DAL, so now we need to write code 
  to disable operations, in the UI, that will fail due to FK constraints.

* It shouldn't be possible to disable all providers from the UI, or even all providers for a type of message (email or text), since that effectively breaks the processing pipeline. So, we need that dialog to check for that case and save us from ourselves with a pretty error message.

* Need a better UI for messages, just not sure what, yet.

* Need a better UI for logs.

* Need a better UI for errors. Toasts just aren't enough.

### Providers

* Need to figure out how to post status back using SignalR.

### REST Controllers

* Should we stand up a swagger page, for the API?

### Other Integrations

* For sure need to integrate with an OIDC / JWT service.

* Possibly need to integrate with a configuration service.

* Possibly need to integrate with a file storage service.

### Testing

* Need to write an overall integration test plan

* Need to write specific unit test fixtures.

### Documentation / Help

* Yup, we'll need this at some point.


