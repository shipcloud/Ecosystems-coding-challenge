# Coding Challenge: Shipment Booking System

## Objective
Create a program that processes a CSV file and books shipments using the Shipcloud SCOUT system. The program should:

1. Read the CSV input without altering it.
2. Book shipments using the Shipcloud SCOUT system.
3. Print the tracking numbers to the console **after** all shipments are successfully booked.

## Constraints
- The CSV file must remain unmodified.
- Use the Shipcloud SCOUT system for all shipment bookings.
- Display tracking numbers only after all shipments have been processed.

## Optional Features
- Save shipment labels to disk.

## Shipment Details
- Use the customer account code: **'SHIPCL'**.
- All weights in the CSV are in **kilograms (KG)**.
- All dimensions in the CSV are in **centimeters (CM)**.
- The CSV may include both **single** and **multi-colli** shipments.
- All shipments should be sent to our Shipcloud office in Madrid:

```
Shipcloud Madrid
P.º de la Castellana, 163
28046 Madrid
Spain
``` 

## Guidelines
- You may use third-party libraries as needed.
- You are allowed to use the internet for reference.

## Resources
- Shipcloud SCOUT API Documentation: [SCOUT Swagger](https://tms-staging.europaket.plus/rest/swagger/index.html)
- User Credentials:
  - **Email:** codingchallenge@shipcloud.com
  - **Password:** Will be provided during the challenge
