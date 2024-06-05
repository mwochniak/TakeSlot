# TakeSlot

Application purpose is be middleware to SlotService to read free test availability slots for example facility and take slot for patient.

## Prerequisite to run: 

Add appsettings section appropriately to environment contains: 

"SlotServiceApiClient": {
    "Username": "",
    "Password": "",
    "BaseUrl": "https://draliatest.azurewebsites.net"
  }


## Presentation

For UI purpose you can use swagger (default: https://localhost:7279/swagger/index.html)

### Technical description

Application is separated for command and queries with MediatR package. Endpoints are created via minimal api. 
Chosen architecture is clean are good to start for now without domain part as is not needed.

Validation for query is for now in directly in endpoints, command are in minimal api filters.

Unit tests are written to crucial parts - command and queries. Most important is to return valid time slots for end user.