
### CG.Purple - README

This project contains core logic for the **CG.Purple** microservice.

#### Notes

* Remember, any place we create or update a model with an associated provider type, we (the manager) are responsible for encrypting and decrypting the parameter types for that associated provider type. That includes find methods! Remember, if we write a parameter value, the value should be encrypted. If we read a parameter value, the value should be decrypted.

