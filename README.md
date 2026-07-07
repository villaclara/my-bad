# my-bad

.NET 8 Minimal API + Angular, with PostgreSQL as db provider. It also has already configured Dockerfile to run
this app in the container.

This project is a helper tool for players of DOTA2 game. It has two main features: 
- Pick hero suggestion - get the suggested hero to pick for your team / enemy will pick that fits the most based on already selected heroes.
- Wards placement analyzer - get the data for your most useful/useless wards based on where it was placed and how much it lived on average.

This project gathers data from [OpenDota API](https://www.opendota.com/). There is background job set up to run each 15 mins to gather info about latest matches
in 'Divine' bracket. After running it approximately for 6 month the saved matches quantity was ~1.5m records in db. Regarding wards when the request is sent it automatically checks last 20 matches played by that account and saves the wards info into db.
