
### CG.Purple Solution - BACKLOG

Things to be done on the project, as of 12/06/2022

### General

* Need to remove the plugins until we solve the dependent assembly problem.

* Eventually, we want to capture metrics, both message and provider related. 

* Eventually, we want a dashboard with pretty graphs for displaying the metrics.

* Eventually, need to deal with provider failover, for times when a provider is overworked, or rate limited.

* Does it make sense to employ a user-configurable rules engine for things like provider assignment, failover, archiving, etc.?

### Database

* Should we map all CRUD operations to stored procedures?

* Should we write stored procedures for all queries?

### Libraries

* Should we add 'tags' for messages? (like properties, but without a value)

* Should we add caching to the managers?

### Providers

* Need to write the SendGrid provider.

### Pipeline

* During idle periods, we should probably slow the pipeline down so we don't chew up resources querying the database over and over. When there is work to do, we can speed it back up.

### Host (UI)

* All dialogs should use the MudBlazor extensions.

* We've removed all cascading deletes from the DAL, so now we need to write code 
  to disable operations, in the UI, that will fail due to FK constraints.

* Need a better UI for messages, just not sure what, yet.

* The logs page still needs some work.

* Should we write our own attachment UI?

* We need a better error presentation. Toasts just aren't enough.

### REST Controllers

* Need to work on the design/coding of the REST controllers.

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


