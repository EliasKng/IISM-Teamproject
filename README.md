# IISM Teamprojekt Dashboard

The dashboard was created as a part of the course 'Teamprojekt' in the winter semester of 2020/21 for the Institute of Information Systems and Marketing at KIT. The goal of the dashboard is to demonstrate several desing principles in intuitively working with data and its visualizations. For demonstrating purpose and comparability reasons the data being used is the [Financial Sample](https://docs.microsoft.com/en-us/power-bi/create-reports/sample-financial-download) for Power BI.

The backend is implemented in Python using mainly the libraries flask, pandas and the package NL4DV. The frontend utilizes the JavaScript framework Vue.js and library D3.js. Frontend and backend are connected via REST API. [Bot]

## Backend
### Installation 
For installing the backend cd into the Backend directory, then install the requirements.txt. 
```bash
pip install -r requirements.txt 
```
## Frontend
### Installation 
For installing the frontend cd into the client directory. 
```
npm install
```
### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Lints and fixes files
```
npm run lint
```
## Bot
In the final version of this Prototype, the bot is hosted on Azure. To test the Bot, you can use the [Bot Framework Emulator](https://docs.microsoft.com/de-de/azure/bot-service/bot-service-debug-emulator?view=azure-bot-service-4.0&tabs=csharp) and login to it with the Credentials given in Bot/appsettings.json. 
However, if you want to Debug the Bot, you have to Install [.NET Core SDK](https://dotnet.microsoft.com/download) version 3.1 and Visual Studio.

Then you can debug and test the Bot in Visual Studio.

If you want to start over with a new bot, follow the installation instructions as shown in Bot/README.md