# AuctionHouseAPI
REST API for managing auctions and bids, built with ASP.NET Core, Entity Frameowrk / Dapper, and MediatR with PostgreSQL
## Table of contents
- [Overview](#overview)
- [Technologies](#technologies)
- [Installing](#installing)
- [Requirements](#requirements)
- [Running the project](#running-the-project)
- [Testing](#testing)
- [API Usage](#api-usage)
- [Contributing](#contributing)
- [License](#license)
- [Author](#author)

## Overview
This project is backend service for creating, and managing bids, and auctions. It includes layered architecture, auto migrations, and CQRS pattern.

## Technologies
- .NET 8
- ASP.NET Core
- Dapper / Entity Framework
- MediatR
- AutoMapper
- NUnit & Moq
- DbUp
- PostgreSQL

## Installing
```bash
git clone https://github.com/NickyyHere/AuctionHouseAPI.git
cd AuctionHouseAPI
dotnet restore
```
## Requirements
  PGSQL_CONNECTION_STRING enviorment variable<br>
  Windows:
```bash
set PGSQL_CONNECTION_STRING=Host={HostAddress};Port={PortNumber};Username={Username};Password={Password};Database={DatabaseName};
set MONGO_CONNECTION_STRING=mongodb://{HostAddress}:{PortNumber}/{DatabaseName}
```
## Running the project
```bash
dotnet run --project AuctionHouseAPI.Presentation
```

## Testing
```bash
dotnet test
```

## API usage
```http
GET /api/[endpoint]
```
Example endpoints:
<pre>
GET /api/auctions
POST /api/auctions
PUT /api/auctions/{id}
DELETE /api/auctions/{id}
GET /api/bids
</pre>

## Contributing
- NickyyHere (Author)
- iwedaz (Mentor)

## License
This projects is licensed under [The Unlicense](https://choosealicense.com/licenses/unlicense/)

## Author
[Mail](mailto:jwojcicki001@gmail.com)<br>
[GitHub](https://github.com/NickyyHere)
