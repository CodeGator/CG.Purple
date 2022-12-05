
### CG.Purple Solution - BACKLOG

Things to be done on the project, as of 12/05/2022

### Database

* Should we map all CRUD operations to stored procedures?

* Is there are way to reliably tell EFCORE not to update certain associated entities and/or child collections?

* Should we write stored procedures for all queries?

### Libraries

* Should we remove the parameter collection on provider type so we won't have to encrypt/decrypt the values everywhere those object might be read/written?

* Should we add caching to the managers?

### Providers

* Need to write the SendGrid and Twillio providers.

### Pipeline

* Need to finish the rework on the pipeline.

* Eventually, need to deal with provider fallback, for times when a provider is overworked, or rate limited.

### Host

* All dialogs should use the mudblazor extensions.

* We've removed all cascading deletes from the DAL, so now we need to write code 
  to disable operations, in the UI, that will fail due to FK constraints.

* Need a better UI for messages, just not sure what, yet.

* The logs page still needs some work.

* Should we write our own attachment UI?

### REST Controllers

* Need to work on the design of the REST controllers.

### Seeding

* Should we add seeding by scenario?

### Other Integrations

* For sure need to integrate with an OIDC / JWT service.

* Possibly need to integrate with a configuration service.

* Possibly need to integrate with a file storage service.

### Testing

* Need to write an overall integration test plan

* Need to write unit test fixtures.

* Do we need to write integration test fixtures?.

### Documentation / Help

* Yup, we'll need this at some point.


