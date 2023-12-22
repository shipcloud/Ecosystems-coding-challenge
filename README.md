# Intro
For our coding challenge, we would like to create a simple program that takes an CSV as input and prints the tracking number of all booked shipments.
# Constraints
- The CSV may not be altered
- The Shipcloud SCOUT system must be used for booking shipments.
- The tracking numbers must be printed to the console **after** all shipments have been booked.
#optional
- Saving PDF labels to disk.
# Misc info
All shipments should be booked using a customer with accountcode 'SHIPCL'.\
All weights in the CSV are in KG.\
All dimensions in the CSV are in CM.\
The CSV can contain single and multi colli shipments.\
All shipments should be sent to our Shipcloud office in Madrid:
```
Shipcloud Madrid
P.º de la Castellana, 163
28046 Madrid
Spain
``` 
# Resources
Swagger: [SCOUT Swagger](https://tms-staging.europaket.plus/rest/swagger/index.html)\
User login: codingchallenge@shipcloud.com\
User password: will be provided at the time of the challenge