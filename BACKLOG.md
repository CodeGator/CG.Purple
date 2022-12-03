
### CG.Purple Solution - BACKLOG

Things to be done on the project, as of 12/03/2022

### Design

* ... All in my head, need to write it down.

### Database

* Need to remove all cascaded deletes in EFCORE.

* Need to figure out why adding an optional provider type to message gives auto mapper and EFCORE a nervous breakdown.

* At some point should we map all CRUD operations to stored procedures?

* At some point, should we write stored procedures for all queries?

* Need to test the DB design.

### Libraries

* Need to test the managers / repositories.

### Providers

* Need to write the SendGrid and Twillio providers.

* Need to test all the providers.

### Hosted Services

* Need to design a recovery mechanism that will deal with unrecoverable errors in a provider by reassigning any affected messages.

* Need to test the service.

### Host

* All dialogs should use the mudblazor extensions.

* Need to write code to prevent delete operations that will fail due to FK relations.

* Need to finish the UI.

### Services 

* Need to design and build the REST controllers.

### Other Integrations

* For sure need to integrate with an OIDC / JWT service.

* Possibly need to integrate with a configuration service.

* Possibly need to integrate with a file storage service.

* Possibly need to integrate with a REDIS cache service.

### Testing

* Need to write an overall test plan

* Need to write unit test fixtures.

### Documentation / Help

* Yup, will need this at some point.


